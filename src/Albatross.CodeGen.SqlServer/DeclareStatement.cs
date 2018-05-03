﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.Database;
using Albatross.CodeGen.Generation;
using Albatross.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("declare statement", "sql", Category ="Sql Server", Description = "Create a declare statement based on the parameters generated by the children")]
	public class DeclareStatement : ICodeGenerator<object, SqlCodeGenOption>{

		IBuildSqlVariable buildVariable;
		IGetSqlVariable getVariable;

		public DeclareStatement(IBuildSqlVariable buildVariable, IGetSqlVariable getVariable) {
			this.buildVariable = buildVariable;
			this.getVariable = getVariable;
		}

		public event Func<StringBuilder, IEnumerable<object>> Yield;

		public IEnumerable<object>  Build(StringBuilder sb, object src, SqlCodeGenOption options) {
			List<object> list = new List<object> { this };

			StringBuilder child = new StringBuilder();
			var items = Yield?.Invoke(child);

			IEnumerable<Variable> variables = options.Variables??new Variable[0];
			foreach (var item in items) {
				variables  = variables.Union(getVariable.Get(item));
			}
			if (variables.Count() > 0) {
				sb.AppendLine("declare");
				foreach (var variable in variables) {
					buildVariable.Build(sb.Tab(), variable);
					if (variable == variables.Last()) {
						sb.Semicolon().AppendLine();
					} else {
						sb.Comma().AppendLine();
					}
				}
			}

			sb.Append(child);
			list.AddRange(items);
			return list;
		}

		public IEnumerable<object> Build(StringBuilder sb, object source, object option) {
			return this.ValidateNBuild(sb, source, option);
		}

		public void Configure(object data) { }
	}
}

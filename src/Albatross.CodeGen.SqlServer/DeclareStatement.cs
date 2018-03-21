﻿using Albatross.CodeGen;
using Albatross.CodeGen.Database;
using Albatross.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("declare statement", "sql", Category ="Sql Server", Description = "Create a declare statement based on the parameters generated by the children")]
	public class DeclareStatement : ICodeGenerator<object, SqlCodeGenOption>{

		IBuildSqlType buildSqlType;
		IGetVariable getVariable;
		IBuildVariable buildVariable;

		public DeclareStatement(IBuildSqlType buildSqlType, IGetVariable getVariable, IBuildVariable buildVariable) {
			this.buildSqlType = buildSqlType;
			this.getVariable = getVariable;
			this.buildVariable = buildVariable;
		}

		public event Func<StringBuilder, IEnumerable<object>> Yield;

		public IEnumerable<object>  Build(StringBuilder sb, object src, SqlCodeGenOption options) {
			List<object> list = new List<object> { this };

			StringBuilder child = new StringBuilder();
			var items = Yield?.Invoke(child);

			IEnumerable<Variable> variables = new Variable[0];
			foreach (var item in items) {
				variables.Union(getVariable.Get(item));
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

		public void Configure(object data) { }
	}
}

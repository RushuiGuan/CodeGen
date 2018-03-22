﻿using Albatross.CodeGen.Database;
using Albatross.CodeGen.SqlServer;
using Albatross.Database;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("table_merge_select", GeneratorTarget.Sql, Category = GeneratorCategory.SQLServer, Description = "Merge statement select clause")]
	public class TableMergeSelect : TableQueryGenerator {
		IGetTable getTable;
		ICreateVariableName getVariableName;
		IBuildSqlType buildSqlType;
		IStoreVariable createVariable;

		public TableMergeSelect(IGetTable getTable, ICreateVariableName getVariableName, IBuildSqlType buildSqlType, IStoreVariable createVariable) {
			this.getTable = getTable;
			this.getVariableName = getVariableName;
			this.buildSqlType = buildSqlType;
			this.createVariable = createVariable;
		}

		public override IEnumerable<object>  Build(StringBuilder sb, Table table, SqlCodeGenOption options) {
			table = getTable.Get(table.Database, table.Schema, table.Name);
			if (table.Columns.Count() == 0) {
				throw new CodeGenException("Editor column doesn't exist");
			}
			sb.Append("using ").OpenParenthesis().AppendLine();
			sb.Tab().AppendLine("select");
			foreach (var column in table.Columns) {
				sb.Tab(2);
				string name = getVariableName.Get(column.Name);
				if (options.Expressions.TryGetValue(name, out string expression)) {
					sb.Append(expression);
				} else {
					sb.Append(name);
					createVariable.Store(this, column.GetVariable());
				}
				sb.Append(" as ").EscapeName(column.Name);
				if (column != table.Columns.Last()) {
					sb.Comma();
				}
				sb.AppendLine();
			}
			sb.CloseParenthesis().Append(" as src");
			return new[] { this, };
		}
	}
}

﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.Database;
using Albatross.CodeGen.Faults;
using Albatross.CodeGen.Generation;
using Albatross.Database;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("sql.where.table", GeneratorTarget.Sql, Category = GeneratorCategory.SQLServer, Description = "Table where clause, can handle identity column and\\or primary keys.  Use the SqlQueryOption.Filter flag to indicate filter method")]
	public class WhereClause : TableQueryGenerator {
		IGetTable getTable;
		ICreateSqlVariableName getVariableName;
		IRenderSqlType buildSqlType;
		IStoreSqlVariable createVariable;

		public WhereClause(IGetTable getTable, ICreateSqlVariableName getVariableName, IRenderSqlType typeBuilder, IStoreSqlVariable createVariable) {
			this.getTable = getTable;
			this.getVariableName = getVariableName;
			this.buildSqlType = typeBuilder;
			this.createVariable = createVariable;
		}

		public override IEnumerable<object> Generate(StringBuilder sb, Table t, SqlCodeGenOption option) {
			getTable.Get(ref t);
			sb.Append("where");
			int count = 0;

			if ((option.Filter & FilterOption.ByIdentityColumn) > 0){
				Column column = t.IdentityColumn;
				if (column == null) {
					throw new CodeGeneratorException("Identity Column doesn't exist");
				}
				AppendColumn(sb, column, count, option);
				count++;
			}

			if ((option.Filter & FilterOption.ByPrimaryKey) > 0) {
				var columns = t.PrimaryKeys;
				foreach (var column in t.GetPrimaryKeyColumns()) {
					AppendColumn(sb, column, count, option);
					count++;
				}
			}
			return new[] { this };
		}

		void AppendColumn(StringBuilder sb, Column c, int count, SqlCodeGenOption option) {
			sb.AppendLine().Tab();
			if (count > 0) { sb.Append("and "); }
			string variable = getVariableName.Get(c.Name);
			sb.EscapeName(c.Name).Append(" = ").Append(variable);
			createVariable.Store(this, c.GetVariable());
		}
	}
}

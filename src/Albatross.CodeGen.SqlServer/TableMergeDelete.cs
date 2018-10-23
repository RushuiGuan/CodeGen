﻿using Albatross.CodeGen.Database;
using Albatross.Database;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("table-merge-delete", GeneratorTarget.Sql, Category = GeneratorCategory.SQLServer, Description = "Merage statement delete clause")]
	public class TableMergeDelete : CodeGeneratorBase<Table, SqlCodeGenOption> {
		public override IEnumerable<object>  Build(StringBuilder sb, Table t, SqlCodeGenOption options) {
			sb.Append("when not matched by source then delete");
			return new[] { this, };
		}
	}
}

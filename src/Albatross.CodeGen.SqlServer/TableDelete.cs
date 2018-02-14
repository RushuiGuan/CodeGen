﻿using Albatross.CodeGen;
using Albatross.CodeGen.Database;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	[CodeGenerator("table_delete", "sql", Description = "Default delete statement")]
	public class TableDelete : TableQueryGenerator {
		public override IEnumerable<object> Build(StringBuilder sb, DatabaseObject t, SqlCodeGenOption options) {
			sb.Append($"delete from [{t.Schema}].[{t.Name}]");
			return new[] { this };
		}
	}
}

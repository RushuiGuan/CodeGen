﻿using Albatross.CodeGen.Database;
using System.Data.SqlClient;
using System.Text;

namespace Albatross.CodeGen.SqlServer {
	public static class Extension
    {
		public static string GetConnectionString(this Table table) {
			SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder {
				DataSource = table.Server.DataSource,
				InitialCatalog = table.Server.InitialCatalog,
				IntegratedSecurity = true
			};
			return builder.ToString();
		}

		public static Composite NewSqlTableComposite(string name, string description, params string[] children) {
			return new Composite {
				Name = name,
				Description = description,
				Category = "Sql Server",
				Generators = children,
				SourceType = typeof(Table),
				Target = "sql",
				Seperator = "\r\n",
			};
		}

		public static StringBuilder EscapeName(this StringBuilder sb, string name) {
			return sb.Append('[').Append(name).Append(']');
		}

		public static bool Match(this IBuiltInColumnFactory builtInColumnFactory, Column column) {
			return builtInColumnFactory.Get(column.Name).Match(column);
		}

		#region columns
		public static bool IsString(this Column c) {
			string dataType = c.DataType.ToLower();
			return dataType == "nvarchar" || dataType == "varchar" || dataType == "char" || dataType == "nchar";
		}
		public static bool IsDateTime(this Column c) {
			string dataType = c.DataType.ToLower();
			return dataType == "datetime" || dataType == "datetime2" || dataType == "smalldatetime";
		}
		public static bool IsBoolean(this Column c) {
			return c.DataType.ToLower() == "bit";
		}
		public static bool IsNumeric(this Column c) {
			string dataType = c.DataType.ToLower();
			return
				dataType == "tinyint"
				|| dataType == "smallint"
				|| dataType == "int"
				|| dataType == "bigint"

				|| dataType == "float"
				|| dataType == "real"
				|| dataType == "decimal"
				|| dataType == "numeric"

				|| dataType == "money"
				|| dataType == "smallmoney";
		}
		#endregion
	}
}
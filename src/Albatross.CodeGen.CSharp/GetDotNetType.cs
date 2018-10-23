﻿using System;
using System.Collections.Generic;
using System.Text;
using Albatross.CodeGen.CSharp.Core;
using Albatross.Database;

namespace Albatross.CodeGen.CSharp {
	public class GetDotNetType : IGetDotNetType {
		public DotNetType Get(SqlType sqlType) {
			DotNetType type;
			bool isValueType = false;
			switch (sqlType.Name.ToLower()) {
				case "bigint":
					isValueType = true;
					type = DotNetType.Long;
					break;

				case "tinyint":
					isValueType = true;
					type = DotNetType.Byte;
					break;

				case "int":
					isValueType = true;
					type = DotNetType.Integer;
					break;

				case "smallint":
					isValueType = true;
					type = DotNetType.Short;
					break;


				case "money": 
				case "smallmoney": 
				case "numeric": 
				case "decimal": 
					isValueType = true;
					type = DotNetType.Decimal;
					break;

				case "real":
					isValueType = true;
					type = DotNetType.Single;
					break;

				case "float":
					isValueType = true;
					type = DotNetType.Double;
					break;

				case "uniqueidentifier":
					isValueType = true;
					type = DotNetType.Guid;
					break;

				case "binary":
				case "varbinary":
					if (sqlType.MaxLength == 1) {
					isValueType = true;
						type = DotNetType.Byte;
					} else {
						type = DotNetType.ByteArray;
					}
					break;

				case "bit":
					isValueType = true;
					type = DotNetType.Boolean;
					break;

				case "date": 
				case "datetime": 
				case "datetime2":
				case "smalldatetime":
					isValueType = true;
					type = DotNetType.DateTime;
					break;

				case "datetimeoffset":
					isValueType = true;
					type = DotNetType.DateTimeOffset;
					break;

				case "time":
					isValueType = true;
					type = DotNetType.TimeSpan;
					break;

				case "nchar": 
				case "nvarchar": 
				case "char": 
				case "varchar":
					if(sqlType.MaxLength == 1) {
						type = DotNetType.Char;
					} else {
						type = DotNetType.String;
					}
					break;

				case "xml":
				case "text":
				case "ntext":
				case "sysname":
					type = DotNetType.String;
					break;

				case "sql_variant":
					type = DotNetType.Object;
					break;

				case "geography": 
				case "geometry": 
				case "hierarchyid": 
				case "image":
				case "timestamp":
					throw new NotSupportedException();
				default:
					throw new NotSupportedException();
			}

			if(isValueType && sqlType.IsNullable) {
				type = new DotNetType("Nullable") {
					IsGeneric = true,
					GenericTypes = new DotNetType[] { type },
				};
			}
			return type;
		}
	}
}

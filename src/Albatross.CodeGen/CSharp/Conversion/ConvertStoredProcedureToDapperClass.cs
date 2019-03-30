﻿using Albatross.CodeGen.Core;
using Albatross.CodeGen.CSharp.Model;
using Albatross.Database;
using System.Linq;

namespace Albatross.CodeGen.CSharp.Conversion {
	public class ConvertStoredProcedureToDapperClass : IConvertObject<Procedure, Class> {
		ConvertSqlTypeToDotNetType getDotNetType;

		public ConvertStoredProcedureToDapperClass(ConvertSqlTypeToDotNetType getDotNetType) {
			this.getDotNetType = getDotNetType;
		}

		public Class Convert(Procedure procedure) {
			Class @class = new Class {
				Name = procedure.Name.Proper(),
				AccessModifier = AccessModifier.Public,
				Imports = new string[] { "Dapper", "System.Data", },
				Dependencies = new Dependency[] {
					  new Dependency("dbConn") {
						   FieldType = DotNetType.IDbConnection,
						   Type = DotNetType.IDbConnection,
					  }
				 },
				Methods = new Method[] {
					GetCreateMethod(procedure),
				},
			};
			return @class;
		}

        object IConvertObject<Procedure>.Convert(Procedure from)
        {
            return this.Convert(from);
        }

        Method GetCreateMethod(Procedure procedure) {
			Method method = new Method("Create") {
				ReturnType = new DotNetType("CommandDefinition"),
				Variables = from sqlParam
							 in procedure.Parameters
							 select new Albatross.CodeGen.CSharp.Model.Variable(Extension.VariableName(sqlParam.Name)) {
								 Type = getDotNetType.Convert(sqlParam.Type),
							 },
			};

            method.Body = new CodeBlock { Content = "DynamicParameters dynamicParameters = new DynamicParameters();\nreturn new CommandDefinition(dbConnection,);" };
			return method;
		}
	}
}
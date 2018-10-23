﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Albatross.CodeGen.CSharp;
using Albatross.CodeGen.CSharp.Core;
using Albatross.Database;

namespace Albatross.CodeGen.SqlServer {
	/// <summary>
	/// Create a CSharp class from a SQL Stored procedure
	/// </summary>
	[CodeGenerator("procedure-to-proxy-class", GeneratorTarget.CSharp, Description = "Genearte a CSharp class from a database stored procedure object")]
	public class StoredProcedureProxy : CodeGeneratorBase<Procedure, Class> {
		IWriteObject<Class> writeClass;
		IGetDotNetType getDotNetType;

		public StoredProcedureProxy(IWriteObject<Class> writeClass, IGetDotNetType getDotNetType) {
			this.writeClass = writeClass;
			this.getDotNetType = getDotNetType;
		}

		public override IEnumerable<object> Build(StringBuilder sb, Procedure source, Class classOption) {

			if (string.IsNullOrEmpty(classOption.Name)) {
				classOption.Name = source.Name;
			}
			var method = new Method("CreateDefinition") {
				AccessModifier = AccessModifier.Public,
				ReturnType = new DotNetType("Dapper.CommandDefinition"),
				
			};

			method.Body.AppendLine("DynamicParameters dynamicParameters = new DynamicParameters();");
			method.Body.Append("return new CommandDefinition(dbConnection,);");

			classOption.Methods = new Method[] { method };
			sb.Write(writeClass, classOption);
			return new object[] { this };
		}
	}
}


/*
namespace Test {
	public class StoredProcedureName {
		IDbConnection dbConnection;
		public StoredProcedureName(IDbConnection dbConnection) {
			this.dbConnection = dbConnection;
		}
		public CommandDefinition GetDefinition(int id) {
			DynamicParameters dynamicParameters = new DynamicParameters();
			dynamicParameters.Add("type", @type, dbType: DbType.AnsiString);
			dynamicParameters.Add("name", @name, dbType: DbType.AnsiString);
			dynamicParameters.Add("title", @title, dbType: DbType.String);
			dynamicParameters.Add("description", @description, dbType: DbType.String);
			dynamicParameters.Add("definition", @definition, dbType: DbType.String);
			dynamicParameters.Add("system", @system, dbType: DbType.Boolean);
			dynamicParameters.Add("references", Db.GetServiceReference(@references));
			dynamicParameters.Add("user", @user, dbType: DbType.AnsiString);
			return new CommandDefinition(db, new CommandDefinition("[dyn].[CreateSvc]", dynamicParameters, commandType: CommandType.StoredProcedure, transaction: transaction));
		}
	}
}
*/

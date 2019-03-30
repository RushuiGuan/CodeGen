﻿using Albatross.Database;
using System.Management.Automation;

namespace Albatross.CodeGen.PowerShell {
	[Cmdlet(VerbsCommon.Get, "StoredProcedure")]
	public class GetStoredProcedure : BaseCmdlet<IListProcedure> {
		[Parameter(Position = 0)]
		public string Criteria { get; set; }

		[Parameter(Position = 1, Mandatory = true, ValueFromPipeline = true)]
		public Albatross.Database.Database Database { get; set; }

		protected override void ProcessRecord() {
			var items = Handle.Get(Database, Criteria);
			foreach(var item in items) {
				WriteObject(item);
			}
		}
	}
}
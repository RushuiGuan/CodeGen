﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen {
	public class Composite {
		public Composite() {
		}

		public Composite(Type srcType, Type optType) {
			this.SourceType = srcType;
			this.OptionType = optType;
		}

		public Type SourceType { get; set; }
		public Type OptionType { get; set; }
		public Branch Branch { get; set; }

		public string Name { get; set; }
		public string Category { get; set; }
		public string Target { get; set; }
		public string Description { get; set; }
	}
}
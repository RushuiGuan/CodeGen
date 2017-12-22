﻿using Albatross.CodeGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen {
	public class CompositeCodeGenerator : ICodeGenerator {
		IEnumerable<string> _generators;

		public CompositeCodeGenerator(Composite composite) {
			SourceType = composite.SourceType;
			_generators = composite.Generators;
			Name = composite.Name;
			Category = composite.Category;
			Description = composite.Description;
			Target = composite.Target;
			Seperator = composite.Seperator;
		}

		public string Name { get; private set; }
		public string Category { get; private set; }
		public string Description { get; private set; }
		public string Seperator { get; set; }
		public string Target { get; private set; }

		public Type SourceType { get; private set; }

		public StringBuilder Build(StringBuilder sb, object t, object options, ICodeGeneratorFactory factory) {
			if (t.GetType() == SourceType) {
				foreach (var item in _generators) {
					var gen = factory.Get(SourceType, item);
					gen.Build(sb, t, options, factory);
					if (item != _generators.Last()) {
						sb.Append(Seperator);
					}
				}
			}
			return sb;
		}
	}
}
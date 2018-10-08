﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen {
	public abstract class WriteScopedObject<T> : IWriteScopedObject<T> {
		protected StringBuilder Parent { get; private set; }
		public StringBuilder Content { get; } = new StringBuilder();

		public WriteScopedObject(StringBuilder parent) {
			Parent = parent;
		}

		public abstract IWriteScopedObject<T> BeginScope(T t);
		public abstract IWriteScopedObject<T> BeginChildScope(T t);
		public virtual void WriteContent() {
			string content = Content.ToString();
			if (!string.IsNullOrEmpty(content)) {
				Parent.Tabify(content, 1, true);
				Parent.AppendLine();
			}
		}
		public abstract void EndScope();

		public void Dispose() {
			WriteContent();
			EndScope();
		}
	}
}

﻿using Albatross.Logging.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.Shell.ViewModel {
	public class CompositeDetailViewModel : WorkspaceViewModel {
		public CompositeDetailViewModel(IWorkspaceService svc, ILogFactory logFactory) : base(svc, logFactory) { }

		public void Load(Composite c) {
			if (c == null) {
				c = new Composite {
					Name = "New",
				};
			}
			SetTitle(c);
		}

		void SetTitle(Composite c) {
			Title = "Composit:" + c.Name;
		}
	}
}
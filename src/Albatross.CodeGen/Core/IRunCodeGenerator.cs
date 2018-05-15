﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.CodeGen.Core
{
    public interface IRunCodeGenerator {
		IEnumerable<object> Run(CodeGenerator gen, StringBuilder sb, object source, ICodeGeneratorOption option);
	}
}

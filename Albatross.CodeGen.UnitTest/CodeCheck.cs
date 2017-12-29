﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Albatross.CodeGen.UnitTest {
	[TestFixture]
	public class CodeCheck {

		[TestCase(@"c:\temp\test.txt", ExpectedResult =@"c:\temp")]
		[TestCase(@"c:\temp\test*.txt", ExpectedResult = @"c:\temp")]
		public string PathCheck(string path) {
			return Path.GetDirectoryName(path);
		}


		[TestCase(@"c:\temp\test.txt", ExpectedResult = @"test.txt")]
		[TestCase(@"c:\temp\test*.txt", ExpectedResult = @"test*.txt")]
		public string FileCheck(string path) {
			return Path.GetFileName(path);
		}
	}
}
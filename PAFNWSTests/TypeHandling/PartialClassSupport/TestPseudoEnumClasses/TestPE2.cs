//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives

using System;
using PlatformAgileFramework.TypeHandling.PartialClassSupport;
#endregion

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.PartialClassSupport.Tests
{
	/// <summary>
	/// Test class implementing a PsuedoEnum.
	/// </summary>
	// ReSharper disable PartialTypeWithSinglePart
	// ReSharper disable once InconsistentNaming
	public sealed partial class TestPE2: ExtendablePseudoEnumInt32
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Set this bit to prevent writing. Overidden by "Replace"
		/// if the file does not exist.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly TestPE2 TESTPE2_VALUE1
			= new TestPE2("TESTPE2_VALUE1", 1, true);
		/// <summary>
		/// Scrolls to the end of a file if a file exists. Applicable to open
		/// operations. New files should be created if they do not exist
		/// and cursor left at the begining.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly TestPE2 TESTPE2_VALUE2
		= new TestPE2("TESTPE2_VALUE2", 4, true);
		/// <summary>
		/// Replaces a file if it already exists. Applicable to open
		/// operations.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static readonly TestPE2 TESTPE2_VALUE3
		= new TestPE2("TESTPE2_VALUE3", 16, true);
		#endregion // Class Fields And Autoproperties
		/// <remarks>
		/// See base.
		/// </remarks>
		public TestPE2(string name, int value)
			: base(name, value)
		{
		}
		/// <remarks>
		/// Handy optional constructor that can build from the results of
		/// base operations delivered on the components held by the interface.
		/// </remarks>
		public TestPE2(IExtendablePseudoEnumTypeType<int> peInterface)
			: base(peInterface.Name, peInterface.EnumValueAsGeneric)
		{
		}
		/// <remarks>
		/// See base.
		/// </remarks>
		internal TestPE2(string name, int value, bool addToDictonary)
			: base(name, value, addToDictonary)
		{
		}
	}
}

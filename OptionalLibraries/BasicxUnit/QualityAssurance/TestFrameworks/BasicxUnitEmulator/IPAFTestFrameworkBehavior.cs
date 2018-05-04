//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-


using System;
using PlatformAgileFramework.Manufacturing;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// <para>
	/// This interface provides descriptions for the various behaviors that a test framework
	/// may exhibit.
	/// </para>
	/// <para>
	/// Even if a given test framework does not natively exibit a given
	/// behavior, it may be forced to do so by setting one of the behavior parameters.
	/// For example, Junit initializes a test fixture for every test that is run.
	/// Nunit creates one class instance, calls any per-class setup method, then calls
	/// all test methods in alphabetical order, with their test setup and tearddown
	/// methods called before/after each, if present.
	/// </para>
	/// <para>
	/// The PAF Testing framework is an hierarchical framework. Each test entity
	/// (DLL, Class, method, etc.) generally inherits it's parameters from a parent.
	/// This interface represents parameters that are specified at the assembly level.
	/// </para>
	/// </summary>
	public interface IPAFTestFrameworkBehavior
	{
		/// <summary>
		/// This is the test framework behavior that is desired to be emulated.
		/// </summary>
		TestFrameworkBehaviorType FrameworkBehaviorType { get; set; }
		/// <summary>
		/// This is the test framework that is assumed to be used uniformly on all test
		/// fixtures in the test assembly.
		/// </summary>
		string FrameworkName { get; set; }
		/// <summary>
		/// This is the loader that is employed to load a DLL or EXE that contains test
		/// fixtures (classes).
		/// </summary>
		IPAFAssemblyLoader TestAssemblyLoader { get; set; }
	}
}

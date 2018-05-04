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

// Exception shorthand.
using PAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;


namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	#region Enumerations
	/// <summary>
	/// This is the type of the test framework behavior that is to be used for a given
	/// test fixture. Even though a test fixture may be implemented with the Nunit attributes
	/// and methods, it can have Junit behavior, for instance, where the fixture setup
	/// method is called before every test.
	/// </summary>
	public enum TestFrameworkBehaviorType
	{
		/// <summary>
		/// Indicates the default behavior for the test framework in use.
		/// </summary>
		None = 0,
		/// <summary>
		/// Indicates that Nunit behavior is emulated.
		/// </summary>
		Nunit = 1,
		/// <summary>
		/// Indicates that MS is emulated.
		/// </summary>
		MSTest = 2,
		/// <summary>
		/// Indicates that ReSharper tester is emulated.
		/// </summary>
		JetBrains = 3,
		/// <summary>
		/// Indicates that Junit is emulated.
		/// </summary>
		JUnit = 4
	}
	#endregion // Enumerations
}

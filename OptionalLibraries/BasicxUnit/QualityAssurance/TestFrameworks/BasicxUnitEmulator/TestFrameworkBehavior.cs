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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Properties;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Default implementation of the inteface for "BasicxUnit".
	/// </summary>
	/// <threadsafety>
	/// This class is NOT thread-safe.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 04aug2012 </date>
	/// <contribution>
	/// <para>
	/// Created interface implementation and added static helper methods to this class.
	/// </para>
	/// </contribution>
	/// </history>
	public class PAFTestFrameworkBehavior : IPAFTestFrameworkBehavior
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// See <see cref="IPAFTestFrameworkBehavior"/>.
		/// </summary>
		public TestFrameworkBehaviorType FrameworkBehaviorType { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFrameworkBehavior"/>.
		/// </summary>
		public string FrameworkName { get; set; }
		/// <summary>
		/// See <see cref="IPAFTestFrameworkBehavior"/>.
		/// </summary>
		public IPAFAssemblyLoader TestAssemblyLoader { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Methods
		/// <summary>
		/// Returns a <see cref="IPAFTestFrameworkBehavior"/> that is appropriate
		/// for uniform JUnit use and behavior. The only difference in these
		/// parameters is that test fixture setup and teardown methods are called
		/// before and after each test.
		/// </summary>
		/// <param name="testAssemblyLoader">
		/// An <see cref="IPAFAssemblyLoader"/> or <see langword="null"/> if not required.
		/// Default is <see cref="PAFAssemblyLoader"/>
		/// </param>
		/// <returns>The constructed <see cref="PAFTestFrameworkBehavior"/>.</returns>
		public static IPAFTestFrameworkBehavior GetStandardJUnitParams(IPAFAssemblyLoader testAssemblyLoader = null)
		{
			if(testAssemblyLoader == null) testAssemblyLoader = PAFAssemblyLoader.GetDefaultAssemblyLoader();
			var testFrameworkParams = GetStandardNUnitParams(testAssemblyLoader);
			testFrameworkParams.FrameworkBehaviorType = TestFrameworkBehaviorType.JUnit;
			return testFrameworkParams;
		}
		/// <summary>
		/// Returns a <see cref="IPAFTestFrameworkBehavior"/> that is appropriate
		/// for uniform NUnit use and behavior.
		/// </summary>
		/// <param name="testAssemblyLoader">
		/// An <see cref="IPAFAssemblyLoader"/> or <see langword="null"/> if not required.
		/// Default is <see cref="PAFAssemblyLoader"/>
		/// </param>
		/// <returns>The constructed <see cref="PAFTestFrameworkBehavior"/>.</returns>
		public static IPAFTestFrameworkBehavior GetStandardNUnitParams(IPAFAssemblyLoader testAssemblyLoader = null)
		{
			if (testAssemblyLoader == null) testAssemblyLoader = PAFAssemblyLoader.GetDefaultAssemblyLoader();
			var testFrameworkParams = new PAFTestFrameworkBehavior();
			testFrameworkParams.FrameworkName = TestFrameworkData.NUnitFramework;
			testFrameworkParams.FrameworkBehaviorType = TestFrameworkBehaviorType.Nunit;
			testFrameworkParams.TestAssemblyLoader = testAssemblyLoader;
			return testFrameworkParams;
		}
		/// <summary>
		/// Returns a <see cref="IPAFTestFrameworkBehavior"/> that is appropriate
		/// for uniform PAFUnit use and behavior. PAFUnit uses the same behavioral
		/// parameters as NUnit. Only the framework name is different.
		/// </summary>
		/// <param name="testAssemblyLoader">
		/// An <see cref="IPAFAssemblyLoader"/> or <see langword="null"/> if not required.
		/// Default is <see cref="PAFAssemblyLoader"/>
		/// </param>
		/// <returns>The constructed <see cref="PAFTestFrameworkBehavior"/>.</returns>
		[NotNull]
		public static IPAFTestFrameworkBehavior GetStandardPAFUnitParams(IPAFAssemblyLoader testAssemblyLoader = null)
		{
			if (testAssemblyLoader == null) testAssemblyLoader = PAFAssemblyLoader.GetDefaultAssemblyLoader();
			var testFrameworkParams = GetStandardNUnitParams(testAssemblyLoader);
			testFrameworkParams.FrameworkName = TestFrameworkData.PAFUnitFramework;
			return testFrameworkParams;
		}
		#endregion // Methods
	}
}

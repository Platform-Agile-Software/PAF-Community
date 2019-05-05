//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

using System;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.QualityAssurance.Exceptions;

namespace PlatformAgileFramework.QualityAssurance
{
	/// <summary>
	/// Basic utilities for unit testing. It is assumed that this class will be used
	/// by unit testers, thus many members are given internal visibility. It is assumed
	/// that the internals of this assembly will be made visible to any asssemblies
	/// containing test code needing to access members in this class.
	/// </summary>
	/// <history>
	/// <author> Bogas </author>
	/// <date> 04jan2012 </date>
	/// <contribution>
	/// Documented and cleaned up a bit.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// NOT thread-safe. It is assumed that testers will not do anything crazy like
	/// setting fields in the middle of a test on multiple threads.
	/// </threadsafety>
	/// <remarks>
	/// Most unit testing stuff is in the extended libraries. This little class contains
	/// utilities that anyone working with .Net should have to do platform-agile testing
	/// and testing in general.
	/// </remarks>
	public class QAUtils
	{
		#region Delegates
		/// <summary>
		/// This is the signature for a method that is called indirectly and will throw an
		/// exception in Nunit and MSTest. It is the only assert method that should ever
		/// be used when trying to develop test code that is independent of the test framework
		/// in use. Almost everybody's test framework has a method with this signature and
		/// if it doesn't, you only have to build ONE.
		/// </summary>
		protected internal delegate void AssertMethod(bool success, string description);
		#endregion // Delegates

		#region Class Fields and Autoproperties
		/// <summary>
		/// Common assert method that can be loaded by tester. If this is not <see langword="null"/>,
		/// it will be used instead of <see cref="s_AssertMethodInfo"/>.
		/// </summary>
		protected internal static AssertMethod s_AssertMethodDelegate;

		/// <summary>
		/// Common assert method info for "IsTrue" method in Nunuit and MStest. This is
		/// loaded with the first framework found in the assembly search. If neither
		/// framework is statically linked or loaded before this class is accessed in
		/// a given <see cref="AppDomain"/>, this is null.
		/// </summary>
		/// <remarks>
		/// The rationale for this approach is simplicity and ease of porting
		/// the functionality across platforms. That is why we don't attempt any
		/// dynamic extensions of test framework methods or any other such thing.
		/// </remarks>
		protected internal static MethodInfo s_AssertMethodInfo;

		/// <summary>
		/// This variable is an <see cref="AppDomain"/>-wide flag to indicate that
		/// an exception is to be thrown in finalizers. Normally, this will be set
		/// once in any <see cref="AppDomain"/>, but can be reset/unset for individual
		/// tests. It defaults to <see langword="false"/>, but is normally set to <see langword="true"/> for
		/// testing.
		/// </summary>
		protected internal static bool s_ExceptionOnForgetToDispose;

		/// <summary>
		/// Handy prefix for printouts of unit test results.
		/// </summary>
		public static readonly string s_TestFailurePrefix = "Failure on: ";

		/// <summary>
		/// Handy prefix for printouts of unit test results.
		/// </summary>
		public static readonly string s_TestSuccessPrefix = "Success on: ";
		#endregion // Class Fields and Autoproperties

		/// <summary>
		/// Calls <see cref="Initialize"/>.
		/// </summary>
		static QAUtils()
		{
			// Note this is currently hardwired for PAF.
			s_AssertMethodDelegate = Assert;
			// TODO KRM restore lookup if it makes sense. This would have to become a lazy singleton, probably.
			//Initialize();
		}
		/// <summary>
		/// This method will load the method info for the standard "Assert.IsTrue(...)"
		/// method for either NUnit or MSTest if it is linked in statically. NUnit is searched for
		/// first.
		/// </summary>
		protected internal static void Initialize()
		{
			if (s_AssertMethodInfo != null) return;
			Type assertClassType;
            var nunitAssy = ManufacturingUtils.Instance.GetAppDomainAssemblies()
				.Where(s => s.FullName.Contains("nunit.framework")).GetFirstElement();
			if (nunitAssy != null)
			{
                assertClassType = ManufacturingUtils.Instance.LocateReflectionTypeInAssembly(nunitAssy, "Assert");
				if (assertClassType != null)
				{
					s_AssertMethodInfo = assertClassType.GetRuntimeMethod("IsTrue", new[] {typeof (bool), typeof (string)});
					return;
				}
			}

            var msTestAssy = ManufacturingUtils.Instance.GetAppDomainAssemblies().
				Where(s => s.FullName.Contains("Microsoft.VisualStudio.TestTools.UnitTesting")).GetFirstElement();
			if (msTestAssy == null) return;
            assertClassType = ManufacturingUtils.Instance.LocateReflectionTypeInAssembly(msTestAssy, "Assert");
			if (assertClassType != null)
			{
				s_AssertMethodInfo = assertClassType.GetRuntimeMethod("IsTrue", new[] {typeof (bool), typeof (string)});
			}
		}

		/// <summary>
		/// Generates an assertion for the particular test framework in use.
		/// </summary>
		/// <param name="testInfo">
		/// Information about the test.
		/// </param>
		/// <param name="success">
		/// Whether the test was succussful. <see langword="false"/> generates an assertion with a
		/// message.
		/// </param>
		/// <param name="description">
		/// The test description that is printed with either a failure message or success
		/// message.
		/// </param>
		/// <param name="printOnSuccess">
		/// Prints at console on success if <see langword="true"/>. Useful for aggregating test cases inside
		/// a single method or reporting on intermediate stages in the test method.
		/// </param>
		/// <exceptions>
		/// This method is designed to throw exceptions of various types, depending on the
		/// test framework in use.
		/// </exceptions>
		// ReSharper disable once InconsistentNaming
		public static void xUnitAssertionService(object testInfo, bool success, string description,
												 bool printOnSuccess)
		{
			if (s_AssertMethodDelegate != null) {
				s_AssertMethodDelegate(success, description);
			}
			else if (s_AssertMethodInfo != null) {
				s_AssertMethodInfo.Invoke(null, new object[] { success, description });
			}
			else {
				if (!success) {
					// TODO krm replace with IAsserter when converted
					Console.WriteLine(description);
					throw new PAFxUnitAssertionException(description);
				}
			}
			// TODO krm replace with IAsserter when converted
			if (printOnSuccess) Console.WriteLine(s_TestSuccessPrefix + description);
		}
		/// <summary>
		/// Simple method that throws an exception with a description.
		/// </summary>
		/// <param name="success">
		/// <see langword="true"/> to throw a <see cref="PAFxUnitAssertionException"/>.
		/// </param>
		/// <param name="description"></param>
		public static void Assert(bool success, string description)
		{
			if (!success) {
				// TODO krm replace with IAsserter when converted
				Console.WriteLine(description);
				throw new PAFxUnitAssertionException(description);
			}	
		}
	}
}


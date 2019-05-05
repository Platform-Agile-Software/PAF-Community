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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders;
using PlatformAgileFramework.TypeHandling.MemberExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

// Exception shorthand.
using PAFTFIED = PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.PAFTestFixtureInitializationExceptionData;
using PAFAED = PlatformAgileFramework.ErrorAndException.PAFAggregateExceptionData;
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
using PAFTEMT = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionMessageTags;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// This is a new helper class for fixtures made during the .Net standard conversion.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03nov2017 </date>
	/// <desription>
	/// New.
	/// </desription>
	/// </contribution>
	/// </history>
	public static class PAFTestFixtureInfoExtensions
	{
		/// <summary>
		/// Assigns a default test method enumerator if not has been loaded. The two choices
		/// are an alphabetizing enumerator or an enumerator that just spits out methods
		/// in the order they are gathered off the type. NUnit style is alphabetizing. This
		/// is also used for PAFUnit framework when NUnit behavior is utilized. Both enumerators
		/// "one-shot" types.
		/// </summary>
		/// <param name="testFixtureInfo">
		/// Incoming test fixture info which is to have its enumerator set.
		/// </param>
		/// <remarks>
		/// It is currently our belief that JUnit and MS do not alphabetize. If we're wrong,
		/// please let us know. This class is really just a handy little helper to demonstrate
		/// how to attach a simple enumerator, anyway.
		/// </remarks>
		public static void AssignDefaultEnumerator(IPAFTestFixtureInfo testFixtureInfo)
		{
			if (testFixtureInfo.GetTestElementInfoEnumerableProviderProvider<IPAFTestMethodInfo>() == null)
			{
				// Explicit NUnit behavior type or NUnit in use with no explicit behavior.
				if ((testFixtureInfo.FrameworkBehavior.FrameworkBehaviorType == TestFrameworkBehaviorType.Nunit)
					|| ((testFixtureInfo.FrameworkBehavior.FrameworkName == TestFrameworkData.NUnitFramework)
						&& (testFixtureInfo.FrameworkBehavior.FrameworkBehaviorType == 0))
					)
				{
					var alphabetizingEnumerableProvider
						= new AlphabetizingTestEnumerableProvider<IPAFTestMethodInfo>(testFixtureInfo.GetActiveTestMethods());
					testFixtureInfo.SetProvider(alphabetizingEnumerableProvider);
					return;
				}
				// It is currently our belief that MS does not alphabetize. If we're wrong,
				// please let us know.
				var enumerableProvider = new OneShotArrayEnumerableProvider<IPAFTestMethodInfo>(testFixtureInfo.GetActiveTestMethods());
				testFixtureInfo.SetProvider(enumerableProvider);
			}
		}

		/// <summary>
		/// Just converts an array to it's base.
		/// </summary>
		/// <param name="infos">
		/// test fixture infos.
		/// </param>
		/// <returns>
		/// test element infos.
		/// </returns>
		public static IEnumerable<IPAFTestElementInfo> ConvertToElements(this IEnumerable<IPAFTestFixtureInfo> infos)
		{
			if (infos == null) return null;
			var col = new Collection<IPAFTestElementInfo>();
			foreach (var info in infos) {
				col.Add(info);
			}
			return col;
		}
	    /// <summary>
	    /// Utility method constructs an instance of the text fixture type if the instance
	    /// is not present on the fixture info already.
	    /// </summary>
	    /// <param name="fixtureInfo">
	    /// A <see cref="IPAFTestFixtureInfo"/> which should have already been checked for
	    /// vailidity.
	    /// </param>
	    /// <exception cref="ArgumentNullException">
	    /// <paramref name="fixtureInfo"/> is <see langword="null"/>.
	    /// </exception>
	    /// <returns>
	    /// A <see cref=" PAFStandardException{T}"/> wrapping any exceptions that have
	    /// occurred during instantiation of the type. If the fixture has already been constructed,
	    /// <see langword="null"/> is returned.
	    /// </returns>
	    public static Exception PAFConstructTestFixtureInstance(this IPAFTestFixtureInfo fixtureInfo)
	    {
	        if (fixtureInfo == null) throw new ArgumentNullException(nameof(fixtureInfo));
	        try
	        {
	            // Already constructed?
	            if (fixtureInfo.FixtureInstance != null) return null;
	            fixtureInfo.FixtureInstance = fixtureInfo.FixtureConstructor.Invoke(new object[] { });
	        }
	        catch (Exception ex)
	        {
	            var data = new PAFTFIED(fixtureInfo.FixtureType);
	            return new PAFStandardException<PAFTFIED>(data,
                    PAFTestFixtureInitiaizationExceptionMessageTags.FIXTURE_INSTANTIATION_FAILURE, ex);
	        }
	        return null;
	    }
        /// <summary>
        /// Returns an aggregate of exceptions that characterize an analysis of a potential
        /// test fixture type.
        /// </summary>
        /// <param name="fixtureInfo">
        /// <see cref="IPAFTestFixtureInfo"/> fixture.
        /// </param>
        /// <param name="frameworkBehavior">
        /// The framework we are emulating.
        /// </param>
        /// <returns>
        /// An aggregate exception containing problems found. <see langword="null"/> if
        /// all is well.
        /// </returns>
        /// <exceptions>
        /// <exception>
        /// <see cref="PAFTypeExceptionMessageTags.ERROR_RESOLVING_TYPE"/> if fixture
        /// <see cref="Type"/> is not loaded.
        /// </exception>
        /// <exception>
        /// <see cref="PAFTypeExceptionMessageTags.ERROR_RESOLVING_TYPE"/> if fixture
        /// <see cref="Type"/> is not loaded.
        /// </exception>
        /// </exceptions>
        public static PAFStandardException<PAFAED> CheckFixtureType
            (IPAFTestFixtureInfo fixtureInfo, IPAFTestFrameworkBehavior frameworkBehavior)
        {
            var exceptionList = new List<Exception>();
            var badFixtureExceptionString = "Test Class: " + fixtureInfo.FixtureType + " ";

            var fixtureTypeType = fixtureInfo.FixtureType.TypeType;

            if (fixtureTypeType == null)
            {
                var data = new PAFTED(fixtureInfo.FixtureType, badFixtureExceptionString);
                exceptionList.Add(new PAFStandardException<PAFTED>(data, PAFTEMT.ERROR_RESOLVING_TYPE));
                return new PAFStandardException<PAFAED>(new PAFAED(exceptionList));
            }               

            if (fixtureTypeType.GetInstanceConstructor() == null)
            {
	            var message = badFixtureExceptionString + PAFTestFixtureInitiaizationExceptionMessageTags.TYPE_HAS_NO_DEFAULT_CONSTRUCTOR;
				var data = new PAFTFIED(fixtureInfo.FixtureType);
                exceptionList.Add(new PAFStandardException<PAFTFIED>(data, message));
                return new PAFStandardException<PAFAED>(new PAFAED(exceptionList));
            }
            if (!fixtureTypeType.PAFGetTypeInfo().DoesMemberHavePublicNamedAttributeInfo(
                TestFrameworkData.TestClassAttributeName(frameworkBehavior.FrameworkName)))
            {
	            var message = badFixtureExceptionString + PAFTestFixtureInitiaizationExceptionMessageTags.TYPE_NOT_A_FIXTURE;
				var data = new PAFTFIED(fixtureInfo.FixtureType);
                exceptionList.Add(new PAFStandardException<PAFTFIED>(data, message));
                return new PAFStandardException<PAFAED>(new PAFAED(exceptionList));
            }
            if (exceptionList.Count == 0) return null;
            var aggregateException = new PAFStandardException<PAFAED>(new PAFAED(exceptionList));
            return aggregateException;
        }
		/// <summary>
		/// Returns the methods contained in the incoming methods that are
		/// attributed as test methods.
		/// </summary>
		/// <param name="fixtureInfo">
		/// <see cref="IPAFTestFixtureInfo"/> fixture.
		/// </param>
		/// <param name="methodInfos">
		/// Collection of all methods on a fixture class.
		/// </param>
		/// <returns>
		/// List of test methods - never <see langword="null"/> 
		/// </returns>
		/// <exceptions>
		/// <exception>
		/// <see cref="PAFTypeExceptionMessageTags.ERROR_RESOLVING_TYPE"/> if fixture
		/// <see cref="Type"/> is not loaded.
		/// </exception>
		/// <exception>
		/// <see cref="PAFTypeExceptionMessageTags.ERROR_RESOLVING_TYPE"/> if fixture
		/// <see cref="Type"/> is not loaded.
		/// </exception>
		/// </exceptions>
		public static IList<MethodInfo> GatherTestMethodsOnFixture
			(IPAFTestFixtureInfo fixtureInfo, IEnumerable<MethodInfo> methodInfos)
		{
			// Now gather up all the test methods.
			var infos = methodInfos.GetMembersWithPublicNamedAttributeInfo(
				TestFrameworkData.TestMethodAttributeName(fixtureInfo.FrameworkBehavior.FrameworkName)).BuildIList();

			return infos.EnumIntoSubtypeList<MemberInfo, MethodInfo>();
		}
	}
}

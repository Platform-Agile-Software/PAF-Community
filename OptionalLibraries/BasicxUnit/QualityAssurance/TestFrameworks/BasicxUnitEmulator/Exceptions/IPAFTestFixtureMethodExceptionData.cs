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

using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Exceptions
{
	/// <summary>
	///	Exceptions that occur during operation of test fixture and harnesses.
	/// </summary>
	/// <remarks>
	/// Always use distinguishable names in interfaces members that will be
	/// aggregated.
	/// </remarks>
	public interface IPAFTestFixtureMethodExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Representation of the fixture type we had a problem with. In case of
		/// a Goshaloma harness, it is the harness type.
		/// </summary>
		IPAFTypeHolder TestFixtureType { get; }
		/// <summary>
		/// Method name.
		/// </summary>
		string TestFixtureMethodName { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTestFixtureMethodExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTestFixtureMethodExceptionData>
    {
        #region Fields and Autoproperties
        #region Standard Attributed Test Method Failures
        /// <summary>
        /// Error message.
        /// </summary>
        public const string FIXTURE_DISPOSAL_FAILURE = "Fixture disposal failure";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string FIXTURE_SETUP_FAILURE = "Fixture setup failure";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string FIXTURE_TEARDOWN_FAILURE = "Fixture teardown failure";
        /// <summary>
        /// Error message. Only for Goshaloma harnesses.
        /// </summary>
        public const string HARNESS_SETUP_FAILURE = "Harness setup failure";
        /// <summary>
        /// Error message. Only for Goshaloma harnesses.
        /// </summary>
        public const string HARNESS_TEARDOWN_FAILURE = "Harness teardown failure";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TEST_METHOD_FAILURE = "Test method failure";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TEST_SETUP_FAILURE = "Test setup failure";
        /// <summary>
        /// Error message.
        /// </summary>
        public const string TEST_TEARDOWN_FAILURE = "Test teardown failure";
        #endregion // Standard Attributed Test Method Failures
        #region Miscellaneous Failures
        /// <summary>
        /// Error message. This is designed to pinpoint a problem in the test runner
        /// delegate methods. These are the standard looping methods supported in core
        /// that generally simply run attributed fixture methods under some protocol
        /// or order.
        /// </summary>
        public const string TEST_RUNNER_FAILURE = "Test runner failure";
        #endregion // Miscellaneous Failures
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTestFixtureMethodExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                FIXTURE_DISPOSAL_FAILURE,
                FIXTURE_SETUP_FAILURE,
                FIXTURE_TEARDOWN_FAILURE,
                HARNESS_SETUP_FAILURE,
                HARNESS_TEARDOWN_FAILURE,
                TEST_METHOD_FAILURE,
                TEST_RUNNER_FAILURE,
                TEST_SETUP_FAILURE,
                TEST_TEARDOWN_FAILURE
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFTestFixtureMethodExceptionData));
        }

    }

}
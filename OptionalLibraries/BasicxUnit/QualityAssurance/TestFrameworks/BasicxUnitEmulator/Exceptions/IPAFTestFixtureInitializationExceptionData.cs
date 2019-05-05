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
	///	Exceptions that occur during initialization of test fixture and harnesses.
	/// These exceptions can be generated during other scenarios, but are designed
	/// to indicate a flaw in the fixture's state or an error preparing the fixture
	/// for a run. They are not designed to report problems during actual running of
	/// a fixture's methods. Use <see cref="PAFTestFixtureMethodExceptionData"/>
	/// for this purpose, instead.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21dec2017 </date>
	/// <description>
	/// New. For reconstituted BasicxUnit.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Always use distinguishable names in interface members that will be
	/// aggregated.
	/// </remarks>
	public interface IPAFTestFixtureInitializationExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Representation of the fixture type we had a problem with. In case of
		/// a Goshaloma harness, it is the harness type.
		/// </summary>
		IPAFTypeHolder TestFixtureTypeInitializing { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTestFixtureInitiaizationExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTestFixtureInitializationExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. This indicates a failure when initializing a test fixture. It
        /// may wrap more specific exceptions. It is thrown when examining or attepting to
        /// construct the test fixture instance itself.
        /// </summary>
        public const string FIXTURE_INITIALIZATION_FAILURE = "Fixture initialization failure.";
        /// <summary>
        /// Error message. this error occurs after a fixture's type has been indentified
        /// as valid (contains proper attributes, etc.) and a proper constructor had been
        /// found/provided, but the construction of the fixture failed.
        /// </summary>
        public const string FIXTURE_INSTANTIATION_FAILURE = "Fixture instantiation failure.";
        /// <summary>
        /// Error message. This indicates a failure when initializing a test fixture wrapper.
        /// This is an exception that is GENERALLY thrown in the
        /// <c>ITestFixtureWrapper.InitializeExePipeline</c>. It may wrap more specific
        /// exceptions.
        /// </summary>
        public const string FIXTURE_WRAPPER_INITIALIZATION_FAILURE = "Fixture wrapper initialization failure.";
        /// <summary>
        /// Error message. Valid fixture must have test methods.
        /// </summary>
        public const string NO_TEST_METHODS_FOUND_ON_FIXTURE = "No test methods found on fixture.";
        /// <summary>
        /// Error message. Valid fixture must have proper attribute.
        /// </summary>
        public const string TYPE_NOT_A_FIXTURE = "Type not a fixture.";
        /// <summary>
        /// Error message. Fixture must have default constructor under the current framework emulation.
        /// </summary>
        public const string TYPE_HAS_NO_DEFAULT_CONSTRUCTOR = "Type has no default constructor.";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTestFixtureInitiaizationExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                FIXTURE_INITIALIZATION_FAILURE,
                FIXTURE_INSTANTIATION_FAILURE,
                FIXTURE_WRAPPER_INITIALIZATION_FAILURE,
                NO_TEST_METHODS_FOUND_ON_FIXTURE,
                TYPE_NOT_A_FIXTURE,
                TYPE_HAS_NO_DEFAULT_CONSTRUCTOR
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTestFixtureInitializationExceptionData));
        }

    }
}
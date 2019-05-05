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
	///	Exceptions that occur during initialization of test assembly.
	/// </summary>
	/// <remarks>
	/// Always use distinguishable names in interface members that will be
	/// aggregated.
	/// </remarks>
	public interface IPAFTestAssemblyInitializationExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// Representation of the assembly we had a problem with.
		/// </summary>
		IPAFAssemblyHolder TestAssemblyInitializing { get; }
		#endregion // Properties
	}

    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTestAssemblyInitializationExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTestAssemblyInitializationExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Error message. Valid test assembly must have test fixtures.
        /// </summary>
        public const string NO_TEST_FIXTURES_FOUND_IN_ASSEMBLY = "No test fixtures found in assembly.";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFTestAssemblyInitializationExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                NO_TEST_FIXTURES_FOUND_IN_ASSEMBLY
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFTestAssemblyInitializationExceptionData));
        }

    }

}
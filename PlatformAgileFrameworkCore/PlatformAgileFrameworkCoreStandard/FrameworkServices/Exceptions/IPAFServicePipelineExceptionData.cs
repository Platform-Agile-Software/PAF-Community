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
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{

	/// <summary>
	///	Exceptions that occur when executing the service "pipeline" associated with
	/// service construction or teardown. An example is a problem resolving the
	/// order of execution of either load or initialization phases of a set of
	/// services supported by a given SM. Use <see cref="IPAFServiceExceptionData"/>
	/// to extract the name of the service manager - also a service.
	/// </summary>
	[PAFSerializable]
	public interface IPAFServicePipelineExceptionData
		: IPAFServiceExceptionData
	{
		#region Properties
		/// <summary>
		/// Set of services that the manager had a problem with.
		/// </summary>
		IEnumerable<IPAFService> ProblematicServices { get; }
		/// <summary>
		/// Stage in the pipeline that we had the problem in. This will return
		/// <see langword="null"/> if the problem is not related to any particular
		/// stage. 
		/// </summary>
		ServicePipelineStage PipelineStage { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFServicePipelineExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFServicePipelineExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// <para>
        /// Error message. Used when the scheduling procedure cannot break
        /// service dependency deadlocks and the pipeline methods associated
        /// with a given stage cannot be completed. This error really does not
        /// make sense without a <see cref="ServicePipelineStage"/> loaded. 
        /// </para>
        /// <para>
        /// In this case, the
        /// <see cref="IPAFServiceExceptionData.ProblematicService"/> is
        /// loaded with the type of the service manager (also a service) that
        /// encountered the deadlock.
        /// </para>
        /// </summary>
        public const string SERVICE_DEADLOCK_IN_STAGE
            = "Service deadlock in stage";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFServicePipelineExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
                SERVICE_DEADLOCK_IN_STAGE
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags,
                typeof(IPAFServicePipelineExceptionData));
        }

    }
}
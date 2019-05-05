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

using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Notification.Exceptions
{
	/// <summary>
	///	Exceptions that occur handling delegates and pseudodelegates.
	/// </summary>
	[PAFSerializable]
	public interface IPAFDelegateExceptionData : IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The type of the problematic delegate or pseudodelegate.
		/// </summary>
		IPAFTypeHolder ProblematicDelegateType { get; }
		#endregion // Properties
	}

	/// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFDelegateExceptionMessageTags
		: PAFExceptionMessageTagsBase<IPAFDelegateExceptionData>
    {
		#region Fields and Autoproperties
		/// <summary>
	    /// Error message. Usually results from trying to subscribe
	    /// to a pseudodelegate store with a delegate, which itself
	    /// has subscribers other than the method it was built with.
	    /// </summary>
	    public const string DELEGATE_HAS_SUBSCRIBERS = "Delegate has subscribers";
	    /// <summary>
	    /// Error message. This occurs when a method of certain name
	    /// cannot be found on a type. This condition frequently happens when
	    /// instance/static methods are confused. Searches are often specific
	    /// for instance/static. 
	    /// </summary>
	    public const string NO_NAMED_METHOD_FOUND_ON_TYPE = "No named method found on type";
	    /// <summary>
	    /// Error message. This occurs when the creation of a static delegate is
	    /// requested with an instance method or vice-versa.
	    /// </summary>
	    public const string STATIC_INSTANCE_MISMATCH = "Static/Instance mismatch";
		/// <summary>
	    /// Error message. This occurs when a start method has not been called
	    /// prior to accessing a subscriber store.
	    /// </summary>
	    public const string SUBSCRIBER_STORE_HAS_NOT_BEEN_STARTED = "Subscriber store has not been started";
        #endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFDelegateExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
					DELEGATE_HAS_SUBSCRIBERS,
					NO_NAMED_METHOD_FOUND_ON_TYPE,
					STATIC_INSTANCE_MISMATCH,
					SUBSCRIBER_STORE_HAS_NOT_BEEN_STARTED
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal
	            (s_Tags, typeof(IPAFDelegateExceptionData));
        }
    }
}
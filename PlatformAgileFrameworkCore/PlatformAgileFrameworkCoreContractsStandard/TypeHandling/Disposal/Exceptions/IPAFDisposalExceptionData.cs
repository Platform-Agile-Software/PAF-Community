//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
namespace PlatformAgileFramework.TypeHandling.Disposal.Exceptions
{
	/// <summary>
	///	Exceptions that occur during disposal operations.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 08mar2019 </date>
	/// <description>
	/// New. Cleaning up and enhancing disposal capabilities for Golea.
	/// </description>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public interface IPAFDisposalExceptionData: IPAFTypeExceptionData
	{
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFDisposalExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFDisposalExceptionData>
    {
        #region Fields and Autoproperties
        /// <summary>
        /// Occurs when the disposal registry gets an exception disposing a type or an instance.
        /// </summary>
        public const string ERROR_DISPOSING_TYPE = "Error Disposing Type";
        #endregion // Fields and Autoproperties
        /// <summary>
        /// Just puts the tags in a list to hand out.
        /// </summary>
        static PAFDisposalExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
					ERROR_DISPOSING_TYPE
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFDisposalExceptionData));
        }

    }
}
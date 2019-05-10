//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

namespace PlatformAgileFramework.TypeHandling.Exceptions
{
	/// <summary>
	///	Exceptions that occur handling Generic types. Thrown because the type
	/// is not right somehow. Has a stringful payload, since often the types
	/// cannot be found or loaded.
	/// </summary>
	[PAFSerializable]
	public interface IPAFGenericTypeHandlingExceptionData
		:IPAFStandardExceptionData
	{
		#region Properties
		/// <summary>
		/// The problematic type that is either Generic or non-Generic when
		/// it should be or is malformed or null/empty.
		/// </summary>
		/// <remarks>
		/// String may be <see langword="null"/> or <see cref="string.Empty"/>.
		/// </remarks>
		/// <history>
		/// <contribution>
		/// <author> KRM </author>
		/// <date> 04feb2019 </date>
		/// <description>
		/// New. For manufacturing Generic types
		/// </description>
		/// </contribution>
		/// </history>

		string ProblematicTypeString { get; }
		#endregion // Properties
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFGenericTypeHandlingExceptionMessageTags
		: PAFExceptionMessageTagsBase<IPAFGenericTypeHandlingExceptionData>
    {
		#region Fields and Autoproperties
	    /// <summary>
	    /// Sometimes thrown when a type is a Generic.
	    /// </summary>
	    public const string EXPECTING_NON_GENERIC_TYPE = "Expecting non-Generic Type";
	    /// <summary>
	    /// Sometimes thrown when a type is a non - Generic.
	    /// </summary>
	    public const string EXPECTING_GENERIC_TYPE = "Expecting Generic Type";
		/// <summary>
		/// Thrown when a type string can't be processed.
		/// </summary>
		public const string BADLY_FORMED_TYPE_STRING = "Badly formed type string";
        #endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFGenericTypeHandlingExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
                {
                    EXPECTING_NON_GENERIC_TYPE
                };
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFGenericTypeHandlingExceptionData));
        }

    }


}
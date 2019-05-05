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

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Exceptions
{
	/// <summary>
	/// Just a signature for catching.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Needed for console UI.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFTestResultNavigationExceptionData : IPAFStandardExceptionData
	{
	}
    /// <summary>
    /// Set of tags with an enumerator for exception messages. These are the dictionary keys
    /// for extended.
    /// </summary>
    public class PAFTestResultNavigationExceptionMessageTags
        : PAFExceptionMessageTagsBase<IPAFTestResultNavigationExceptionData>
    {
		#region Fields and Autoproperties
		/// <summary>
	    /// Error message. This indicates that a navigation "UP" was attempted
	    /// when at the "root" node.
	    /// </summary>
	    public const string NODE_IS_ROOT_NODE = "Node is root node";
		/// <summary>
	    /// Error message. This indicates that a navigation "DN" was attempted
	    /// when at a "leaf" node.
	    /// </summary>
	    public const string NODE_IS_LEAF_NODE = "Node is leaf node";
	    /// <summary>
	    /// Error message. This indicates that a navigation "LF" was attempted
	    /// when at first child node.
	    /// </summary>
	    public const string NODE_IS_FIRST_CHILD = "Node is first child";
	    /// <summary>
	    /// Error message. This indicates that a navigation "RT" was attempted
	    /// when at last child node.
	    /// </summary>
	    public const string NODE_IS_LAST_CHILD = "Node is last child";
        /// <summary>
        /// Error message. This indicates that a navigation was attepted
        /// off the beginning or end of child node list.
        /// </summary>
        public const string NODE_IS_OUT_OF_RANGE = "Node is out of range";
        /// <summary>
        /// Error message. This indicates that an incorrect detail level was specified..
        /// </summary>
        public const string DETAIL_LEVEL_IS_0_1_2 = "Detail level is 0, 1 or 2";
        #endregion // Fields and Autoproperties
		/// <summary>
		/// Just puts the tags in a list to hand out.
		/// </summary>
		static PAFTestResultNavigationExceptionMessageTags()
        {
            if ((s_Tags != null) && (s_Tags.Count > 0)) return;
            s_Tags = new List<string>
            {
				NODE_IS_ROOT_NODE,
	            NODE_IS_FIRST_CHILD,
				NODE_IS_LAST_CHILD,
				NODE_IS_LAST_CHILD,
				NODE_IS_OUT_OF_RANGE,
                DETAIL_LEVEL_IS_0_1_2
            };
            // Always store by the interface for an aggregable exception.
            PAFAbstractStandardExceptionDataBase.RegisterNamedExceptionTagsInternal(s_Tags, typeof(IPAFTestResultNavigationExceptionData));
        }

    }
}
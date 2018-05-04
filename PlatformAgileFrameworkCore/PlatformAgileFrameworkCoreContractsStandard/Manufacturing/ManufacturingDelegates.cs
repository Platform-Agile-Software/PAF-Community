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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

// ReSharper disable once RedundantUsingDirective
using System;

namespace PlatformAgileFramework.Manufacturing
{
    /// <summary>
    /// Set of delegates used in manufacturing.
    /// </summary>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 04nov2017 </date>
    /// <desription>
    /// <para>
    /// New - needed to split out delegates for new assembly loading mechanism.
    /// </para>
    /// </desription>
    /// </contribution>
    // ReSharper disable PartialTypeWithSinglePart
    public partial class ManufacturingDelegates
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Delegates
		/// <summary>
		/// A simple delegate that constructs an object from an argument.
		/// </summary>
		/// <param name="constructionArgument">
		/// The argument for construction. Can be <see langword="null"/> depending on usage.
		/// </param>
		/// <returns>
		/// The constructed object.
		/// </returns>
		public delegate object ObjectConstructionDelegate(object constructionArgument);
		/// <summary>
		/// A simple delegate that constructs a type from an argument.
		/// </summary>
		/// <param name="constructionArgument">
		/// The argument for construction. Can be <see langword="null"/> depending on usage.
		/// </param>
		/// <typeparam name="T">
		/// The reference type that is to be constructed.
		/// </typeparam>
		/// <returns>
		/// The constructed type.
		/// </returns>
		public delegate T ReferenceTypeConstructionDelegate<out T>(object constructionArgument) where T : class;
		/// <summary>
		/// A simple delegate that constructs a type from an argument.
		/// </summary>
		/// <param name="constructionArgument">
		/// The argument for construction. Can be <see langword="null"/> depending on usage.
		/// </param>
		/// <typeparam name="T">
		/// The reference type that is to be constructed.
		/// </typeparam>
		/// <typeparam name="U">
		/// The parameters that are needed to construct the type.
		/// </typeparam>
		/// <returns>
		/// The constructed type.
		/// </returns>
		public delegate T ReferenceTypeConstructionDelegate<out T, in U>(U constructionArgument) where T : class;
        #endregion // Delegates
        #region Methods
        /// <summary>
        /// This utility method decides whether a name is in fully-qualified format
        /// such as "System.Drawing.Size" and separates it into name
        /// (<code>Size</code>) and namespace (<code>System.Drawing</code>) if it is.
        /// </summary>
        /// <param name="fullyQualifiedTypeName">
        /// Fully qualified name that would be of the format fetched with Type.FullName.
        /// </param>
        /// <param name="unqualifiedName">
        /// This out parameter receives the unqualified name if the incoming name
        /// was properly formed or receives the input string back if it is not
        /// <see langword="null"/> or <see cref="String.Empty"/>. In this case, the input
        /// is assumed to contain the unqualified name alone (no dots in the string).
        /// </param>
        /// <param name="nameSpace">
        /// This out parameter receives the namespace if the incoming name was properly
        /// formed.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if the <paramref name="fullyQualifiedTypeName"/> was properly
        /// formed and <see langword="false"/> otherwise.
        /// </returns>
        public static bool GetTypeAndNamespace(string fullyQualifiedTypeName,
            ref string unqualifiedName, ref string nameSpace)
        {
            // If we've got a name set the name fields.
            if (string.IsNullOrEmpty(fullyQualifiedTypeName))
                return false;
            // If it's here, its gotta' be the correct format.
            var index = fullyQualifiedTypeName.LastIndexOf(".", StringComparison.Ordinal);
            // It's gotta' be here and not at beginning or end.
            if ((index <= 0) || (index == fullyQualifiedTypeName.Length - 1))
            {
                if (index < 0)
                    unqualifiedName = fullyQualifiedTypeName;
                return false;
            }
            // If we got this far, we can pull it apart.
            nameSpace = fullyQualifiedTypeName.Substring(0, index);
            unqualifiedName = fullyQualifiedTypeName.Substring(index + 1);
            return true;
        }
	    #endregion // Methods

    }
}

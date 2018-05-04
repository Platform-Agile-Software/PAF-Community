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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
using System;
using System.Collections;
using System.IO;
using System.Xml.Linq;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;

namespace PlatformAgileFramework.XML.Linq
{
	/// <summary>
	/// <para>
	/// This is a set of utilities for LinqToXml objects.
	/// </para>
	/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
	// Core
	public static partial class PAFLinqToXMLUtils
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Attribute containing the <see cref="XMLUtils.XML_PREFIX_NAMESPACE"/>
		/// </summary>
		public static readonly XAttribute XmlNamespaceDeclarationAttribute
			= new XAttribute(XNamespace.Xmlns.GetName("xml"), XMLUtils.XML_PREFIX_NAMESPACE);
		/// <summary>
		/// Attribute containing the <see cref="XMLUtils.XMLNS_PREFIX_NAMESPACE"/>
		/// </summary>
		public static readonly XAttribute XmlnsNamespaceDeclarationAttribute
			= new XAttribute(XNamespace.Xmlns.GetName("xmlns"), XMLUtils.XMLNS_PREFIX_NAMESPACE);
		#endregion // Class Fields and Autoproperties
		///////////////////////////////////////////////////////////////////////
		// The following are built NOT as extension methods to avoid polluting
		// the extension space with generic "object" extensions.
		///////////////////////////////////////////////////////////////////////
		#region Object Object Pseudo-Extensions
		/// <summary>
		/// Returns <see langword="true"/> if type of the object is 
		/// <see cref="IEnumerable"/>, <see cref="XCData"/>,  <see cref="XText"/>,
		/// <see cref="XElement"/>, <see cref="XAttribute"/>,
		/// <see cref="XProcessingInstruction"/>, <see cref="XComment"/>
		/// or <see langword="null"/>.
		/// </summary>
		/// <param name="objectToCheck">
		/// The incoming object to evaluate.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if object is correct type.
		/// </returns>
		public static bool IsXPathContentType(object objectToCheck)
		{
			if (objectToCheck is IEnumerable) return true;
			if (objectToCheck is XCData) return true;
			if (objectToCheck is XText) return true;
			if (objectToCheck is XElement) return true;
			if (objectToCheck is XAttribute) return true;
			if (objectToCheck is XProcessingInstruction) return true;
			if (objectToCheck is XComment) return true;
			if (objectToCheck == null) return true;
			return false;
		}
		/// <summary>
		/// Returns <see langword="true"/> if type of the object is 
		/// <see cref="XCData"/>,  <see cref="XText"/>,
		/// <see cref="XElement"/>, <see cref="XAttribute"/>,
		/// <see cref="XProcessingInstruction"/> or <see cref="XComment"/>.
		/// </summary>
		/// <param name="objectToCheck">
		/// The incoming object to evaluate.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if object is correct type.
		/// </returns>
		public static bool IsStrictXPathContentType(object objectToCheck)
		{
			if (objectToCheck is XCData) return true;
			if (objectToCheck is XText) return true;
			if (objectToCheck is XElement) return true;
			if (objectToCheck is XAttribute) return true;
			if (objectToCheck is XProcessingInstruction) return true;
			if (objectToCheck is XComment) return true;
			return false;
		}

		/// <summary>
		/// Returns <see langword="true"/> if type of the object is 
		/// an <see cref="XContainer"/> and has a child.
		/// </summary>
		/// <param name="objectToCheck">
		/// The incoming object to evaluate.
		/// </param>
		/// <returns>
		/// <paramref name="objectToCheck"/> cast to a container if conditions
		/// are satisfied.
		/// </returns>
		/// <remarks>
		/// Note that calling this method causes an <see cref="XContainer"/>'s content
		/// to be wrapped in an <see cref="XText"/> type if the content is a lazy string.
		/// </remarks>
		public static XContainer GetMeAsANonEmptyXContainer(object objectToCheck)
		{
			var xcontainer = objectToCheck as XContainer;
			if (xcontainer != null && xcontainer.FirstNode != null)
				return xcontainer;
			return null;
		}
		#endregion // Object Pseudo-Extensions
	}
}

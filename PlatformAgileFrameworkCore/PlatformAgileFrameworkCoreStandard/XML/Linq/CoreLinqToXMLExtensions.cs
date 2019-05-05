//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using System;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlatformAgileFramework.XML.Linq
{
	/// <summary>
	/// <para>
	/// This is a set of extensions for LinqToXml objects. These are the simple
	/// ones that MS seems to have left out.
	/// </para>
	/// <para>
	/// Many of these methods also exist because there are either broken
	/// methods in either .Net or Mono or there is inconsistency in the way they work.
	/// </para>
	/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
	// Core version without XPath.
	public static partial class PAFLinqToXMLExtensions
	{
		#region XElement Extensions
		/// <summary>
		/// Climbs an element tree to get the last attribute in either
		/// the current element or containing elements.
		/// </summary>
		/// <param name="element">Current element.</param>
		/// <param name="levelsToClimb">
		/// Number of levels to go up the tree in the search. -1 goes to top.
		/// Default = -1.
		/// </param>
		/// <returns>
		/// The last attribute found on a search up the XML tree. <see langword="null"/>
		/// if none found.
		/// </returns>
		public static XAttribute GetLastAttributeDeclarationUpXMLTree(
			this XElement element, int levelsToClimb = -1)
		{
			do
			{
				var locatedAttribute = element.LastAttribute;
				if (locatedAttribute != null)
					return locatedAttribute;
				if (--levelsToClimb == 0) return null;
				element = element.Parent;
			}
			while (element != null);
			return null;
		}

		/// <summary>
		/// Gets the first namespace declaration from an element's attributes.
		/// </summary>
		/// <param name="element">Element to examine.</param>
		/// <returns><see langword="null"/> if no namespace.</returns>
		public static XAttribute GetFirstNamespaceDeclarationAttribute(
			this XElement element)
		{
			var xattribute = element.FirstAttribute;
			if (xattribute == null) return null;

			while (!xattribute.IsNamespaceDeclaration)
			{
				xattribute = xattribute.NextAttribute;
				if (xattribute == null)
					return null;
			}
			return xattribute;
		}
		/// <summary>
		/// Climbs an element tree to get the first namespace attribute in either
		/// the current element or containing elements.
		/// </summary>
		/// <param name="element">Current element.</param>
		/// <returns>
		/// The first namespace found on a search up the XML tree. <see langword="null"/>
		/// if none found.
		/// </returns>
		public static XAttribute GetFirstNamespaceDeclarationUpXMLTree(this XElement element)
		{
			do
			{
				var locatedDeclarationAttribute = element.GetFirstNamespaceDeclarationAttribute();
				if (locatedDeclarationAttribute != null)
					return locatedDeclarationAttribute;
				element = element.Parent;
			}
			while (element != null);
			return null;
		}

		/// <summary>
		/// Finds a namespace with a given tag. The search will proceed up
		/// any element tree that the current element is contained in. Excludes
		/// the "xmlns" namespace.
		/// </summary>
		/// <param name="xElement">
		/// The element to start the search on. <see langword="null"/>
		/// gets <see langword="null"/>.
		/// </param>
		/// <param name="localName">
		/// Namespace tag. If <see cref="string.Empty"/> or <see langword="null"/>
		/// gets <see langword="null"/>.
		/// </param>
		/// <returns><see langword="null"/> if namespace was not located.</returns>
		public static XAttribute GetNamespace(this XElement xElement, string localName)
		{
			// Safety valves.
			if (xElement == null) return null;
			if (string.IsNullOrEmpty(localName)) return null;

			for (var foundNamespace = xElement.GetFirstNamespaceDeclarationUpXMLTree();
				foundNamespace != null;
				foundNamespace
				= foundNamespace.GetNextNamespaceDeclarationUpXMLTree()
				)
			{
				if (foundNamespace.Name.LocalName != localName) continue;
				return foundNamespace;
			}
			if (localName != "xml") return null;

			return PAFLinqToXMLUtils.XmlNamespaceDeclarationAttribute;
		}

		/// <summary>
		/// Creates a list of all namespaces on an element.
		/// </summary>
		/// <param name="xElement">
		/// The element to search. <see langword="null"/>
		/// gets <see langword="null"/>.
		/// </param>
		/// <returns>Empty list if element has no namespaces.</returns>
		public static IList<XAttribute> GetNamespacesOnElement(this XElement xElement)
		{
			// Safety valves.
			if (xElement == null) return null;

			var namespaceList = new List<XAttribute>();

			for (var foundNamespace = xElement.GetFirstNamespaceDeclarationUpXMLTree();
				foundNamespace != null;
				foundNamespace
				= foundNamespace.GetNextNamespaceDeclarationUpXMLTree(0)
				)
			{
				namespaceList.Add(foundNamespace);
			}

			return namespaceList;
		}
		/// <summary>
		/// Creates a list of all attributes on an element.
		/// </summary>
		/// <param name="xElement">
		/// The element to search. <see langword="null"/>
		/// gets <see langword="null"/>.
		/// </param>
		/// <returns>Empty list if element has no attributes.</returns>
		public static IList<XAttribute> GetAttributesOnElement(this XElement xElement)
		{
			// Safety valve.
			if (xElement == null) return null;

			var namespaceList = new List<XAttribute>();

			for (var foundAttribute = xElement.FirstAttribute;
				foundAttribute != null;
				foundAttribute
				= foundAttribute.NextAttribute
				)
			{
				namespaceList.Add(foundAttribute);
			}

			return namespaceList;
		}

		/// <summary>
		/// Returns the first <see cref="XElement"/> in the element's
		/// descendants.
		/// </summary>
		/// <param name="xElement">The element to check.</param>
		/// <returns><see langword="null"/> for no descendant elements.</returns>
		public static XElement FirstElement(this XElement xElement)
		{
			return xElement.DescendantNodes().OfType<XElement>().FirstOrDefault();
		}

		/// <summary>
		/// Climbs the XML tree to see if the name of the attribute is associated with
		/// the current element or an enclosing element.
		/// </summary>
		/// <param name="element">Element at current point in tree.</param>
		/// <param name="attribute">Attribute under consideration.</param>
		/// <returns><see langword="true"/> if named attribute found.</returns>
		/// <remarks>
		/// Used typically to find namespaces. Any <see langword="null"/> arguments returns 
		/// <see langword="null"/>.
		/// </remarks>
		public static bool FindNamedAttributeUpTree(this XElement element, XAttribute attribute)
		{
			if((element == null) || (attribute == null))
			return false;
			var name = attribute.Name;
			for (; element != null && element != attribute.Parent; element = element.Parent)
			{
				if (element.Attribute(name) != null)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Determines if an element has non-namespace attributes.
		/// </summary>
		/// <param name="xelement">Element to examine.</param>
		/// <returns>
		/// <see langword="true"/> if element has any "regular" attributes.
		/// </returns>
		public static bool HasNonNamespaceAttributes(this XElement xelement)
		{
			for (var xattribute = xelement.FirstAttribute; xattribute != null; xattribute = xattribute.NextAttribute)
			{
				if (!xattribute.IsNamespaceDeclaration)
					return true;
			}
			return false;
		}
		/// <summary>
		/// Method simply moves to the first element inside the root element if
		/// the current element has "root" in its value. It's useful for moving
		/// off the obligatory root element in legacy documents.
		/// </summary>
		/// <param name="xElement">The element to check.</param>
		/// <returns>
		/// If this element is not a root, it returns itself unchanged. If it
		/// is a root element with no contained elements, <see langword="null"/> is returned.
		/// </returns>
		public static XElement MovePastRoot(this XElement xElement)
		{
			if (xElement.Name.LocalName.Contains("root")) return xElement.FirstElement();
			return xElement;
		}
		/// <summary>
		/// This method looks up a single attribute by name. It finds the first
		/// one with that name on an <see cref="XElement"/>.
		/// </summary>
		/// <param name="xElement">The element to examine.</param>
		/// <param name="xName">The name of the attribute to find.</param>
		/// <returns>A named attribute or <see langword="null"/> if none found.</returns>
		public static XAttribute NamedAttribute(this XElement xElement, XName xName)
		{
			// Interested in just this element.
			var elts = new List<XElement> { xElement };
			// Call the IEnum extensions.
			var allAtts = elts.Attributes(xName);
			var atts = allAtts.ToList();
			if (atts.Count > 0) return atts[0];
			return null;
		}
		/// <summary>
		/// This method looks up a single element by name. It finds the first
		/// one with that name on an <see cref="XElement"/>.  It finds the immediate
		/// descendant element (child element) with that name on an <see cref="XElement"/>.
		/// </summary>
		/// <param name="xElement">The element to examine.</param>
		/// <param name="xName">The name of the element to find.</param>
		/// <returns>A named element or <see langword="null"/> if none found.</returns>
		public static XElement NamedChildElement(this XElement xElement, XName xName)
		{
			// Interested in just this element.
			var elts = new List<XElement> { xElement };
			// Call the IEnum extensions.
			var allElts = elts.Elements(xName);
			var eltsList = allElts.ToList();
			if (eltsList.Count > 0) return eltsList[0];
			return null;
		}
		/// <summary>
		/// This method looks up elements by name. It gathers the immediate
		/// descendant elements (child elements) with that name on an <see cref="XElement"/>.
		/// </summary>
		/// <param name="xElement">The element to examine.</param>
		/// <param name="xName">The name of the child elements to find.</param>
		/// <returns>A non- <see langword="null"/> enumeration.</returns>
		public static IEnumerable<XElement> NamedChildElements(this XElement xElement, XName xName)
		{
			// Interested in just this element.
			var elts = new List<XElement> { xElement };
			// Call the IEnum extensions.
			return elts.Elements(xName);
		}
		/// <summary>
		/// This method accepts a set of elements. It attempts to put the named attributes on
		/// the elements into a key/value collection. If it does not find keys and values of the
		/// appropriate names, it doesn't add them.
		/// </summary>
		/// <param name="xElements">The elements to process.</param>
		/// <param name="keyAttributeName">The name of the key attribute.</param>
		/// <param name="valueAttributeName">The name of the value attribute.</param>
		/// <returns>A non- <see langword="null"/> enumeration.</returns>
		public static IEnumerable<KeyValuePair<string, string>> KeyValuePairsFromElements
			(this IEnumerable<XElement> xElements
			,XName keyAttributeName, XName valueAttributeName)
		{
			var output = new Collection<KeyValuePair<string, string>>();
			if (xElements == null) return output;
			foreach (var elt in xElements)
			{
				XAttribute keyAttribute = null;
				XAttribute valueAttribute = null;
				if ((keyAttribute = elt.NamedAttribute(keyAttributeName)) == null)
					continue;
				if ((valueAttribute = elt.NamedAttribute(valueAttributeName)) == null)
					continue;
				output.Add(new KeyValuePair<string, string>(keyAttribute.Value, valueAttribute.Value));
			}
			return output;
		}
		#endregion // XElement Extensions
		#region XAttribute Extensions
		/// <summary>
		/// Looks for the next namespace attribute either at this element level
		/// or grabs the first one at the next level that has namespace attributes
		/// up the tree, if one exists. This is an iterator to search the XML tree
		/// in an ordered way.
		/// </summary>
		/// <param name="attribute">
		/// Attribute to find a forward sibling for. <see langword="null"/> gets 
		/// <see langword="null"/>.
		/// </param>
		/// <param name="levelsToClimb">
		/// Number of levels to go up the tree in the search. -1 goes to top.
		/// Default = -1.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if attribute not parented.
		/// <see langword="null"/> another namespace attribute not found.
		/// </returns>
		public static XAttribute GetNextNamespaceDeclarationUpXMLTree(this XAttribute attribute,
			int levelsToClimb = -1)
		{
			var attributeParent = attribute.Parent;
			while (true)
			{
				if (attributeParent == null)
					return null;
				var declarationLocal = attribute.GetNextNamespaceDeclarationOnCurrentElement();
				if (declarationLocal != null)
					return declarationLocal;

				// Done going up?
				if (levelsToClimb == 0) return null;

				levelsToClimb--;
				attributeParent = attributeParent.Parent;
				if ((attribute = attributeParent.GetFirstNamespaceDeclarationUpXMLTree()) != null)
					return attribute;
			}
		}
		/// <summary>
		/// Looks for the previous attribute either at this element level
		/// or grabs the last one at the next level that has attributes
		/// up the tree, if one exists. This is an iterator to search the XML tree
		/// in an ordered way.
		/// </summary>
		/// <param name="attribute">
		/// Attribute to find a backward sibling for. <see langword="null"/> gets 
		/// <see langword="null"/>.
		/// </param>
		/// <param name="levelsToClimb">
		/// Number of levels to go up the tree in the search. -1 goes to top.
		/// Default = -1.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if attribute not parented.
		/// <see langword="null"/> another attribute not found.
		/// </returns>
		public static XAttribute GetPreviousAttributeUpXMLTree(this XAttribute attribute,
			int levelsToClimb = -1)
		{
			if (attribute == null) return null;

			var attributeParentElement = attribute.Parent;
			while (attributeParentElement != null)
			{
				// See if we've got something at the current level.
			    var declarationLocal = attribute?.PreviousAttribute;
			    if (declarationLocal != null)
			        return declarationLocal;

			    // Done going up?
				if (levelsToClimb == 0) return null;

				// Climb up one level.
				levelsToClimb--;
				attributeParentElement = attributeParentElement.Parent;

				// Can't go past the top.
				if (attributeParentElement == null) return null;

				if ((attribute = attributeParentElement.LastAttribute) != null)
					return attribute;
			}
			return null;
		}
		/// <summary>
		/// Checks both local name and namespacename against the template for the xml namespace.
		/// </summary>
		/// <param name="attribute">
		/// Attribute to be checked to see if it's the xml namespace.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if we have the xml namespace def.
		/// </returns>
		public static bool IsXmlNamespaceDeclaration(this XAttribute attribute)
		{
			if (attribute.Name.LocalName != PAFLinqToXMLUtils.XmlNamespaceDeclarationAttribute.Name.LocalName)
				return false;
			if (attribute.Name.NamespaceName != PAFLinqToXMLUtils.XmlNamespaceDeclarationAttribute.Name.NamespaceName)
				return false;
			return true;
		}
		#endregion // XAttribute Extensions
		#region XObject Extensions
		/// <summary>
		/// Returns this node's container if it has one. If the node doesn't
		/// have a parent, it's containing <see cref="XDocument"/> is returned,
		/// if it has one. A node needn't be contained in a document.
		/// </summary>
		/// <param name="node">The node to be checked.</param>
		/// <returns><see langword="null"/> if no container.</returns>
		public static XContainer PAFGetXContainerParent(this XObject node)
		{
			var parent = node.Parent;
			if (parent != null)
				return parent;
			if (node is XDocument)
				return null;
			return node.Document;
		}

		#endregion //XObject Extensions
		#region XAttribute Extensions
		/// <summary>
		/// This method just searches to the right of the current parented attribute
		/// to grab the next namespace declaration.
		/// </summary>
		/// <param name="attribute">The attribute to operate on.</param>
		/// <returns>
		/// <see langword="null"/> if the attribute is not parented or there are no more namespaces
		/// found.
		/// </returns>
		public static XAttribute GetNextNamespaceDeclarationOnCurrentElement(this XAttribute attribute)
		{
			var parent = attribute.Parent;
			if (parent == null)
				return null;
			while (attribute != parent.LastAttribute)
			{
				attribute = attribute.NextAttribute;
				if (attribute.IsNamespaceDeclaration)
					return attribute;
			}
			return null;
		}
		#endregion // XAttribute Extensions
		#region XmlNodeType Extensions
		/// <summary>
		/// This method determines whether an <see cref="XmlNodeType"/> can be
		/// an <c>XPathNodeType</c>.
		/// </summary>
		/// <param name="nodeType">The type to check.</param>
		/// <returns>
		/// <see langword="false"/> if the incoming type is not <see cref="XmlNodeType.Attribute"/>,
		/// <see cref="XmlNodeType.Comment"/>, <see cref="XmlNodeType.Text"/>, <see cref="XmlNodeType.CDATA"/>,
		/// <see cref="XmlNodeType.Element"/>, <see cref="XmlNodeType.Attribute"/>,
		/// <see cref="XmlNodeType.ProcessingInstruction"/>, <see cref="XmlNodeType.Whitespace"/>,
		/// <see cref="XmlNodeType.SignificantWhitespace"/>, <see cref="XmlNodeType.Document"/>.
		/// </returns>
		public static bool CanXmlNodeBeAnXPathNode(this XmlNodeType nodeType)
		{
			if (nodeType.CanXmlNodeBeAnXPathChildNode()) return true;
			if (nodeType == XmlNodeType.Document) return true;
			return false;
		}
		/// <summary>
		/// This method determines whether an <see cref="XmlNodeType"/> can be
		/// an <c>XPathNodeType</c>'s child.
		/// </summary>
		/// <param name="nodeType">The type to check.</param>
		/// <returns>
		/// <see langword="false"/> if the incoming type is not
		/// <see cref="XmlNodeType.Comment"/>, <see cref="XmlNodeType.Text"/>, <see cref="XmlNodeType.CDATA"/>,
		/// <see cref="XmlNodeType.Element"/>, <see cref="XmlNodeType.Attribute"/>,
		/// <see cref="XmlNodeType.ProcessingInstruction"/>, <see cref="XmlNodeType.Whitespace"/>,
		/// <see cref="XmlNodeType.SignificantWhitespace"/>.
		/// </returns>
		public static bool CanXmlNodeBeAnXPathChildNode(this XmlNodeType nodeType)
		{
			if (nodeType == XmlNodeType.Comment) return true;
			if (nodeType == XmlNodeType.Text) return true;
			if (nodeType == XmlNodeType.CDATA) return true;
			if (nodeType == XmlNodeType.Element) return true;
			if (nodeType == XmlNodeType.ProcessingInstruction) return true;
			if (nodeType == XmlNodeType.Whitespace) return true;
			if (nodeType == XmlNodeType.SignificantWhitespace) return true;
			return false;
		}
		#endregion // XmlNodeType Extensions
		#region XText Extensions
		/// <summary>
		/// Scrapes the text from a text node. If the text node is
		/// in a container, the method also appends all of the text
		/// from contiguous forward sibling nodes if they are also
		/// all text nodes.
		/// </summary>
		/// <param name="textNode">The text node to process.</param>
		/// <returns>Gathered text.</returns>
		public static string CollectText(this XText textNode)
		{
			var str = textNode.Value;
			if (textNode.PAFGetXContainerParent() == null) return str;
			while (textNode != textNode.PAFGetXContainerParent().LastNode)
			{
				textNode = textNode.NextNode as XText;
				if (textNode != null)
					str = str + textNode.Value;
				else
					break;
			}
			return str;
		}
		#endregion // XText Extensions
	}
}

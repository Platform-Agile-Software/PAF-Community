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
using System.Collections.Generic;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML
{
	/// <summary>
	/// Class that holds the XML namespace names found in a tree.
	/// </summary>
	/// <threadsafety>
	/// safe.
	/// </threadsafety>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// <para>
	/// Created. Attempt to bring some sanity to the world by rewriting MS's "NameTable".
	/// </para>
	/// </contribution>
	/// </history>
	[PAFSerializable]
	public class PAFXMLNameTable
	{
		#region Class AutoProperties
		/// <summary>
		/// The backing dictionary.
		/// </summary>
		protected internal IDictionary<string, string> m_NamespaceCollection;
		#endregion // Class AutoProperties
		#region Constructors
		/// <summary>
		/// Constructor just set the fields.
		/// </summary>
		public PAFXMLNameTable()
		{
			m_NamespaceCollection = new Dictionary<string, string>();
		}
		/// <summary>
		/// Deep copy constructor.
		/// </summary>
		/// <param name="other">Another one of us.</param>
		public PAFXMLNameTable(PAFXMLNameTable other)
		{
			m_NamespaceCollection = new Dictionary<string, string>(other.m_NamespaceCollection);
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Allows a namespace to be added.
		/// </summary>
		/// <returns><see langword="false"/> if namespace already in collection.
		/// </returns>
		/// <remarks>
		/// This method corresponds to the storage notion in MS's "NameTable", providing backward
		/// compatibility.
		/// </remarks>
		public bool AddNamespace(string namespaceEntry)
		{
			lock (m_NamespaceCollection)
			{
				return m_NamespaceCollection.AddUniqueAsThoughCollection(namespaceEntry);
			}
		}
		/// <summary>
		/// Allows a namespace to be added.
		/// </summary>
		/// <returns><see langword="false"/> if namespace already in collection.
		/// </returns>
		/// <remarks>
		/// This method allows a namespace to be keyed by its prefix tag.
		/// </remarks>
		public bool AddNamespace(string namespaceTag, string namespaceString)
		{
			lock (m_NamespaceCollection)
			{
				string outString;
				if (m_NamespaceCollection.TryGetValue(namespaceTag, out outString)) return false;
				m_NamespaceCollection.Add(namespaceTag, namespaceString);
				return true;
			}
		}
		/// <summary>
		/// Allows a namespace to be retrieved.
		/// </summary>
		/// <returns><see langword="null"/> if namespace not found.
		/// </returns>
		/// <remarks>
		/// This method retrieves a namespace by its key, which may be just the
		/// namespace string itself.
		/// </remarks>
		public string GetNamespace(string namespaceTag)
		{
			lock (m_NamespaceCollection)
			{
				string outString;
				if (!m_NamespaceCollection.TryGetValue(namespaceTag, out outString)) return null;
				return outString;
			}
		}
		/// <summary>
		/// Allows all namespaces to be retrieved.
		/// </summary>
		/// <returns>
		/// A list of namespaces.
		/// </returns>
		public IList<KeyValuePair<string, string>> GetNamespaces()
		{
			lock (m_NamespaceCollection)
			{
				var list = new List<KeyValuePair<string, string>>(m_NamespaceCollection);
				return list;
			}
		}
		/// <summary>
		/// Creates a name table preloaded with
		/// <see cref="XMLUtils.XMLNS_PREFIX_NAMESPACE"/> and
		/// <see cref="XMLUtils.XML_PREFIX_NAMESPACE"/>.
		/// </summary>
		/// <returns>Preloaded table.</returns>
		public static PAFXMLNameTable CreateNameTable()
		{
			var xmlNameTable = new PAFXMLNameTable();
			xmlNameTable.AddNamespace(XMLUtils.XMLNS_PREFIX_NAMESPACE);
			xmlNameTable.AddNamespace(XMLUtils.XML_PREFIX_NAMESPACE);
			return xmlNameTable;
		}
		#endregion // Methods
	}
}

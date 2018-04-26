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
using PlatformAgileFramework.MultiProcessing.Threading.Attributes;
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.XML.Namespace
{
	/// <summary>
	/// Default class that allows manipulating a type with its XML
	/// namespace.
	/// </summary>
	/// <threadsafety>
	/// thread-safe
	/// </threadsafety>
	[PAFSynchronized(PAFSynchronizedVisibilityType.All)]
	public class XMLNamedTypeBase: IXMLNamedTypeInternal
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for Name.
		/// </summary>
		/// <remarks>
		/// Never serialized.
		/// </remarks>
		[PAFSerialized(Serialize = false)]
		private readonly NullableSynchronizedWrapper<string> m_NameWrapper
			= new NullableSynchronizedWrapper<string>(null, new DummyReaderWriterLock());
		/// <summary>
		/// Backing for XMLNamespace.
		/// </summary>
		/// <remarks>
		/// Never serialized.
		/// </remarks>
		[PAFSerialized(Serialize = false)]
		private readonly NullableSynchronizedWrapper<string> m_XMLNamespaceWrapper
			= new NullableSynchronizedWrapper<string>(null, new DummyReaderWriterLock());
		/// <summary>
		/// Backing for ECMANamespace.
		/// </summary>
		/// <remarks>
		/// Never serialized.
		/// </remarks>
		[PAFSerialized(Serialize = false)]
		private readonly NullableSynchronizedWrapper<string> m_ECMANamespaceWrapper
			= new NullableSynchronizedWrapper<string>(null, new DummyReaderWriterLock());
		#endregion //Class Fields and Autoproperties
		#region Properties
		/// <summary>
		/// See <see cref="IXMLNamedType"/>.
		/// </summary>
		public virtual string Name
		{
			get { lock (m_NameWrapper) { return m_NameWrapper.NullableObject; } }
			internal set { lock (m_NameWrapper) { m_NameWrapper.NullableObject = value; } }
		}
		/// <summary>
		/// See <see cref="IXMLNamedType"/>.
		/// </summary>
		public virtual string XMLNamespace
		{
			get { lock (m_XMLNamespaceWrapper) { return m_XMLNamespaceWrapper.NullableObject; } }
			internal set { lock (m_XMLNamespaceWrapper) { m_XMLNamespaceWrapper.NullableObject = value; } }
		}
		/// <summary>
		/// See <see cref="IXMLNamedTypeInternal"/>.
		/// </summary>
		internal virtual string ECMANameSpaceInternal
		{
			get { lock (m_ECMANamespaceWrapper) { return m_ECMANamespaceWrapper.NullableObject; } }
			set { lock (m_ECMANamespaceWrapper) { m_ECMANamespaceWrapper.NullableObject = value; } }
		}
		#endregion // Properties
		#region IXMLNamedTypeInternal Implementation
		/// <summary>
		/// See <see cref="IXMLNamedTypeInternal"/>.
		/// </summary>
		string IXMLNamedTypeInternal.ECMANamespace
		{ get { return ECMANameSpaceInternal; } set { ECMANameSpaceInternal = value; } }
		/// <summary>
		/// See <see cref="IXMLNamedTypeInternal"/>.
		/// </summary>
		void IXMLNamedTypeInternal.SetName(string name)
		{
			Name = name;
		}
		/// <summary>
		/// See <see cref="IXMLNamedTypeInternal"/>.
		/// </summary>
		void IXMLNamedTypeInternal.SetXMLNamespace(string xMLNamespace)
		{
			XMLNamespace = xMLNamespace;
		}
		#endregion // IXMLNamedTypeInternal Implementation
	}
}

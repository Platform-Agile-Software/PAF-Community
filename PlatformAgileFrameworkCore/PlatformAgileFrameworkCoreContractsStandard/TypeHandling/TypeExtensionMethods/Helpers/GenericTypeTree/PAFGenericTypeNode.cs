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

#region Using Directives
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
#endregion

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers
{
	/// <summary>
	/// Default implementation of <see cref="IPAFGenericTypeNode"/>.
	/// </summary>
	/// <threadsafety>
	/// safe - Immutable type.
	/// </threadsafety>
	public class PAFGenericTypeNode : IPAFGenericTypeNode
	{
		internal IList<IPAFGenericTypeNode> m_GenericChildTypes;
		internal IPAFGenericTypeNode m_GenericParentNode;
		internal Type m_NodeType;
		#region Constructors
		/// <summary>
		/// Builds the node by examining the incoming <see cref="Type"/>.
		/// </summary>
		/// <param name="genericParentNode">
		/// Sets the property.
		/// </param>
		/// <param name="nodeType">
		/// This is the type of the node we are building. Sets the prop.
		/// </param>
		public PAFGenericTypeNode(IPAFGenericTypeNode genericParentNode, Type nodeType)
		{
			m_NodeType
				= nodeType ?? throw new ArgumentNullException(nameof(nodeType));

			m_GenericParentNode = genericParentNode;

			if (m_GenericParentNode == null)
				return;

			if (!m_GenericParentNode.NodeType.IsGenericType) return;

			// Generate children if not a leaf node.
			m_GenericChildTypes = new Collection<IPAFGenericTypeNode>();
			m_GenericChildTypes.AddItems(GetWrappedTypes(m_NodeType.GetGenericArguments()));
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>. internal setter for testing.
		/// </summary>
		public Type NodeType
		{
			get { return m_NodeType; }
			protected internal set { m_NodeType = value; }
		}
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>. internal setter for testing.
		/// </summary>
		public IPAFGenericTypeNode GenericParentNode
		{
			get { return m_GenericParentNode;}
			protected internal set { m_GenericParentNode = value; }
		}
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>. internal setter for testing.
		/// </summary>
		public IList<IPAFGenericTypeNode> ChildTypeNodes
		{
			get { return m_GenericChildTypes; }
			protected internal set { m_GenericChildTypes = value; }
		}
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>.
		/// </summary>
		public IList<Type> ActualTypes
		{
			get
			{
				if (m_GenericChildTypes.SafeCount() == 0)
					return null;

				var types = new Collection<Type>();

				foreach (var gType in m_GenericChildTypes)
				{
					types.Add(gType.NodeType);
				}

				return types;
			}
		}

		#endregion Properties
		#region Methods
		#region Static Helpers
		/// <summary>
		/// Wraps an enumeration of <see cref="Type"/>s in our
		/// <see cref="PAFGenericTypeNode"/>s.
		/// </summary>
		/// <param name="actualTypes">
		/// Incoming <see cref="Type"/>s. <see langword="null"/> is OK.
		/// </param>
		/// <returns>
		/// <see langword="null"/> for no types coming in.
		/// </returns>
		public IList<IPAFGenericTypeNode> GetWrappedTypes(IEnumerable<Type> actualTypes)
		{
			actualTypes = actualTypes.ToArray();
			if (actualTypes.SafeCount() == 0)
				return null;

			var gTypes = new Collection<IPAFGenericTypeNode>();

			foreach (var type in actualTypes)
			{
				gTypes.Add(new PAFGenericTypeNode(this, type));
			}

			return gTypes;
		}
		#endregion // Static Helpers
		#endregion Methods
	}
}

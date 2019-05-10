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

using System;
using System.Reflection;
using NUnit.Framework;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.TypeTree.Tests
{
	/// <summary>
	/// Tests basic construction of a tree of
	/// <see cref="IPAFGenericTypeNode"/>s.
	/// </summary>
	[TestFixture]
	public class BasicTypeTreeTests
	{
		/// <summary>
		/// This type will fail construction, but we can build
		/// the tree with it.
		/// </summary>
		public Type m_ComplexAbstractInterfaceType
			= typeof(IPAFServiceDescription<PAFService>);
		/// <summary>
		/// This type will fail construction, but we can build
		/// the tree with it.
		/// </summary>
		public Type m_SimpleGenericInterfaceType
			= typeof(ISeparateGenericHolder<>);
		/// <summary>
		/// Just builds a tree from an interface type.
		/// </summary>
		[Test]
		public void BuildTheTreeFromAnInterface()
		{
			var root = new PAFGenericTypeNode(null, m_ComplexAbstractInterfaceType);
		}
		/// <summary>
		/// Builds a tree from an OPEN Generic interface type. There are additional
		/// tests here to ensure the new Generic stuff in the reflection library
		/// works on all platforms. We had some trouble in the past.
		/// </summary>
		[Test]
		public void BuildTheTreeFromAnOpenGenericInterface()
		{
			var root = new PAFGenericTypeNode(null, m_SimpleGenericInterfaceType);

			var rootType = root.NodeType;
			var isRootTypeGeneric = rootType.IsGenericTypeDefinition;
			Assert.IsTrue(isRootTypeGeneric, "root type is Generic.");

			var constrainedGenericArgument = rootType.GetGenericArguments()[0];
			var attributes = constrainedGenericArgument.GenericParameterAttributes;
			var attributeIsJustReference = attributes == GenericParameterAttributes.ReferenceTypeConstraint;

			// Ensure that attribute is just reference - nothing more.
			Assert.IsTrue(attributeIsJustReference, "attributeIsJustReference");
		}
	}
}


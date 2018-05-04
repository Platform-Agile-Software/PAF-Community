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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Enumerators;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Default implementation of <see cref="IPAFTestElementInfo{T}"/>
	/// </summary>
	/// <threadsafety>
	/// Implementations are NOT necessarily expected to be thread-safe. This class
	/// is setup once and is then only read.
	/// </threadsafety>
	/// <history>
	/// <author> KRM </author>
	/// <date> 07aug2012 </date>
	/// <contribution>
	/// <para>
	/// Added history. Reconstructed so we can keep the variegated
	/// <see cref="IPAFTestElementInfo"/>'s, but provide a simple type-safe
	/// interface so we don't have a technical support disaster when we
	/// release this.
	/// </para>
	/// </contribution>
	/// </history>
	public class PAFTestElementInfo<T>: PAFTestElementInfo, IPAFTestElementInfo<T>
		where T: IPAFTestElementInfo
	{

		#region Fields and Autoprops
		/// <summary>
		/// Static default for the display element gatherer.
		/// See <see cref="IPAFTestElementInfo"/>.
		/// </summary>
		// It's OK, ReSharper, we want type-specific child gatherers.
		// ReSharper disable once StaticMemberInGenericType
		public static Func<IPAFTestElementInfo<T>, IList<IPAFTestElementInfo>>
			DefaultGetDisplayChildElementItems { get; set; }
			= PAFTestElementInfoExtensions.GetTypedChildInfosOfType<T, IPAFTestElementInfo>;
		/// <summary>
		/// Backing for inheritors
		/// </summary>
		protected IPAFEnumerableProvider<T> m_EnumerableProvider;

		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// Never set during tests running.
		/// </summary>
		public virtual IPAFEnumerableProvider<T> TestElementInfoItemEnumerableProvider { get; set; }
		#endregion // Fields and Autoprops
		#region Constructors
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="name">
		/// See base.
		/// </param>
		/// <param name="children">
		/// See base.
		/// </param>
		/// <param name="parent">
		/// See base.
		/// </param>
		/// <exceptions>
		/// See base.
		/// </exceptions>
		protected PAFTestElementInfo(string name, IEnumerable<IPAFTestElementInfo> children = null,
			IPAFTestElementInfo parent = null) :base(name,
			children, parent)
		{
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// Default is the static.
		/// </summary>
		public Func<IPAFTestElementInfo<T>, IList<IPAFTestElementInfo>>
			GetDisplayChildElementItems { get; set; }
			= DefaultGetDisplayChildElementItems;

		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// </summary>
		public virtual IPAFEnumerableProvider<T> EnumerableProvider
		{
			get { return m_EnumerableProvider; }
		}
		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>
		/// </summary>
		public virtual IEnumerable<T> TypedChildren
		{
			get
			{
				return this.GetChildInfoSubtypesOfType<IPAFTestElementInfo,T>();
			}
		}

		#endregion Properties
		#region Methods
		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// </summary>
		/// <param name="testElement">
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// </param>
		/// <remarks>
		/// Calls base virtual method.
		/// </remarks>
		public virtual void AddTestElement(T testElement)
		{
			AddTestElement((IPAFTestElementInfo) testElement);
		}

		public override IList<IPAFTestElementInfo> GetElementsToDisplay()
		{
			return GetDisplayChildElementItems(this);
		}

		/// <summary>
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// </summary>
		/// <param name="provider">
		/// See <see cref="IPAFTestElementInfo{T}"/>.
		/// </param>
		public virtual void SetProvider(IPAFEnumerableProvider<T> provider)
		{
			m_EnumerableProvider = provider;
		}
		#endregion // Methods
	}
}

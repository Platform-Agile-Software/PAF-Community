//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2018 Icucom Corporation
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
using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// A implementation of <see cref="IPAFTestResultNavigationCommandArgumentHolder"/>
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21jan2018 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// As ReSharper says, there will be an ambiguity when accessing two
	/// closures of the same Generic type. Explicit interface implementation
	/// must is used.
	/// </remarks>
	public class PAFTestResultNavigationCommandArgumentHolder
		: IPAFTestResultNavigationCommandArgumentHolder
	{
		#region Fields and AutoProperties
		/// <summary>
		/// We encapsulate the int holder and delegate to it.
		/// </summary>
		protected internal IPAFIntArgumentHolder m_IntArgumentHolder;
		/// <summary>
		/// We encapsulate the exception holder and delegate to it.
		/// </summary>
		protected internal IPAFExceptionArgumentHolder m_ExceptionArgumentHolder;
		#endregion // Fields and AutoProperties
		#region Constructors
		/// <summary>
		/// Default constructor just build the encapsulated containers.
		/// </summary>
		public PAFTestResultNavigationCommandArgumentHolder()
		{
			m_IntArgumentHolder = new PAFIntArgumentHolder(0);
			m_ExceptionArgumentHolder = new PAFExceptionArgumentHolder();
		}
		/// <summary>
		/// Constructor sets an index.
		/// </summary>
		/// <remarks>
		/// This is the only non-default constructor we provide, since our
		/// use case is to push this thing in and let it gather exceptions
		/// from the methods it is passed to.
		/// </remarks>
		public PAFTestResultNavigationCommandArgumentHolder(int childIndex)
		:this()
		{
			m_IntArgumentHolder.Argument = childIndex;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFIntArgumentHolder"/>.
		/// </summary>
		/// <remarks>
		/// Explicit implementation to avoid ambiguity. We normally
		/// build these with a virtual backing prop, but we don't
		/// expect this to be used by anybody but us.
		/// </remarks>
		int IPAFArgumentHolder<int>.Argument
		{
			get { return m_IntArgumentHolder.Argument; }
			set { m_IntArgumentHolder.Argument = value; }
		}
		/// <summary>
		/// See <see cref="IPAFExceptionArgumentHolder"/>.
		/// </summary>
		/// <remarks>
		/// Explicit implementation to avoid ambiguity. We normally
		/// build these with a virtual backing prop, but we don't
		/// expect this to be used by anybody but us.
		/// </remarks>

		Exception IPAFArgumentHolder<Exception>.Argument
		{
			get { return m_ExceptionArgumentHolder.Argument; }
			set { m_ExceptionArgumentHolder.Argument = value; }
		}
		#endregion // Properties
	}
}
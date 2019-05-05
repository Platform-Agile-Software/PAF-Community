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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;

namespace PlatformAgileFramework.TypeHandling.AggregableObjectArguments
{
	/// <summary>
	/// An implementation of <see cref="IPAFExceptionArgumentHolder"/>.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 10nov2011 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe.
	/// </threadsafety>
	public class PAFExceptionArgumentHolder : IPAFExceptionArgumentHolder
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing...
		/// </summary>
		protected internal Exception m_Argument;
		#endregion // Fields and Autoproperties

		#region Constructors
		/// <summary>
		/// Default constructor for construct and set style.
		/// </summary>
		public PAFExceptionArgumentHolder()
		{
		}
		/// <summary>
		/// Just sets <see cref="m_Argument"/>.
		/// </summary>
		/// <param name="exceptionArgument">exception to set to.</param>
		public PAFExceptionArgumentHolder(Exception exceptionArgument)
		{
			m_Argument = exceptionArgument;
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFExceptionArgumentHolder"/>
		/// </summary>
		public virtual Exception Argument
		{
			get { return m_Argument; }
			set { m_Argument = value; }
		}
		#endregion // Properties
	}
}
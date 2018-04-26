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

namespace PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders
{
    /// <summary>
    /// Just wraps a <see cref="IEnumerable{T}"/>.
    /// </summary>
    /// <threadsafety>
    /// Safe.
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 12dec2017 </date>
    /// <description>
    /// Factored this out of the testing stuff, since we need a
    /// base provider, generally.
    /// </description>
    /// </contribution>
    /// </history>
    public class PAFEnumerableProvider<T>: IPAFEnumerableProvider<T>
    {
		/// <summary>
		/// Backing. Don't mess with it in derived classes unless thee
		/// knows what thee is doing.
		/// </summary>
	    protected IEnumerable<T> m_Enumerable;
		public PAFEnumerableProvider(IEnumerable<T> enumerable)
		{
			m_Enumerable = enumerable;
		}
		#region Properties
		/// <summary>
		/// <see cref="IPAFEnumerableProvider{T}"/>
		/// </summary>
		public virtual IEnumerable<T> GetEnumerable()
		{return m_Enumerable;  }
		#endregion Properties
	    #region IDisposable Implementation
	    ///////////////////////////////////////////////////////////////////////
	    /// <summary>
	    /// Calls main method.
	    /// </summary>
	    public void Dispose()
	    {
		    Dispose(true);
	    }
	    /// <summary>
	    /// Doesn't do anything but prevent finalization on this base class.
	    /// </summary>
	    protected virtual void Dispose(bool disposing)
	    {
			if(disposing)
				GC.SuppressFinalize(this);

	    }
	    ///////////////////////////////////////////////////////////////////////
	    #endregion //IDisposable Implementation

	}
}

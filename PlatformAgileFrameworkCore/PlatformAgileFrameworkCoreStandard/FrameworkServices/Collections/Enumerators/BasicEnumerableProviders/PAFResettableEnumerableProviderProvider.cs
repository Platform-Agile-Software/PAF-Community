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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

namespace PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders
{
	/// <summary>
	/// Default implementation of <see cref="ITestElementInfoItemResettableEnumerableProvider{T}"/>
	/// </summary>
	/// <typeparam name="T">A <see cref="IPAFTestElementInfo"/>.</typeparam>
	/// <threadsafety>
	/// Safe
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 13dec2017 </date>
	/// <description>
	/// Built this container so we can just delegate to it.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFResettableEnumerableProviderProvider<T>
		:IPAFResettableEnumerableProviderProvider<T>
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Protected for subclasing the accessors 
		/// </summary>
		protected  IPAFResettableEnumerableProvider<T> m_EnumerableProvider;
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just wraps the provider.
		/// </summary>
		/// <param name="provider">
		/// Provider to be wrapped.
		/// </param>
		public PAFResettableEnumerableProviderProvider
			(IPAFResettableEnumerableProvider<T> provider)
		{
			m_EnumerableProvider = provider;
		}
		#endregion // Constructors

		/// <summary>
		/// <see cref="IPAFResettableEnumerableProviderProvider{T}"/>
		/// </summary>
		public IPAFResettableEnumerableProvider<T> EnumerableProvider
		{
			get { return m_EnumerableProvider; }
		}
	}
}
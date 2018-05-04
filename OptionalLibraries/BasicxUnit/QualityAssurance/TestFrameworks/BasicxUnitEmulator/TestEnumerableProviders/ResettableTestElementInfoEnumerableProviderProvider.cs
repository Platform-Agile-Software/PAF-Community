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

using PlatformAgileFramework.Collections.Enumerators.BasicEnumerableProviders;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.TestEnumerableProviders
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
	public class ResettableTestElementInfoEnumerableProviderProvider<T>:
		PAFResettableEnumerableProviderProvider<T>,
		ITestElementInfoItemResettableEnumerableProviderProvider<T>
		where T:IPAFTestElementInfo
	{
		#region Constructors
		/// <summary>
		/// Just wraps the provider.
		/// </summary>
		/// <param name="provider">
		/// Provider to be wrapped.
		/// </param>
		public ResettableTestElementInfoEnumerableProviderProvider
			(ITestElementInfoItemResettableEnumerableProvider<T> provider)
			:base(provider)
		{
			m_EnumerableProvider = provider;
		}
		#endregion // Constructors
		/// <summary>
		/// <see cref="ITestElementInfoItemResettableEnumerableProviderProvider{T}"/>
		/// </summary>
		/// <param name="provider"></param>
		public void SetResettableEnumerableProvider(ITestElementInfoItemResettableEnumerableProvider<T> provider)
		{
			m_EnumerableProvider = provider;
		}
	}
}
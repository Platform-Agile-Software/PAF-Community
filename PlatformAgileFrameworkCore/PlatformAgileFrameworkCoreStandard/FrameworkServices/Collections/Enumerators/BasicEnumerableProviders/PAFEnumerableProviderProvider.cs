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
    /// Just wraps a <see cref="IPAFEnumerableProvider{T}"/>.
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
    /// base provider provider generally.
    /// </description>
    /// </contribution>
    /// </history>
    public class PAFEnumerableProviderProvider<T>: IPAFEnumerableProviderProvider<T>
    {
	    protected IPAFEnumerableProvider<T> m_Provider;
		public PAFEnumerableProviderProvider(IPAFEnumerableProvider<T> provider)
		{
			m_Provider = provider;
		}
		#region Properties
		/// <summary>
		/// Provides enumerator provider for items.
		/// </summary>
		public virtual IPAFEnumerableProvider<T> EnumerableProvider
		{ get { return m_Provider; } protected set { m_Provider = value; } }
		#endregion Properties
	}
}

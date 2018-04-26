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

namespace PlatformAgileFramework.Collections.Enumerators
{
    /// <summary>
    /// Allows enumeration of items with a provider provider.
    /// </summary>
    /// <threadsafety>
    /// Safe.
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 12dec2017 </date>
    /// <description>
    /// Factored this out of the testing stuff, since in rare cases we need a
    /// provider provider generally. Changed the name to IPAF..., just
    /// for consistency. Wasn't a name space issue, since there's probably
    /// nobody else using a provider provider, anyway....
    /// </description>
    /// </contribution>
    /// </history>
    public interface IPAFEnumerableProviderProvider<T>
	{
        #region Properties
		/// <summary>
		/// Provides enumerator for items.
		/// </summary>
		IPAFEnumerableProvider<T> EnumerableProvider { get; }
		#endregion Properties
	}
}

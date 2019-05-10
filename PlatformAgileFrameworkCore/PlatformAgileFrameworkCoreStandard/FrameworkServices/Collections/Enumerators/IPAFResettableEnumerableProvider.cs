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

using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Collections.Enumerators
{
    /// <summary>
    /// <para>
    /// This interface and its implementations are designed to solve
    /// problems with Microsoft's design of Iterators and the ambiguity of
    /// useage of enumerators. In our world, the enumerator cannot hold on
    /// to the last item in the iteration after the foreach loop has exited
    /// as do enumerators generated with the yield statement. Attempt to
    /// access the "Current" value after the enumerator has been disposed
    /// cannot be allowed, since we can't hold on to those items in our
    /// world. In the language specification, the behavior of "Current"
    /// after the enumerator has been disposed is undefined. Microsoft
    /// yield constructs will return the last value in the iteration. Our
    /// implementation throws an exception.
    /// </para>
    /// <para>
    /// Microsoft's implementation of the yield construct also causes the
    /// "Reset" method to throw an exception. Resetting an enumerable is
    /// important in our concurrency work. So, in order to resolve the
    /// ambiguity in the way hand-written versus yield-implemented enumerables
    /// work, we've defined this interface to provide a clear definition of
    /// what the concept of resetting means and to disallow the holding of
    /// the final value.
    /// </para>
    /// <para>
    /// The design puts the reset capability on this interface rather than
    /// the <see cref="IPAFResettableEnumerable{T}"/> interface. This allows for various
    /// strategies for factory-style patterns to regenerate fresh provided
    /// enumerables.
    /// </para>
    /// </summary>
    /// <typeparam name="T">Type that is to be enumerated.</typeparam>
    /// <threadsafety>
    /// Implementations should be thread-safe. The enumerators that are handed out
    /// by implementations are normally NOT expected to be thread-safe.
    /// </threadsafety>
    /// <history>
    /// <contribution>
    /// <author> KRM </author>
    /// <date> 14oct2012 </date>
    /// <description>
    /// Added history and better DOCs - people did not understand this interface.
    /// </description>
    /// </contribution>
    /// </history>
    public interface IPAFResettableEnumerableProviderProvider<T>
    {
		/// <summary>
		/// Gets the provider.
		/// </summary>
	    IPAFResettableEnumerableProvider<T> EnumerableProvider { get; }

	}
}
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

namespace PlatformAgileFramework.TypeHandling.Delegates
{
	/// <summary>
	/// Few helper methods for casting delegates. Taken from Ed Ball's stuff.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 07apr2013 </date>
	/// <desription>
	/// New. This has to live in ECMA, since <see cref="Delegate"/> is crippled in the profiles.
	/// </desription>
	/// </contribution>
	/// </history>
	public static class DelegateHelpers
	{
        /// <summary>
        /// Cast the specified delegate to another type of generic delegate.
        /// </summary>
        /// <returns>The constructed delegate.</returns>
        /// <param name="source">Source delegate.</param>
        /// <typeparam name="T">The type of delegate to construct.</typeparam>
		public static T Cast<T>(Delegate source) where T : class
		{
			return Cast(source, typeof(T)) as T;
		}

        /// <summary>
        /// Force the specified delegate to be another type of delegate.
        /// </summary>
        /// <returns>The constructed delegate.</returns>
        /// <param name="source">Source delegate.</param>
        /// <param name="type">Type of delegate to construct.</param>
		public static Delegate Cast(Delegate source, Type type)
		{
			if (source == null)
				return null;

			Delegate[] delegates = source.GetInvocationList();

			if (delegates.Length == 1)
				return Delegate.CreateDelegate(type,
					delegates[0].Target, delegates[0].Method);

			Delegate[] delegatesDest = new Delegate[delegates.Length];

			for (int nDelegate = 0; nDelegate < delegates.Length; nDelegate++)
				delegatesDest[nDelegate] = Delegate.CreateDelegate(type,
					delegates[nDelegate].Target, delegates[nDelegate].Method);

			return Delegate.Combine(delegatesDest);
		}
	}
}

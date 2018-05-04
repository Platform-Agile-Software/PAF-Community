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

namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// Provides an interface to internals for testing.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 29dec2017 </date>
	/// <description>
	/// New. Created as testing progressed.
	/// </description>
	/// </contribution>
	/// </history>
	internal interface IWeakableSubscriberStoreInternal<TDelegate>
		: IWeakableSubscriberStore<TDelegate>
		where TDelegate : class
	{
		#region Properties
		/// <summary>
		/// Get the interval for purging.
		/// </summary>
		int PurgeIntervalInMillisecondsInternal { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Gets number of subscribers.
		/// </summary>
		/// <param name="numAlive">
		/// Number alive. These are either strong subscribers or weak
		/// subscribers that are not collected.
		/// </param>
		/// <param name="numDead">
		/// Number dead. These are weak subscribers that have been collected.
		/// </param>
		/// <remarks>
		/// This must collect both tallys at once, since we are only given
		/// access to a "snapshot" of the collection at any instant. This
		/// the ONLY way to collect this information from a concurrent
		/// collection. Note, also, that subscribers may be dying during
		/// the process of observing them and afterward, so this method
		/// only returns numbers consistent with the subscribers as each
		/// one is observed. More could immediately die after this method
		/// returns. For testing purposes, the lifetime of subscribers must
		/// controlled by holding strong references to them externally
		/// (or not).
		/// </remarks>
		void GetNumSubscribersInternal(out int numAlive, out int numDead);
		/// <summary>
		/// Calls the internal purge method
		/// </summary>
		void PurgeDeadSubscribersInternal();
		/// <summary>
		/// Allows the set/reset of the purge interval.
		/// </summary>
		/// <param name="purgeIntervalInMilliseconds">The interval.</param>
		void SetPurgeIntervalInMillisecondsInternal(int purgeIntervalInMilliseconds);

		#endregion // Methods
	}
}
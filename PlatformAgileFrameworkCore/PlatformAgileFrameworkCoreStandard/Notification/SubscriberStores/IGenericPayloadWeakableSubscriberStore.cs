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
namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// <see cref="IWeakableSubscriberStore{TDelegate}"/>. This is the
	/// Generic extension for delegates carrying a Generic payload.
	/// </summary>
	/// <typeparam name="TPayload">
	/// An unconstrained Generic. We have a client use case for structs.
	/// </typeparam>
	/// <typeparam name="TDelegate">
	/// See	<see cref="IWeakableSubscriberStore{TDelegate}"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. Created for strongly-typed payloads.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPayloadWeakableSubscriberStore<TDelegate, TPayload>
		:IWeakableSubscriberStore<TDelegate>
		where TDelegate : class
	{
		#region Properties
		/// <summary>
		/// Sets the payload for the delegate.
		/// </summary>
		TPayload Payload { get; set; }
		/// <summary>
		/// Determines if the payload has ever been set. Needed since we support
		/// value type payloads.
		/// </summary>
		bool IsPayloadSet { get; }
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Clears the payload - needed for value types.
		/// </summary>
		void ClearPayload();
		/// <summary>
		/// This method broadcasts to subscribers and allows a payload to
		/// be pushed in.
		/// </summary>
		/// <param name="payload">
		/// Incoming payload to be OPTIONALLY used in the notification.
		/// </param>
		/// <remarks>
		/// Typical implementations will check whether a payload has been pushed into
		/// the implementing class and use that instead of the <paramref name="payload"/>
		/// for the notification.
		/// </remarks>
		void NotifySubscribers(TPayload payload);
		#endregion // Methods
	}
}
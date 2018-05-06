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

using System.ComponentModel;
using System.Runtime.CompilerServices;
using PlatformAgileFramework.Notification.SubscriberStores.EventSubscriberStores;
using PlatformAgileFramework.TypeHandling.TypeComparison.Comparators;

namespace PlatformAgileFramework.Notification.Helpers
{
	/// <summary>
	/// Helpers for notification.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 11jun2017 </date>
	/// <contribution>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe.
	/// </threadsafety>
	public static class NotifyOrRaiseExtensionMethods
	{
		/// <summary>
		/// Checks if a new property value is different from the old. Sets the value
		/// only when different, then notifies the subscribers in the store. This is
		/// implemented as an extension on the store to shift the responsibility
		/// out of a class implementing <see cref="INotifyPropertyChanged"/>. 
		/// There are many cases where we must already inherit from a specific
		/// base class, so we can't inherit from a special binding class like
		/// other frameworks are wont to force one to do. That type of design
		/// is a very poor architectural choice for a single-interitance language
		/// framework.
		/// </summary>
		/// <typeparam name="T">Type of the value.</typeparam>
		/// <param name="pcArgsStore">One of us.</param>
		/// <param name="oldValue">
		/// Reference to a value that is typically a class field.
		/// </param>
		/// <param name="newValue">
		/// Desired value for the item.
		/// </param>
		/// <param name="propertyName">
		/// Name of the property being set. This method is typically called from
		/// within a property setter. In this case, the correct property name is generated
		/// automatically.
		/// </param>
		/// <returns>
		/// True if the value was changed, false if the existing value matched the
		/// desired value.
		/// </returns>
		/// <threadsafety>
		/// Unsafe. <typeparamref name="T"/> has no contraints, so can't be accessed atomically.
		/// </threadsafety>
		public static bool NotifyOrRaiseIfPropertyChanged<T>(this IPropertyChangedEventArgsSubscriberStore pcArgsStore,
			// ReSharper disable once AnnotateCanBeNullParameter
			ref T oldValue, T newValue, [CallerMemberName]string propertyName = null)
		{
			if (PAFEquatable<T>.Instance.AreEqual(oldValue, newValue)) return false;
			oldValue = newValue;
			var eventArgs = new PropertyChangedEventArgs(propertyName);
			pcArgsStore.Payload = eventArgs;
			pcArgsStore.NotifySubscribers();
			return true;
		}
	}
}
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
	/// This interface just provides a means to grab the
	/// publishing class that may be using a subscriber store.
	/// Note that there is no Generic cover for this interface.
	/// This is because the notification source is always passed
	/// as object in the .Net event system. Specific characteristics
	/// must be discovered through reflection.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args support. Made this general
	/// for notifications.
	/// </description>
	/// </contribution>
	/// </history>
	public interface INotificationSourcedSubscriberStore
	{
		/// <summary>
		/// This is for a non-Generic publisher <see cref="object"/> argument.
		/// It refers to the object that published the notification.
		/// </summary>
		object NotificationSource { get; }
	}
}
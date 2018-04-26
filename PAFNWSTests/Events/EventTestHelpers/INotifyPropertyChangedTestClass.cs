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

using System;
using System.ComponentModel;

namespace PlatformAgileFramework.Events.EventTestHelpers
{
	/// <summary>
	/// <para>
	/// This is the protocol for the minimal information exchange
	/// between a notifier and a subscriber of a "property changed"
	/// occurrance. For optimal loose coupling, it's necessary
	/// to use interfaces, not classes or structs. No, not even
	/// abstract classes... If you bind to someone's abstract class,
	/// YOU ARE BOUND TO THEIR TECHNOLOGY FOREVER!
	/// </para>
	/// <para>
	/// Although the <see cref="PropertyChangedEventArgs"/> could have
	/// easily been made a Generic and incorporated the property value,
	/// in addition to the name, it does not. This is due to limitations
	/// of the UI framworks it was designed to support. Thus the notifier
	/// needs to provide the ability to query (get) its named properties to
	/// determine their current value. We must have the ability to subscribe
	/// to the change events and to then check those properties on the
	/// notifier. These things both have to be enabled on communicating
	/// entities wishing to broadcast and receive property change occurances.
	/// </para>
	/// </summary>
	/// <remarks>
	/// This interface is an example that supports our test classes.
	/// We are disposable, since some of our tests use strong references.
	/// </remarks>
	public interface INotifyPropertyChangedTestClass
		:INotifyPropertyChanged, IDisposable
	{
		/// <summary>
		/// Getter for the age.
		/// </summary>
		/// <remarks>
		/// We don't NECESSARILY expose the setter, since there are situations
		/// where changes should come ONLY in response to to change notifications
		/// received by the implementing class. 
		/// </remarks>
		int AnAge { get; }
		/// <summary>
		/// Getter for the name.
		/// </summary>
		/// <remarks>
		/// We don't NECESSARILY expose the setter, since there are situations
		/// where changes should come ONLY in response to to change notifications
		/// received by the implementing class. 
		/// </remarks>
		string Aname { get; }
	}
}
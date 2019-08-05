//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
namespace PlatformAgileFramework.Events
{
	/// <summary>
	/// Protocol for an event transmitting object that can receive
	/// timeoutss. This gets around the problem of the .Net
	/// event system not being able to handle the "undisciplined subscriber"
	/// problem. This interface when worn by the <see cref="object"/>
	/// in the <see cref="EventHandler"/> can be discovered at run time
	/// and be used to transmit an acknowledgement. This is key in gracefully
	/// extending the .Net event system with it's standard method signatures
	/// to cover, for example, distributed systems where subscribers may
	/// go offline. Subscribers may be disconnected by a publisher
	/// if they don't respond within a certain time or generate exceptions.
	/// This issue also becomes extremely important for publishing
	/// notifications on threads. Methods returning void are traditionally
	/// untestable and this overcomes this problem.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16jul2019 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Also see <see cref="IPAFEventAcknowledgementReceiver"/>.
	/// </remarks>
	public interface IPAFEventCallbackReceiver
	{
		/// <summary>
		/// Just pings the sender back.
		/// </summary>
		/// <param name="obj">
		/// Can be anything, but is often <see langword="null"/>. For remote
		/// communication, it obviously must be serializable.
		/// </param>
		void LogEventPing(object obj);
	}
}
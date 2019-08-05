//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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

#region Using Directives

#endregion

namespace PlatformAgileFramework.MultiProcessing.Tasking
{
	/// <summary>
	/// This is a protocol for helper class that deals with the issue of methods with async/await
	/// and many awaits which have no timeouts. This one has a payload.n of the event system.
	/// </summary>
	/// <typeparam name="T">Unconstrained Generic.</typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 14jul2019 </date>
	/// <description>
	/// New. Made little helper for async/await problems.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public interface ITimedOutTaskPayload<T>: ITimedOutTask
	{
		/// <summary>
		/// This will be the default value of <typeparamref name="T"/> if
		/// the method has timed out.
		/// </summary>
		T ReturnValue { get; set; }
	}
}

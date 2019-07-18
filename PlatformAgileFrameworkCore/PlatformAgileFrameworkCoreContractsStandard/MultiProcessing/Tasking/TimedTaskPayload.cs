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

using System.Threading.Tasks;

#endregion

namespace PlatformAgileFramework.MultiProcessing.Tasking
{
	/// <summary>
	/// This is a helper class that deals with the issue of methods with async/await
	/// and many awaits which have no timeouts. This is a payload that can be used
	/// in conjunction with a <see cref="Task{T}"/> to return both a timeout indication
	/// the original payload. This helper can be used when we don't want to throw exceptions
	/// inside an async method that is awaited.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 14jul2019 </date>
	/// <description>
	/// New. Made little helper for await/await problems.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	public class TimedOutTaskPayload<T>: ITimedOutTaskPayload<T>
	{
		/// <summary>
		/// This will be the default value of <typeparamref name="T"/> if
		/// the metho has timed out.
		/// </summary>
		public T ReturnValue { get; set; }
		/// <summary>
		/// If this value is <see langword="true"/>, the return value will not be valid.
		/// </summary>
		public bool TimedOut { get; set; }

	}
}

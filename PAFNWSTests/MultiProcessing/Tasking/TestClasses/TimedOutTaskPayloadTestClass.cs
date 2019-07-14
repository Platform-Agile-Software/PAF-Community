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
using System.Threading.Tasks;namespace PlatformAgileFramework.MultiProcessing.Tasking.TestClasses{
	/// <summary>	/// This class to test the  <see cref="TimedOutTaskPayload{T}"/>.	/// </summary>	/// <history>	/// <contribution>	/// <author> KRM </author>	/// <date> 14jul2019 </date>	/// <description>	/// New.	/// </description>	/// </contribution>	/// </history>    public class TimedOutTaskPayloadTestClass	{
		/// <summary>
		/// Provides a value type for testing.
		/// </summary>        public int IntElement { get; set; }

		/// <summary>
		/// Provides a reference type for testing.
		/// </summary>
        public string StringElement { get; set; }

		/// <summary>
		/// This is the delay in the return of the data types.
		/// </summary>
        public int DelayInMilliseconds { get; set; }
		/// <summary>
		/// A method with delay to fetch the int.
		/// </summary>
		/// <returns><see cref="Task{int}"/></returns>
		public Task<int> GetTheInt()
		{
			var tcs = new TaskCompletionSource<int>();

			Task.Delay(DelayInMilliseconds).ContinueWith
				((task, o) => { tcs.SetResult(IntElement); }, null);
			return tcs.Task;
		}
		/// <summary>
		/// A method with delay to fetch the string.
		/// </summary>
		/// <returns><see cref="Task{string}"/></returns>
		public Task<string> GetTheString()
		{
			var tcs = new TaskCompletionSource<string>();

			Task.Delay(DelayInMilliseconds).ContinueWith
				((task, o) => { tcs.SetResult(StringElement); }, null);
			return tcs.Task;
		}
	}}
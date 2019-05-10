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

using System.Collections.Generic;
namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Model is designed to add awareness of <see cref="IAsyncPayloadControlObject"/>
	/// so we can get a controller to run both work and control threads. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <description>
	/// Built this as a convenience for new stochastic tests.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IAsyncWorkControllerObject: IAsyncControllerObject
	{
		/// <summary>
		/// This is a list of <see cref="IAsyncWorkControlObject"/>s, which
		/// may be filtered from the controller's <see cref="IAsyncControlObject"/> children.
		/// </summary>
		IList<IAsyncWorkControlObject> WorkControlObjects { get; }
	}
}

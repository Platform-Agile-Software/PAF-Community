//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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

using System.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Interface supporting the PAF dispose pattern. This the internal part.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 29apr2012 </date>
	/// <contribution>
	/// Factored out of typehandling for use in core. Anybody doing .Net
	/// programming should have what's left in here.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Any implementations should NORMALLY be made thread-safe.
	/// </threadsafety>
	/// <remarks>
	/// Framework extenders can add members marked as
	/// <see cref="SecurityCriticalAttribute"/> to <see cref="IPAFDisposable"/>.
	/// </remarks>
	internal interface IPAFDisposableInternal: IPAFDisposable
	{
		#region Properties
		/// <summary>
		/// This property manipulates a delegate that is called from the <see cref="IPAFDisposable.PAFDispose"/>
		/// method. It can be installed dynamically during testing. This value can be <see langword="null"/>.
		/// In this case, a standard default behavior should be provided and documented in the
		/// implementation.
		/// </summary>
		/// <threadsafety>
		/// This property should be synchronized if there is any chance of accessing it
		/// after startup in an implementation.
		/// </threadsafety>
		PAFDisposerMethod PAFDisposeCallerInternal { get; set; }
		#endregion // Properties
	}
}


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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Simple type to identify a registrant.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 21apr2012 </date>
	/// <contribution>
	/// Factored out of type handling for use in core. Anybody doing .Net
	/// programming should have this.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// thread-safe. Derived classes would have to synchronize any set access.
	/// </threadsafety>
	/// <serialization>
	/// Not anticipated.
	/// </serialization>
	public class DisposalRegistrant
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Registrant's Guid.
		/// </summary>
		public Guid RegistrantGuid { get; protected set; }
		/// <summary>
		/// Registrant's Object.
		/// </summary>
		public IDisposable RegistrantDisposable { get; protected set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just builds with the props.
		/// </summary>
		/// <param name="registrantGuid">See <see cref="RegistrantGuid"/>.</param>
		/// <param name="registrantDisposable">See <see cref="RegistrantDisposable"/>.</param>
		public DisposalRegistrant(Guid registrantGuid, IDisposable registrantDisposable)
		{
			RegistrantGuid = registrantGuid;
			RegistrantDisposable = registrantDisposable;
		}
		#endregion // Constructors
	}
}


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

#region Using Directives
using System;
#endregion

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	/// Holds the data from the PAFAbort call plus an expiration time.
	/// </summary>
	public class ExpiringManagedThreadAbortObject: IExpiringManagedThreadAbortObjectInternal
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Contains the actual data payload.
		/// </summary>
		public object AbortObject { get; protected set; }
		/// <summary>
		/// Expiry. The object can be removed after this time.
		/// </summary>
		protected DateTime ExpiryInternal { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Just builds with the props
		/// </summary>
		/// <param name="abortObject">See <see cref="AbortObject"/>.</param>
		/// <param name="expiry">See <see cref="ExpiryInternal"/>.</param>
		public ExpiringManagedThreadAbortObject(object abortObject, DateTime expiry)
		{
			AbortObject = abortObject;
			ExpiryInternal = expiry;
		}

		#endregion // Constructors
		#region IExpiringManagedThreadAbortObjectInternal Implementation
		DateTime IExpiringManagedThreadAbortObjectInternal.Expiry
		{
			get { return ExpiryInternal; }
		}
		#endregion // IExpiringManagedThreadAbortObjectInternal Implementation
	}
}

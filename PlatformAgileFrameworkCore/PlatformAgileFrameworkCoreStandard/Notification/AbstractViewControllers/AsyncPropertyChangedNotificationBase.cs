//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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

using PlatformAgileFramework.Notification.Helpers;

namespace PlatformAgileFramework.Notification.AbstractViewControllers
{
	/// <summary>
	/// This is an asynchronous version of <see cref="PropertyChangedNotificationBase"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JWM(P) </author>
	/// <date>26jan2018 </date>
	/// <description>
	/// New. Took over refactoring view models.
	/// </description>
	/// </contribution>
	/// </history>
	public class AsyncPropertyChangedNotificationBase :
		PropertyChangedNotificationBase, IAsyncPropertyChangedNotificationBase
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		private bool m_Processing;
		/// <summary>
		/// Backing.
		/// </summary>
		private double m_FractionDone;
		#endregion // Fields and Autoproperties

		#region IAsyncProgressModel Implementation
		/// <summary>
		/// See <see cref="IAsyncProgressModel"/>
		/// </summary>
		public virtual bool Processing
		{
			get { return m_Processing; }
			set
			{
				PceStore.NotifyOrRaiseIfPropertyChanged(ref m_Processing, value);
			}

		}
		/// <summary>
		/// See <see cref="IAsyncProgressModel"/>
		/// </summary>
		public virtual double FractionDone
		{
			get { return m_FractionDone; }
			set
			{
				PceStore.NotifyOrRaiseIfPropertyChanged(ref m_FractionDone, value);
			}
		}
		#endregion IAsyncProgessModel Implementation
	}
}

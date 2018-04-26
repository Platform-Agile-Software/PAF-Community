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

using System.ComponentModel;
using PlatformAgileFramework.Annotations;

namespace PlatformAgileFramework.Events.EventTestHelpers
{
	/// <summary>
	/// This is a simple class with two properties to test the speed of conventional
	/// .Net event dispatch through the multicast delegate mechanism. We need
	/// this as a basis of comparison for our pseudodelagate approach.
	/// </summary>
	public class MulticastNotifyPropertyChangedTestClass : MulticastStandardBaseViewModel
	{
		#region Fields and Autoproperties
		/// <summary>
		/// This is a test node as the same type as us.
		/// </summary>
		public MulticastNotifyPropertyChangedTestClass m_MyTwoWayCommunicatingTestPartner;
		/// <summary>
		/// Backing...
		/// </summary>
		public string m_Aname;
		/// <summary>
		/// Backing...
		/// </summary>
		public int m_AnAge;
		/// <summary>
		/// For the test fixture.
		/// </summary>
		public int m_NumSetAttempts;
		/// <summary>
		/// For the test fixture. This one is set when the new prop
		/// value is different.
		/// </summary>
		public int m_NumSetSuccesses;
		#endregion // Fields and Autoproperties
		#region Properties
		/// <summary>
		/// Typical string property to test.
		/// </summary>
		public virtual string Aname
		{
			get { return m_Aname; }
			set
			{
				m_NumSetAttempts++;
				if (SetProperty(ref m_Aname, value))
					m_NumSetSuccesses++;
			}
		}
		/// <summary>
		/// Typical integer property to test.
		/// </summary>
		public virtual int AnAge
		{
			get { return m_AnAge; }
			set
			{
				m_NumSetAttempts++;
				if (SetProperty(ref m_AnAge, value))
					m_NumSetSuccesses++;
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// This is the receiver method for the property change occurrance broadcast.
		/// </summary>
		/// <param name="obj">Standard args for the event.</param>
		/// <param name="args">Standard args for the event.</param>
		public virtual void EventBroadcastReceiverMethod(object obj, [NotNull] PropertyChangedEventArgs args)
		{
			if (args.PropertyName == "Aname")
				Aname = m_MyTwoWayCommunicatingTestPartner.Aname;
			if (args.PropertyName == "AnAge")
				AnAge = m_MyTwoWayCommunicatingTestPartner.AnAge;
		}

		/// <summary>
		/// This method allows us to subscribe to property changed occurrances
		/// from another "node" in PAF-speak and to query it for actual updated
		/// property values. This is called by some external coordinating mechanism
		/// and the same method would be called on our peer to hook it up to us.
		/// In UI applications, this is ordinarily performed by an MVC-style framework.
		/// </summary>
		/// <param name="myPeer">
		/// Another node that I want to receive notifications from.
		/// </param>
		public virtual void OneWayBindToMyPeer(MulticastNotifyPropertyChangedTestClass myPeer)
		{
			m_MyTwoWayCommunicatingTestPartner = myPeer;
			m_MyTwoWayCommunicatingTestPartner.PropertyChanged += EventBroadcastReceiverMethod;
		}
		#region IDisposable Implementation
		/// <summary>
		/// Not the correct canonical dispose pattern, but we don't need it for
		/// this test class.
		/// </summary>
		public virtual void Dispose()
		{
			if (m_MyTwoWayCommunicatingTestPartner != null)
				m_MyTwoWayCommunicatingTestPartner.PropertyChanged -= EventBroadcastReceiverMethod;
		}
		#endregion //IDisposable Implementation
		#endregion //Methods

	}
}

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
using System.Threading.Tasks;
using PlatformAgileFramework.MultiProcessing.Threading.Delegates;
// ReSharper disable StaticMemberInGenericType
namespace PlatformAgileFramework.StochasticTestHelpers
{
	/// <summary>
	/// Closure of the delegator for delegate calling on a timer. We are doing this
	/// the old-fashioned way so we can also run this on our legacy threading
	/// infrastructure. This class is primarily designed for "stochastic" testing.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 24mar2019 </date>
	/// <description>
	/// New. Built to test calls on multiple threads.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not thread-safe. The method exposed by this class is designed to be
	/// called by a single thread.
	/// </threadsafety>
	public class TimedCallDelegator<T> :
		ParameterizedThreadStartMethodDelegator<T>
		where T: class
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// We need a settable seed so we can regenerate a particular concurrency
		/// pattern that may have caused a thread collision. For a full (long-running)
		/// test, the test is typically run with different seeds, multiple times. The
		/// default value of <see cref="int.MinValue"/> causes the number generator
		/// to be initialized according to TIME OF DAY.
		/// </summary>
		public readonly int m_RandomSeed;
		/// <summary>
		/// Random number generator for delays.
		/// </summary>
		protected readonly Random m_RandomNumberGenerator;
		/// <summary>
		/// Mask for the integer value of millisecond delay. Default setting
		/// gives 0 to 255 millisecond delay.
		/// </summary>
		public const int DEFAULT_MILLISECOND_DELAY_MASK = 255;
		/// <summary>
		/// Mask for the integer value of millisecond delay.
		/// </summary>
		public readonly int m_MillisecondDelayMask;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected object m_DelegateDelayCallerDefaultPayload;
		#endregion // Class Fields And Autoproperties
		#region Constructors
		/// <summary>
		/// Builds with a delegate and payload.
		/// </summary>
		/// <param name="contravariantThreadMethod">
		/// The delegate, which is typically called on a separate thread on each
		/// instance of this class.
		/// </param>
		/// <param name="payload">
		/// The payload, which is typically shared among instances in order to
		/// provoke thread collisions.
		/// </param>
		/// <param name="randomSeed">
		/// This is the seed used to initialize the state of the random number generator.
		/// Specify <see cref="int.MinValue"/> to initialize the number generator
		/// according to TIME OF DAY. Specify something else to create a re-producible
		/// sequence of delays.
		/// </param>
		/// <param name="defaultDelegatePayload">
		/// Loads <see cref="DelegateDelayCallerDefaultPayload"/>.
		/// </param>
		/// <param name="millisecondDelayMask">
		/// This is a bit mask that is applied to the integer-valued random number
		/// to set the maximum random delay in <see cref="DelegateDelayCaller"/>.
		/// For example, 255 will provide a random delay uniform on [0-255].
		/// </param>
		public TimedCallDelegator(
			Action<T> contravariantThreadMethod,
			T payload,
			object defaultDelegatePayload = null,
			int millisecondDelayMask = int.MinValue,
			int randomSeed = int.MinValue)
			:base(contravariantThreadMethod, payload)
		{
			m_DelegateDelayCallerDefaultPayload = defaultDelegatePayload;

			if (millisecondDelayMask != int.MinValue)
				m_MillisecondDelayMask = millisecondDelayMask;
			else
				m_MillisecondDelayMask = DEFAULT_MILLISECOND_DELAY_MASK;

			m_RandomSeed = randomSeed;

			if(m_RandomSeed != int.MinValue)
				m_RandomNumberGenerator = new Random(m_RandomSeed);
			else
				m_RandomNumberGenerator = new Random();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Default payload if argument to <see cref="DelegateDelayCaller"/>
		/// is <see langword="null"/>.
		/// </summary>
		public virtual object DelegateDelayCallerDefaultPayload
		{
			get => m_DelegateDelayCallerDefaultPayload;
			set => m_DelegateDelayCallerDefaultPayload = value;
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Delays a call to the delegate for a given time.
		/// </summary>
		/// <param name="payload">
		/// The Generic payload passed to the method.
		/// </param>
		public virtual void DelegateDelayCaller(object payload)
		{
			if (payload == null)
				payload = DelegateDelayCallerDefaultPayload;

			var delayInMilliseconds = m_RandomNumberGenerator.Next();
			delayInMilliseconds = delayInMilliseconds & m_MillisecondDelayMask;
			Task.Delay(delayInMilliseconds).Wait();
			WaitCallbackMethod(payload);
		}
		#endregion // Methods
	}
}

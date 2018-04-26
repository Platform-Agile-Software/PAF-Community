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
using PlatformAgileFramework.MultiProcessing.Threading.Locks;
using PlatformAgileFramework.MultiProcessing.Threading.NullableObjects;

namespace PlatformAgileFramework.Notification.SubscriberStores
{
	/// <summary>
	/// "typical" implementation of <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}"/>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 26dec2017 </date>
	/// <description>
	/// New. Created an implementation that suits our needs.
	/// </description>
	/// </contribution>
	/// </history>
	public abstract class WeakableSubscriberStore<TDelegate, TPayload>
		: WeakableSubscriberStore<TDelegate>, IPayloadWeakableSubscriberStore<TDelegate, TPayload>
		where TDelegate : class
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal NullableSynchronizedWrapper<TPayload> m_PayloadWrapper;
		#endregion Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// It would have been nice if we could have made a constructor
		/// that could accept the <typeparamref name="TPayload"/> as
		/// an argument. The problem is that we don't want to constrain
		/// the payload to be a class to be able to determine if it
		/// was not passed in and defaulting the payload in the case
		/// of a value type just won't do. So we create the nullable
		/// wrapper and specifically null it here. The notion of
		/// not <see langword="null"/> is one of the things that the
		/// nullable wrapper provides, when something is pushed into it.
		/// We push in a <c>default(T)</c> but then set the nullable
		/// wrapper to null, to indicate that nothing was ever loaded.
		/// One must load it with the <see cref="Payload"/> property or
		/// set it dynamically, on a per-call basis in
		/// <see cref="NotifySubscribers"/>.
		/// </summary>
		/// <param name="purgeIntervalInMilliseconds">
		/// See base.
		/// </param>
		protected WeakableSubscriberStore(int purgeIntervalInMilliseconds = -1)
			:base(purgeIntervalInMilliseconds)
		{
			m_PayloadWrapper
				= new NullableSynchronizedWrapper<TPayload>
					(default(TPayload), new ReaderWriterLockSlimWrapper());
			m_PayloadWrapper.NullTheObject();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}"/>
		/// </summary>
		public virtual TPayload Payload
		{
			get
			{
				using (var readablePayloadWrapper = m_PayloadWrapper.GetReadLockedObject())
				{
					var readablePayload = readablePayloadWrapper.ReadLockedNullableObject;
					return readablePayload;
				}
			}
			set
			{
				using (var readwriteablePayloadWrapper = m_PayloadWrapper.GetWriteLockedObject())
				{
					readwriteablePayloadWrapper.WriteLockedNullableObject = value;
				}
			}
		}

		/// <summary>
		/// <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}"/>
		/// </summary>
		public virtual bool IsPayloadSet
		{
			get{ return m_PayloadWrapper.HasValue(); }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}"/>
		/// </summary>
		public abstract void NotifySubscribers(TPayload payload);

		/// <summary>
		/// <see cref="IPayloadWeakableSubscriberStore{TDelegate,TPayload}"/>
		/// </summary>
		public virtual void ClearPayload()
		{
			m_PayloadWrapper.NullTheObject();
		}
		#endregion // Methods
	}
}
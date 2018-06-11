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
using System.Security;
using PlatformAgileFramework.MultiProcessing.Tasking;
using PlatformAgileFramework.MultiProcessing.Threading;
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// <para>
	/// Basic utility to support <see cref="IPAFDisposable"/>. This class acts
	/// as a surrogate disposer for other classes. This offers an advantage, since
	/// all logic and synchronization can be done CORRECTLY in one place and be reused
	/// by composition on arbitrary classes 
	/// </para>
	/// <para>
	/// There are two ways to attach this class as a surrogate. The first is to build
	/// it with a client that at least implements <see cref="IDisposable"/>. See
	/// <see cref="Client"/> for more information. The second way is to set the disposal
	/// delegate, <see cref="PAFDisposeCaller"/>. This delegate will be passed whatever
	/// <see cref="Client"/> the class is constructed with, perhaps even <see langword="null"/>,
	/// if no argument is needed.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// This is the type of an OPTIONAL securing key. This key is present to
	/// lock down the entire disposal process so that dispose methods cannot be
	/// called in a low-trust environment. A standard <see cref="DisposalRegistry"/>
	/// is provided in SLcore so that leaking objects (those that should be disposed
	/// but have not been) can be disposed before an AppDomain shuts down or an
	/// application shuts down. This registry uses <see cref="Guid"/> keys. The
	/// key is kept open in this class so it can be used with other security
	/// mechanisms.
	/// </typeparam>
	/// <history>
	/// <author> KRM </author>
	/// <date> 02may2012 </date>
	/// <contribution>
	/// Did the unsealed base/sealed subclass thing for the framework.
	/// </contribution>
	/// <author> BMC </author>
	/// <date> 29apr2012 </date>
	/// <contribution>
	/// Factored out of type handling for use in core. Anybody doing .Net
	/// programming should have this.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// thread-safe.
	/// </threadsafety>
	/// <remarks>
	/// Closed subclasses are often implemented as internal or private in
	/// order to keep them from being discovered through reflection in
	/// low-trust environments.
	/// Note that this class implements a finalizer only if "FINALIZER_ANALYSIS"
	/// is a defined constant.
	/// </remarks>
	public class PAFDisposerBase<T> : IPAFDisposableInternal,
		IUnprotectedDisposableProvider, IPAFDisposableDisposalClientProvider
	{
		#region Nested Class
		/// <summary>
		/// This class is designed to be a disposer for the outer class that
		/// the outer class can hand out to clients which are trusted. It calls
		/// into the version of the dispose method that is not marked
		/// <see cref="SecurityCriticalAttribute"/>.
		/// </summary>
		protected class LowTrustDisposerSource : IDisposable
		{
			#region Class Fields and Autoproperties
			/// <summary>
			/// Holds the outer class.
			/// </summary>
			protected PAFDisposerBase<T> m_Disposer;
			#endregion // Class Fields and Autoproperties
			#region Constructors
			/// <summary>
			/// Just buils with an instance of the outer class to get
			/// its methods.
			/// </summary>
			/// <param name="disposer">
			/// Instance of the outer.
			/// </param>
			protected internal LowTrustDisposerSource(PAFDisposerBase<T> disposer)
			{
				m_Disposer = disposer;
			}
			#endregion // Constructors
			/// <summary>
			/// Calls the unprotected method for dispose.
			/// </summary>
			public virtual void Dispose()
			{
				m_Disposer.UnprotectedDispose();
			}
		}

		#endregion // Nested Class
		#region Class Fields and Autoproperties
		/// <summary>
		/// Read/write of this primitive is atomic.
		/// </summary>
		private int m_1ForDisposed;
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		private object m_Client;
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal IPAFSecretKeyProviderInternal<T> m_DisposalKeyProvider;
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		protected PAFDisposerMethod m_PAFDisposeCaller;
		/// <summary>
		/// This is us as a surrogate client provider. The <see cref="DisposalRegistry"/>
		/// can call into us, then we call into our client.
		/// </summary>
		protected IPAFDisposableDisposalClientProvider m_UsAsClientProvider;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		///
		/// Default for inheritors.
		///
		protected internal PAFDisposerBase(){}
		/// <summary>
		/// Constructor builds with client and/or delegate. We throw an
		/// exception if both are <see langword="null"/>, since then the class can do
		/// nothing for the client.
		/// </summary>
		/// <param name="clientProvider">
		/// Client object provider. Not <see langword="null"/> and can't provide a <see langword="null"/>
		/// client. May also be a <see cref="IPAFSecretKeyProviderInternal{T}"/>
		/// if secure operations are desired. May be <see langword="null"/> if a
		/// <paramref name="disposalDelegate"/> is provided.
		/// </param>
		/// <param name="disposalDelegate">
		/// Delegate or <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// <paramref name="clientProvider"/> does not supply an <see cref="IDisposable"/>
		/// and <paramref name="disposalDelegate"/> is also <see langword="null"/>.
		/// "Nothing to do".
		/// </exception>
		/// </exceptions>
		public PAFDisposerBase(IPAFDisposalClientProvider clientProvider,
			PAFDisposerMethod disposalDelegate = null)
		{
			if(clientProvider != null)
				m_Client = clientProvider.DisposalClient;

			if((disposalDelegate == null) && (m_Client as IDisposable == null))
					throw new ArgumentException("Nothing to do");
			
			//// Being a bit sneaky with an optional interface.
			// Deal with the key if we have one.
			m_DisposalKeyProvider
				= PAFSecureDisposalClientProvider<T>.GetKeyProviderFromClientProvider
					(clientProvider);

			m_UsAsClientProvider = new PAFSecureDisposableDisposalClientProvider<T>
				(this, m_DisposalKeyProvider.GetSecretKeyInternal());

			// We must have a default delegate installed from the start so we
			// can chain it.
			m_PAFDisposeCaller = disposalDelegate ?? DefaultPAFDisposeCaller;
		}

		/// <summary>
		/// This constructor deals with our client and key directly to make it
		/// easier to construct the disposer.
		/// </summary>
		/// <param name="disposalClient">
		/// Used to construct a <see cref="PAFSecureDisposalClientProvider{T}"/>
		/// to pass into the other constructor.
		/// </param>
		/// <param name="secretKey">
		/// Used to construct a <see cref="PAFSecureDisposalClientProvider{T}"/>
		/// to pass into the other constructor.
		/// Can be "default(T)". This is considered no key at all.
		/// </param>
		/// <param name="disposalDelegate">
		/// See "PAFDisposerBase(IPAFDisposalClientProvider, PAFDisposerMethod)"
		/// </param>
		/// <remarks>
		/// The "PAFDisposerBase(IPAFDisposalClientProvider, PAFDisposerMethod)" constructor
		/// must be used if it is desired to pass in something other than a
		/// <see cref="PAFSecureDisposalClientProvider{T}"/>.
		/// </remarks>
		public PAFDisposerBase(T secretKey, object disposalClient = null,
			PAFDisposerMethod disposalDelegate = null)
			: this(new PAFSecureDisposalClientProvider<T>(disposalClient, secretKey), disposalDelegate)
		{
		}

		#endregion // Constructors
		#region Properties
		/// <summary>
		/// This is a reference to an object which this class uses in all calls
		/// to dispose its client. This may be <see langword="null"/> in cases where an
		/// argument is not needed. It is sometimes a reference to the client instance
		/// itself, but needn't be. See <see cref="DefaultPAFDisposeCaller"/> for
		/// details of how this object is used when no <see cref="PAFDisposeCaller"/>
		/// has been loaded. If the client has installed a custom <see cref="PAFDisposeCaller"/>,
		/// this object is passed into it as the second argument. So, if the client loads
		/// both of these, they must be consistent.
		/// </summary>
		protected internal virtual object Client
		{ get { return m_Client; } set { m_Client = value; } }

		/// <summary>
		/// This is a key that we pass to others to give them permission to
		/// dispose us. It is "default(T)" by default to indicate that
		/// no secret key protection is in use. It is passed in by the creator of the
		/// class and thus can be handed out to legitimate disposers.
		/// </summary>
		internal virtual IPAFSecretKeyProviderInternal<T> DisposalKeyProvider
		{
			get { return m_DisposalKeyProvider; }
		}
		/// <summary>
		/// Setter for the provider. We always hold an <see cref="IPAFSecretKeyProviderInternal{Guid}"/>
		/// internally, but hand out a <see cref="IPAFSecretKey"/>.
		/// </summary>
		/// <param name="providerInternal"></param>
		internal void SetDisposalKeyProvider(IPAFSecretKeyProviderInternal<T> providerInternal)
		{
			m_DisposalKeyProvider = providerInternal;
		}
		/// <summary>
		/// Backing for the explicit implementation. Not synchronized, since this is normally
		/// only set at startup.
		/// </summary>
		protected virtual PAFDisposerMethod PAFDisposeCaller
		{ get { return m_PAFDisposeCaller; } set { m_PAFDisposeCaller = value; } }
		#endregion // Properties
		#region Methods
		#region Static Helper Methods
		/// <summary>
		/// Standard implementation of the caller delegate.
		/// </summary>
		/// <param name="obj">
		/// Arbitrary object. This method's action depends on the nature
		/// of <paramref name="obj"/>.
		/// <list type="number">
		/// <item>
		/// <description>
		/// <see langword="null"/> returns without doing anything.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// <see cref="IPAFDisposable"/> will return without doing anything
		/// if <see cref="IPAFDisposable.IsPAFObjectDisposed"/> is true.
		/// </description>
		/// </item>
		/// <item>
		/// <description>
		/// <see cref="IDisposable"/> will call <see cref="IDisposable.Dispose"/>
		/// if <paramref name="disposing"/> is <see langword="true"/>.
		/// </description>
		/// </item>
		/// </list>
		/// </param>
		/// <param name="disposing">
		/// Should be <see langword="true"/> when called from <see cref="IDisposable.Dispose"/>.
		/// </param>
		/// <returns>
		/// An exception from <see cref="IPAFDisposable.PAFDispose"/> or its delegate is
		/// returned to the top, if, in fact, <paramref name="obj"/> is a
		/// <see cref="IPAFDisposable"/>.
		/// </returns>
		internal static Exception DefaultPAFDisposeCaller(bool disposing, object obj)
		{
			var pAFDisposable = obj as IPAFDisposable;
			if (pAFDisposable != null) {
				if (pAFDisposable.IsPAFObjectDisposed)
					return null;
				var disposalException = pAFDisposable.PAFDispose(disposing, obj);
				// Set the client's disposed flag in case they wanted to let us
				// do it.
// ReSharper disable GCSuppressFinalizeForTypeWithoutDestructor
// We don't care if the type has a finalizer. The call is
// lightweight - just flips a bit on the object.
				GC.SuppressFinalize(pAFDisposable);
// ReSharper restore GCSuppressFinalizeForTypeWithoutDestructor
				// Note - KRM. Placing this call to set here only works
				// if the client has not exposed its disposal methods to
				// the outside world, possibly permitting simultaneous
				// attempts at disposal on more than one thread. Oh, well,
				// we're not designing this thing for 1st graders, are we?
				pAFDisposable.SetPAFObjectDisposed(obj);
				return disposalException;
			}
			var disposable = obj as IDisposable;

			// Get outta' here if we're not at least IDisposable.
			if ((disposable == null) || (!disposing)) return null;
			// TODO - KRM How come we're not putting the disposal call in a try/catch
			// TODO block and returning the exception?
			disposable.Dispose();
// ReSharper disable GCSuppressFinalizeForTypeWithoutDestructor
			GC.SuppressFinalize(disposable);
// ReSharper restore GCSuppressFinalizeForTypeWithoutDestructor
			return null;
		}
		/// <summary>
		/// Little static helper that can be used by anybody to set their status
		/// to "Disposed". It just uses a lock to determine if the caller is the
		/// first to set the disposal flag.
		/// </summary>
		/// <param name="disposed">
		/// A by-reference integer that is set atomically to "1".
		/// </param>
		/// <returns>
		/// <see langword="true"/> if we were the first one to attempt to set the
		/// <paramref name="disposed"/> bit to "1".
		/// </returns>
		/// <remarks>
		/// The <paramref name="disposed"/> is always set to 1 when this method
		/// returns.
		/// </remarks>
		public static bool SetPAFObjectDisposedHelper(ref int disposed)
		{
			return ThreadingUtils.IsFirstSetBooleanInt(ref disposed);
		}
		#endregion // Static Helper Methods
		#endregion // Methods
		#region Implementation of IDisposable
		/// <summary>
		/// Dispose does the usual thing, but it is marked with
		/// <see cref="SecurityCriticalAttribute"/> to avoid calling
		/// in a low trust scenario.
		/// </summary>
		[SecurityCritical]
		public void Dispose()
		{
			UnprotectedDispose();
		}
		/// <summary>
		/// This is the unprotected version of <see cref="Dispose"/> that gets
		/// handed out by the <see cref="GetDisposeMethod"/>.
		/// </summary>
		protected virtual void UnprotectedDispose()
		{
			PAFDispose(true, Client);
		}
		#endregion
		#region IPAFDisposableInternal Implementation
		#region IPAFDisposable Implementation
		/// <summary>
		/// See <see cref="IUnprotectedDisposableProvider"/>.
		/// </summary>
		/// <param name="secretKey"></param>
		/// <returns></returns>
		public virtual DisposeMethod GetDisposeMethod(object secretKey)
		{
			if (!PAFSecretKeyProviderBase.KeysMatch(DisposalKeyProvider, secretKey)) return null;
			return UnprotectedDispose;
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		public bool IsPAFObjectDisposed
		{
			get { return m_1ForDisposed == 1; }
		}
		/// <summary>
		/// See <see cref="IPAFDisposable.PAFDispose"/>.
		/// </summary>
		/// <param name="disposing">
		/// See <see cref="IPAFDisposable.PAFDispose"/>.
		/// </param>
		/// <param name="obj">
		/// See <see cref="IPAFDisposable.PAFDispose"/>.
		/// </param>
		/// <remarks>
		/// This default virtual method employs the <see cref="PAFDisposeCaller"/>
		/// if it is loaded. If not, it calls <see cref="DefaultPAFDisposeCaller"/>.
		/// Override this method if there are ever any objects needing disposal
		/// on any subclasses of this class and don't forget to call base!
		/// </remarks>
		public virtual Exception PAFDispose(bool disposing, object obj)
		{
			// Secret key barrier.
			if (!PAFSecretKeyProviderBase.KeysMatch(DisposalKeyProvider, obj)) return null;
			// TODO diagnostic log/exception here.
			// TODO - KRM.  We actually probably want to place a security exception
			// TODO - KRM in the disposal registry and let the client decide what to
			// TODO - KRM do with other exceptions. Which is I guess what you meant - Duh.

			// See if we (and our client) have already been disposed.
			if (!SetPAFObjectDisposed(obj)) return null;

			// If we've got a caller installed, use it.
			if (PAFDisposeCaller != null) {
				return PAFDisposeCaller(disposing, obj);
			}
			return DefaultPAFDisposeCaller(disposing, obj);
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		/// <param name="obj">
		/// See <see cref="IPAFDisposable"/>.
		/// </param>
		/// <returns>
		/// <see cref="IPAFDisposable"/>
		/// </returns>
		/// <remarks>
		/// <para>
		/// This is a backing method. Override this one to change behavior in subclasses.
		/// We only go one way on this variable, so no need for retry loops or any other
		/// complex synchronization. An exchange works fine here.
		/// </para>
		/// <para>
		/// This method examines the <paramref name="obj"/> to see if it is a
		/// <see cref="IPAFSecretKey"/>. If it is, it's key must match the
		/// <see cref="DisposalKeyProvider"/>'s version on this class or
		/// the method simply returns without doing anything. 
		/// </para>
		/// </remarks>
		public virtual bool SetPAFObjectDisposed(object obj)
		{
			// Secret key barrier.
			if (!PAFSecretKeyProviderBase.KeysMatch(DisposalKeyProvider, obj)) return false;
			// TODO diagnostic log/exception here. Need "PAFSecurityException"
			return SetPAFObjectDisposedHelper(ref m_1ForDisposed);
		}
		#endregion // IPAFDisposable Implementation
		/// <summary>
		/// See <see cref="IPAFDisposableInternal"/>.
		/// </summary>
		PAFDisposerMethod IPAFDisposableInternal.PAFDisposeCallerInternal
		{
			get { return PAFDisposeCaller; }
			set { PAFDisposeCaller = value; }
		}
		#endregion // IPAFDisposableInternal Implementation
		#region IUnprotectedDisposableProvider Implementation
		/// <summary>
		/// See <see cref="IUnprotectedDisposableProvider"/>.
		/// </summary>
		/// <param name="secretKey">
		/// See <see cref="IUnprotectedDisposableProvider"/>.
		/// </param>
		/// <remarks>
		/// This method examines the <paramref name="secretKey"/> and returns
		/// <see langword="null"/> if there is not a match.
		/// </remarks>
		public virtual IDisposable GetUnprotectedDisposable(object secretKey)
		{
			// Secret key barrier.
			if (!PAFSecretKeyProviderBase.KeysMatch(DisposalKeyProvider, secretKey)) return null;
			// TODO diagnostic log/exception here. Need "PAFSecurityException"
			return new LowTrustDisposerSource(this);
		}
		#endregion // IUnprotectedDisposableProvider Implementation
		#region Finalizer
#if FINALIZER_ANALYSIS
		/// <summary>
		/// This finalizer simply calls our <see cref="PAFDispose"/> method
		/// with a <see langword="false"/> argument.
		/// </summary>
		[SecuritySafeCritical]
		~PAFDisposerBase()
		{
			PAFDispose(false, null);
		}
#endif
		#endregion // Finalizer
		#region Implementation of IPAFDisposalClientProvider
		/// <summary>
		/// We delegate to our internal provider of ourselves.
		/// </summary>
		public virtual object DisposalClient
		{
			get { return m_UsAsClientProvider.DisposalClient; }
		}
		#endregion
		#region Implementation of IPAFDisposableDisposalClientProvider
		/// <summary>
		/// We delegate to our internal provider of ourselves. This will
		/// return our own "Dispose()" method, which is attributed with
		/// <see cref="SecurityCriticalAttribute"/>.
		/// </summary>
		public virtual IDisposable DisposableDisposalClient
		{
			get { return m_UsAsClientProvider.DisposableDisposalClient; }
		}
		#endregion
	}
}


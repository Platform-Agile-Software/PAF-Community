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
using PlatformAgileFramework.MultiProcessing.Threading;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// <para>
	/// This is a class demonstrating the PAF finalizer pattern. Existing Microsoft
	/// "best practice" guidance promotes the use of a standard finalizer/dispose pattern.
	/// References abound. This guidance, while perhaps well-intended, results in dangerous
	/// and buggy code. In the majority of cases, a finalizer is called only when a programmer
	/// neglects to call <see cref="IDisposable.Dispose"/> when an object is no longer needed.
	/// The problem is that finalization is indeterminate. If there are, if fact, resources
	/// that need to be disposed (either managed or unmanaged) and they are not disposed,
	/// resource starvation can occur. To make matters worse, since the finalization times
	/// are indeterminate, this results in bugs that are exteremly difficult to track down.
	/// The use of "SafeHandle"'s does not address this problem, it only kicks a very rusty,
	/// very ugly can down the road.
	/// </para>
	/// <para>
	/// Under the PAF philosophy, the execution of a finalizer, in the manner in which
	/// Microsoft spins the tale, is indicative of a serious program error. It means that
	/// the proper logic has not been created to call <see cref="IDisposable.Dispose"/> on
	/// the object under all conditions. Finalizers are not totally, useless, however,
	/// since they provide an excellent instrumentation mechanism for testing. Under the
	/// PAF philosophy this is one of the few legitimate uses of finalizers. While an
	/// application is undergoing testing, the logic of the program (or class/whatever)
	/// should be evaluated to ensure that finalizers are NEVER called. To ensure this,
	/// a programmer must check to see that type members that are <see cref="IDisposable"/>
	/// are themselves disposed and otherwise see to it that all type instances that
	/// implement a finalizer are disposed when the type is no longer needed. Under test
	/// conditions, finalizers may be programmed to log the fact that they are called
	/// or throw an exception or provide some other indication in the test that the
	/// object has not been properly disposed in a deterministic fashion.
	/// </para>
	/// <para>
	/// This class demonstrates the direct use of the PAF Finalizer pattern. In practice,
	/// the use of a disposal surrogate such as <see cref="PAFDisposerBase{T}"/> is
	/// preferred, since it moves all of the disposal logic into the surrogate. Very
	/// little code is required in the client class to implement disposal through a
	/// surrogate. TODO - develop a simple example for this, perhaps from AsyncControlObject.
	/// </para>
	/// </summary>
	public class FinalizerDemonstrationClass : IPAFDisposableInternal
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// This field is not part of the dispose/finalization pattern necessarily.
		/// It is often used to indicate that dispose has already been called on this
		/// object. Good programming practice dictates that <see cref="IDisposable.Dispose"/>
		/// be callable multiple times and this is a practical way to implement the logic.
		/// Use of this variable can allow triggering of exceptions if clients attempt
		/// to use this object after it has been disposed.
		/// </summary>
		/// <threadsafety>
		/// <para>
		/// This is 4-byte field. Under NUMA rules, this field is read/written atomically.
		/// Noted that this CANNOT be made an auto-property, since the compiler will generate
		/// set/set methods for it, which are NOT atomic. The "volatile" keyword won't allow
		/// it anyway.
		/// </para>
		/// <para>
		/// Noted that placing a lock on the instance or using an atomic variable as is done
		/// here does NOT in any way solve all the disposal issues associated with a CLR type.
		/// The only thing it does is prevent two callers from trying to simultaneously dispose
		/// the instance. If other objects have references to this object or any of it's
		/// members, they must be either shut down or advised of the disposal of this object.
		/// Gates that throw exceptions are useful for identifying use of disposed objects
		/// during testing.
		/// </para>
		/// </threadsafety>
		private int m_1ForDisposed;
		/// <summary>
		/// Reference to myself.
		/// </summary>
		internal IPAFDisposableInternal MeAsMyInternals { get; set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Sets internal references and hooks the trivial caller method.
		/// </summary>
		public FinalizerDemonstrationClass()
		{
			MeAsMyInternals = this;
			// This statement is redundant, since the default caller method
			// does the same thing as ours.
			MeAsMyInternals.PAFDisposeCallerInternal
				= PAFDisposerBase<Guid>.DefaultPAFDisposeCaller;
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Never make this method virtual. The developer of any subclass must not be
		/// allowed to change the logic.
		/// </summary>
		[SecurityCritical]
		public void Dispose()
		{
			UnprotectedDispose();
		}
		/// <summary>
		/// This is the dispose method that is callable by untrusted clients. It
		/// is normally secured by a "secret key". This class does not demonstrate
		/// the use of a secret key.
		/// </summary>
		protected internal virtual void UnprotectedDispose()
		{
			// This call will mark the object to indicate that it is not necessary
			// for the runtime to call this type's finalize method. SuppressFinalize
			// simply sets a bit on the type (fast), so it can always be called, even if the
			// type does not implement a finalizer. It should be here in case a derived
			// CLASS does. This is not an issue for value types.
			GC.SuppressFinalize(this);
			//
			if (MeAsMyInternals.PAFDisposeCallerInternal != null)
				MeAsMyInternals.PAFDisposeCallerInternal(true, MeAsMyInternals);
			else MeAsMyInternals.PAFDispose(true, null);
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>. This is a virtual "backing" class for
		/// the explicit implementation.
		/// </summary>
		/// <param name="disposing">
		/// <see cref="IPAFDisposable"/>.
		/// </param>
		/// <param name="obj">
		/// <see cref="IPAFDisposable"/>.
		/// This is not used in this demonstration class.
		/// </param>
		/// <remarks>
		/// <see cref="IPAFDisposable"/>.
		/// When subclassing this class (or a class like it), this is the method that should
		/// be overridden. Obviously the designer of the subclass should keep in mind the order
		/// of resource disposal that should be followed and call the base at the appropriate
		/// point (usually after the subclass call, but not always). The call to the base
		/// implementation should be wrapped in a try/catch block.
		/// </remarks>
		public virtual Exception PAFDispose(bool disposing, object obj)
		{
			if (!MeAsMyInternals.SetPAFObjectDisposed(obj)) return null;
			if (disposing) {
				try {
					// Dispose or otherwise access some managed resource here.
				}
				// ReSharper disable EmptyGeneralCatchClause
				catch (Exception)
					// ReSharper restore EmptyGeneralCatchClause
				{
					// A logging call or something similar can be placed here.
				}
				try {
					// Dispose or otherwise access another managed resource here.
				}
				// ReSharper disable EmptyGeneralCatchClause
				catch (Exception)
					// ReSharper restore EmptyGeneralCatchClause
				{
					// A logging call or something similar can be placed here.
				}
			}
			// We can always access UNMANAGED resources here. This could be in the
			// form of an IntPtr member that we have captured when we created the
			// resource or an interop method call or even access to an
			// "unsafe" pointer. What we can't do is access anything that is on
			// the managed heap. The only exceptions are SafeHandles accessed from
			// non-SafeHandle classes.
			try {
				// Dispose or otherwise access some unmanaged resource here.
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
				// ReSharper restore EmptyGeneralCatchClause
			{
				// A logging call or something similar can be placed here.
			}
			try {
				// Dispose or otherwise access another unmanaged resource here.
			}
			// ReSharper disable EmptyGeneralCatchClause
			catch (Exception)
				// ReSharper restore EmptyGeneralCatchClause
			{
				// A logging call or something similar can be placed here.
			}
			// We can throw an exception or return an exception or terminate the
			// App here or record it in the disposal registry or decide to do nothing.
			return null;
		}
		#endregion // Methods
		#region Finalizer
#if FINALIZER_ANALYSIS
		/// <summary>
		/// Under PAF philospohy, a finalizer should never be called. Thus, finalizers
		/// are "auditable" through the use of the PAFDisposable infrastructure to determine
		/// when they are called. For testing and diagnostic purposes, finalizers can be
		/// installed in any type that implements <see cref="IDisposable"/> with a conditional
		/// compilation instruction such as "#if FINALIZER_ANALYSIS" compiler directives.
		/// If an object has not been properly disposed, the finalizer will be called either
		/// upon "AppDomain" unload or the termination of the application. If
		/// a mechanism has been devised to detect and report the execution of a finalizer,
		/// this mechanism must not depend on a type which is itself finalizable and does not
		/// depend on a <see cref="SafeHandle"/>. Using a logger which writes to a file is OK,
		/// since <see cref="FileStream"/> wraps a <see cref="SafeHandle"/>. This cannot be
		/// used to detect the execution of a finalizer on any type wrapping a <see cref="SafeHandle"/>,
		/// since the ordering of finalization is then non-deterministic.
		/// </summary>
		/// <remarks>
		/// This method is marked with the <see cref="SecuritySafeCriticalAttribute"/> so that it
		/// can be called from a non-critical context. There is no security risk here, since unpriviledged
		/// user code cannot call finalizers. The problem arises when this method is called from within
		/// "AppDomain"'s that may have low priviledges, such as from test frameworks
		/// that have specifically constructed the domains with low priviledge to test security issues.
		/// </remarks>
		[SecuritySafeCritical]
		~FinalizerDemonstrationClass()
		{
			if (MeAsMyInternals.PAFDisposeCaller != null)
				MeAsMyInternals.PAFDisposeCaller(false, MeAsMyInternals);
			else MeAsMyInternals.PAFDispose(false, null);
		}
#endif // FINALIZER_ANALYSIS
		#endregion // Finalizer
		#region IPAFDisposableInternal Implementation
		#region IPAFDisposable Implementation
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		/// <remarks>
		/// In most implementations, the <paramref name="obj"/> contains a "secret key"
		/// that is checked against the secret key that this class is built with.
		/// We keep this class simple to demonstrate just the implementation of the
		/// PAF dispose pattern.
		/// </remarks>
		public virtual DisposeMethod GetDisposeMethod(object obj)
		{
			return UnprotectedDispose;
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		public bool IsPAFObjectDisposed
		{
			get {return m_1ForDisposed > 0; }
		}
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		/// <returns>
		/// <see cref="IPAFDisposable"/>
		/// </returns>
		/// <remarks>
		/// We only go one way on this variable, so no need for retry loops or any other
		/// complex synchronization. An exchange works fine here.
		/// </remarks>
		public virtual bool SetPAFObjectDisposed(object obj)
		{
			return ThreadingUtils.IsFirstSetBooleanInt(ref m_1ForDisposed);
		}
		#endregion // IPAFDisposable Implementation
		#region Properties
		/// <summary>
		/// <see cref="IPAFDisposable"/>
		/// </summary>
		PAFDisposerMethod IPAFDisposableInternal.PAFDisposeCallerInternal { get; set; }
		#endregion // Properties
		#region Methods
		#endregion // Methods
		#endregion // IPAFDisposableInternal Implementation
	}
}


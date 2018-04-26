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

#region Using Directives

using System;
using System.Threading;
using System.Threading.Tasks;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.Manufacturing;
using CED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFConstructorExceptionData;

#endregion

namespace PlatformAgileFramework.MultiProcessing.Threading
{
	/// <summary>
	/// This class implements simple utilities that assist in managing threads.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <description>
	/// <author> JAW(P) </author>
	/// <date> 17jun2015 </date>
	/// Redid stuff for Task-oriented operation. Removed references to Thread.
	/// Kept DOCs the same to indicate thread operations needed.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 27jun2011 </date>
	/// <description>
	/// Unified the "constructionn lock" replicated in various places and put
	/// it here.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
// ReSharper disable PartialTypeWithSinglePart
	public partial class ThreadingUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Some Static Data
		/// <summary>
		/// Name of the main thread for ID purposes.
		/// </summary>
		public static readonly string MainThreadName = "MainThread";
		/// <summary>
		/// Message issued for normal shutdown.
		/// </summary>
		public static readonly string NormalShutDownMessage = "Stopping Process";
		#endregion // Some Static Data
		#region Utility Methods
		/// <summary>
		/// Works with an integer that is used to act as a <see cref="Boolean"/>. A reference
		/// to the int is passed in. We attempt to clear it (set the int to 0) and and
		/// a method from the <see cref="Interlocked"/> class is used to determine if we
		/// were the first one to clear it.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if this thread was the first one to clear it.
		/// </returns>
		public static bool IsFirstClearedBooleanInt(ref int oneForTrue)
		{
			var oneForFirstCleared = Interlocked.Exchange(ref oneForTrue, 0);
			return oneForFirstCleared == 1;
		}
		/// <summary>
		/// Works with an integer that is used to act as a <see cref="Boolean"/>. A reference
		/// to the int is passed in. We attempt to set it (set the int to 1) and
		/// a method from the <see cref="Interlocked"/> class is used to determine if we
		/// were the first one to set it.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if this thread was the first one to set it.
		/// </returns>
		public static bool IsFirstSetBooleanInt(ref int oneForTrue)
		{
			var zeroForFirstSet = Interlocked.Exchange(ref oneForTrue, 1);
			return zeroForFirstSet == 0;
		}
		/// <summary>
		/// This method provides thread-safe "lazy construction" of a shared variable. The
		/// variable is normally a class field, but could be anything. The method requires
		/// references to the type instance to be constructed and an integer flag to indicate whether
		/// the type's construction and initialization is complete.
		/// </summary>
		/// <param name="isInitializing">
		/// Indicates whether the type instance is being initialized (non-zero) or not.
		/// </param>
		/// <param name="constructedTypeInstance">
		/// The instance of <typeparamref name="T"/> that will be constructed and initialized
		/// by this method.
		/// </param>
		/// <param name="constructionParameters">
		/// The parameter object that is to be passed to the <paramref name="cDelegate"/>.
		/// </param>
		/// <param name="cDelegate">
		/// Delegate that will build and possibly initialize the type instance. This delegate
		/// may perform a simple action like calling a constructor for the type. However,
		/// it can really do anything it wants, including setting <paramref name="constructedTypeInstance"/>
		/// on the object on which it lives. The only caveat is that if it also clears 
		/// <paramref name="isInitializing"/>, all threads have access to <paramref name="constructedTypeInstance"/>
		/// at that time. Obviously, the delegate should be called from nowhere except
		/// this method.
		/// </param>
		/// <typeparam name="T">
		/// The reference type that is to be constructed.
		/// </typeparam>
		/// <typeparam name="U">
		/// The parameters that are needed to construct the type.
		/// </typeparam>
		/// <returns>
		/// The constructed type. Never <see langword="null"/>.
		/// </returns>
		/// <exceptions>
		/// No exceptions are caught. <paramref name="cDelegate"/> may or may not
		/// throw exceptions.
		/// <exception> <see cref="PAFStandardException{T}"/> is thrown if
		/// there is a failure to contruct the type, <typeparamref name="T"/>.
		/// <see cref="CED.FAILED_TO_CONSTRUCT_TYPE"/>.
		/// </exception>
		/// </exceptions>
		public static T LazyConstructionLock<T, U>(ref int isInitializing, ref T constructedTypeInstance,
			U constructionParameters, ManufacturingDelegates.ReferenceTypeConstructionDelegate<T,U> cDelegate)
			where T : class
		{
			// We keep the threads bouncing around in here until the object is ready for
			// consumption.
			while (true) {
				// If we are constructed and complete, just hand ourselves out.
				if ((constructedTypeInstance != null) && (isInitializing == 0)) return constructedTypeInstance;
				// Establish a thread barrier for any thread but the initializing thread.
				if (Interlocked.CompareExchange(ref isInitializing, 1, 0) == 1) {
					if (constructedTypeInstance == null) {
						// If we are here, it means that we are the first thread that requested
						// the type. We will be the one to construct the type while
						// the others wait. We are the "initializing" thread.
						var delegateConstructedType = cDelegate(constructionParameters);
						if (delegateConstructedType == null)
						{
							var data = new CED(typeof(T));
							throw new PAFStandardException<CED>(data, PAFConstructorExceptionMessageTags.FAILED_TO_CONSTRUCT_TYPE);
						}
						// The delegate may have already constructed and set the type instance behind
						// the scenes. If so, we don't reset it.
						if (constructedTypeInstance == null) constructedTypeInstance = delegateConstructedType;
					}
					// Clear the flag to signal we are ready. At this point, the flag may
					// have already been cleared in the cDelegate, so the flag is AT LEAST
					// cleared after this line.
					isInitializing = 0;
				}
				else {
					// If we are here, this means another thread has already called the
					// constructor and we must wait until we have a completely constructed
					// object. We MUST place a Sleep(1) here, since it gives a lower priority
					// thread a chance to run. Since the initializing thread might be of lower
					// priority, this avoids a livelock.
					Task.Delay(1).Wait();
				}
			}
		}
		#endregion // Utility Methods
	}
}

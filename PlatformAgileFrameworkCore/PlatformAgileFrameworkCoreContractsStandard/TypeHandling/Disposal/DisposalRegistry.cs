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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.Security;
using PlatformAgileFramework.TypeHandling.Disposal.Exceptions;

#region Exception Shorthand
// ReSharper disable IdentifierTypo
using IPAFDED = PlatformAgileFramework.TypeHandling.Disposal.Exceptions.IPAFDisposalExceptionData;
using PAFDED = PlatformAgileFramework.TypeHandling.Disposal.Exceptions.PAFDisposalExceptionData;
// ReSharper restore IdentifierTypo
#endregion // Exception Shorthand


namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// <para>
	/// Basic utilities to support disposal of static classes. The registry can
	/// actually be used for any class implementing <see cref="IDisposable"/>. The
	/// need for this mechanism is greatest, however, for static classes that have
	/// resources needing disposal during shutdown or AppDomain unload. Static classes
	/// can hold a nested subclass that derives from <see cref="PAFDisposerBase{T}"/>, for
	/// example that will act as a disposer for the static class.
	/// </para>
	/// <para>
	/// This mechanism was originally designed for retrofit of existing static classes
	/// that were not always being disposed. An application developer can easily retrofit
	/// an existing static class with this disposal mechanism without breaking existing
	/// code that uses the static class. Is this just kicking the can down the road? Yes and
	/// no. The developer can retrofit the class and register it, then any application
	/// builders need only to remember to call the <see cref="DisposeRegistrantsInternal"/> method
	/// on this class. The other advantage is that it's a bit easier to have discipline
	/// enforced on the CREATION side of the process by registering classes for disposal
	/// and have reports generated if classes are NOT disposed. Otherwise. it's hard
	/// to force the discipline at app shutdown time, since one has no control over how
	/// one's infrastructure is used.
	/// </para>
	/// <para>
	/// This mechanism has also been used to dispose singletons. The problem with
	/// disposable singletons is that <see cref="IDisposable"/> is a public interface
	/// open for all to access. In a SOA application, the only singleton should really
	/// be the service manager, but retrofit of existing singleton classes on legacy apps
	/// is often desired. 
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 15mar2019 </date>
	/// <description>
	/// Finished the "to do"s so this thing was ready for prime time.
	/// Made some additions so the thing was more usable and testable.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21nov2011 </date>
	/// <description>
	/// Converted from GL core for the SL4 rework.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// thread-safe.
	/// </threadsafety>
	// ReSharper disable once PartialTypeWithSinglePart
	//// core part.
	public static partial class DisposalRegistry
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Dictionary to hold exceptions by type. We don't hold
		/// references to instances so we don't prevent disposers
		/// from being garbage-collected. All the information we
		/// need is in the exception.
		/// </summary>
		private static readonly IDictionary<Type, IList<Exception>> s_Exceptions;
		/// <summary>
		/// List to hold registrants.
		/// </summary>
		private static readonly IList<DisposalRegistrant> s_Registrants;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Sets up collections.
		/// </summary>
		static DisposalRegistry()
		{
			s_Registrants = new List<DisposalRegistrant>();
			s_Exceptions = new Dictionary<Type, IList<Exception>>();
		}
		#endregion // Constructors
		/// <summary>
		/// Allows a client to record an exception occurring during disposal. Good for fielded
		/// applications that can survive with some exceptions, but where these need to
		/// be logged for analysis.
		/// </summary>
		/// <param name="recorder">
		/// Not <see langword="null"/>. Type/instance of the class recording the exception.
		/// </param>
		/// <param name="ex">
		/// Exception to register. Not <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"><c>recorder</c></exception>
		/// <exception cref="ArgumentNullException"><c>ex</c></exception>
		/// </exceptions>
		public static void RecordDisposalException(Type recorder, Exception ex)
		{
			if (recorder == null) throw new ArgumentNullException(nameof(recorder));
			if (ex == null) throw new ArgumentNullException(nameof(ex));

			lock (s_Exceptions)
			{
				if (!s_Exceptions.ContainsKey(recorder))
					s_Exceptions.Add(recorder, new List<Exception>());

				s_Exceptions[recorder].Add(ex);
			}
		}
		/// <summary>
		/// This method disposes a single registrant which is looked up by
		/// its registered Guid.
		/// </summary>
		/// <param name="guid">
		/// Filter to pick out just one disposer if not <c>default(Guid)</c>.
		/// Default does nothing.
		/// </param>
		/// <exceptions>
		/// No exceptions are thrown. Any exceptions occurring in disposal are logged
		/// with level <see cref="PAFLoggingLevel.Error"/> after they are wrapped in a
		/// <see cref="PAFStandardException{IPAFDED}"/> with error message
		/// <see cref="PAFDisposalExceptionMessageTags.ERROR_DISPOSING_TYPE"/>.
		/// </exceptions>
		[SecuritySafeCritical]
		public static void DisposeRegistrant(Guid guid)
		{
			if (guid == default(Guid))
				return;
			DisposeRegistrantsInternal(guid);
		}
		/// <summary>
		/// This method disposes all of the registrants in reverse order of how they
		/// were registered. This is done to take into account the fact that some static
		/// classes are dependent on others, even in the disposal process. If this is
		/// the case, an application can typically call a method that "touches" static
		/// classes or otherwise initializes them in a certain order and they will be
		/// disposed in the reverse order.
		/// </summary>
		/// <param name="guid">
		/// Optional filter to pick out just one disposer if not <c>default(Guid)</c>.
		/// Default picks out all unsecured disposers.
		/// </param>
		/// <param name="obj">
		/// Instance for non-static or Type for a static. This is a further
		/// filter on disposers. Only the disposer for the instance or Type
		/// will be invoked.
		/// </param>
		/// <exceptions>
		/// No exceptions are thrown. Any exceptions occurring in disposal are logged
		/// with level <see cref="PAFLoggingLevel.Error"/> after they are wrapped in a
		/// <see cref="PAFStandardException{IPAFDED}"/> with error message
		/// <see cref="PAFDisposalExceptionMessageTags.ERROR_DISPOSING_TYPE"/>.
		/// </exceptions>
		[SecurityCritical]
		internal static void DisposeRegistrantsInternal(Guid guid = default(Guid), object obj = null)
		{


			Guid? guidFilter = null;
			if (guid != default(Guid))
				guidFilter = guid;

			if (s_Registrants == null) return;
			lock (s_Registrants)
			{
				foreach (var registrant in s_Registrants)
				{
					if (guidFilter.HasValue && (registrant.RegistrantGuid != guidFilter.Value))
						continue;

					// Have we already been disposed?
					if (registrant.Registrant == null)
						continue;

					Exception caughtException = null;


					try
					{
						var typeOfRegistrant = registrant.Registrant.GetType();

						// If registrant implements PAF interface, take advantage of it.
						if (registrant.Registrant is IPAFDisposable pafDisposable)
						{
							pafDisposable.PAFDispose(true, registrant.Registrant);
						}
						else if (registrant.Registrant is IDisposable disposable)
						{
							// We are just a regular IDisposable.
							disposable.Dispose();
						}
						else if (registrant.Registrant is Action action)
						{
							// Nope, we are just a delegate.
							action();
						}
					}
					catch (Exception ex)
					{
						caughtException = ex;
					}
					finally
					{
						var typeOfRegistrant = registrant.Registrant.GetType();
						// Don't hold a reference in memory.
						registrant.Registrant = null;
						if (caughtException != null)
						{
							RecordDisposalException(typeOfRegistrant, caughtException);
							var exceptionData
								= new PAFDED(typeOfRegistrant, caughtException, PAFLoggingLevel.Error);
							var exception = new PAFStandardException<IPAFDED>(exceptionData,
								PAFDisposalExceptionMessageTags.ERROR_DISPOSING_TYPE);
							var logger = PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFLoggingService>();
							logger.LogEntry(PAFDisposalExceptionMessageTags.ERROR_DISPOSING_TYPE, PAFLoggingLevel.Error, exception);
						}
					}
				}
			}
		}
		/// <summary>
		/// Returns a snapshot of the exception dictionary.
		/// </summary>
		/// <returns>
		/// The dictionary.
		/// </returns>
		[NotNull]
		internal static IDictionary<Type, IList<Exception>> GetExceptions()
		{
			lock (s_Exceptions)
			{
				if (s_Exceptions.SafeCount() == 0) return null;
				var exceptions = new Dictionary<Type, IList<Exception>>();

				// We do a special procedure here, since thread-safety is not maintained if
				// we just copy list references.
				foreach (var entry in s_Exceptions.PAFToArray())
				{
					// We make an EXACT copy of the dictionary, even though, at
					// the time of this writing, the dictionary should not have
					// any vacuous entries, by construction.
					IList<Exception> newList = null;

					// If we had a null, leave the null.
					if (entry.Value != null)
					{
						// Not null, so recreate the list items.
						// Copying references to individual list items maintains thread safety.
						newList = entry.Value.ToList();
					}

					var newEntry = new KeyValuePair<Type, IList<Exception>>(entry.Key, newList);

					exceptions.Add(newEntry.Key, newEntry.Value);
				}

				return exceptions;
			}
		}
		/// <summary>
		/// This method was added so someone could traverse registrants in an arbitrary
		/// order (still unsure why).
		/// </summary>
		/// <returns>
		/// A list in the original installed order of the registrants. This means that
		/// the last registrant would be seen first in the list. If there are no registrants,
		/// <see langword="null"/> is returned.
		/// </returns>
		internal static IList<DisposalRegistrant> GetRegistrants()
		{
			lock (s_Registrants)
			{
				// TODO DAP - check order in Card Display app.....
				if (s_Registrants.SafeCount() == 0) return null;
				var registrants = new List<DisposalRegistrant>(s_Registrants);
				return registrants;
			}
		}
		/// <summary>
		/// Registers the disposable object for disposal.
		/// </summary>
		/// <param name="clientProvider">
		/// Not <see langword="null"/>. If not a <see cref="IPAFSecretKeyProviderInternal{T}"/>,
		/// a random <see cref="Guid"/> is generated and used as a key. 
		/// </param>
		/// <returns>
		/// The input Guid if not the empty Guid and a randomly-generated Guid
		/// if it is.
		/// </returns>
		public static Guid RegisterForDisposal(IPAFDisposableDisposalClientProvider clientProvider)
		{
			Guid returnedGuid;
			var secretKeyProvider = clientProvider as IPAFSecretKeyProviderInternal<Guid>;
			if (secretKeyProvider == null)
			{
				returnedGuid = Guid.NewGuid();
			}
			else
			{
				returnedGuid = secretKeyProvider.GetSecretKeyInternal();
			}
			RegisterForDisposal(new DisposalRegistrant(returnedGuid, clientProvider.DisposableDisposalClient));
			return returnedGuid;
		}
		/// <summary>
		/// Registers the disposable object for disposal.
		/// </summary>
		/// <param name="action">
		/// Delegate called to do the disposal. Not <see langword="null"/>.
		/// </param>
		/// <param name="secretKey">
		/// If <see langword="null"/>, a <see cref="Guid"/> key for disposal.
		/// </param>
		/// <returns>
		/// The input Guid if not the empty Guid and a randomly-generated Guid
		/// if it is.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="action"/>.
		/// </exception>
		/// </exceptions>
		public static Guid RegisterForDisposal(Action action, Guid? secretKey = null)
		{
			if (action == null) throw new ArgumentNullException(nameof(action));
			Guid returnedGuid;
			if (secretKey == null)
			{
				returnedGuid = Guid.NewGuid();
			}
			else
			{
				returnedGuid = secretKey.Value;
			}

			RegisterForDisposal(new DisposalRegistrant(returnedGuid, action));
			return returnedGuid;
		}
		/// <summary>
		/// Registers the registrant for disposal.
		/// </summary>
		/// <param name="registrant">
		/// Registrant. Not <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="registrant"/>.
		/// </exception>
		/// </exceptions>
		public static void RegisterForDisposal(DisposalRegistrant registrant)
		{
			if (registrant == null) throw new ArgumentNullException(nameof(registrant));
			// Not high traffic - monitor is fine.
			lock (s_Registrants)
			{
				s_Registrants.Add(registrant);
			}
		}
	}
}

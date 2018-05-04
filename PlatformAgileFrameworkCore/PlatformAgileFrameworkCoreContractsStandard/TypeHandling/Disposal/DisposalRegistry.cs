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
using System.Collections.Generic;
using System.Threading;
using PlatformAgileFramework.Security;

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
	/// builders need only to remember to call the <see cref="DisposeRegistrants"/> method
	/// on this class.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21nov2011 </date>
	/// <contribution>
	/// Converted from GL core for the SL4 rework.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// thread-safe.
	/// </threadsafety>
	public static class DisposalRegistry
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// List to hold registrants.
		/// </summary>
		private static readonly IList<DisposalRegistrant> m_Registrants;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Sets up list of registrants.
		/// </summary>
		static DisposalRegistry()
		{
			m_Registrants = new List<DisposalRegistrant>();
		}
		#endregion // Constructors

		/// <summary>
		/// Allows a client to record an exception occurring during disposal. Good for fielded
		/// applications that can survive with some exceptions, but where these need to
		/// be logged for analysis.
		/// </summary>
		/// <param name="recordingInstanceOrType">
		/// Not <see langword="null"/>. For a static class, this is its type. For a non-static
		/// class, this is an instance.
		/// </param>
		/// <param name="ex">
		/// Exception to register.
		/// </param>
		public static void RecordDisposalException(object recordingInstanceOrType, Exception ex)
		{
			// TODO Implement me!!
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
			if (secretKeyProvider == null) {
				returnedGuid = Guid.NewGuid();
			}
			else {
				returnedGuid = secretKeyProvider.GetSecretKeyInternal();
			}
			// Not high traffic - monitor is fine.
			lock(m_Registrants)
			{
				m_Registrants.Add
					(new DisposalRegistrant(returnedGuid, clientProvider.DisposableDisposalClient));
			}
			return returnedGuid;
		}
		/// <summary>
		/// This method disposes all of the registrants in reverse order of how they
		/// were registered. This is done to take into account the fact that some static
		/// classes are dependent on others, even in the disposal process. If this is
		/// the case, an application can typically call a method that "touches" static
		/// classes or otherwise initializes them in a certain order and they will be
		/// disposed in the reverse order.
		/// </summary>
		internal static void DisposeRegistrants()
		{
			if (m_Registrants == null) return;
			lock(m_Registrants)
			{
				foreach (var registrant in m_Registrants)
				{
					// Awaiting logging.
// ReSharper disable TooWideLocalVariableScope
					IPAFDisposable pafDisposable;
// ReSharper disable RedundantAssignment
					IDisposable disposable = null;
// ReSharper restore RedundantAssignment
// ReSharper restore TooWideLocalVariableScope
					try {
						pafDisposable = registrant.RegistrantDisposable as IPAFDisposable;
						// If registrant implements PAF interface, take advantage of it.
						if (pafDisposable != null)
						{
							pafDisposable.PAFDispose(true, registrant.RegistrantDisposable);
						}
						else
						{
							// Nope, we are just a regular IDisposable
							disposable = registrant.RegistrantDisposable;
							disposable.Dispose();
						}
					}
// ReSharper disable EmptyGeneralCatchClause
					catch
// ReSharper restore EmptyGeneralCatchClause
					{
						// TODO DAP put logging here when SL4 logger done.
					}
				}
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
			lock (m_Registrants)
			{
				// TODO DAP - check order in Card Display app.....
				if ((m_Registrants == null) || (m_Registrants.Count == 0)) return null;
				var registrants = new List<DisposalRegistrant>(m_Registrants);
				return registrants;
			}
		}
	}
}


using System.Security;
using PlatformAgileFramework.Collections;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Generic interface adds the capability to provide a type-safe service. It also
	/// provides extra flexibility to pre-instantiate services before adding them to
	/// a manager. This allows the implementation type to be left unspecified for services
	/// that do not need to be constructed "on-demand". In core, only services with
	/// parameterless constructors for their implementations are supported as "on-demand"
	/// services. We also implement <see cref="IPAFNamedAndTypedObject{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// This is an INTERFACE type. Implementations must test to see whether the Generic
	/// is indeed an interface type, not a class or struct during construction.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <description>
	/// New - part of new service description. This generic interface was added to
	/// prevent (to an extent) type mismatch errors on the interface, especially when
	/// late binding to the implementation. The type of the interface is usually known
	/// at run time. It's the implementation type that my not be available even during
	/// the life of the program, except as a proxy.
	/// </description>
	/// </contribution>
	/// </history>
	/// <typesafety>
	/// Implementations do not necessarily need to be type-safe, since the implementation is
	/// usually constructed once and then only modified by the SM, which keeps these
	/// in a concurrent container.
	/// </typesafety>
// ReSharper disable PartialTypeWithSinglePart
	//Core
	public partial interface IPAFServiceDescription<T>
		: IPAFServiceDescription, IPAFNamedAndTypedObject<T> where T: class, IPAFService
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// This is an optional instantiated service. It can be <see langword="null"/>.
		/// It provides the capability to pre-build the service.
		/// </summary>
		T Service { get; [SecurityCritical] set; }

		/// <summary>
		/// Setter for the service - only for full-trust environments.
		/// </summary>
		[SecurityCritical]
		void SetService(T serviceObject);
	}
}

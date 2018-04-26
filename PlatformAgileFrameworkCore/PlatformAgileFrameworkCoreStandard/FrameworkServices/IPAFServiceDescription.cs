
using System;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Notification.Exceptions;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Exceptions;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This interface defines components that are used to describe a service
	/// - both it's service interface and it's implementing type. The service
	/// is optionally assigned a name. Service names must be unique within any
	/// "AppDomain". This interface contains sufficient information
	/// to describe local services only. This is the information exposed in Core.
	/// Service dictionaries have been rewritten to make it difficult, but not
	/// impossible to access services by specifying their implementation type.
	/// It is possible for clients to assign names to services corresonding to
	/// assembly-qualified names of their implementing types. This is not a
	/// good idea, since it is at odds with the principles of "Service Orientation".
	/// </para>
	/// <para>
	/// This interface is needed to support our service discovery process. In PAF,
	/// the discovery process is designed to work with partial information or very
	/// specific information. One extreme is to simply provide an interface name.
	/// The service creation logic can search through loaded assemblies to find
	/// constructable types that implement the interface. However, in Core, no
	/// code generation is supported, so the client usually has access to an actual
	/// interface <see cref="Type"/> object. Additional information about a
	/// service's implementation can be provided. This is provided in the
	/// <see cref="ServiceImplementationType"/> property. This is necessary when
	/// information needs to be provided to the service manager concerning a
	/// specific implementation of the service interface needs to be used.
	/// </para>
	/// <para>
	/// It should be mentioned that any conceivable type of filter on
	/// <see cref="Type"/>s  can be created using <see cref="IPAFTypeFilter"/>.
	/// This can be applied additionally to provide further filtering on
	/// service types.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <contribution>
	/// New - made this to unify the way we describe services. It was done in
	/// different ways in different places. Sometimes this was necessary, but
	/// mostly not.
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	//Core
	public partial interface IPAFServiceDescription: IPAFNamedAndTypedObject
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Properties
		/// <summary>
		/// This property is used when the interface is used
		/// within a collection of services to identify the default
		/// implementation of the service interface to be handed out to
		/// clients requesting services by type only.
		/// </summary>
		bool IsDefault { get; [SecurityCritical] set; }
		/// <summary>
		/// Description of the interface type that will be requested from the
		/// service manager. As a best practice, services should not be exposed as
		/// concrete types. It is recommended that this property always be
		/// populated with an interface type and that a service be requested by
		/// interface and possibly name. This can never be <see langword="null"/>.
		/// </summary>
		/// <remarks>
		/// Note that setter should almost never be used - any implementation of
		/// <see cref="IPAFServiceDescription"/> is normally immutable
		/// WRT to interface type.
		/// </remarks>
		IPAFTypeHolder ServiceInterfaceType { get; [SecurityCritical] set; }
		/// <summary>
		/// <para>
		/// Description of the concrete implementation type that implements the service.
		/// Only elevated-trust callers can access a service according to its concrete
		/// type in PAF. It is generally discouraged. This property need be
		/// non-<see langword="null"/> only when there is a need to describe the
		/// concrete implementation when the implementation is known.
		/// </para>
		/// <para>
		/// This property can contain full type information, including an assembly name
		/// that could be searched for in various locations, depending on the settings
		/// of the <see cref="IPAFAssemblyLoader"/> in use within the service manager or
		/// the <see cref="IPAFTypeHolder"/>.
		/// </para>
		/// <para>
		/// Partial information can be in the form of a namespace (with or without an
		/// assembly spec). An example would be something like "System.Drawing" or
		/// <see cref="System"/>. If the namespace is not <see langword="null"/>, it
		/// is used as a filter on returned types. Only Types within the given namespace
		/// will be checked for the interface implementation. This is useful in cases
		/// where services may be implemented by several different Types in an assembly,
		/// but the Types are organized so that only one Type within a given namespace
		/// implements an interface. An example of this is <c>System.Data.OleDb.OleDbConnection</c>
		/// versus <c>System.Data.SqlClient.SqlClientConnection</c>. Both Types
		/// implement <c>System.Data.IDbConnection</c> and live in the same assembly,
		/// but in different namespaces. This is a questionable practice for designers
		/// of assemblies, but it's done often, so we support it.
		/// </para>
		/// </summary>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}"> with
		/// <see cref="Notification.Exceptions.PAFTypeMismatchExceptionDataBase.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message if the implementation type does not inherit from the interface.
		/// </exception>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="value"/> is <see langword="null"/> in set.
		/// </exception>
		/// </exceptions>
		IPAFTypeHolder ServiceImplementationType { get; [SecurityCritical] set; }
		/// <summary>
		/// This is an optional name for the service. If blank or <see cref="String.Empty"/>,
		/// this service will be the default service implementation for this interface.
		/// There can only be one default service in any "AppDomain" for
		/// a given interface type (<see cref="ServiceInterfaceType"/>). This property
		/// must never be <see langword="null"/>.
		/// </summary>
		string ServiceName { get; [SecurityCritical] set; }
		/// <summary>
		/// This is a member to hold the instantiated service.
		/// </summary>
		/// <exceptions>
		/// <exception cref="PAFStandardException{T}"> with
		/// <see cref="Notification.Exceptions.PAFTypeMismatchExceptionDataBase.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message if the object does not inherit from
		// ReSharper disable CSharpWarnings::CS1584
		// ReSharper Problem
		/// <see cref="IPAFServiceDescription.ServiceImplementationType.TypeType"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}"> with
		/// <see cref="Notification.Exceptions.PAFTypeMismatchExceptionDataBase.TYPES_NOT_AN_EXACT_MATCH"/>
		/// message is thrown if the object does not exactly match the
		/// <see cref="IPAFServiceDescription.ServiceImplementationType.TypeType"/>
		/// if it is here.
		// ReSharper restore CSharpWarnings::CS1584
		/// </exception">
		/// </exceptions>
		/// <remarks>
		/// Can be set to <see langword="null"/> when service is disposed or otherwise
		/// unavaliable.
		/// </remarks>
		object ServiceObject { get; [SecurityCritical] set; }
		#endregion // Properties
		///// <summary>
		///// This provides filtering capabilities for the IMPLEMENTATION type.
		///// </summary>
		//// TODO - KRM removed in Core - make sure it gets back in extended.
		//IPAFTypeFilter ServiceImplementationTypeFilter { get; }
	}
}

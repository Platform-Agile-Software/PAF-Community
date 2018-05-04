using System;
using System.Collections.Generic;
using System.Reflection;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Interface I built for the service manager. Designed to call into
	/// "ManufacturingUtils" and other heplers.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 22sep2015 </date>
	/// <desription>
	/// New - KRM thought it was a good idea :-)
	/// </desription>
	/// </contribution>
	/// </history>
	public interface IManufacturingUtils: IPAFService
	{
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		bool AddAssemblyToAssembliesLoaded(Assembly assembly);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		bool DefaultTypeFilter(Type typeToCheck, object criteriaObj);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		IEnumerable<Assembly> GetAppDomainAssemblies();

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		bool GetTypeAndNamespace(string fullyQualifiedTypeName, ref string unqualifiedName, ref string nameSpace);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		ICollection<Type> LocateReflectionInterfacesInAssembly(Assembly assemblyToSearch, string interfaceName,
			string typeNameSpace = null, IPAFTypeFilter typeFilter = null);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		ICollection<Type> LocateReflectionServices(string interfaceName, string typeNameSpace = null, string typeName = null,
			IPAFTypeFilter typeFilter = null, IEnumerable<Assembly> assemblyList = null);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		ICollection<Type> LocateReflectionServicesInAssembly(Assembly assemblyToSearch, string interfaceName,
			string typeNameSpace = null, string typeName = null, IPAFTypeFilter typeFilter = null);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		Type LocateReflectionType(string typeName, string typeNameSpace, string interfaceName,
			IEnumerable<Assembly> assemblyList = null);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		Type LocateReflectionTypeInAssembly(Assembly assemblyToSearch, string typeName, string typeNameSpace = null,
			string interfaceName = null, IPAFTypeFilter typeFilter = null);

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		ICollection<Type> LocateReflectionTypesInAssembly(Assembly assemblyToSearch, string typeName = null,
			string typeNameSpace = null, string interfaceName = null, IPAFTypeFilter typeFilter = null);
	}

	/// <summary>
	/// Interface for the internals.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 22sep2015 </date>
	/// <desription>
	/// New.
	/// </desription>
	/// </contribution>
	/// </history>
	//
	// KRM - The ONLY time we put more than one interface/class/struct in one
	// file is when we want to hide it! Good f**kin' job on the whole thing, Brian!
	// I love this extensibility pattern.
	//
	internal interface IManufacturingUtilsInternal : IManufacturingUtils
	{
		/// <summary>
		/// Unprotected version of <see cref="IManufacturingUtils.AddAssemblyToAssembliesLoaded"/>
		/// </summary>
		/// <param name="assembly"></param>
		/// <returns></returns>
		bool AddAssemblyToAssembliesLoadedInternal(Assembly assembly);
	}
}
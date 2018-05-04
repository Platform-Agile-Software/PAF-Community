//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2015 Icucom Corporation
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
using System.Reflection;
using System.Security;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Non-static class for inheritance - avoids messy partial classes.
	/// This part is Silverlight compatible (single AppDomain).
	/// Calls into static class <see cref="ManufacturingUtils"/>.
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
	// ReSharper disable PartialTypeWithSinglePart
	// TODO - KRM change to internal when in the SM.
	public partial class ManufacturingUtilsInstance : PAFService, 
		IManufacturingUtilsInternal
	// ReSharper restore PartialTypeWithSinglePart
	{
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// </remarks>
		public virtual bool DefaultTypeFilter(Type typeToCheck, object criteriaObj)
		{
			return ManufacturingUtils.DefaultTypeFilter(typeToCheck, criteriaObj);
		}

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>. Base implementation usually returns
		/// assemblies loaded at construction time only. Inheritors are free to make
		/// this dynamic, as derived frameworks may include dynamic loading.
		/// </remarks>
		public virtual IEnumerable<Assembly> GetAppDomainAssemblies()
		{
			return ManufacturingUtils.Instance.GetAppDomainAssemblies();
		}
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual bool GetTypeAndNamespace(string fullyQualifiedTypeName,
			ref string unqualifiedName, ref string nameSpace)
		{
			return ManufacturingDelegates.GetTypeAndNamespace(fullyQualifiedTypeName, ref unqualifiedName, ref nameSpace);
		}
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual Type LocateReflectionType(string typeName, string typeNameSpace,
			string interfaceName, IEnumerable<Assembly> assemblyList = null)
		{
			return ManufacturingUtils.Instance.LocateReflectionType(typeName, typeNameSpace, interfaceName, assemblyList);
		}

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual Type LocateReflectionTypeInAssembly(Assembly assemblyToSearch,
			string typeName, string typeNameSpace = null, string interfaceName = null,
			IPAFTypeFilter typeFilter = null)
		{
			return ManufacturingUtils.Instance.LocateReflectionTypeInAssembly(assemblyToSearch,
				typeName, typeNameSpace, interfaceName,
				typeFilter);
		}

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionTypesInAssembly(Assembly assemblyToSearch,
			string typeName = null, string typeNameSpace = null, string interfaceName = null,
			IPAFTypeFilter typeFilter = null)
		{
            return ManufacturingUtils.Instance.LocateReflectionTypesInAssembly(assemblyToSearch,
				typeName, typeNameSpace, interfaceName, typeFilter);
		}

		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionInterfacesInAssembly(
			Assembly assemblyToSearch, string interfaceName, string typeNameSpace = null,
			IPAFTypeFilter typeFilter = null)
		{
            return ManufacturingUtils.Instance.LocateReflectionInterfacesInAssembly(assemblyToSearch, interfaceName,
				typeNameSpace, typeFilter);
		}
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionServices(string interfaceName,
			string typeNameSpace = null, string typeName = null,
			IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null)
		{
            return ManufacturingUtils.Instance.LocateReflectionServices(interfaceName,
				typeNameSpace, typeName, typeFilter, assemblyList);
		}
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionServicesInAssembly(Assembly assemblyToSearch,
			string interfaceName, string typeNameSpace = null, string typeName = null,
			IPAFTypeFilter typeFilter = null)
		{
            return ManufacturingUtils.Instance.LocateReflectionServicesInAssembly(assemblyToSearch,
				interfaceName, typeNameSpace, typeName,
				typeFilter);
		}
		#region Internal/Secure Methods for Framework Extenders
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>.
		/// Virtual method for extenders.
		/// </remarks>
		internal virtual bool AddAssemblyToAssembliesLoadedInternal(Assembly assembly)
		{
			return ManufacturingUtils.AddAssemblyToAssembliesLoadedInternal(assembly);
		}
		/// <remarks>
		/// <see cref="IManufacturingUtilsInternal"/>
		/// </remarks>
		bool IManufacturingUtilsInternal.AddAssemblyToAssembliesLoadedInternal(Assembly assembly)
		{
			return ManufacturingUtils.AddAssemblyToAssembliesLoadedInternal(assembly);
		}
		/// <remarks>
		/// <see cref="ManufacturingUtils"/>
		/// </remarks>
		[SecurityCritical]
		public virtual bool AddAssemblyToAssembliesLoaded(Assembly assembly)
		{
			return ManufacturingUtils.AddAssemblyToAssembliesLoaded(assembly);
		}
		#endregion // Internal/Secure Methods for Framework Extenders
	}
}

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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.FileAndIO;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Helper methods for manufacturing Types in AppDomains. This class is where
	/// the system "bootstrapping" actually begins. This class is accessed by
	/// other system initialization components to load and scan assemblies. There
	/// are several styles of platform coverage supported here, based on the methods
	/// and properties on this class. Platform-specific assemblies can be loaded
	/// dynamically or they can be linked in statically to the app.
	/// This part is Silverlight compatible (single AppDomain).
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04nov2017 </date>
	/// <desription>
	/// <para>
	/// Changed to a singleton so we can set static props before use. Moved many functions
	/// here from bootstrapper, since bootstrapper is used for service manager and clients
	/// want to use another service manager sometimes.
	/// </para>
	/// </desription>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 02sep2011 </date>
	/// <desription>
	/// <para>
	/// Trying to "morph" our way into the new 4.0 security model and the Silverlight
	/// implementation. "SILVERLIGHT" still means the low-security silverlight rules
	/// with no possibility of including full-trust stuff, but this file is being made
	/// to work in elevated priviledge mode, too.
	/// </para>
	/// </desription>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// <para>
	/// Added by KRM - this class does not employ the service manager by design intent.
	/// This class must be available at application initialization time in order to
	/// locate services dynamically. The other option is to push root services into
	/// the manager as statics before the app is started. However, if this class is
	/// usable before SM is available, more options are possible.
	/// </para>
	/// <para>
	/// Core only supports loading ONE platform-specific plugin providing logging,
	/// storage and UI. It's a good practice to aggregate these functions into a single
	/// DLL, anyway.
	/// </para>
	/// </remarks>
	// ReSharper disable PartialTypeWithSinglePart
	public partial class ManufacturingUtils
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// This a thread-safe wrapper for constructing the singleton.
		/// </summary>
		/// <remarks>
		/// Lazy class is thread-safe by default.
		/// </remarks>
		private static readonly Lazy<ManufacturingUtils> s_Singleton =
			new Lazy<ManufacturingUtils>(ConstructManufacturingUtils);

		/// <summary>
		/// Set when the platform-specific assembly is loaded or an attempt has been
		/// made to load it. Or set it to <see langword="true"/> to indicate we don't
		/// even want to load any platform-specific assy.
		/// </summary>
		internal static volatile bool s_IsPlatformAssemblyLoaded;

		/// <summary>
		/// Platform assy for basic services.
		/// </summary>
		protected internal static Assembly s_PlatformAssembly;

		/// <summary>
		/// Container for the assemblies loaded into the current AppDomain. key is the
		/// assembly "simple name", in our case, the filename.
		/// </summary>
		// ReSharper disable once InconsistentNaming
		public static IDictionary<string, Assembly> AssembliesLoadedInAppdomain
		{
			get; [SecurityCritical] set;
		}
		#region Extension points for static linking
		/// <summary>
		/// Push this in for loading assemblies on platform. Loads "from" a fileSpec.
		/// </summary>
		public static Func<string, Assembly> AssemblyLoadFromLoader { get; [SecurityCritical] set; }
		/// <summary>
		/// Push this in for listing assemblies on platform. Even on platforms where
		/// some sort of "GetAssemblies" functionality is not available, the platform
		/// assembly can push in itself if it is statically linked. Any assemblies
		/// returned from this call are not added if they are already in our collection.
		/// The function may return <see langword="null"/>.
		/// </summary>
		public static Func<IEnumerable<Assembly>> AssemblyLister { get; [SecurityCritical] set; }
		#endregion // Extension points for static linking
		///// <summary>
		///// This one has to be manually set, since there is no reliable way
		///// to find out what platform we are on from Microsoft/Xamarin. Used
		///// to be able to examine BCL assys, but no more. Too many variations.....
		///// </summary>
		//   public static readonly IPlatformInfo s_CurrentPlatformInfo = new PAF_ECMA4_6_2PlatformInfo();
		//   // public static readonly IPlatformInfo s_CurrentPlatformInfo = new WinPlatformInfo();
		////public static readonly IPlatformInfo s_CurrentPlatformInfo = new MacPlatformInfo();

		/// <summary>
		/// We need this to do symbolic directory mapping. Set in construction path.
		/// </summary>
		public static ISymbolicDirectoryMappingDictionary DirectoryMappings
		{ get; [SecurityCritical] set; }

		/// <summary>
		/// We need this to do symbolic directory mapping. Set in construction path.
		/// </summary>
		internal static ISymbolicDirectoryMappingDictionaryInternal DirectoryMappingsInternal
		{ get; set; }

		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Create needed collections. Load this assembly
		/// into <c>AssembliesLoadedInAppdomain</c>.
		/// </summary>
		static ManufacturingUtils()
		{
			AssembliesLoadedInAppdomain = new Dictionary<string, Assembly>();
			// Make sure that Platform utils gets loaded when this class gets touched.
			// ReSharper disable once UnusedVariable
			var platform = PlatformUtils.PlatformInfoInternal;
		}

		/// <summary>
		/// Non-public for testing.
		/// </summary>
		protected internal ManufacturingUtils()
		{
		}
		#endregion Constructors

		#region Properties
		/// <summary>
		/// Get the singleton instance of the class.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static ManufacturingUtils Instance
		{
			get
			{
				return s_Singleton.Value;
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Not quite a constructor - a factory for the lazy construction.
		/// </summary>
		protected static ManufacturingUtils ConstructManufacturingUtils()
		{
			var utils = new ManufacturingUtils();
			// Just loading the statics, ReSharper...............
			// ReSharper disable once UnusedVariable
			// Generate an exception here in the constructor so we get it out
			// of the way.
			var unusedVariable = PlatformUtils.Instance;


			// platform statics are loaded, so now we can load the dictionary.
			DirectoryMappingsInternal = new SymbolicDirectoryMappingDictionary();
			DirectoryMappings = DirectoryMappingsInternal;

			// FileUtils must be provisioned with this before SM is released to the public.
			PAFFileUtils.SetMappings(DirectoryMappingsInternal);

			FindLoadedAssemblies();
			return utils;
		}
		#region Partial Methods
		/// <summary>
		/// This method is used to find loaded assemblies from a mechanism in a linked-in class.
		/// </summary>
		/// <param name="assemblies">Collection to add to.</param>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void FindLoadedAssembliesPartial(ref ICollection<Assembly> assemblies);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		/// <summary>
		/// This method is used to load an assembly from a mechanism in a linked-in class.
		/// </summary>
		/// <param name="assemblyPath">Fully qualified path with assembly name.</param>
		/// <param name="assembly">Assembly reference to load into.</param>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void LoadAssemblyFromFilePartial(string assemblyPath, ref Assembly assembly);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods
		/// <summary>
		/// Standard Type filter for searches. <c>ToString()</c> is performed
		/// on criteriaObj to generate a string to match against <c>typeToCheck.FullName</c>.
		/// This works out of the box for either <see cref="System.String"/>s or
		/// <see cref="System.Type"/>s passed as <paramref name="criteriaObj"/>.
		/// </summary>
		/// <param name="typeToCheck">
		/// <see cref="System.Type"/> to check against the criteria. <see langword="null"/>
		/// returns <see langword="false"/>.
		/// </param>
		/// <param name="criteriaObj">
		/// <see langword="null"/> returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> for a match.
		/// </returns>
		public static bool DefaultTypeFilter(Type typeToCheck, object criteriaObj)
		{
			if ((typeToCheck == null) || (criteriaObj == null)) return false;
			var stringToCheck = criteriaObj.ToString();
			if (criteriaObj.GetType() == typeof(Type)) stringToCheck = ((Type)criteriaObj).FullName;
			if (typeToCheck.FullName == stringToCheck)
				return true;
			return false;
		}
		#region Partial Method Callers
		/// <summary>
		/// .Net core cannot find loaded assemblies. They must be pushed in by the
		/// platform-specific app. Thus <see cref="AssemblyLister"/> must be present
		/// if this assembly is linked with .Net Core. Otherwise it is possible to
		/// combine this file with another file which has the implementation of the
		/// partial method.
		/// </summary>
		/// <remarks>
		/// Called at application startup time only. No lock needed.
		/// </remarks>
		protected static void FindLoadedAssemblies()
		{
			// We always add ourselves.
			// ReSharper disable once InconsistentlySynchronizedField
			var ourType = typeof(ManufacturingUtils);
			var ourselves = ourType.PAFGetAssemblyForType();
			var ourName = ourType.PAFGetAssemblySimpleName();

			// ReSharper disable once InconsistentlySynchronizedField
			AssembliesLoadedInAppdomain.Add(ourName, ourselves);

			// try with a lister.
			var assys = AssemblyLister?.Invoke();
			if (assys != null)
			{
				foreach (var asm in assys)
				{
					// ReSharper disable once InconsistentlySynchronizedField
					AssembliesLoadedInAppdomain.Add(asm.GetName().FullName, asm);
				}
			}

			var collection = (ICollection<Assembly>)new Collection<Assembly>();

			// try with a partial method.
			// ReSharper disable once InvocationIsSkipped
			FindLoadedAssembliesPartial(ref collection);

			foreach (var asm in collection)
			{
				// ReSharper disable once InconsistentlySynchronizedField
				AssembliesLoadedInAppdomain.Add(asm.GetName().FullName, asm);
			}

			// No name means statically linked.
			if (string.IsNullOrEmpty(PlatformUtils.PlatformInfo.CurrentPlatformAssyName)) s_IsPlatformAssemblyLoaded = true;

			/////////////////////////////////////////////////////////////////////////////
			// Remainder is executed if platform assy is still needed.
			if (!s_IsPlatformAssemblyLoaded) return;

			// We determine if the platform assy has gotten loaded along the way.
			var platformAssembly =
				// ReSharper disable once InconsistentlySynchronizedField
				PlatformUtils.PlatformInfo.CurrentPlatformAssyName.GetAssemblyFromFullNameKeyedDictionary(AssembliesLoadedInAppdomain);
			if (platformAssembly != null)
			{
				// ReSharper disable once InconsistentlySynchronizedField
				s_PlatformAssembly = platformAssembly;
				s_IsPlatformAssemblyLoaded = true;
				return;
			}

			// If we pushed in a way to load an assembly, use it.
			s_PlatformAssembly = AssemblyLoadFromLoader?.Invoke(PlatformUtils.FormPlatformAssemblyLoadPathWithAssembly());
			if (s_PlatformAssembly != null)
			{
				// ReSharper disable once InconsistentlySynchronizedField
				AssembliesLoadedInAppdomain.Add(s_PlatformAssembly.GetAssemblySimpleName(), s_PlatformAssembly);
			}
			s_PlatformAssembly = LoadPlatformAssembly();
			// TODO krm exception here.
			// This was the last chance, so we must complain.
		}
		/// <summary>
		/// Call this to load platform-specific assembly.
		/// </summary>
		/// <remarks>
		/// Separated out mostly to support tests not involving the service manager.
		/// </remarks>
		protected internal static Assembly LoadPlatformAssembly()
		{
			if (s_IsPlatformAssemblyLoaded) return s_PlatformAssembly;
			s_IsPlatformAssemblyLoaded = true;

			var platformAssemblyLoadPath = PlatformUtils.FormPlatformAssemblyLoadPathWithAssembly();

			// LoadFrom is OK, since we ONLY bind to interfaces.
			s_PlatformAssembly = LoadAssembly(platformAssemblyLoadPath);

			if (s_PlatformAssembly != null)
				AddAssemblyToAssembliesLoadedInternal(s_PlatformAssembly);

			return s_PlatformAssembly;
		}
		/// <summary>
		/// Will return <see langword="null"/> if partial class implementation is not present.
		/// </summary>
		/// <exceptions>
		/// None caught, none thrown. Load exceptions are typically generated.
		/// </exceptions>
		public static Assembly LoadAssembly(string assemblyPath)
		{
			var assembly = LoadAssemblyFromFile(assemblyPath);

			if (assembly != null)
				return assembly;

			var pathParts = PAFFileUtils.SeparateDirectoryFromFile(assemblyPath);
			return Assembly.Load(new AssemblyName(pathParts[1]));
		}
		private static Assembly LoadAssemblyFromFile(string assemblyPath)
		{
			Assembly assembly = null;
			// ReSharper disable once InvocationIsSkipped
			LoadAssemblyFromFilePartial(assemblyPath, ref assembly);
			// ReSharper disable once ExpressionIsAlwaysNull
			return assembly;
		}
		#endregion Partial Method Callers
		/// <summary>
		/// This utility method is needed since listing assemblies is done
		/// differently in Silverlight and other platforms.
		/// </summary>
		/// <returns>
		/// Set of assemblies in the current "AppDomain".
		/// </returns>
		public virtual IEnumerable<Assembly> GetAppDomainAssemblies()
		{
			IEnumerable<Assembly> col;
			lock (AssembliesLoadedInAppdomain)
			{
				col = new List<Assembly>(AssembliesLoadedInAppdomain.Values);
			}
			return col;
		}
		/// <summary>
		/// Locates a <see cref="Type"/> in assemblies loaded in an
		/// "AppDomain". This method will optionally find a
		/// <see cref="Type"/> implementing an interface. Uses
		/// <see cref="LocateReflectionTypeInAssembly"/> to find the Type.
		/// See that method for details.
		/// </summary>
		/// <param name="typeName">
		/// This is a fully-qualified type name such as "System.Drawing.Size"
		/// or <see cref="System.IComparable"/>. See <see cref="LocateReflectionTypeInAssembly"/>
		/// for details. If <paramref name="typeNameSpace"/> is specified, this parameter
		/// should be a simple name (no namespace).
		/// </param>
		/// <param name="typeNameSpace">
		/// This is a NameSpace such as "System.Drawing" or <see cref="System"/>.
		/// If this string is not <see langword="null"/>, it is prepended to the typeName.
		/// If the search is for an interface only, (<paramref name="typeName"/> is null),
		/// Only Types within the given namespace will be searched for the interface
		/// implementation. This is useful in cases where services may be implemented
		/// by several different Types in an assembly, but the Types are organized so
		/// that only one Type within a given namespace implements an interface.
		/// An example of this is <c>System.Data.OleDb.OleDbConnection</c>
		/// versus <c>System.Data.SqlClient.SqlClientConnection</c>. Both Types
		/// implement <c>System.Data.IDbConnection</c> and live in the same assembly,
		/// but in different namespaces.
		/// </param>
		/// <param name="interfaceName">
		/// This is a fully-qualified type name such as <see cref="System.IComparable"/>.
		/// See <see cref="LocateReflectionTypeInAssembly"/> for details.
		/// </param>
		/// <param name="assemblyList">
		/// The array of <see cref="Assembly"/>'s to search. These will be searched
		/// in order to find the Type. If this parameter is <see langword="null"/>, all assemblies
		/// in the current "AppDomain" will be searched.
		/// </param>
		/// <returns>
		/// If the <see cref="Type"/> is found in any assemblies searched, it is
		/// returned. If not, <see langword="null"/> is returned without any exceptions.
		/// </returns>
		/// <remarks>
		/// If both <paramref name="typeName"/> and <paramref name="interfaceName"/>
		/// are blank or <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public virtual Type LocateReflectionType(string typeName, string typeNameSpace,
			string interfaceName, IEnumerable<Assembly> assemblyList = null)
		{
			if (assemblyList == null) assemblyList = GetAppDomainAssemblies();
			// Try to load from any assembly.
			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var asm in assemblyList)
			// ReSharper restore LoopCanBeConvertedToQuery
			{
				var type = LocateReflectionTypeInAssembly(asm, typeName, typeNameSpace, interfaceName);
				if (type != null) return type;
			}
			// Fall-through returns null.
			return null;
		}

		/// <summary>
		/// Locates a <see cref="Type"/> in a single assembly. This just takes the
		/// first type returned from <see cref="LocateReflectionTypesInAssembly"/>.
		/// If the client needs to respond to multiple returned types (e.g. an exception),
		/// use <see cref="LocateReflectionTypesInAssembly"/>.
		/// </summary>
		/// <param name="assemblyToSearch">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="typeName">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="typeNameSpace">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="interfaceName">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <returns>
		/// If the <see cref="Type"/> is found in the assemblies searched, it is
		/// returned. If not, <see langword="null"/> is returned without any exceptions. If
		/// multiple <see cref="Type"/>s are found that meet the criteria, only
		/// the first is returned.
		/// </returns>
		/// <remarks>
		/// If both <paramref name="typeName"/> and <paramref name="interfaceName"/> are
		/// blank or <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public virtual Type LocateReflectionTypeInAssembly(Assembly assemblyToSearch,
			string typeName, string typeNameSpace = null, string interfaceName = null,
			IPAFTypeFilter typeFilter = null)
		{
			var col = LocateReflectionTypesInAssembly(assemblyToSearch, typeName,
				typeNameSpace, interfaceName, typeFilter);

			if ((col != null) && (col.Count > 0)) return col.GetFirstElement();
			return null;
		}

		/// <summary>
		/// Locates a <see cref="Type"/> in a single assembly.
		/// </summary>
		/// <param name="assemblyToSearch">
		/// A specific assembly to search for. Cannot be <see langword="null"/>.
		/// </param>
		/// <param name="typeName">
		/// This is a fully-qualified type name such as "System.Drawing.Size"
		/// or <see cref="System.IComparable"/> or an unqualified type name such as
		/// "Size".  If this string is <see langword="null"/> or blank, the first
		/// non-interface Type implementing the specified interface is returned.
		/// If <paramref name="typeNameSpace"/> is specified, this parameter should be an
		/// unqualified name (no namespace). In the case of an unqualified type name,
		/// with no namespace specified, the first conforming type in the assembly is
		/// returned.
		/// </param>
		/// <param name="typeNameSpace">
		/// This is a NameSpace such as "System.Drawing" or <see cref="System"/>.
		/// If this string is not <see langword="null"/>, it is prepended to the typeName.
		/// If the search is for an interface only, (<paramref name="typeName"/> is null),
		/// only Types within the given namespace will be searched for the interface
		/// implementation. This is useful in cases where services may be implemented
		/// by several different Types in an assembly, but the Types are organized so
		/// that only one Type within a given namespace implements an interface.
		/// An example of this is <c>System.Data.OleDb.OleDbConnection</c>
		/// versus <c>System.Data.SqlClient.SqlClientConnection</c>. Both Types
		/// implement <c>System.Data.IDbConnection</c> and live in the same assembly,
		/// but in different namespaces. If the namespace is not specified, the FIRST
		/// type with the correct <paramref name="typeName"/> is returned. Note that
		/// if a <paramref name="typeName"/> and <paramref name="typeNameSpace"/> are both
		/// specified, a maximum of one (1) type should ever be returned.
		/// </param>
		/// <param name="interfaceName">
		/// This is a fully-qualified type name such as <see cref="System.IComparable"/>.
		/// If this string is <see langword="null"/> or blank, the <paramref name="typeName"/> is used
		/// to find the <see cref="System.Type"/> without any regard to implemented
		/// interfaces. If the Type name is blank or <see langword="null"/>, the method will
		/// only return a Type if it is constructable (not an abstract Type and not an
		/// interface).
		/// </param>
		/// <param name="typeFilter">
		/// An optional filter that will eliminate unsuitable types from the search.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <returns>
		/// If one or more <see cref="Type"/>s meeting all requirements are found, they are
		/// returned. If not, <see langword="null"/> is returned without any exceptions.
		/// </returns>
		/// <remarks>
		/// If both <paramref name="typeName"/> and <paramref name="interfaceName"/> are
		/// blank or <see langword="null"/>, <see langword="null"/> is returned.
		/// <see langword="null"/> is returned if both <paramref name="typeName"/>
		/// and <paramref name="typeNameSpace"/> are <see langword="null"/>.
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionTypesInAssembly(Assembly assemblyToSearch,
			string typeName = null, string typeNameSpace = null, string interfaceName = null,
			IPAFTypeFilter typeFilter = null)
		{
			// Safety valve.
			if ((string.IsNullOrEmpty(typeName) && string.IsNullOrEmpty(interfaceName)))
				return null;
			// This tells us if we are searching for an implementation of an interface
			// without regard to implementation type.
			var isInterfaceSearch = string.IsNullOrEmpty(typeName);

			// If we had a definate Type/Types, things are relatively easy. All we have
			// to do is see if it implements the interface.
			if (!isInterfaceSearch)
			{
				// If namespace is not null, prepend it. This condition
				// results in return of a single type.
				if (typeNameSpace != null)
				{
					typeName = typeNameSpace + "." + typeName;
					var type = assemblyToSearch.GetType(typeName);

					// Can't go further if nothing here.
					if (type == null) return null;

					// Kill it if it's not right.
					if (typeFilter != null)
						if (!typeFilter.FilteringDelegate(type, typeFilter.FilterAuxiliaryData)) return null;

					var retval = new Collection<Type> { type };
					return retval;
				}

				// Next try to locate by unqualified name. This results in multiple types.
				var returnedTypes = new Collection<Type>();
				var types = assemblyToSearch.PAFGetAccessibleTypes();
				foreach (var aType in types)
				{
					// First apply client filter.
					if (typeFilter != null)
					{
						if (!typeFilter.FilteringDelegate(aType, typeFilter.FilterAuxiliaryData)) continue;
					}
					if (!aType.UnqualifiedNamesMatch(typeName)) continue;

					// Interface present - check it.
					if (!string.IsNullOrEmpty(interfaceName))
					{
						if (aType.FindConformingInterfaces(DefaultTypeFilter, interfaceName) == null)
							continue;
					}

					returnedTypes.Add(aType);
				}
				if (returnedTypes.Count == 0) return null;
				return returnedTypes;
			}

			return LocateReflectionInterfacesInAssembly(assemblyToSearch, interfaceName,
			typeNameSpace, typeFilter);
		}

		/// <summary>
		/// Locates a set of <see cref="Type"/>'s in a single assembly. These
		/// types are constrained to implement an interface and are filtered
		/// by other OPTIONAL criteria. Since this method works with an
		/// <see cref="Assembly"/> instance, it operates locally, finding
		/// services already in the current "AppDomain".
		/// </summary>
		/// <param name="assemblyToSearch">
		/// A specific assembly to search for. <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <param name="interfaceName">
		/// This is a fully-qualified type name such as <see cref="System.IComparable"/>.
		/// The method will only return a Type if it is constructable (not an abstract
		/// Type and not an interface). <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <param name="typeNameSpace">
		/// This is a NameSpace such as "System.Drawing" or <see cref="System"/>.
		/// If this string is not <see langword="null"/>, it is used as a filter on returned types.
		/// Only Types within the given namespace will be checked for the interface
		/// implementation. This is useful in cases where services may be implemented
		/// by several different Types in an assembly, but the Types are organized so
		/// that only one Type within a given namespace implements an interface.
		/// An example of this is <c>System.Data.OleDb.OleDbConnection</c>
		/// versus <c>System.Data.SqlClient.SqlClientConnection</c>. Both Types
		/// implement <c>System.Data.IDbConnection</c> and live in the same assembly,
		/// but in different namespaces. Default = <see langword="null"/>.
		/// </param>
		/// <param name="typeFilter">
		/// An optional <see cref="IPAFTypeFilter"/> delegate that will
		/// eliminate unsuitable types from the search. This can be used to operate
		/// on attributes or anything else on a <see cref="Type"/>. Default = <see langword="null"/>.
		/// </param>
		/// <returns>
		/// If any conforming <see cref="Type"/>s are found in the assemblies searched,
		/// they are returned. If not, <see langword="null"/> is returned without any exceptions.
		/// </returns>
		public virtual ICollection<Type> LocateReflectionInterfacesInAssembly(
			Assembly assemblyToSearch, string interfaceName, string typeNameSpace = null,
			IPAFTypeFilter typeFilter = null)
		{
			// Safety valve.
			if ((assemblyToSearch == null) || (string.IsNullOrEmpty(interfaceName)))
				return null;

			// Loop through all Types and figure out if any implement the interface.
			var types = assemblyToSearch.PAFGetAccessibleTypes();

			var col = new Collection<Type>();
			foreach (var aType in types)
			{
				// Apply the client's filter.
				if (typeFilter != null)
				{
					if (!typeFilter.FilteringDelegate(aType, typeFilter.FilterAuxiliaryData)) continue;
				}
				// Filter on namespace if the caller specfied one.
				// ReSharper disable PossibleNullReferenceException
				if ((string.IsNullOrEmpty(typeNameSpace))
					&& (!(aType.FullName.Contains(typeNameSpace + ".")))) continue;
				// ReSharper restore PossibleNullReferenceException

				// Use DefaultTypeFilter for the interface name only.
				var interfaceTypes = aType.FindConformingInterfaces(DefaultTypeFilter, interfaceName);
				if ((interfaceTypes != null) && (interfaceTypes.GetFirstElement() != null))
				{
					col.Add(aType);
				}
			}
			return col.Count == 0 ? null : col;
		}
		/// <summary>
		/// Locates a <see cref="Type"/> in assemblies loaded in an
		/// "AppDomain". This method will optionally find a
		/// <see cref="Type"/> implementing an interface. Uses
		/// <see cref="LocateReflectionTypeInAssembly"/> to find the Type.
		/// See that method for details.
		/// </summary>
		/// <param name="interfaceName">
		/// This is a fully-qualified type name such as <see cref="System.IComparable"/>.
		/// See <see cref="LocateReflectionTypeInAssembly"/> for details.
		/// </param>
		/// <param name="typeNameSpace">
		/// This is a NameSpace such as "System.Drawing" or <see cref="System"/>.
		/// If this string is not <see langword="null"/>, it is prepended to the typeName.
		/// If the search is for an interface only, (<paramref name="typeName"/> is null),
		/// Only Types within the given namespace will be searched for the interface
		/// implementation. This is useful in cases where services may be implemented
		/// by several different Types in an assembly, but the Types are organized so
		/// that only one Type within a given namespace implements an interface.
		/// An example of this is <c>System.Data.OleDb.OleDbConnection</c>
		/// versus <c>System.Data.SqlClient.SqlClientConnection</c>. Both Types
		/// implement <c>System.Data.IDbConnection</c> and live in the same assembly,
		/// but in different namespaces.
		/// </param>
		/// <param name="typeName">
		/// This can be a fully-qualified type name such as "System.Drawing.Size".
		/// See <see cref="LocateReflectionTypeInAssembly"/> for details. If
		/// <paramref name="typeNameSpace"/> is specified, this parameter should
		/// be a simple name (no namespace).
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="assemblyList">
		/// The array of <see cref="Assembly"/>'s to search. These will be searched
		/// in order to find the Type. If this parameter is <see langword="null"/>, all assemblies
		/// in the current "AppDomain" will be searched.
		/// </param>
		/// <returns>
		/// If the <see cref="Type"/> is found in any assemblies searched, it is
		/// returned. If not, <see langword="null"/> is returned without any exceptions.
		/// </returns>
		/// <remarks>
		/// If both <paramref name="typeName"/> and <paramref name="interfaceName"/>
		/// are blank or <see langword="null"/>, <see langword="null"/> is returned.
		/// </remarks>
		public virtual ICollection<Type> LocateReflectionServices(string interfaceName,
			string typeNameSpace = null, string typeName = null,
			IPAFTypeFilter typeFilter = null,
			IEnumerable<Assembly> assemblyList = null)
		{
			if (assemblyList == null) assemblyList = GetAppDomainAssemblies();
			// Try to load from any assembly.
			var outCol = new Collection<Type>();

			// ReSharper disable LoopCanBeConvertedToQuery
			foreach (var asm in assemblyList)
			// ReSharper restore LoopCanBeConvertedToQuery
			{
				var col = LocateReflectionServicesInAssembly(asm, interfaceName, typeNameSpace,
					typeName, typeFilter);
				if (col != null) outCol.AddItems(col);
			}

			if ((outCol.Count == 0))
				return null;
			return outCol;
		}
		/// <summary>
		/// This method works exactly the same as <see cref="LocateReflectionTypesInAssembly"/>
		/// with the added restriction on returned <see cref="Type"/>s that they be instantiable.
		/// The check is made for an abstract class or a pure interface type and those types are
		/// culled from the returned set. This is intended to produce a set of implementing types
		/// that can be built and run. No check is made for the presence of any particular
		/// constructor. It is up to the client to do this in their installed filter.
		/// </summary>
		/// <param name="assemblyToSearch">
		/// See <see cref="LocateReflectionInterfacesInAssembly"/>.
		/// </param>
		/// <param name="interfaceName">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// The method will only return a Type if it is constructable (not an abstract
		/// Type and not an interface). <see langword="null"/> returns <see langword="null"/>.
		/// </param>
		/// <param name="typeNameSpace">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="typeName">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <param name="typeFilter">
		/// See <see cref="LocateReflectionTypesInAssembly"/>.
		/// </param>
		/// <returns>
		/// If any conforming <see cref="Type"/>s are found in the assemblies searched,
		/// they are returned. If not, <see langword="null"/> is returned without any exceptions.
		/// </returns>
		public virtual ICollection<Type> LocateReflectionServicesInAssembly(Assembly assemblyToSearch,
			string interfaceName, string typeNameSpace = null, string typeName = null,
			IPAFTypeFilter typeFilter = null)
		{
			// We wish to apply a filter that culls out interfaces and abstract classes.
			var filterContainer = new PAFTypeFilter(TypeExtensions.IsTypeInstantiable);

			// We must combine the filter with the one the client has provided, if any.
			var filterAggregator = new PAFTypeFilterAggregator();
			filterAggregator.AddFilter(filterContainer);

			if (typeFilter != null)
			{
				filterAggregator.AddFilter(typeFilter);
			}
			var outCol = LocateReflectionTypesInAssembly(assemblyToSearch, typeName,
				typeNameSpace, interfaceName, filterAggregator);

			if ((outCol == null) || (outCol.Count == 0)) return null;
			return outCol;
		}
		#region Internal/Secure Methods for Framework Extenders
		/// <summary>
		/// Adds an assembly into the collection of loaded assemblies.
		/// </summary>
		/// <param name="assembly">Assembly to add.</param>
		/// <returns>
		/// <see langword="false"/> if the assembly was already in the collection.
		/// </returns>
		/// <threadsafety>
		/// Made thread-safe through a monitor. Not a high-traffic zone, so this
		/// is fine.
		/// </threadsafety>
		internal static bool AddAssemblyToAssembliesLoadedInternal(Assembly assembly)
		{
			lock (AssembliesLoadedInAppdomain)
			{
				if (AssembliesLoadedInAppdomain.ContainsKey(assembly.GetName().Name)) return false;
				AssembliesLoadedInAppdomain.Add(assembly.GetName().Name, assembly);
				return true;
			}
		}
		/// <summary>
		/// Elevated-trust entry into <see cref="AddAssemblyToAssembliesLoadedInternal"/>.
		/// </summary>
		/// <see cref="AddAssemblyToAssembliesLoadedInternal"/>.
		/// <returns>
		/// <see cref="AddAssemblyToAssembliesLoadedInternal"/>.
		/// </returns>
		/// <threadsafety>
		/// <see cref="AddAssemblyToAssembliesLoadedInternal"/>.
		/// </threadsafety>
		[SecurityCritical]
		public static bool AddAssemblyToAssembliesLoaded(Assembly assembly)
		{
			return AddAssemblyToAssembliesLoadedInternal(assembly);
		}
		#endregion // Internal/Secure Methods for Framework Extenders
		#endregion //Methods
	}
}

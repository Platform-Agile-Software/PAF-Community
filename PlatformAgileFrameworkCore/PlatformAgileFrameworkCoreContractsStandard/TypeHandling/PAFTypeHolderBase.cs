//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
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
using System.Linq;
using System.Runtime.CompilerServices;
using PlatformAgileFramework.AssemblyHandling;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.StringParsing;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers;
#region Exception shorthand.
// ReSharper disable IdentifierTypo
using PAFTMEMT = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionMessageTags;
using PAFTMED = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
using IPAFTMED = PlatformAgileFramework.TypeHandling.Exceptions.IPAFTypeMismatchExceptionData;
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
using IPAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFTypeExceptionData;
using PAFTEMT = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionMessageTags;
using PlatformAgileFramework.TypeHandling.Exceptions;
// ReSharper restore IdentifierTypo
#endregion // Exception shorthand.


namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	///	Contains all possible information about a type.
	/// </summary>
	/// <threadsafety>
	/// Safe.
	/// </threadsafety>
	/// <remarks>
	/// This holder now supports versioning. The holder can be built with a
	/// "template" in the form of the "AssemblyHolder" parameter.
	/// This can be wildcarded for the assembly name, in which case the
	/// <see cref="ResolveType(IPAFAssemblyLoader, bool)"/> method looks
	/// for an assembly containing the type and resets the information on
	/// this holder to reflect the characteristics of the discovered
	/// assembly. This is the extent of versioning support in core. It
	/// is not really even versioning, but a capability to resolve a
	/// named type from loaded assemblies.
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 01feb2019 </date>
	/// <description>
	/// This had to be extensively rewritten in a backward-compatible way to support
	/// Generic types. The original version was written back in 2004 and was used for
	/// non-Generic loading. This is the new version that exploits the new reflection
	/// library and the fact that creating a <see cref="Type"/> does not entail loading
	/// all its dependencies. This was the purpose of this original class, but is now
	/// handled by <see cref="Type"/>. Also took out unnecessary locks.
	/// </description>
	/// </contribution>
	/// <author> DAP </author>
	/// <date> 08dec2011 </date>
	/// <contribution>
	/// Rewrote to add the "AssemblyHolder" and rewrote the
	/// <see cref="ResolveType(IPAFAssemblyLoader, bool)"/> method to
	/// deal with wildcards.
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 06nov2011 </date>
	/// <description>
	/// Added history, changed name space.
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core version
	public partial class PAFTypeHolderBase : PAFAssemblyHolderBase, IPAFTypeHolder,
		IPAFGenericTypeNode
	{
		#region Class Fields and Autoproperties
		#region For the type tree.
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IList<IPAFGenericTypeNode> m_GenericChildTypes;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFGenericTypeNode m_GenericParentNode;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal Type m_NodeType;
		#endregion // For the type tree.
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFAssemblyHolder m_AssemblyHolder;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyQualifiedTypeName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_Namespace;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_NamespaceQualifiedTypeName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_SimpleTypeName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal Type m_TypeType;
		#endregion // Class Fields and AutoProperties
		#region Constructors
		/// <summary>
		/// For the surrogate.
		/// </summary>
		protected internal PAFTypeHolderBase()
		{
		}
		/// <summary>
		/// Builds with a string representation of a type or with an actual
		/// <see cref="Type"/>, if available.
		/// The format is:
		/// <c>NamespaceQualifiedTypeName, SimpleAssemblyName {,Culture = CultureInfo} {,Version = Major.Minor.Build.Revision} {,StrongName} {,PublicKeyToken}</c>,
		/// where braces indicate optional fields.
		/// This is the format of a non-Generic type string.
		/// </summary>
		/// <param name="typeType">
		/// Type of the type, if available.
		/// </param>
		/// <param name="assemblyQualifiedTypeName">
		/// String to parse and build from.
		/// </param>
		/// <param name="assemblyChecker">
		/// Optional assembly suitability checker. (Brian T.) This is the ONLY way we check assemblies for
		/// version compatibility. The problem is that mono has different criteria for loading
		/// compatible assemblies, so the check must be platform-dependent. Normally, this is not
		/// a problem for a service interface, since these interfaces are early bound, known
		/// to both main and loaded assemblies. Other usage modes are arcane. In the "loose-coupling" or
		/// service-oriented style, implementations are accessed by their interfaces.
		/// </param>
		/// <param name="assemblyLoader">
		/// Optional loader to be installed. If <see langword="null"/>, the default loader for
		/// the environment is used.
		/// </param>
		/// <param name="assemblyHolder">
		/// If we are built with this contained holder, we delegate to it instead of
		/// using our base class. This allows us to use one holder/loader for an
		/// entire collection of types.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is malformed.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFTypeExceptionData}"> is thrown if
		/// both <paramref name="typeType"/> and <paramref name="assemblyQualifiedTypeName"/>
		/// are <see langword="null"/>.
		/// <see cref="PAFTEMT.TYPE_STRING_CANNOT_BE_NULL"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFTypeExceptionData}"> is thrown if
		/// the type is a Generic type and <paramref name="typeType"/> is <see langword="null"/>.
		/// <see cref="PAFTEMT.TYPE_CANNOT_BE_NULL"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}"> is thrown if
		/// <paramref name="assemblyHolder"/> is supplied and it's assembly is different from
		/// that of the (this) type.
		/// <see cref="PAFTMEMT.TYPES_NOT_AN_EXACT_MATCH"/>
		/// </exception>
		/// </exceptions>
		protected PAFTypeHolderBase(Type typeType, string assemblyQualifiedTypeName = null,
			CheckCandidateAssembly assemblyChecker = null,
			IPAFAssemblyLoader assemblyLoader = null, IPAFAssemblyHolder assemblyHolder = null)
			: base(GetAssemblyName(OverrideAssemblyName(assemblyQualifiedTypeName, typeType), typeType),
			AssemblyFromHolder(assemblyHolder), assemblyChecker, assemblyLoader)
		{
			Initialize_PAFTypeHolderBase();

			// Early bound solves everything!
			if (typeType != null)
			{
				assemblyQualifiedTypeName = typeType.AssemblyQualifiedName;
				m_Assembly = typeType.GetDefiningAssembly();
			}

			m_AssemblyQualifiedTypeName = assemblyQualifiedTypeName;

			// Neither Type nor string input?
			if (string.IsNullOrEmpty(m_AssemblyQualifiedTypeName))
			{
				var exceptionData = new PAFTED(new PAFTypeHolder(null, ""), null, PAFLoggingLevel.Error);
				throw new PAFStandardException<IPAFTED>(exceptionData, PAFTEMT.TYPE_STRING_CANNOT_BE_NULL);
			}

			m_TypeType = typeType;

			m_AssemblyHolder = assemblyHolder;

			// Brian T. this is our major mod for Generics
			///////////////////////////////////////////////
			var isGeneric = IsGenericTypeName(m_AssemblyQualifiedTypeName);
			if (isGeneric)
			{
				// Kill any assembly holder. This pattern is used only for non-Generics.
				m_AssemblyHolder = null;

				// Generics in core are handled by the new reflection library, so type must
				// be here.
				if (m_TypeType == null)
				{
					var exceptionData = new PAFTED(new PAFTypeHolder(null, ""), null, PAFLoggingLevel.Error);
					throw new PAFStandardException<IPAFTED>(exceptionData, PAFTEMT.TYPE_CANNOT_BE_NULL);
				}
				AssemblySimpleName = m_TypeType.Assembly.GetName().Name;

				m_NodeType = m_TypeType;

				if (!m_NodeType.IsGenericType) return;

				// Keep going if not a leaf node.
				m_GenericChildTypes = new Collection<IPAFGenericTypeNode>();
				m_GenericChildTypes.AddItems(GetWrappedTypes(m_NodeType.GetGenericArguments()));

				return;
			}

			// If we got here, we are a non-Generic type.
			ParseAssemblyQualifiedTypeName(assemblyQualifiedTypeName);

			if (m_AssemblyHolder == null) return;

			var assemblyString1 = PAFString.Compress(m_AssemblyNameString);
			var assemblyString2 = PAFString.Compress(m_AssemblyHolder.AssemblyNameString);
			if (string.Compare(assemblyString1, assemblyString2, StringComparison.Ordinal) != 0)
			{
				var assemblyType1
					= new PAFTypeHolder(null, NamespaceQualifiedTypeName + "," + AssemblyQualifiedTypeName);
				var assemblyType2
					= new PAFTypeHolder(null, NamespaceQualifiedTypeName + "," + m_AssemblyHolder.AssemblyNameString);
				var data = new PAFTMED(assemblyType1, assemblyType2);
				throw new PAFStandardException<IPAFTMED>(data, PAFTMEMT.TYPES_NOT_AN_EXACT_MATCH);
			}
		}
		/// <summary>
		/// Builds with a string representation of a type.
		/// The format is:
		/// <c>NamespaceQualifiedTypeName, SimpleAssemblyName {,Culture = CultureInfo} {,Version = Major.Minor.Build.Revision} {,StrongName} {,PublicKeyToken}</c>,
		/// where braces indicate optional fields.
		/// </summary>
		/// <param name="assemblyQualifiedTypeName">
		/// String to parse and build from.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is malformed.
		/// </exception>
		/// </exceptions>
		protected PAFTypeHolderBase(string assemblyQualifiedTypeName)
			: this(null, assemblyQualifiedTypeName)
		{
			Initialize_PAFTypeHolderBase();

		}
		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="typeHolder">
		/// Instance of us.
		/// </param>
		/// <remarks>
		/// Simple implementation just causes a reparse of the string. If
		/// the old one was good, the new one will be, too. Reference copies assembly checker and
		/// loader.
		/// </remarks>
		protected PAFTypeHolderBase(IPAFTypeHolder typeHolder)
			: this(typeHolder.TypeType, typeHolder.GetAssemblyHolder().AssemblyNameString,
			typeHolder.GetAssemblyHolder().AssemblyChecker,
			typeHolder.GetAssemblyHolder().AssemblyLoader) //,
			// typeHolder.GetAssemblyHolder())
		{
			Initialize_PAFTypeHolderBase();
		}

		/// <summary>
		/// Copy constructor.
		/// </summary>
		/// <param name="typeHolder">
		/// Instance of us.
		/// </param>
		/// <remarks>
		/// Accepts a concrete type.
		/// </remarks>
		protected PAFTypeHolderBase(PAFTypeHolderBase typeHolder)
			: this((IPAFTypeHolder)typeHolder)
		{
			Initialize_PAFTypeHolderBase();
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// This is an override of the base method that returns the assembly
		/// holder we were built with, if any. If we do not contain an
		/// assembly holder, we use the base version.
		/// </summary>
		public override IPAFAssemblyHolder GetAssemblyHolder()
		{
			var assemblyHolder = m_AssemblyHolder;
			return assemblyHolder ?? base.GetAssemblyHolder();
		}
		/// <summary>
		/// Holds the full type name.
		/// </summary>
		public string AssemblyQualifiedTypeName
		{
			get
			{
				return m_AssemblyQualifiedTypeName;
			}
			protected internal set
			{
				ParseAssemblyQualifiedTypeName(value);
				m_AssemblyQualifiedTypeName = value;
			}
		}

		/// <summary>
		/// Holds the namespace punctuated with the usual dots.
		/// </summary>
		public string Namespace
		{
			get
			{
				return m_Namespace;
			}
			protected internal set
			{
				m_Namespace = value;
			}
		}
		/// <summary>
		/// Holds the namespace-qualified type name. Namespace prefixed on
		/// type with the usual dots.
		/// </summary>
		public string NamespaceQualifiedTypeName
		{
			get
			{
				return m_NamespaceQualifiedTypeName;
			}
			protected internal set
			{
				m_NamespaceQualifiedTypeName = value;
			}
		}

		/// <summary>
		/// Holds the type name. No namespace - no dots.
		/// </summary>
		public string SimpleTypeName
		{
			get
			{
				return m_SimpleTypeName;
			}
			protected internal set
			{
				m_SimpleTypeName = value;
			}
		}
		/// <summary>
		/// Holds the type, if available. Will return <see langword="null"/> if the type
		/// is not local or has not been resolved.
		/// </summary>
		public virtual Type TypeType
		{
			get
			{
				return m_TypeType;
			}
			set
			{
				if (value != null)
				{
					AssemblyQualifiedTypeName = value.AssemblyQualifiedName;
				}
				m_TypeType = value;
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Helper for constructors to perform class initialization. Safe for
		/// redundant calls.
		/// </summary>
		protected internal void Initialize_PAFTypeHolderBase()
		{
			Services = PAFServiceManagerContainer.ServiceManager;

		}
		/// <summary>
		/// Sets internal properties from the incoming name string.
		/// </summary>
		/// <param name="assemblyQualifiedTypeName">
		/// Type name with namespace and assembly name in canonical
		/// (standard) format. Assembly name may be missing. Either namespace
		/// or unqualified name may be missing, but not both. This is needed
		/// when we want to supply only a partial type specification.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/>
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if
		/// <paramref name="assemblyQualifiedTypeName"/> is malformed. or
		/// does not at least have an unqualified type specification or a namespace.
		/// Sometimes it is desired to describe only a namespace as a filter
		/// on returned types.
		/// </exception>
		/// </exceptions>
		protected void ParseAssemblyQualifiedTypeName(string assemblyQualifiedTypeName)
		{
			if (string.IsNullOrEmpty(assemblyQualifiedTypeName))
				throw new ArgumentNullException(nameof(assemblyQualifiedTypeName));
			// Allow wildcard for assembly.
			if (assemblyQualifiedTypeName.IndexOf(',') < 0)
				assemblyQualifiedTypeName += ",*";

			// We build the components here, so we can flag invalid inputs
			// immediately.
			var split = assemblyQualifiedTypeName.BreakStringInTwo();
			if ((split == null) || (split.Count != 2) || (split[0] == null) || (split[1] == null))
				throw new ArgumentException("assemblyQualifiedTypeName = " + assemblyQualifiedTypeName);

			// This lives on the front.
			NamespaceQualifiedTypeName = split[0].Trim();

			// Use our utilities to get the type and namespace.
			string namespaceString = null;
			string typeString = null;
			ManufacturingDelegates.GetTypeAndNamespace(NamespaceQualifiedTypeName,
				ref typeString, ref namespaceString);
			if ((typeString == null) && (namespaceString == null))
				throw new ArgumentException("NamespaceQualifiedTypeName = " + NamespaceQualifiedTypeName);
			if (!string.IsNullOrEmpty(namespaceString)) Namespace = namespaceString.Trim();
			if (!string.IsNullOrEmpty(typeString)) SimpleTypeName = typeString.Trim();
		}
		/// <summary>
		/// Resolves a type if it is not already resolved. Looks through loaded
		/// assemblies and optionally loads a specific assembly if
		/// <paramref name="staticLinkedOnly"/> is <see langword="false"/>. Sets the
		/// <see cref="TypeType"/> and also returns the type.
		/// </summary>
		/// <param name="loader">
		/// Loader that can load an assembly, if needed. Default = <see langword="null"/>.
		/// </param>
		/// <param name="staticLinkedOnly">
		/// <see langword="true"/> to load only from this (current) assembly's manifest
		/// or from assemblies already loaded. Default = <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if type not found.
		/// </returns>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFTypeExceptionData}"> wraps any
		/// miscellaneous exceptions that are generated within the framework for various reasons
		/// when types cannot be loaded and this method will throw this exception.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// "AssemblySimpleName" should be an asterisk (*) to look through all loaded
		/// assemblies to find a type with the correct namespace qualified type name.
		/// If a suitable assembly is found (verified with <see cref="IPAFAssemblyHolder.AssemblyChecker"/>),
		/// the assembly's information is loaded locally on our instance and
		/// <see cref="GetAssemblyHolder"/> is reset to point to us, if it was attached
		/// to an external holder. This mechanism is our hook to type versioning within
		/// Extended.
		/// </remarks>
		// TODO - KRM. We should also allow a type's simple name to be used
		// TODO and grab the first type that has the simple name. We have
		// TODO the tools in manufacturing utils to do it already!!
		public virtual Type ResolveType(IPAFAssemblyLoader loader, bool staticLinkedOnly)
		{
			// Already resolved?
			if (TypeType != null) return TypeType;
			// TODO - KRM. Class is not thread-safe. Need a lock here
			// TODO (Monitor is probably OK) and need to announce thread-safety.
			try
			{
				var assys = Services.GetTypedService<IManufacturingUtils>().GetAppDomainAssemblies();
				// See if we can find this in loaded assemblies.
				Type returnedType;
				foreach (var assy in assys)
				{
					returnedType
						= assy.PAFGetAccessibleType(NamespaceQualifiedTypeName);
					if (returnedType == null) continue;

					var candidateAssyHolder
						= new PAFAssemblyHolder(assy.FullName, assy, AssemblyChecker, AssemblyLoader);
					// We must see if the other specified attributes match.
					if (AssemblyChecker(GetAssemblyHolder(), candidateAssyHolder))
					{
						// Do we just want to look through all assemblies?
						if (AssemblySimpleName == "*")
						{
							// Populate ourselves with the properties.
							TransferAssemblyHolderProps(candidateAssyHolder);
							// Kill the common assy holder, if we have one.
							// We now have our own.
							m_AssemblyHolder = null;
						}
						TypeType = returnedType;
						return returnedType;
					}
				}

				// Nothing loaded - if static, quit.
				if ((staticLinkedOnly) || (AssemblySimpleName == "*")) return null;

				//// Load with the loader.
				// If we've got one, use it directly, otherwise, get it from
				// type holder.
				if (loader == null) loader = GetAssemblyHolder().AssemblyLoader;

				//
				var asm = loader.LoadAssembly(AssemblySimpleName);
				returnedType = asm.PAFGetAccessibleType(NamespaceQualifiedTypeName);
				TypeType = returnedType;
				return returnedType;
			}
			catch (Exception ex)
			{
				var data = new PAFTypeExceptionData(this);
				throw new PAFStandardException<PAFTypeExceptionData>
				(data, PAFTypeMismatchExceptionMessageTags.ERROR_RESOLVING_TYPE, ex);

			}
		}
		/// <remarks>
		/// Calls "ResolveType(loader, false)".
		/// </remarks>
		public Type ResolveType(IPAFAssemblyLoader loader)
		{
			return ResolveType(loader, false);
		}
		/// <remarks>
		/// Calls "ResolveType(null, staticLinkedOnly)".
		/// </remarks>
		public Type ResolveType(bool staticLinkedOnly)
		{
			return ResolveType(null, staticLinkedOnly);
		}
		/// <remarks>
		/// Calls "ResolveType(null, false)".
		/// </remarks>
		public Type ResolveType()
		{
			return ResolveType(null, false);
		}
		#region Static Helpers
		/// <summary>
		/// This method assumes that we have an incoming string with the
		/// type name on the front end, separated by a comma from the assembly
		/// name. This method returns the namespace with no trailing dot.
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// It need not have an assembly name.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if the string is not in correct format or has no
		/// namespace.
		/// </returns>
		/// <remarks>
		/// The incoming string can be just a namespace with a trailing dot, although
		/// this is an unusual case.
		/// </remarks>
		public static string GetNameSpace(string assemblyQualifiedTypeNameString)
		{
			if (assemblyQualifiedTypeNameString == null) return null;
			var typeName = GetNamespaceQualifiedTypeName(assemblyQualifiedTypeNameString);
			if (typeName == null) return null;
			string simpleName = null;
			string nameSpace = null;
			Services.GetTypedService<IManufacturingUtils>().GetTypeAndNamespace(typeName, ref simpleName, ref nameSpace);
			return nameSpace;
		}
		/// <summary>
		/// This method assumes that we have an incoming string with the
		/// type name on the front end, separated by a comma from the assembly
		/// name. This method returns the unqualified (simple) type name.
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// It need not have an assembly name or namespace.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if the string is not in correct format or has no
		/// unqualified name.
		/// </returns>
		public static string GetUnqualifiedName(string assemblyQualifiedTypeNameString)
		{
			if (assemblyQualifiedTypeNameString == null) return null;
			var typeName = GetNamespaceQualifiedTypeName(assemblyQualifiedTypeNameString);
			if (typeName == null) return null;
			string simpleName = null;
			string nameSpace = null;
			Services.GetTypedService<IManufacturingUtils>().GetTypeAndNamespace(typeName, ref simpleName, ref nameSpace);
			return simpleName;
		}
		/// <summary>
		/// This method assumes that we have an incoming string with the
		/// type name on the front end, separated by a comma from the assembly
		/// name. This method returns the namespace-qualified type name. Note
		/// that the type may also be a simple type (the string at the end of
		/// the dots in a namespace-qualified type name) and contain no namespace
		/// specification. In this case, the simple name is returned.
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if the string is not in correct format or has no
		/// type name.
		/// </returns>
		public static string GetNamespaceQualifiedTypeName(string assemblyQualifiedTypeNameString)
		{
			if (assemblyQualifiedTypeNameString == null) return null;
			var assemblyName = GetAssemblyName(assemblyQualifiedTypeNameString);
			return assemblyName == null ? assemblyQualifiedTypeNameString
				: assemblyQualifiedTypeNameString.BreakStringInTwo()[0];
		}
		/// <summary>
		/// This method assumes that we have an incoming string with the
		/// type name on the front end, separated by a comma from the assembly
		/// name. This method returns the base assembly name without version
		/// information or anything else.
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// <see langword="null"/> or <see cref="string.Empty"/> returns <see langword="null"/>
		/// if <paramref name="typeType"/> is <see langword="null"/>.
		/// </param>
		/// <param name="typeType">
		/// The actual <see cref="Type"/>. For handling a Generic, this should ordinarily
		/// be non-Null.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if the both inputs are vacuous.
		/// type.
		/// </returns>
		/// <remarks>
		/// note: Brain T. - Modified to extract assembly name if a	Generic and
		/// added OPTIONAL type parameter.
		/// </remarks>
		public static string GetAssemblyName(string assemblyQualifiedTypeNameString, Type typeType = null)
		{
			string assemblyName;
			if (typeType != null)
			{
				return typeType.Assembly.AssemblySimpleName();
			}

			if (string.IsNullOrEmpty(assemblyQualifiedTypeNameString))
				return null;

			// If a generic, parse to get the simple name.
			if (assemblyQualifiedTypeNameString.Contains("`"))
			{
				// OK with no checks because of known name format.
				////////////
				assemblyName
					= assemblyQualifiedTypeNameString.SafeStringBeyondLastString("]");
				// Name of the assembly for this Generic is at the end of the whole thing.
				assemblyName = assemblyName.Substring(assemblyName.IndexOf(",", StringComparison.Ordinal) + 1);
				// Name has blanks.
				assemblyName = PAFString.Compress(assemblyName);
				return assemblyName.BreakStringInTwo()[0];
			}

			// OK with no checks because of known name format.
			////////////
			assemblyName = assemblyQualifiedTypeNameString.BreakStringInTwo()[1];
			// Name has blanks.
			assemblyName = PAFString.Compress(assemblyName);
			return assemblyName.BreakStringInTwo()[0];
		}

		/// <summary>
		/// This method overrides an assembly name if a concrete type is present to
		/// get the name from.
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// </param>
		/// <param name="type">
		/// If non-<see langword="null"/>, the assembly name is derived from the type.
		/// </param>
		/// <returns>
		/// If <paramref name="type"/> is <see langword="null"/>, the original
		/// <paramref name="assemblyQualifiedTypeNameString"/> is returned.
		/// </returns>
		public static string OverrideAssemblyName(string assemblyQualifiedTypeNameString,
			Type type)
		{
			if (type != null)
				assemblyQualifiedTypeNameString = type.AssemblyQualifiedName;
			return assemblyQualifiedTypeNameString;
		}

		/// <summary>
		/// This method just checks to see whether the type is POTENTIALLY resolvable
		/// with assemblies that are already loaded. It functions by just checking for
		/// the presence of a namespace. If a specific assembly name is present, no
		/// check is made to see whether that assembly is actually loaded into the
		/// current appdomain. TODO - KRM - why not?
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// <see langword="null"/> or <see cref="string.Empty"/> returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if the string is not in correct format or has no
		/// namespace.
		/// </returns>
		public static bool IsResolvableInternally(string assemblyQualifiedTypeNameString)
		{
			if (string.IsNullOrEmpty(assemblyQualifiedTypeNameString))
				return false;
			return (GetNameSpace(assemblyQualifiedTypeNameString) != null);
		}
		/// <summary>
		/// This method just checks to see whether the type is POTENTIALLY resolvable
		/// with an external assembly It functions by just checking for the presence
		/// of an assembly name namespace. If a specific assembly name is present, no
		/// check is made to see whether that assembly is actually available is the
		/// search path, manifest or other access mechanism. TODO - KRM - why not?
		/// TODO - please put on TODO list. Probably need partial method here.....
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString">
		/// String in the same format that comes into the constructor of this class.
		/// <see langword="null"/> or <see cref="string.Empty"/> returns <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if the string is not in correct format or has no
		/// assembly name or assembly name is the "*" wildcard..
		/// </returns>
		public static bool IsResolvableExternally(string assemblyQualifiedTypeNameString)
		{
			if (string.IsNullOrEmpty(assemblyQualifiedTypeNameString))
				return false;
			if (GetAssemblyName(assemblyQualifiedTypeNameString) == null) return false;
			return (GetAssemblyName(assemblyQualifiedTypeNameString) != "*");
		}
		#endregion // Static Helpers
		#region Conversion Operators
		/// <summary>
		/// Calls <c>PAFTypeHolderBase(Type)</c>.
		/// </summary>
		/// <param name="type">
		/// The type to be wrapped. Not <see langword="null"/>.
		/// </param>
		/// <returns>
		/// One of us.
		/// </returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if <paramref name="type"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public static implicit operator PAFTypeHolderBase(Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));
			return new PAFTypeHolderBase(type);
		}
		#endregion // Conversion Operators
		/// <summary>
		/// Override to print out something useful.
		/// </summary>
		/// <returns>
		/// The <see cref="AssemblyQualifiedTypeName"/>.
		/// </returns>
		public override string ToString()
		{
			return AssemblyQualifiedTypeName;
		}
		#region Obligatory Patch for Equals and Hash Code
		/// <summary>
		/// Determines whether the specified <see cref="object"/> is equal to the
		/// current <see cref="object"/>.
		/// </summary>
		/// <returns>
		/// <see langword="true"/> if the specified <see cref="object"/> is equal to the current
		/// <see cref="object"/>; otherwise, false.
		/// </returns>
		/// <param name="obj">
		/// The <see cref="object"/> to compare with the current <see cref="object"/>.
		/// </param>
		/// <remarks>
		/// Patch for Microsoft's mistake.
		/// </remarks>
		public override bool Equals(object obj)
		{
			if (obj == null) return false;
			// ReSharper disable BaseObjectEqualsIsObjectEquals
			return GetType() == obj.GetType() && base.Equals(obj);
			// ReSharper restore BaseObjectEqualsIsObjectEquals
		}
		/// <summary>
		/// We are a reference type so just call base to shut up the compiler/tools.
		/// </summary>
		/// <returns>
		/// The original hash code.
		/// </returns>
		public override int GetHashCode()
		{
			// ReSharper disable BaseObjectGetHashCodeCallInGetHashCode
			return base.GetHashCode();
			// ReSharper restore BaseObjectGetHashCodeCallInGetHashCode
		}
		#endregion // Obligatory Patch for Equals and Hash Code
		#endregion // Methods
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>.
		/// </summary>
		public IList<Type> ActualTypes
		{
			get
			{
				if (m_GenericChildTypes.SafeCount() == 0)
					return null;

				var types = new Collection<Type>();

				foreach (var gType in m_GenericChildTypes)
				{
					types.Add(gType.NodeType);
				}

				return types;
			}
		}
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>. internal setter for testing.
		/// </summary>
		public IList<IPAFGenericTypeNode> ChildTypeNodes
		{
			get { return m_GenericChildTypes;}
			protected internal set { m_GenericChildTypes = value; }
		}
		/// <summary>
		/// <see cref="IPAFGenericTypeNode"/>. internal setter for testing.
		/// </summary>
		public Type NodeType
		{
			get { return m_NodeType; }
			protected internal set { m_NodeType = value; }
		}
		public IPAFGenericTypeNode GenericParentNode
		{
			get { return m_GenericParentNode;}
			protected internal set { m_GenericParentNode = value; }
		}
		#region Methods
		#region Static Helpers
		/// <summary>
		/// Wraps an enumeration of <see cref="Type"/>s in our
		/// <see cref="PAFGenericTypeNode"/>s.
		/// </summary>
		/// <param name="actualTypes">
		/// Incoming <see cref="Type"/>s. <see langword="null"/> is OK.
		/// </param>
		/// <returns>
		/// <see langword="null"/> for no types coming in.
		/// </returns>
		public IList<IPAFGenericTypeNode> GetWrappedTypes(IEnumerable<Type> actualTypes)
		{
			actualTypes = actualTypes.ToArray();
			if (actualTypes.SafeCount() == 0)
				return null;

			var gTypes = new Collection<IPAFGenericTypeNode>();

			foreach (var type in actualTypes)
			{
				var holder = new PAFTypeHolderBase(type);
				holder.m_GenericParentNode = this;
				gTypes.Add(new PAFTypeHolderBase(type));
			}

			return gTypes;
		}
		#endregion // Static Helpers
		#endregion Methods
	}
}
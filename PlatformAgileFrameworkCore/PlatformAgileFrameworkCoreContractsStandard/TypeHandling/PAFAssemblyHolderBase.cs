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
using System.Reflection;
using System.Security;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.StringParsing;

namespace PlatformAgileFramework.TypeHandling
{
	#region Delegates
	/// <summary>
	/// Compares two assembly names to see if one (<paramref name="candidateAssembly"/>
	/// can satisfy the requirements of the other (<paramref name="assemblyToMatch"/>).
	/// </summary>
	/// <param name="assemblyToMatch">First one.</param>
	/// <param name="candidateAssembly">Second one.</param>
	/// <returns>
	/// <see langword="true"/> if the <paramref name="candidateAssembly"/> is compatible.
	/// </returns>
	public delegate bool CheckCandidateAssembly(IPAFAssemblyHolder assemblyToMatch, IPAFAssemblyHolder candidateAssembly);
	#endregion Delegates

	/// <summary>
	/// <para>
	///	Contains information about an assembly that we need in PAF. Simpler
	/// than the <c>AssemblyName</c> class in BCL. Just let's us putz with
	/// the actual stringful name. This class can only hold "leaf" nodes in
	/// a Generic type tree.
	/// </para>
	/// </summary>
	/// <threadsafety>
	/// Immutable class.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> Brian T. </author>
	/// <date> 01feb2019 </date>
	/// <description>
	/// Modified to handle Generic types.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <date> 06nov2011 </date>
	/// <description>
	/// <author> DAP </author>
	/// <para>
	/// Added history, changed name space. Put on the interfaces. Refactored to use
	/// a provider for the holder.
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	// ReSharper disable once PartialTypeWithSinglePart
	// Core part.
	public abstract partial class PAFAssemblyHolderBase : IPAFAssemblyHolderInternal,
		IPAFAssemblyHolderProvider
	{
		#region Fields and Autoproperties

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal Assembly m_Assembly;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal CheckCandidateAssembly m_AssemblyChecker;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyCulture;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal IPAFAssemblyLoader m_AssemblyLoader;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyNameString;

		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyPublicKeyToken;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblySimpleName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyStrongName;
		/// <summary>
		/// Backing.
		/// </summary>
		protected internal string m_AssemblyVersion;
	    /// <summary>
	    /// See <see cref="IPAFServiceManager{T}"/>. We staple
	    /// the root generic service manager in.
	    /// </summary>
	    protected internal static IPAFServiceManager<IPAFService> Services { get; set; }
		#endregion // Fields and Autoproperties
        #region Constructors
        /// <summary>
        /// For the serializer.
        /// </summary>
        protected internal PAFAssemblyHolderBase(){}

		/// <summary>
		/// Builds with a string representation of an assembly name.
		/// The format is:
		/// <c>SimpleAssemblyName {,Culture = CultureInfo} {,Version = Major.Minor.Build.Revision} {,StrongName} {,PublicKeyToken}</c>,
		/// where braces indicate optional fields.
		/// </summary>
		/// <param name="assemblyName">
		///     String to parse and build from. Ignored if <paramref name="assembly"/> is not <see langword="null"/>.
		/// </param>
		/// <param name="assembly">
		///     Assembly to be built with if it is available locally.
		/// </param>
		/// <param name="assemblyChecker">
		///     Delegate to check a candidate assembly for suitability to load.
		/// </param>
		/// <param name="assemblyLoader">
		///     Optional loader to be installed. If <see langword="null"/>, the default loader for
		///     the environment is used.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="assemblyName"/>
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// <exception cref="ArgumentException"> is thrown if
		/// <paramref name="assemblyName"/>
		/// is malformed.
		/// </exception>
		/// </exceptions>
		/// <remarks>
		/// This version only populates the <see cref="AssemblySimpleName"/>.
		/// </remarks>
		protected PAFAssemblyHolderBase(string assemblyName, Assembly assembly = null,
			CheckCandidateAssembly assemblyChecker = null, IPAFAssemblyLoader assemblyLoader = null)
		{
			m_Assembly = assembly;
			if (m_Assembly != null)
				assemblyName = m_Assembly.FullName;

			if (string.IsNullOrEmpty(assemblyName))
				throw new ArgumentNullException(nameof(assemblyName));
			m_AssemblyNameString = assemblyName;

			// Install the default checker if we don't have one.
			m_AssemblyChecker = assemblyChecker ?? SecondAssemblyWillWork;

			if (assemblyLoader == null)
				m_AssemblyLoader = PAFAssemblyLoader.GetDefaultAssemblyLoader();
			// If it's in simple format, we're done.
			if (m_AssemblyNameString.IndexOf(',') < 0)
			{
				m_AssemblySimpleName = m_AssemblyNameString.Trim();
				return;
			}

			// Work on the optional stuff.
			var split = assemblyName.BreakStringInTwo();
			if ((split == null) || (split.Count != 2) || (split[0] == null) || (split[1] == null))
				throw new ArgumentException("assemblyNameString = " + assemblyName);

			// This lives on the front.
			m_AssemblySimpleName = split[0].Trim();
		}
		/// <summary>
		/// Copy constructor. Note that this is NOT a full deep copy. We copy
		/// the loader and the checker by reference. This is a conscious design decision,
		/// since the loaders in extended are more heavyweight than
		/// ones like <see cref="PAFAssemblyLoader"/> and they are all supposed
		/// to be thread-safe, anyway. Same deal with the checker. It should
		/// be thread-safe.
		/// </summary>
		/// <param name="assemblyHolder">
		/// Instance of us.
		/// </param>
		/// <remarks>
		/// Simple implementation just causes a reparse of the string. If
		/// the old one was good, the new one will be, too. Also copies the
		/// assembly checker.
		/// </remarks>
		protected PAFAssemblyHolderBase(IPAFAssemblyHolder assemblyHolder)
		{
			if(assemblyHolder == null)
				throw new ArgumentNullException(nameof(assemblyHolder));
			TransferAssemblyHolderProps(assemblyHolder);
		}
		
		#endregion Constructors
		#region Properties
		/// <summary>
		/// Holds the assembly, if available. Will return <see langword="null"/> if the assembly
		/// is not local or has not been resolved.
		/// </summary>
		public Assembly Asmbly
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.Asmbly : m_Assembly;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				if (value != null) {
					AssemblyNameString = value.FullName;
				}
				m_Assembly = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		// TODO - KRM - this should be specifiable in the resolve method.
		// TODO - maybe it is intended to be loadable in the holder?
		// TODO - document the mechanism, pleasze?
		public CheckCandidateAssembly AssemblyChecker
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyChecker : m_AssemblyChecker;
			}
			[SecurityCritical]
			set { m_AssemblyChecker = value; }
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblyCulture
		{ 
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyCulture : m_AssemblyCulture;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyCulture = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public IPAFAssemblyLoader AssemblyLoader
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyLoader : m_AssemblyLoader;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyLoader = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblyNameString
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyNameString : m_AssemblyNameString;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyNameString = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblyPublicKeyToken
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyPublicKeyToken : m_AssemblyPublicKeyToken;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyPublicKeyToken = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblySimpleName
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblySimpleName : m_AssemblySimpleName;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblySimpleName = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblyStrongName
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyStrongName : m_AssemblyStrongName;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyStrongName = value;
			}
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		public string AssemblyVersion
		{
			get
			{
				var holder = GetAssemblyHolder();
				return !Equals(holder, this) ? holder.AssemblyVersion : m_AssemblyVersion;
			}
			[SecurityCritical]
			set
			{
				DelegatedHolderSetGuard();
				m_AssemblyVersion = value;
			}
		}
		#endregion // Properties
		#region IPAFAssemblyHolderProvider Implementation
		/// <summary>
		/// See <see cref="IPAFAssemblyHolderProvider"/>.
		/// This just allows us to provide ourselves.
		/// </summary>
		public virtual IPAFAssemblyHolder GetAssemblyHolder()
		{
			return this;
		}
		#endregion IPAFAssemblyHolderProvider Implementation
		#region Methods
		/// <summary>
		/// This method just throws an exception when we try to set a property on
		/// a <see cref="IPAFAssemblyHolder"/> that we are delegating to. It's
		/// properties are not settable.
		/// </summary>
		/// <returns>
		/// A holder if it is us. We still can't set its properties.
		/// </returns>
		/// <exceptions>
		/// <exception cref="InvalidOperationException"> is thrown if
		/// one of our subclasses is using a delegated <see cref="IPAFAssemblyHolder"/>.
		/// "Can't set singleton assy delegate".
		/// </exception>
		/// </exceptions>
		protected IPAFAssemblyHolder DelegatedHolderSetGuard()
		{
			var holder = GetAssemblyHolder();
			if (!Equals(holder, this))
				throw new InvalidOperationException("Can't set singleton assy delegate");
			return holder;
		}
		/// <summary>
		/// Resolves an assembly if it is not already resolved.
		/// </summary>
		/// <param name="holder">
		/// Descriptor for the assembly.
		/// </param>
		/// <param name="loader">
		/// Loader that can load an assembly, if needed. If <see langword="null"/>,
		/// the loader on the <paramref name="holder"/> is tried. If this is
		/// <see langword="null"/>, <see cref="PAFAssemblyLoader.GetDefaultAssemblyLoader"/>
		/// is used.
		/// </param>
		/// <param name="staticLinkedOnly">
		/// <see langword="true"/> to load only from this (current) assembly's manifest
		/// or from assemblies already loaded. Default is <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword="null"/> if assembly not found.
		/// </returns>
		public static Assembly ResolveAssembly(IPAFAssemblyHolder holder, IPAFAssemblyLoader loader = null,
			bool staticLinkedOnly = false)
		{
			// Already resolved?
			if (holder.Asmbly != null) return holder.Asmbly;

			var assys = Services.GetTypedService<IManufacturingUtils>().GetAppDomainAssemblies();
			foreach (var assy in assys) {
				// Scan loaded assemblies.
				var assyHolder = new PAFAssemblyHolder(assy.FullName, assy);
				if (holder.AssemblyChecker(assyHolder, holder)) {
					return assy;
				}
			}

			// Nothing loaded - if static, quit.
			if (staticLinkedOnly) return null;

			// Make sure we've got a loader.
			if (loader == null) loader = holder.AssemblyLoader;
			if (loader == null) loader = PAFAssemblyLoader.GetDefaultAssemblyLoader();

			// Load with the loader.
			var asm = loader.LoadAssembly(holder.AssemblyNameString);
			return asm;
		}
		/// <summary>
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </summary>
		/// <param name="loader">
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </param>
		/// <param name="staticLinkedOnly">
		/// See <see cref="IPAFAssemblyHolder"/>.
		/// </param>
		public virtual void ResolveMyAssembly(IPAFAssemblyLoader loader = null, bool staticLinkedOnly = false)
		{
			if(Asmbly != null) return;
			var assy = ResolveAssembly(GetAssemblyHolder(), loader, staticLinkedOnly);
			m_Assembly = assy;
		}
		/// <summary>
		/// Little helper for copy constructors and the like that just copies
		/// fields.
		/// </summary>
		/// <param name="assemblyHolder">
		/// Incoming <see cref="IPAFAssemblyHolder"/> to copy fields from.
		/// </param>
		protected void TransferAssemblyHolderProps(IPAFAssemblyHolder assemblyHolder)
		{
			m_Assembly = assemblyHolder.Asmbly;
			m_AssemblyChecker = assemblyHolder.AssemblyChecker;
			m_AssemblyCulture = assemblyHolder.AssemblyCulture;
			m_AssemblyLoader = assemblyHolder.AssemblyLoader;
			m_AssemblyNameString = assemblyHolder.AssemblyNameString;
			m_AssemblyPublicKeyToken = assemblyHolder.AssemblyPublicKeyToken;
			m_AssemblySimpleName = assemblyHolder.AssemblySimpleName;
			m_AssemblyStrongName = assemblyHolder.AssemblyStrongName;
			m_AssemblyVersion = assemblyHolder.AssemblyVersion;
		}

		/// <summary>
		/// Returns <see langword="null"/> if holder is <see langword="null"/>.
		/// </summary>
		/// <param name="assemblyHolder">Incoming holder.</param>
		/// <returns>
		/// Wrapped assembly or <see langword="null"/>.
		/// </returns>
		public static Assembly AssemblyFromHolder(IPAFAssemblyHolder assemblyHolder)
		{
		    return assemblyHolder?.Asmbly;
		}

		/// <summary>
		/// Compares two assembly names and determines whether the <paramref name="candidateAssy"/>
		/// will fulfill the specs of the <paramref name="assyToMatch"/>. If the <paramref name="assyToMatch"/>
		/// has certain fields defined, the <paramref name="candidateAssy"/> must have matching fields
		/// defined. If <paramref name="assyToMatch"/> is wildcarded
		/// (<see cref="IPAFAssemblyHolder.AssemblySimpleName"/> = "*"), match will be succcessful
		/// if other fields are compatible.
		/// </summary>
		/// <param name="assyToMatch">Our template assembly.</param>
		/// <param name="candidateAssy">The assembly we hope will match.</param>
		/// <returns><see langword="false"/> if a mismatch.</returns>
		public static bool SecondAssemblyWillWork(IPAFAssemblyHolder assyToMatch,
			IPAFAssemblyHolder candidateAssy)
		{
			// Name can be wildcarded.
			if (assyToMatch.AssemblySimpleName != "*") {
				if (string.CompareOrdinal(assyToMatch.AssemblySimpleName, candidateAssy.AssemblySimpleName) != 0)
					return false;
			}

			// version.
			if (!string.IsNullOrEmpty(assyToMatch.AssemblyVersion))
				if (string.CompareOrdinal(assyToMatch.AssemblyVersion, candidateAssy.AssemblyVersion) != 0)
					return false;

			// culture.
			if (!string.IsNullOrEmpty(assyToMatch.AssemblyCulture))
				if (string.CompareOrdinal(assyToMatch.AssemblyCulture, candidateAssy.AssemblyCulture) != 0)
					return false;

			// strongname.
			if (!string.IsNullOrEmpty(assyToMatch.AssemblyStrongName))
				if (string.CompareOrdinal(assyToMatch.AssemblyStrongName, candidateAssy.AssemblyStrongName) != 0)
					return false;

			// token.
			if (!string.IsNullOrEmpty(assyToMatch.AssemblyPublicKeyToken))
				if (string.CompareOrdinal(assyToMatch.AssemblyPublicKeyToken, candidateAssy.AssemblyPublicKeyToken) != 0)
					return false;
			return true;
		}
		/// <summary>
		/// Override to print out something useful.
		/// </summary>
		/// <returns>
		/// The <see cref="AssemblyNameString"/>.
		/// </returns>
		public override string ToString()
		{
			return AssemblyNameString;
		}
		#endregion // Methods
		#region IPAFAssemblyHolderInternal Implementation
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssembly(Assembly asmbly)
		{
			m_Assembly = asmbly;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyChecker(CheckCandidateAssembly assemblyChecker)
		{
			m_AssemblyChecker = assemblyChecker;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyCulture(string assemblyCulture)
		{
			m_AssemblyCulture = assemblyCulture;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyLoader(IPAFAssemblyLoader assemblyLoader)
		{
			m_AssemblyLoader = assemblyLoader;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyNameString(string assemblyNameString)
		{
			m_AssemblyNameString = assemblyNameString;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyPublicKeyToken(string assemblyPublicKeyToken)
		{
			m_AssemblyPublicKeyToken = assemblyPublicKeyToken;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblySimpleName(string assemblySimpleName)
		{
			m_AssemblySimpleName = assemblySimpleName;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyStrongName(string assemblyStrongName)
		{
			m_AssemblyStrongName = assemblyStrongName;
		}
		/// <remarks>
		/// See <see cref="IPAFAssemblyHolderInternal"/>.
		/// </remarks>
		void IPAFAssemblyHolderInternal.SetAssemblyVersion(string assemblyVersion)
		{
			m_AssemblyVersion = assemblyVersion;
		}
		#endregion // IPAFAssemblyHolderInternal Implementation
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
		#region Static Helpers
		/// <summary>
		/// Just checks for the <c>'</c> character in Generic type name string. 
		/// </summary>
		/// <param name="assemblyQualifiedTypeNameString"></param>
		/// <returns>
		/// <see langword="true"/> if non - <see langword="null"/> and a
		/// Generic.
		/// </returns>
		public static bool IsGenericTypeName(string assemblyQualifiedTypeNameString)
		{
			if ((!string.IsNullOrEmpty(assemblyQualifiedTypeNameString)
			     && assemblyQualifiedTypeNameString.Contains("`")))
				return true;
			return false;
		}

		#endregion // Static Helpers
	}
}

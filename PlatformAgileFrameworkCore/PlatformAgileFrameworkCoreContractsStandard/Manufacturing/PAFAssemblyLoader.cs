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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-
#pragma warning restore 1587

using System;
using System.Reflection;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Manufacturing.Exceptions;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Simple default implementation of <see cref="IPAFAssemblyResolver"/> that loads
	/// assemblies into the current "AppDomain".
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	// core
	public partial class PAFAssemblyLoader : IPAFAssemblyLoader
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class AutoProperties
		/// <summary>
		/// See <see cref="IPAFAssemblyLoader"/>. Bad format is never ignored in this
		/// implementation.
		/// </summary>
		public bool IgnoreBadFormat { get; set; }
	    /// <summary>
	    /// See <see cref="IPAFAssemblyLoader"/>. We always allow simple names in this
	    /// implementation.
	    /// </summary>
	    public bool ProcessSimpleAssemblyName { get; set; }
        /// <summary>
        /// See <see cref="IPAFServiceManager{IPAFService}"/>. We staple the root generic service manager in.
        /// </summary>
        protected internal IPAFServiceManager<IPAFService> Services { get;  internal set; }
        #endregion Class AutoProperties
        #region Constructors

	    public PAFAssemblyLoader()
	    {
	        Services = PAFServiceManagerContainer.ServiceManager;
	    }

	    #endregion // Constructors
        /// <summary>
        /// See <see cref="IPAFAssemblyLoader"/>. This simple implementation employs the
        /// <see cref="Assembly.Load(AssemblyName)"/> static method to load DLL's. This means that
        /// DLL's must be in the standard probe path of an application. Dumping them all in
        /// the application directory will do nicely.
        /// </summary>
        /// <param name="assemblyName">
        /// <remarks>
        /// This method also has the side effect of adding the assembly to the list of
        /// loaded assemblies, if it is not already there. For the full standard ECMA
        /// environment, this step is always unnecessary, since the <c>AppDomain.GetAssemblies()</c>
        /// is available. In Silverlight and other environments, other tricks have
        /// to be played to ensure that <see cref="IManufacturingUtils.GetAppDomainAssemblies"/>
        /// provides the correct result.
        /// </remarks>
        /// See <see cref="IPAFAssemblyLoader"/>.
        /// </param>
        /// <returns>
        /// See <see cref="IPAFAssemblyLoader"/>.
        /// </returns>
        /// <exceptions>
        /// See <see cref="IPAFAssemblyLoader"/>.
        /// </exceptions>
        public virtual Assembly LoadAssembly(string assemblyName)
		{
			if(string.IsNullOrEmpty(assemblyName))
				throw new ArgumentNullException("assemblyName");
			Assembly asm;
			try
			{
				asm = Assembly.Load(new AssemblyName(assemblyName));
			}
			catch (Exception ex)
			{
				var data = new PAFAssemblyLoadExceptionData(new PAFAssemblyHolder(assemblyName, null));
				throw new PAFStandardException<IPAFAssemblyLoadExceptionData>
					(data, PAFAssemblyLoadExceptionMessageTags.GENERAL_ASSEMBLY_LOAD_ERROR, ex);

			}
			if (asm != null) Services.GetTypedService<IManufacturingUtils>().AddAssemblyToAssembliesLoaded(asm);
			return asm;
		}
		/// <summary>
		/// If there is a "standard" loader for the current environment, this
		/// will return it.
		/// </summary>
		/// <returns>
		/// <see langword="null"/> if no loader is available.
		/// </returns>
		// TODO - KRM. Rework this into a partial class so a standard loader can
		// TODO be grabbed from the other part. I don't know how we'll handle things
		// TODO like the plugin loader, which has construction params.
		public static IPAFAssemblyLoader GetDefaultAssemblyLoader()
		{
			return new PAFAssemblyLoader();
		}
	}

}
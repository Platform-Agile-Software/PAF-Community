using System;
using System.Linq;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.Manufacturing;

namespace PlatformAgileFramework.Testing
{
    /// <summary>
    /// Bootstraps the system. This should be inherited by all tests.
    /// </summary>
    //[TestFixture] TODO - KRM move this into a separate testing utils assembly.
    public class BootstrappingFixture
	{
		#region Startup
		/// <summary>
		/// This one just loads the service manager with default services
		/// for tests.
		/// </summary>
		//[Test]
		public virtual void BootstrapSystem()
		{
			ManufacturingUtils.DirectoryMappingFileName
				= "TestSymbolicDirectories.xml";

            // We push in the lister, since it is not available in .Net core.
		    ManufacturingUtils.AssemblyLister =  ( ) => AppDomain.CurrentDomain.GetAssemblies().ToList();

		    // Needed for implementations to be found.
		    ManufacturingUtils.Instance.LoadPlatformAssembly();


            ServiceBootStrapper.Instance.LoadCoreServices();
		}
		#endregion  //Startup
	}
}


using NUnit.Framework;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.Testing;

namespace PlatformAgileFramework.FrameworkServices
{
    /// <summary>
    /// Does some very basic tests for the service manager, like loading. This
    /// should be inherited by all tests.
    /// </summary>
    [TestFixture]
	public class ECMAServiceManagerTests: BootstrappingFixture
	{
		#region Startup
		/// <summary>
		/// This one just loads the service manager with default services
		/// for tests.
		/// </summary>
		[Test]
		public override void BootstrapSystem()
		{
            // So our platform assembly gets loaded.
            var type = typeof(PAFStorageServiceECMA);
            base.BootstrapSystem();

			ServiceBootStrapper.Instance.LoadCoreServices();

            // Passes if we got through setup.
            Assert.True(true);
		}
		#endregion  //Startup
	}
}


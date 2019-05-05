using NUnit.Framework;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.Testing;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FrameworkServices.Tests
{
	/// <summary>
	/// This is a base test fixture that should be inherited from for any test fixture
	/// containing tests that access services. This must be true, since we want to support
	/// ordinary NUnit which doesn't allow test fixture execution to be ordered.
	/// </summary>
	// [TestFixture]
	public class BasicServiceManagerTestFixtureBase: BootstrappingTestFixtureBase
	{
		/// <summary>
		/// Gets around the problem of NUnit being crippled without test fixture setups.
		/// </summary>
		[SetUp]
		public override void SetUp()
		{
			base.SetUp();
		}

		/// <summary>
		/// Loads the platform-specific assy and initializes the service manager, which
		/// discovers its services inside. This represents the INITIAL set of tests themselves,
		/// since the SM must be "bootstrapped" and initialized at the beginning of
		/// application execution.
		/// </summary>
		public override void TestFixtureSetUp()
        {
            base.TestFixtureSetUp();
            ServiceBootStrapper.Instance.LoadCoreServices();
        }
	}
}


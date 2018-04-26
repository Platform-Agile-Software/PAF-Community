using NUnit.Framework;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.FrameworkServices.Tests
{
	[TestFixture]
	public class BasicServiceManagerTests: BasicServiceManagerTestFixtureBase
	{

		/// <summary>
		/// This one should be fired first (remember NUnit alphabetizes test order),
		/// ideally, but we really don't care. This one will trigger the load of the
		/// SM statics, which happens in the textfixture setup.
		/// </summary>
		[Test]
		public void ATestToSetUpServiceManager()
		{
			// Test frameworks need to recognize that this is a valid test.
			Assert.IsTrue(true);
		}

		/// <summary>
		/// This one just triggers instantiation.
		/// </summary>
		[Test]
		public void InstantiateServiceManager()
		{
			var serviceManager = PAFServices.Manager;
			// Test frameworks need to recognize that this is a valid test.
			Assert.IsTrue(serviceManager != null);
		}
	}
}


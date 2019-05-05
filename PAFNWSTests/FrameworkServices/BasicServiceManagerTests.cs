using NUnit.Framework;
using PlatformAgileFramework.Logging;

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
	    public void BTestToInstantiateServiceManager()
	    {
	        var serviceManager = PAFServices.Manager;
	        // Test frameworks need to recognize that this is a valid test.
	        Assert.IsTrue(serviceManager != null);
	    }

		/// <summary>
		/// This one just checks that the service dictionary can have
		/// entries replaced.
		/// </summary>
		[Test]
		public void CTestToTestDictionaryReplacements()
		{
			var serviceManager = PAFServices.Manager;

			// We can add ONLY service interfaces.
			IPAFLoggingService myFirstLoggingService
				= new PAFLoggingService(PAFLoggingLevel.Error, true, null, "MyFirstLoggingService.log");
			IPAFLoggingService mySecondLoggingService
				= new PAFLoggingService(PAFLoggingLevel.Error, true, null, "MySecondLoggingService.log");

			// Install the first logging service.
			serviceManager.AddTypedService(myFirstLoggingService, "MyLoggingService");

			// Use service 1.
			var service1 = serviceManager.GetTypedService<IPAFLoggingService>("MyLoggingService", true, null);
			service1.LogEntry("MyFirstLoggingService.log");
			var service1Output = ((IPAFLogfileReader)service1).ReadInstanceLogFile();

			// Check the write.
			Assert.IsTrue(service1Output.Contains("MyFirstLoggingService.log"));

			// Now replace the logger with the second.
			IPAFServiceDescription<IPAFLoggingService> desc
				= new PAFServiceDescription<IPAFLoggingService>(mySecondLoggingService, "MyLoggingService");
			((IPAFServiceManagerInternal<IPAFService>)serviceManager).AddOrReplaceServiceInternal(desc);

			// Use service 2.
			var service2 = serviceManager.GetTypedService<IPAFLoggingService>("MyLoggingService", true, null);
			service2.LogEntry("MySecondLoggingService.log");
			var service2Output = ((IPAFLogfileReader)service2).ReadInstanceLogFile();

			// Check the write.
			Assert.IsTrue(service2Output.Contains("MySecondLoggingService.log"));
		}
		/// <summary>
		/// This test checks to ensure that we can't find a service that is not
		/// registered by it's interface with a "registered" search. We work with
		/// <see cref="EmergencyLoggingService"/> which is installed as a standard
		/// component. Then the test looks for the service in "all" services.
		/// Then registers the service and looks again for a registered service.
		/// </summary>
		[Test]
		public void DTestToTestRegisteredUnregistered()
		{
			// Find a service that is REGISTERED (true) by the type
			// IPAFEmergencyServiceProvider<IPAFLoggingService>>
			var service = PAFServiceManagerContainer.ServiceManager
				.GetTypedService<IPAFEmergencyServiceProvider<IPAFLoggingService>>(true, null);

			// Should not find it.
			Assert.IsTrue(service == null);

			// Find a service among ALL services that implement
			// IPAFEmergencyServiceProvider<IPAFLoggingService>>
			service = PAFServiceManagerContainer.ServiceManager
				.GetTypedService<IPAFEmergencyServiceProvider<IPAFLoggingService>>(false, null);

			// Should find it.
			Assert.IsTrue(service != null);

			// Register the emergency service as the default for
			// IPAFEmergencyServiceProvider<IPAFLoggingService>>.
			PAFServiceManagerContainer.ServiceManager.AddTypedService(service);

			// Try finding the service as a registered service again.
			service = PAFServiceManagerContainer.ServiceManager
				.GetTypedService<IPAFEmergencyServiceProvider<IPAFLoggingService>>();

			// Should find it.
			Assert.IsTrue(service != null);
		}
	}
}


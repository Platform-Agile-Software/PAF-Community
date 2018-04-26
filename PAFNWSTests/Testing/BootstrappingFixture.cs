using System;
using NUnit.Framework;
using PlatformAgileFramework.Manufacturing;
using PlatformAgileFramework.Platform;

namespace PlatformAgileFramework.Testing
{
    /// <summary>
    /// Bootstraps the system. This should be inherited by all tests.
    /// This class also allows easy retrofit of NUnit tests fixtures pre - version 3,
    /// which may have "TestFixtureTearDown" and "TestFixtureSetup" attibutes. 
    /// </summary>
    public class BootstrappingTestFixtureBase: IDisposable
	{
	    /// <summary>
	    /// Saves typing.
	    /// </summary>
	    // ReSharper disable once InconsistentNaming
	    public static char DS;

	    protected bool s_HasFixtureBeenSetupForTests;

	    /// <summary>
	    /// Gets around the problem of NUnit being crippled without testfixture setups.
	    /// </summary>
	    [SetUp]
	    public virtual void SetUp()
	    {
	        if (!s_HasFixtureBeenSetupForTests)
	        {
	            TestFixtureSetUp();
	            s_HasFixtureBeenSetupForTests = true;
	        }
	    }

	    /// <summary>
	    /// Original NUnit test fixture attribute name - normally called from
	    /// <see cref="IDisposable.Dispose()"/>, which OLD NUnit and PAFUnit calls.
	    /// </summary>
	    public virtual void TestFixtureTearDown()
	    {

	    }
        /// <summary>
        /// Sets some properties for tests.
        /// </summary>
        public virtual void TestFixtureSetUp()
	    {
	        if (!s_HasFixtureBeenSetupForTests)
	        {
	            s_HasFixtureBeenSetupForTests = true;

	            ManufacturingUtils.DirectoryMappingFileName = "TestSymbolicDirectories.xml";

	            DS = PlatformUtils.GetDirectorySeparatorChar();
	        }
		}
        /// <summary>
        /// This is one case where we don't have to follow the standard
        /// dispose pattern - all test frameworks are assumed to call
        /// dispose if it is there, no need for finalizers.
        /// </summary>
	    public virtual void Dispose()
	    {
	    }
	}
}


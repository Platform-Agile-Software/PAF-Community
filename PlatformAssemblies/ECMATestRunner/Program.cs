using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;
using System;
using System.Diagnostics;
using PlatformAgileFramework.FrameworkServices.ErrorAndException.Tests;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.UserInterface.ConsoleUI;
using PlatformAgileFramework.Manufacturing;
using System.Reflection;
using Newtonsoft.Json;
using PlatformAgileFramework.AssemblyHandling;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.Events.Tests;
using PlatformAgileFramework.FileAndIO.FileAndDirectoryService;
using PlatformAgileFramework.FileAndIO.SymbolicDirectories;
using PlatformAgileFramework.FileAndIO.Tests;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.FrameworkServices.Tests;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Logging.Tests;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display;
using PlatformAgileFramework.Tutorials;
using PlatformAgileFramework.TypeHandling.Disposal;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeTree.Tests;
using PlatformAgileFramework.UserInterface;
using PlatformAgileFramework.UserInterface.Interfaces;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework
{
	// ReSharper disable once InconsistentNaming
	public class ECMATestRunner
    {
	    private static void Main(string[] args)
        {
            // weird linking problem requires this to be linked into main.
            if (false)
#pragma warning disable 162
                JsonConvert.SerializeObject(null);
#pragma warning restore 162
			PlatformUtils.PlatformInfo = new PAF_ECMA4_6_2PlatformInfo();
			// For clarity, we set the application root to its default.
			// For ECMA, this will be the bin folder.
	        PlatformUtils.ApplicationRoot = null;
			// Touch it for initialization.
	        // ReSharper disable once UnusedVariable
	        var instance =  PlatformUtils.Instance;

            // Net standard deficiencies causes us to need to push in a couple of things.
            ManufacturingUtils.AssemblyLister = AppDomain.CurrentDomain.GetAssemblies;
            ManufacturingUtils.AssemblyLoadFromLoader = Assembly.LoadFrom;

			// Override standard file with test file.
            SymbolicDirectoryMappingDictionary.DirectoryMappingFileName
                = "TestSymbolicDirectories.xml";

			// Load our required services needed to even start.
			PAFServices.s_SiPAFUIServiceInternal
				= new PAFServiceDescription<IPAFUIService>(new ConsoleUserInteractionService());
	        PAFServices.SiPAFStorageServiceInternal
		        = new PAFServiceDescription<IPAFStorageService>(new PAFStorageServiceECMA());

			ServiceBootStrapper.Instance.LoadCoreServices();

			// We use Trace for our tests, so we build the factory and install it.
	        var traceLoggerFactory = new PAFTraceLoggerFactory();
	        PAFLoggingUtils.TraceLoggerFactory = traceLoggerFactory;

			// Get our test assembly.
			var assembly = typeof(BasicExceptionTests).Assembly;

			// Install a filter for ALL assembly infos to use.
			// In core, we have a one-to-one correspondence between fixtures
			// and wrappers.
	        PAFTestAssemblyInfo.DefaultGetDisplayChildElementItems
		        = PAFTestElementInfoExtensions.GetUntypedChildInfosOfType<IPAFTestElementInfo<IPAFTestAssemblyInfo>, IPAFTestFixtureInfo>;
			////////////////////////////////////////////////////////////////////////////
			///// To run tests in an assembly.
			//var assemblyInfo = new PAFTestAssemblyInfo(assembly.ToAssemblyholder(), null,
			//PAFTestFrameworkBehavior.GetStandardNUnitParams());
			//assemblyInfo.InitializeExePipeline(null);
			//assemblyInfo.RunPipelinedObject(null);
			//var testConsoleUI = new PAFTestResultUserInteraction(assemblyInfo.TestElementResultInfo);
			//testConsoleUI.ProcessCommand("OR");
			//return;


			//////////////////////////////////////////////////////////////////////////////
			///// To run tests in a fixture.
			var fixtureInfo = new PAFTestFixtureInfo(typeof(BasicServiceManagerTests).ToTypeholder(),
				PAFTestFrameworkBehavior.GetStandardNUnitParams());
			var fixtureWrapper = new PAFTestFixtureWrapper(fixtureInfo);
			fixtureWrapper.InitializeExePipeline(null);
			fixtureWrapper.RunPipelinedObject(null);
			var testConsoleUI = new PAFTestResultUserInteraction(fixtureWrapper.TestElementResultInfo);
			testConsoleUI.ProcessCommand("OR");


			//////////////////////////////////////////////////////////////////////////////
			/////// To run just some tests in a fixture, in this case, 1.
			////fixtureInfo.TestMethodInclusionList
			//// = new[] { "TestProperTaskCreationAndDisposal" };
			//var fixtureWrapper = new PAFTestFixtureWrapper(fixtureInfo);

			//// Fire off the tests.
			//fixtureWrapper.InitializeExePipeline(fixtureWrapper);
			//fixtureWrapper.RunPipelinedObject(null);
			//////////////////
			////// This runs the test Console UI.
			//var testConsoleUI = new PAFTestResultUserInteraction(fixtureInfo.TestElementResultInfo);
			//testConsoleUI.ProcessCommand(null);

		}
	}
}

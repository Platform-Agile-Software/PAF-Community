using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator;
using System;
using PlatformAgileFramework.FrameworkServices.ErrorAndException.Tests;
using PlatformAgileFramework.Application;
using PlatformAgileFramework.UserInterface.ConsoleUI;
using PlatformAgileFramework.Manufacturing;
using System.Reflection;
using PlatformAgileFramework.AssemblyHandling;
using PlatformAgileFramework.Events;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework
{
    class ECMATestRunner
    {
        static void Main(string[] args)
        {
            // Net standard deficiencies causes us to need to push in a couple of things.
            ManufacturingUtils.AssemblyLister = AppDomain.CurrentDomain.GetAssemblies;
            ManufacturingUtils.AssemblyLoadFromLoader = Assembly.LoadFrom;

			// Override standard file with test file.
            ManufacturingUtils.DirectoryMappingFilePathWithFile
                = "TestSymbolicDirectories.xml";

            // Just to get the ECMA assy statically loaded - .Net standard has trouble probing.
            var uiUtils = new ConsoleUIUtils();

            //var assemblies =  AppDomain.CurrentDomain.GetAssemblies();
            ServiceBootStrapper.Instance.LoadCoreServices();

			// Get our test assembly.
	        var assembly = typeof(BasicExceptionTests).Assembly;

			// Install a filter for ALL assembly infos to use.
			// In core, we have a one-to-one correspondance between fixtures
			// and wrappers.
	        PAFTestAssemblyInfo.DefaultGetDisplayChildElementItems
		        = PAFTestElementInfoExtensions.GetUntypedChildInfosOfType<IPAFTestElementInfo<IPAFTestAssemblyInfo>, IPAFTestFixtureInfo>;
			////////////////////////////////////////////////////////////////////////////
			///// To run tests in an assembly.
			var assemblyInfo = new PAFTestAssemblyInfo(assembly.ToAssemblyholder(), null,
			PAFTestFrameworkBehavior.GetStandardNUnitParams());
			assemblyInfo.InitializeExePipeline(null);
			assemblyInfo.RunPipelinedObject(null);
			var testConsoleUI = new PAFTestResultUserInteraction(assemblyInfo.TestElementResultInfo);
			testConsoleUI.ProcessCommand("OR");
			return;

			////////////////////////////////////////////////////////////////////////////
			///// To run tests in a fixture.
			//var fixtureInfo = new PAFTestFixtureInfo(typeof(MulticastNotifyPropertyChangedEventTests).ToTypeholder(),
			//	PAFTestFrameworkBehavior.GetStandardNUnitParams());
			//var fixtureWrapper = new PAFTestFixtureWrapper(fixtureInfo);
	  //      fixtureWrapper.InitializeExePipeline(null);
	  //      fixtureWrapper.RunPipelinedObject(null);
	  //      var testConsoleUI = new PAFTestResultUserInteraction(fixtureWrapper.TestElementResultInfo);
	    //    testConsoleUI.ProcessCommand("OR");


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

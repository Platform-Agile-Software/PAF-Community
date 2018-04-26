using System;
using System.Runtime.CompilerServices;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//[assembly: AssemblyTitle("PlatformAgileFrameworkCoreContracts")]
//[assembly: AssemblyDescription("Core Functionality for a PAF Application.")]

// We don't want to deal with any weird objects being thrown from legacy code.
[assembly : RuntimeCompatibility(WrapNonExceptionThrows = true)]

// This assembly is part of the PlatformAgileFramework and thus needs to be callable
// by multiple languages.
[assembly: CLSCompliant(true)]

// Methods in the core are called by partially trusted clients.
// SL sandbox in the browser is always partially trusted, so there's no attributes.
#if ECMACLR
[assembly: AllowPartiallyTrustedCallers]
[assembly: SecurityRules(SecurityRuleSet.Level2)]
#endif

// Our core framework assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAFCoreStandardLibrary, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our core framework assembly must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkCore, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our core framework assembly must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkCoreNWSPortable, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our test framework assembly must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkCoreTests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our test assembly must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkBasicxUnit, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our test assembly must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkBasicxUnitStandard1.6, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our manual test runner must have access to our internals.
[assembly: InternalsVisibleTo("PlatformAgileFrameworkCoreBasicxUnitCLRCaller, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our Android assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.Android, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our iOS assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.iOS, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our WindowsStore assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.WindowsStore, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// .Net standard tests.
[assembly: InternalsVisibleTo("PAF.CoreNetStandard.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our ECMA assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.ECMA4.6.2, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our ECMA test assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.ECMA4.6.2.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our Xamarin.Forms test runner assy needs access.
[assembly: InternalsVisibleTo("Xamarin.FormsTestRunner, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our Xamarin.Forms.iOS test runner assy needs access.
[assembly: InternalsVisibleTo("Xamarin.FormsTestRunner.iOS, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our Xamarin.Forms.Android test runner assy needs access.
[assembly: InternalsVisibleTo("Xamarin.FormsTestRunner.Droid, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our ECMA assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.ECMAStandard, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

// Our aspx assembly must have access to our internals.
[assembly: InternalsVisibleTo("PAF.WEB.ASPX, PublicKey=0024000004800000940000000602000000240000525341310004000001000100ef2ad5cee7542d2136cda49ea89523aa388485e151afb19119aa1f98b042396c8d43eb7248c8cbe31ea2eafd9d7f49677f2cd5e3764ddb0028370f8a431323498d08b877af41a34b9bd9023dba2ff6279ffdbf117cfeba4308df9b7a8a42cd8f7a7efe87e19297e387461f455edbaa5354b8bf55de6802e635921b2d8adcb095")]

#if ECMACLR
// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components. If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5c9e6eb7-50aa-4600-83f6-05271b6ea1c5")]

#endif
// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
//[assembly: AssemblyVersion("1.0.0.0")]
//[assembly: AssemblyFileVersion("1.0.0.0")]
//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
//[assembly: AssemblyKeyName("")]

using System;
using System.Reflection;
using System.Runtime.CompilerServices;
// ReSharper disable RedundantUsingDirective
// Needed for Framework binds. See below.....
using System.Security;
// ReSharper restore RedundantUsingDirective
using System.Runtime.InteropServices;


// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("PlatformAgileFrameworkBasicxUnit")]
[assembly: AssemblyDescription("Basic xUnit Emulation for testing a PAF Application.")]
#if SILVERLIGHT
[assembly: AssemblyConfiguration("SILVERLIGHT")]
#else
[assembly: AssemblyConfiguration("NetStand1.6")]
#endif
[assembly: AssemblyCompany("Platform Agile Software")]
[assembly: AssemblyProduct("PlatformAgileFrameworkBasicxUnit")]
[assembly: AssemblyCopyright("Copyright ©  2004 - 2012")]
[assembly: AssemblyTrademark("Platform Agile Software ©")]
[assembly: AssemblyCulture("")]

// We don't want to deal with any wierd objects being thrown from legacy code.
[assembly : RuntimeCompatibility(WrapNonExceptionThrows = true)]

// This assembly is part of the PlatformAgileFramework and thus needs to be callable
// by multiple languages.
[assembly: CLSCompliant(false)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components. If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5c9e6eb7-50aa-4600-83f6-05271b6ea1c5")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
//[assembly: AssemblyDelaySign(false)]
//[assembly: AssemblyKeyFile("")]
//[assembly: AssemblyKeyName("")]

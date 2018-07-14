# PAF-Community
This directory contains the "community" version of Platform-Agile Framework.
It is released under the MIT license.
Subdirectories are as follows:
1)PlatformAgileFrameworkCoreContractsStandard
Contains interfaces and items that should be publicly available to all apps.
This is an abstract library which needs to be coupled to a platform assembly 
Currently build again .NetStandard 2.01)PlatformAgileFrameworkCoreContractsStandard

2)PlatformAgileFrameworkCoreStandard
Contains some secure items that items that are available only to "trusted" callers.
This is an abstract library which needs to be coupled to a platform assembly 
Currently build against .NetStandard 2.0

3)OptionalLibraries
Contains libraries like BasicxUnit, which is a testing suite.
Currently build against .NetStandard 2.0

4)PlatformAssemblies
These are the platform-specific assemblies which map the abstractions
in the core libraries into platform-specific functionality. The file system is an example.
Built against many different versions of .Net, depending on what the platforms need.

5)PAFNWSTests
Contains "abstract" tests, which require binding with a platform assembly for implementation
on a specific platform.
Built against .NetStandard 2.0.

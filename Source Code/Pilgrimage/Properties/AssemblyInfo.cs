using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("BlastPhyMe (BLAST, Phylogenies, and Molecular Evolution)")]
[assembly: AssemblyDescription("")]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#elif EEB460
[assembly: AssemblyConfiguration("EEB460")]
#elif BETA
[assembly: AssemblyConfiguration("Beta")]
#elif DEMO
[assembly: AssemblyConfiguration("Demo")]
#elif DOCUMENTATION
[assembly: AssemblyConfiguration("")]
#elif RELEASE
[assembly: AssemblyConfiguration("Release")]
#else
[assembly: AssemblyConfiguration("Configuration Not Specified")]
#endif
[assembly: AssemblyCompany("Chang Lab")]
[assembly: AssemblyProduct("BlastPhyMe")]
[assembly: AssemblyCopyright("Copyright © 2014-2015, Chang Lab")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("d5d3a9fa-1fa8-48ac-9656-172d19582183")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.5.0.12")]
[assembly: AssemblyFileVersion("1.5.0.12")]
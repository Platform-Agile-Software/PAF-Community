using System;
using System.Collections.Generic;
using System.Reflection;
using PlatformAgileFramework.FileAndIO;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	/// Extension methods for the <see cref="Assembly"/> class.
	/// </summary>
	public static class AssemblyExtensionMethods
	{
        /// <summary>
        /// Gets the name of the assembly without any extension, without version or public key
        /// information. Just the base name of the file or the name of the assembly if a
        /// generated assembly.
        /// </summary>
        /// <param name="assembly"><see cref="Assembly"/> to get the name for.</param>
        /// <returns>Simple name.</returns>
        public static string GetAssemblySimpleName(this Assembly assembly)
        {
            var fullName = assembly.FullName;
            // Chop after first comma, if exists.
            int found;
            if ((found = fullName.IndexOf(',')) < 0) return fullName;
            // Chop it off.
            var simpleName = fullName.Substring(0, found);
            simpleName = simpleName.Trim();
            return simpleName;
        }
        /// <summary>
        /// Gets the name, culture, version, strong name and public key of the assembly. The assembly
        /// name may be partial, then MS rules are assumed.
        /// </summary>
        /// <param name="possiblyFullAssemblyName">
        /// String name of the assembly, with components separated by commas. Can be just simple
        /// name, with no commmas involved or a partial name or a full name with all five components
        /// present, in order.
        /// .</param>
        /// <returns>
        /// Dictionary with possible keys Name, Culture, Version, StrongName and PublicKeyToken.
        /// As per MS, StrongName may be replaced with PublicKey if the full public key is known. If the
        /// input <paramref name="possiblyFullAssemblyName"/>is null or blank, null is returned.
        /// </returns>
        /// <remarks>
        /// The assumed form of the assembly name is:
        /// Name %lt;,Culture = CultureInfo%gt; %lt;,Version = Major.Minor.Build.Revision%gt; %lt;, StrongName%gt; %lt;,PublicKeyToken%gt; '\0'
        /// where only the <c>Name</c> is required. See microsoft docs for partial assembly names.
        /// We must output a dictionary, since partial name specs are allowed.
        /// https://msdn.microsoft.com/en-us/library/system.reflection.assemblyname(v=vs.95).aspx
        /// </remarks>
        public static IDictionary<string, string> GetAssemblyFullNameComponents(this string possiblyFullAssemblyName)
        {
            if (string.IsNullOrEmpty(possiblyFullAssemblyName)) return null;

            var dictionary = new Dictionary<string, string>();

            var components = possiblyFullAssemblyName.Split(',');

            // Just the simple name is here.
            if ((components.Length < 2))
            {
                dictionary["Name"] = possiblyFullAssemblyName.Trim();
                return dictionary;
            }

	        // Name is always on the front with no label.
            var name = components[0];
            dictionary["Name"] = components[0].Trim();

            var lastNonPositionalElementIndex = -1;

            // Pull out culture and version if we are in the partial style.
            for (var strNum = 0; strNum < components.Length - 1; strNum++)
            {
                if (components[strNum].Contains("Culture"))
                {
                    var culture = components[strNum].Substring(components[strNum].LastIndexOf("=", StringComparison.Ordinal) + 1);
                    culture = culture.Trim();
                    lastNonPositionalElementIndex = strNum;
                    dictionary["Culture"] = culture;
                }
                if (components[strNum].Contains("Version"))
                {
                    var version = components[strNum].Substring(components[strNum].LastIndexOf("=", StringComparison.Ordinal) + 1);
                    version = version.Trim();
                    lastNonPositionalElementIndex = strNum;
                    dictionary["Version"] = version;
                }
            }

            // Partial style?
            if (lastNonPositionalElementIndex > -1)
            {
                // We are partial style. See if we have other stuff on the end.
                if (components.Length > lastNonPositionalElementIndex + 1)
                {
	                var strongName = components[lastNonPositionalElementIndex + 1].Trim();
	                dictionary["StrongName"] = strongName;
                }
                if (components.Length > lastNonPositionalElementIndex + 2)
                {
	                var publicKeyToken = components[lastNonPositionalElementIndex + 2].Trim();
	                dictionary["PublicKeyToken"] = publicKeyToken;
                }

                // If we are here, everything has to be loaded by now.
                return dictionary;
            }

            // If we are here, just name is loaded, but life is easy.....
            dictionary["Culture"] = components[1];
            if (components.Length > 2)
                dictionary["Version"] = components[2];
            if (components.Length > 3)
                dictionary["StongName"] = components[3];
            if (components.Length > 4)
                dictionary["PublicKeyToken"] = components[4];
            return dictionary;
        }
        /// <summary>
        /// Determines if an assembly, given its "simple" name is in a dictionary of assemblies
        /// keyed by their full name. This ignores the version number, etc. of an assembly we
        /// are trying to find, so there may be multiples - we stop on the first. 
        /// </summary>
        /// <param name="simpleAssemblyName">
        /// String name of the assembly.
        /// .</param>
        /// <returns>
        /// An assembly, if found. If the input <paramref name="simpleAssemblyName"/>is null or blank,
        /// null is returned. if <param name="assemblyDictionary"/> is null, <see langword="null"/>
        /// is returned. 
        /// </returns>
        public static Assembly GetAssemblyFromFullNameKeyedDictionary(this string simpleAssemblyName,
            IDictionary<string, Assembly> assemblyDictionary)
        {
            if (string.IsNullOrEmpty(simpleAssemblyName)) return null;
            if (assemblyDictionary == null) return null;

            foreach(var name in assemblyDictionary.Keys)
            {
                var assembly = assemblyDictionary[name];
                if (string.CompareOrdinal(assembly.GetAssemblySimpleName(), simpleAssemblyName) == 0)
                    return assembly;
            }
            return null;
        }
	}
}

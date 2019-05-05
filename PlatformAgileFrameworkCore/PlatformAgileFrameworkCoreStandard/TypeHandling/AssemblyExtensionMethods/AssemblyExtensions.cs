//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

namespace PlatformAgileFramework.TypeHandling.AssemblyExtensionMethods
{
	/// <summary>
	/// Extension methods for the <see cref="Assembly"/> type.TypeHandling. This is the
	/// Core version. The other part of the partial class is the ECMA/CLR version. This
	/// part works for both.
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public static partial class AssemblyExtensions
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Examines an assembly to determine if there are any types inside
		/// with or without a certain stringful attribute.
		/// </summary>
		/// <param name="assembly">
		/// Assembly to check. <see langword="null"/> gets <see langword="null"/>.
		/// </param>
		/// <param name="attributeName">
		/// Stringful name of the attribute. <see langword="null"/> gets <see langword="null"/>
		/// </param>
		/// <param name="wantAttributePresent">
		/// <see langword="true"/> if we want the attribute to be present, <see langword="false"/> if not.
		/// </param>
		/// <returns>
		/// The set of types with the attribute present or not present. <see langword="null"/>
		/// if the assembly has no types. This method always returns resolved types
		/// (<see cref="PAFTypeHolderBase.TypeType"/> != <see langword="null"/>).
		/// </returns>
		public static IList<IPAFTypeHolder> GatherAttributedTypes(this Assembly assembly,
			string attributeName, bool wantAttributePresent)
		{
			if(assembly == null) return null;
			if (string.IsNullOrEmpty(attributeName)) return null;

			var assemblyTypes = assembly.PAFGetAccessibleTypes().BuildCollection();
			if (assemblyTypes.Count == 0) return null;

			IEnumerable<Type> filteredTypes;

			if (wantAttributePresent)
				filteredTypes
					= assemblyTypes.GetTypesWithPublicNamedAttributeInfo(attributeName);
			else
				filteredTypes = assemblyTypes.GetTypesWithoutPublicNamedAttributeInfo(attributeName);

			return filteredTypes.TypesToTypeHolders();
		}
	}
}
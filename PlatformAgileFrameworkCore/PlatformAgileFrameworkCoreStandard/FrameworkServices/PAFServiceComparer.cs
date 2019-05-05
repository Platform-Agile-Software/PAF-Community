//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2011 Icucom Corporation
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
#region Using directives

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;

#endregion

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFServiceDescription"/>s.
	/// We compare the interface type and the name.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAV </author>
	/// <date> 21jun2012 </date>
	/// <description>
	/// Documented this base comparer.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFServiceComparer : AbstractComparerShell<IPAFServiceDescription>
	{
		#region Class Autoproperties
		/// <summary>
		/// Determines whether a case-insensitive comparision is made on
		/// the name.
		/// </summary>
		public bool IgnoreCase { get; protected internal set; }
		#endregion // Class Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor builds with case-insensitive comparison.
		/// </summary>
		public PAFServiceComparer()
		{
			IgnoreCase = true;
		}
		/// <summary>
		/// Constructor allows case-sensitive comparison to be set.
		/// </summary>
		/// <param name="ignoreCase">
		/// <see langword="true"/> for case-insensitive comparison.
		/// </param>
		public PAFServiceComparer(bool ignoreCase)
		{
			IgnoreCase = ignoreCase;
		}
		#endregion // Constructors
		/// <summary>
		/// First compares the types of the <see cref="IPAFServiceDescription.ServiceInterfaceType"/>s.
		/// then, if those are the same, compares the <see cref="IPAFServiceDescription.ServiceName"/>s.
		/// Blank or <see langword="null"/> name is ordered first.
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IPAFServiceDescription{T}"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IPAFServiceDescription{T}"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// both <see cref="IPAFServiceDescription.ServiceName"/>s are 
		/// is <see langword="null"/> or blank.
		/// </exception>
		/// <exception> <see cref="ArgumentException"/> is thrown if
		/// either <see cref="IPAFServiceDescription.ServiceInterfaceType.TypeType"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public override int DefaultMainCompare(IPAFServiceDescription firstKey,
			IPAFServiceDescription secondKey)
		{

			if (firstKey.ServiceInterfaceType.TypeType == null)
				throw new ArgumentException("firstKey.ServiceInterfaceType.TypeType is null");
			if (secondKey.ServiceInterfaceType == null)
				throw new ArgumentException("secondKey.ServiceInterfaceType.TypeType is null");
			var strcmp = string.CompareOrdinal(firstKey.ServiceInterfaceType.AssemblyQualifiedTypeName,
			 secondKey.ServiceInterfaceType.AssemblyQualifiedTypeName);
			if(strcmp != 0) return strcmp;
			// Handle degenerate cases.
			// We put the exception here, since the one issued from the dictionary is cryptic.
			if ((string.IsNullOrEmpty(firstKey.ServiceName))
				&& (string.IsNullOrEmpty(secondKey.ServiceName)))
				throw new ArgumentException("Only one default service is allowed.");
			if (string.IsNullOrEmpty(firstKey.ServiceName)) return 1;
			if (string.IsNullOrEmpty(secondKey.ServiceName)) return -1;
			//
			if(IgnoreCase)
				return string.Compare(firstKey.ServiceName, secondKey.ServiceName, StringComparison.OrdinalIgnoreCase);
			return string.Compare(firstKey.ServiceName, secondKey.ServiceName, StringComparison.Ordinal);
		}
	}
}

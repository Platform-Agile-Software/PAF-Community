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

using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.FrameworkServices.Exceptions;

#region Using directives

using System;
using System.Collections.Generic;
using PlatformAgileFramework.Collections.Comparers;
#endregion

#region Exception Shorthand
using IPAFSSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServicesExceptionData;
using PAFSSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServicesExceptionData;
#endregion // Exception Shorthand

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFServiceDescription"/>s.
	/// We compare the interface name and the "default" indicator. This is designed
	/// to be used in dictionaries where interface type is the same.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20jun2013 </date>
	/// <description>
	/// Built for the two-level dictionary scheme.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFServiceInstanceComparer : AbstractComparerShell<IPAFNamedAndTypedObject>
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
		public PAFServiceInstanceComparer()
		{
			IgnoreCase = true;
		}
		/// <summary>
		/// Constructor allows case-sensitive comparison to be set.
		/// </summary>
		/// <param name="ignoreCase">
		/// <see langword="true"/> for case-insensitive comparison.
		/// </param>
		public PAFServiceInstanceComparer(bool ignoreCase)
		{
			IgnoreCase = ignoreCase;
		}
		#endregion // Constructors
		/// <summary>
		/// Compares the <see cref="IPAFNamedAndTypedObject.ObjectName"/>s.
		/// Blank or <see langword="null"/> name is ordered first. This is
		/// overridden if the default flag is set on the item.
		/// </summary>
		/// <param name="firstKey">
		/// The first <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <param name="secondKey">
		/// The other <see cref="IPAFNamedAndTypedObject"/> to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="PAFStandardException{IPAFServicesExceptionData}"/>
		/// <see cref="PAFServicesExceptionMessageTags.ONLY_ONE_UNNAMED_SERVICE_IMPLEMENTATION_IS_ALLOWED"/>
		/// is thrown if both <see cref="IPAFNamedAndTypedObject.ObjectName"/>s are 
		/// <see langword="null"/> or blank. Dictionary throws a cryptic
		/// exception, so a better one is thrown here.
		/// </exception>
		/// <exception> <see cref="PAFStandardException{IPAFServicesExceptionData}"/>
		/// <see cref="PAFServicesExceptionMessageTags.ONLY_ONE_DEFAULT_SERVICE_IMPLEMENTATION_IS_ALLOWED"/>
		/// is thrown if both <see cref="IPAFNamedAndTypedObject.IsDefaultObject"/>s are 
		/// <see langword="true"/>. Dictionary throws a cryptic
		/// exception, so a better one is thrown here.
		/// </exception>
		/// <exception> <see cref="ArgumentException"/> is thrown if either
		/// <paramref name="firstKey"/> or <paramref name="secondKey"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public override int DefaultMainCompare(IPAFNamedAndTypedObject firstKey,
			IPAFNamedAndTypedObject secondKey)
		{
			if (firstKey == null)
				throw new ArgumentNullException(nameof(firstKey));
			if (secondKey == null)
				throw new ArgumentNullException(nameof(secondKey));
			// Handle degenerate/default cases.
			if ((firstKey.IsDefaultObject) && (secondKey.IsDefaultObject))
			{
				// Lock the data with a null.
				var data = new PAFSSED(
					new List<IPAFServiceDescription>
					{firstKey.GetServiceDescriptionInterface(),
						secondKey.GetServiceDescriptionInterface(), null});
				throw new PAFStandardException<IPAFSSED>(
					data, PAFServicesExceptionMessageTags.ONLY_ONE_DEFAULT_SERVICE_IMPLEMENTATION_IS_ALLOWED);
			}
			if (firstKey.IsDefaultObject) return 1;
			if (secondKey.IsDefaultObject) return -1;

			// Not resolved by "default" flag.
			if ((string.IsNullOrEmpty(firstKey.ObjectName))
			    && (string.IsNullOrEmpty(secondKey.ObjectName)))
			{
				// Lock the data with a null.
				var data = new PAFSSED(
					new List<IPAFServiceDescription>
					{firstKey.GetServiceDescriptionInterface(),
						secondKey.GetServiceDescriptionInterface(), null});
				throw new PAFStandardException<IPAFSSED>(
                    data, PAFServicesExceptionMessageTags.ONLY_ONE_UNNAMED_SERVICE_IMPLEMENTATION_IS_ALLOWED);
			}
			if (string.IsNullOrEmpty(firstKey.ObjectName)) return 1;
			if (string.IsNullOrEmpty(secondKey.ObjectName)) return -1;
			//
			if(IgnoreCase)
				return string.Compare(firstKey.ObjectName, secondKey.ObjectName, StringComparison.OrdinalIgnoreCase);
			return string.Compare(firstKey.ObjectName, secondKey.ObjectName, StringComparison.Ordinal);
		}

		/// <summary>
		/// This override gets the hash code of the service name, because that is
		/// what we are sorting by.
		/// </summary>
		/// <param name="obj">
		/// An <see cref="IPAFServiceDescription"/>.
		/// </param>
		/// <returns>The hash code of the service name.</returns>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// "obj"
		/// </exception>
		/// <exception cref="ArgumentException">
		/// "Not a 'IPAFServiceDescription'"
		/// </exception>
		/// </exceptions>
		public override int GetHashCode(object obj)
		{
			if (obj == null) throw new ArgumentNullException(nameof(obj));
			var ntod = obj as IPAFNamedAndTypedObject;
			if (ntod == null) throw new ArgumentException("Not a 'IPAFNamedAndTypedObject'");
			return ntod.ObjectName.GetHashCode();
		}
	}
}

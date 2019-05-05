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
using IPAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.IPAFServiceExceptionData;
using PAFSED = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionData;
using PAFSEDB = PlatformAgileFramework.FrameworkServices.Exceptions.PAFServiceExceptionDataBase;
#endregion // Exception Shorthand

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class implements a comparison of <see cref="IPAFServiceDescription"/>s.
	/// We compare the stringful interface type. This is designed to be used in the
	/// outer dictionary of a two-level service dictionary. Dictionary only requires
	/// equality, but other operations require <see cref="IComparer{T}"/> with string
	/// type.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 18jan2019 </date>
	/// <description>
	/// Changed back to string comparison for doing late-bound stuff, which
	/// can be done now in .Net Standard. Needed for client.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20jun2013 </date>
	/// <description>
	/// Built for the two-level dictionary scheme.
	/// </description>
	/// </contribution>
	/// </history>
	public class PAFServiceTypeComparer : AbstractComparerShell<IPAFNamedAndTypedObject>
	{
		/// <summary>
		/// Compares the <see cref="IPAFNamedAndTypedObject.ObjectType"/>s.
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

			var firstInterfaceType = firstKey.AssemblyQualifiedObjectType;
			var secondInterfaceType = secondKey.AssemblyQualifiedObjectType;

			// Types must be resolved.
			if (firstInterfaceType == null)
			{
				var data = new PAFSED(firstKey.GetServiceDescriptionInterface());
				throw new PAFStandardException<IPAFSED>(
					data, PAFServiceExceptionMessageTags.TYPE_IS_NOT_SPECIFIED);
			}
			if (secondInterfaceType == null)
			{
				var data = new PAFSED(secondKey.GetServiceDescriptionInterface());
				throw new PAFStandardException<IPAFSED>(
                    data, PAFServiceExceptionMessageTags.TYPE_IS_NOT_SPECIFIED);
			}
			if (string.Compare(secondInterfaceType, firstInterfaceType, StringComparison.Ordinal) == 0)
				return 0;
			if (string.Compare(secondInterfaceType, firstInterfaceType, StringComparison.Ordinal) == -1)
				return -1;
			return 1;
		}

		public override int GetHashCode(IPAFNamedAndTypedObject item)
		{
			return item.AssemblyQualifiedObjectType.GetHashCode();
		}
	}
}

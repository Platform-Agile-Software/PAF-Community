//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2018 Icucom Corporation
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

using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// A aggregate of <see cref="IPAFIntArgumentHolder"/> and
	/// <see cref="IPAFExceptionArgumentHolder"/> - that's it. We
	/// use this one to sneak everything we need through an <see cref="object"/>
	/// argument.
	/// </summary>
	/// <remarks>
	/// </remarks>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21jan2018 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// As ReSharper says, there will be an ambiguity when accessing two
	/// closures of the same Generic type. Explicit interface implementation
	/// must be used.
	/// </remarks>
	// ReSharper disable once PossibleInterfaceMemberAmbiguity
	public interface IPAFTestResultNavigationCommandArgumentHolder
		:IPAFIntArgumentHolder, IPAFExceptionArgumentHolder
	{
	}
}
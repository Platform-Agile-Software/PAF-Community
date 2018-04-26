//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives



#endregion

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// Wanted a Generic handle.
	/// </summary>
	/// <typeparam name="T">
	/// Constrained in static constructors to be 8, 16, 32 or 64 bits.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 20dec2015 </date>
	/// <description>
	/// Built for handling by the Generic.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IExtendablePseudoEnumTypeType<out T>
		:IExtendablePseudoEnumTypeType
	{
		#region Properties
		/// <summary>
		/// Gets the value of enum.
		/// </summary>
		T EnumValueAsGeneric{ get; }
		#endregion // Properties
	}
}

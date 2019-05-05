﻿//@#$&+
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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives
using System;
#endregion

namespace PlatformAgileFramework.TypeHandling.PartialClassSupport
{
	/// <summary>
	/// Attached to a pseudoenum so it can be handled as a non-Generic
	/// in the master dictionary.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>
	/// <description>
	/// Built for the rewrite of the extendable enum stuff.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IExtendablePseudoEnumTypeType
	{
		#region Properties
		/// <summary>
		/// Gets the type of enum numeric epresentation.
		/// </summary>
		Type EnumIntegerType { get; }
		/// <summary>
		/// Gets the type of the top-level enumvalue container class.
		/// </summary>
		Type EnumType { get; }
		/// <summary>
		/// Gets the value of enum.
		/// </summary>
		object EnumValueAsObject { get; }
		/// <summary>
		/// Gets the name of the pseudoenum value.
		/// </summary>
		string Name { get; }
		#endregion // Properties
	}
}

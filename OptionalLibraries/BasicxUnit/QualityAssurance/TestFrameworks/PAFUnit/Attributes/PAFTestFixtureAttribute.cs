//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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

using System;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.PAFUnit.Attributes
{
	/// <summary>
	/// Attribute placed on a class in PAFUnit to indicate that it is a test fixture.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public class PAFTestFixtureAttribute : Attribute
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// This provides the description of the fixture. May be <see langword="null"/>.
		/// </summary>
		public string Description { get; protected internal set; }
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor provides no description.
		/// </summary>
		public PAFTestFixtureAttribute()
		{
		}
		/// <summary>
		/// Builds with a description.
		/// </summary>
		/// <param name="description">
		/// Textual description for the test fixture. May be <see langword="null"/>.
		/// </param>
		public PAFTestFixtureAttribute(string description)
		{
			Description = description;
		}
		#endregion // Constructors
	}
}

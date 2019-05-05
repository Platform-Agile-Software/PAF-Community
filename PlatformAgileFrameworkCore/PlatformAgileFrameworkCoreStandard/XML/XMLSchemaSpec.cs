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

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.XML
{	/// <summary>
	/// A little class to hold filenames and ns's for schemas.
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 05nov2012 </date>
	/// <contribution>
	/// <para>
	/// Changed to inherit from interface.
	/// </para>
	/// </contribution>
	/// </history>
	public class XMLSchemaSpec : IXMLSchemaSpec
	{
		#region Class Fields
		/// <summary>
		/// Support for <see cref="IXMLSchemaSpec"/>.
		/// </summary>
		private readonly string m_fileName;
		/// <summary>
		/// Support for <see cref="IXMLSchemaSpec"/>.
		/// </summary>
		private readonly string m_nameSpace;
		#endregion //Class Fields
		#region Constructors
		/// <summary>
		/// Constructor just builds from the two input fields.
		/// </summary>
		/// <param name="fileName">
		/// Sets <see cref="FileName"/>.
		/// </param>
		/// <param name="nameSpace">
		/// Sets <see cref="NameSpace"/>.
		/// </param>
		public XMLSchemaSpec (string fileName, string nameSpace)
		{ m_fileName = fileName; m_nameSpace = nameSpace; }
		#endregion //Constructors
		#region Properties
		/// <summary>
		/// <see cref="IXMLSchemaSpec"/>.
		/// </summary>
		public string FileName { get { return m_fileName; } }
		/// <summary>
		/// <see cref="IXMLSchemaSpec"/>.
		/// </summary>
		public string NameSpace { get { return m_nameSpace; } }
		#endregion //Properties
	}
}

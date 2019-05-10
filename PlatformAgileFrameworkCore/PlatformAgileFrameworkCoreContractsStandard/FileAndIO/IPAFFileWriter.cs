//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
#endregion // Using Directives

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// File writing service, designed to be thread-safe.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 23feb2019 </date>
	/// <description>
	/// New. Needed just a thread-safe file writer for Golea.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Implementations are EXPECTED to be thread-safe.
	/// </threadsafety>
	public interface IPAFFileWriter
	{
		#region Methods
		/// <summary>
		/// Writes string data to the "current" output file.
		/// </summary>
		/// <param name="dataEntry">
		/// Data to be written.
		/// </param>
		void WriteDataEntry(string dataEntry);
		#endregion Methods
	}
}
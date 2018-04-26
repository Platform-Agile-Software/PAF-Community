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

namespace PlatformAgileFramework.QualityAssurance
{
	/// <summary>
	/// Basic parameters that are usually needed in our unit tests.
	/// </summary>
	public class PAFTestParams
	{
		#region Class Autoproperties
		/// <summary>
		/// Allows the specification of the configuration file path.
		/// Default is an empty string.
		/// </summary>
		public string ConfigFilePath{get; set;}
		#endregion // Class Autoproperties
		#region Constructors
		/// <summary>
		/// Default constructor. 
		/// </summary>
		public PAFTestParams()
		{
			ConfigFilePath = string.Empty;
		}
		/// <summary>
		/// Deep copy constructor. 
		/// </summary>
		public PAFTestParams(PAFTestParams pAFParams)
			:this()
		{
			ConfigFilePath = pAFParams.ConfigFilePath;
		}
		#endregion // Constructors
	}
	/// <summary>
	/// Interface to allow unit tests classes to be somewhat configurable from
	/// a test runner that is run from a debugger. 
	/// </summary>
	public interface IPAFTestClassBase<T> where T: PAFTestParams
	{
		/// <summary>
		/// Allows setting parameters for a test. Parameters are usually set before
		/// test setup methods are called. They can be used to return test results
		/// as well.
		/// </summary>
		T Params {get; set;}
	}
}


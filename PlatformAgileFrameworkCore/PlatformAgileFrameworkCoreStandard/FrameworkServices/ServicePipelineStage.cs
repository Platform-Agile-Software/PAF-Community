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

#region Using Directives
using System;
using PlatformAgileFramework.TypeHandling.PartialClassSupport;
#endregion // Using Directives

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class describes what stage of the service pipeline we are in.
	/// </summary>
	/// <remarks>
	/// Gaps left in the enums for framework extenders/builders.
	/// </remarks>
	// ReSharper disable PartialTypeWithSinglePart
	public sealed partial class ServicePipelineStage: ExtendablePseudoEnumInt32
	// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Construction of the service object.
		/// </summary>
		public static readonly ServicePipelineStage CONSTRUCTION
		= new ServicePipelineStage("CONSTRUCTION", 0, true);
		/// <summary>
		/// Loading stage and initialization of emergency services.
		/// </summary>
		public static readonly ServicePipelineStage LOAD
			= new ServicePipelineStage("LOAD", 4, true);
		/// <summary>
		/// Full service initialization.
		/// </summary>
		public static readonly ServicePipelineStage INITIALIZE
			= new ServicePipelineStage("INITIALIZE", 8, true);
		/// <summary>
		/// Main operating mode - providing services.
		/// </summary>
		public static readonly ServicePipelineStage SERVICING
			= new ServicePipelineStage("SERVICING", 32, true);
		/// <summary>
		/// Update notifications for services.
		/// </summary>
		public static readonly ServicePipelineStage UPDATE
			= new ServicePipelineStage("UPDATE", 128, true);
		/// <summary>
		/// Full service uninitialization.
		/// </summary>
		public static readonly ServicePipelineStage UNINITIALIZE
			= new ServicePipelineStage("UNINITIALIZE", 512, true);
		/// <summary>
		/// Unloading stage and uninitialization of emergency services.
		/// </summary>
		public static readonly ServicePipelineStage UNLOAD
			= new ServicePipelineStage("UNLOAD", 2048, true);
		#endregion // Class Fields And Autoproperties
		/// <remarks>
		/// See base.
		/// </remarks>
		public ServicePipelineStage(string name, int value)
			: base(name, value)
		{
		}
		/// <remarks>
		/// See base.
		/// </remarks>
		internal ServicePipelineStage(string name, int value, bool addToDictonary)
			: base(name, value, addToDictonary)
		{
		}
	}
}

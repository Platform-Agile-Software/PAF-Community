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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	///	Class providing services to services during the <see cref="IPAFServiceExtended"/>
	/// pipeline stages.
	/// </para>
	/// </summary>
	public class PAFServicePipelineObject<T>: PAFServicePipelineObject , IPAFServicePipelineObject<T>
		where T : class, IPAFService
	{
		#region Constructors
		/// <summary>
		/// For inheritance support.
		/// </summary>
		protected internal PAFServicePipelineObject()
		{
		}
		/// <summary>
		/// Builds with a service manager.
		/// </summary>
		/// <param name="serviceManager">
		/// The manager.
		/// </param>
		/// <param name="pipelineStage">
		/// Sets the <see cref="PipelineStage"/>.
		/// </param>
		public PAFServicePipelineObject(IPAFServiceManager<T> serviceManager,
			ServicePipelineStage pipelineStage)
		{
			m_ServiceManager = serviceManager;
			PipelineStage = pipelineStage;
		}
		#endregion // Constructors
		#region Properties
		public IPAFServiceManager<T> GenericServiceManager
		{
			get { return (IPAFServiceManager<T>) ServiceManager; }
		}

		#endregion // Properties
	}
}
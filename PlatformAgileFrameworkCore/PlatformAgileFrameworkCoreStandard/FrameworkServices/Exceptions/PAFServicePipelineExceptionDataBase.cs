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
using System.Collections.Generic;

using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	/// Base class for service pipeline exceptions.
	/// </summary>
// ReSharper disable PartialTypeWithSinglePart
	public abstract partial class PAFServicePipelineExceptionDataBase :
// ReSharper restore PartialTypeWithSinglePart
		PAFServiceExceptionDataBase, IPAFServicePipelineExceptionData
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal List<IPAFService> m_ProblematicServices;
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		internal ServicePipelineStage m_PipelineStage;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Constructor loads props.
		/// </summary>
		/// <param name="problematicServices">
		/// Loads <see cref="IPAFServicePipelineExceptionData.ProblematicServices"/>.
		/// </param>
		/// <param name="pipelineStage">
		/// Loads <see cref="IPAFServicePipelineExceptionData.PipelineStage"/>.
		/// May be <see langword="null"/>.
		/// </param>
		/// <param name="problematicServiceManagerType">
		/// Can be <see langword="null"/>. This container holds the full
		/// type description of the service manager.
		/// </param>
		/// <param name="problematicServiceBaseType">
		/// Can be <see langword="null"/>. This is the base type that the
		/// service manager serves. For the root manager, it will be
		/// <see cref="IPAFService"/>.
		/// </param>
		/// <param name="extensionData">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		/// <param name="isFatal">
		/// See <see cref="PAFAbstractStandardExceptionDataBase"/>
		/// </param>
		protected PAFServicePipelineExceptionDataBase(
			IEnumerable<IPAFService> problematicServices,
			ServicePipelineStage pipelineStage,
			IPAFTypeHolder problematicServiceManagerType,
			IPAFTypeHolder problematicServiceBaseType,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			: base(new PAFServiceDescription(problematicServiceBaseType, problematicServiceManagerType),
			extensionData, pafLoggingLevel, isFatal)
		{
			ProblematicServices = problematicServices;
			PipelineStage = pipelineStage;
		}
		#endregion Constructors
		#region Properties
		/// <summary>
		/// See <see cref="IPAFServicePipelineExceptionData"/>.
		/// </summary>
		public IEnumerable<IPAFService> ProblematicServices
		{
			get { return m_ProblematicServices; }
			protected internal set
			{
				if(value == null)
				{
					m_ProblematicServices = new List<IPAFService>();
					return;
				}
				m_ProblematicServices = new List<IPAFService>(value);
			}
		}
		/// <summary>
		/// See <see cref="IPAFServicePipelineExceptionData"/>.
		/// </summary>
		public ServicePipelineStage PipelineStage
		{
			get { return m_PipelineStage; }
			protected internal set { m_PipelineStage = value; }
		}
		#endregion // Properties
		// TODO - KRM override renderer.
	}
}

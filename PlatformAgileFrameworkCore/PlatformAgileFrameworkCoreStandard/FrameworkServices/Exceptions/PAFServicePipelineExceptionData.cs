using System;
using System.Collections.Generic;
using PlatformAgileFramework.Logging;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.FrameworkServices.Exceptions
{
	/// <summary>
	///	Sealed version of <see cref="PAFServicePipelineExceptionDataBase"/>.
	/// </summary>
	[PAFSerializable]
	public sealed class PAFServicePipelineExceptionData : PAFServicePipelineExceptionDataBase
	{
		/// <summary>
		/// See base.
		/// </summary>
		/// <param name="problematicServices">
		/// See base.
		/// </param>
		/// <param name="pipelineStage">
		/// See base.
		/// </param>
		/// <param name="problematicImplementationType">
		/// See base.
		/// </param>
		/// <param name="problematicServiceInterface">
		/// See <see cref="IPAFServiceExceptionData.ProblematicService"/>.
		/// </param>
		/// <param name="extensionData">
		/// See base.
		/// </param>
		/// <param name="pafLoggingLevel">
		/// See base.
		/// </param>
		/// <param name="isFatal">
		/// See base.
		/// </param>
		public PAFServicePipelineExceptionData(
			IEnumerable<IPAFService> problematicServices,
			ServicePipelineStage pipelineStage,
			IPAFTypeHolder problematicImplementationType,
			IPAFTypeHolder problematicServiceInterface,
			object extensionData = null,
			PAFLoggingLevel? pafLoggingLevel = null, bool? isFatal = null)
			:base(problematicServices, pipelineStage, problematicImplementationType,
			problematicServiceInterface, extensionData, pafLoggingLevel, isFatal)
		{
		}
	}
}
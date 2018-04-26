using PlatformAgileFramework.TypeHandling;

namespace PlatformAgileFramework.Execution.Pipeline
{
	/// <summary>
	/// This interface aggregates two others to provide a pipeline plus parameters.
	/// </summary>
	/// <typeparam name="T">The actual application parameters.</typeparam>
	/// <history>
	/// <author> BMC </author>
	/// <date> 25aug2011 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
	public interface IPAFBaseExeParameterizedPipelineInitialize<T>
		: IPAFBaseExePipelineInitialize<T>, IPAFProviderPattern<IPAFPipelineParams<T>>
		where T : class
	{
	}
}

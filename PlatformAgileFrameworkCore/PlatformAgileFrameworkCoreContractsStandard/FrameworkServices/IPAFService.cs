
using PlatformAgileFramework.Collections;
namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This interface must be implemented by all services wishing to provide
	/// a PAF Framework service.
	/// </summary>
	/// <remarks>
	/// This is the interface exposed to the outside world to enable request
	/// of services. It is normally placed in a separate interface assembly.
	/// </remarks>
// ReSharper disable PartialTypeWithSinglePart
// Core
	public partial interface IPAFService : IPAFNamedAndTypedObject
	// ReSharper restore PartialTypeWithSinglePart
	{
	}
}

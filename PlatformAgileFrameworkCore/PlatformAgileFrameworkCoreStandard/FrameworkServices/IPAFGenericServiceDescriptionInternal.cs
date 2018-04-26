using System.Security;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Provides a way to set the Generic service object by trusted callers.
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16apr2016 </date>
	/// <description>
	/// New - the internal inerface was missing.
	/// </description>
	/// </contribution>
	/// </history>
	/// <typesafety>
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </typesafety>
	// ReSharper disable PartialTypeWithSinglePart
	//Core
	internal partial interface IPAFServiceDescriptionInternal<T>
		: IPAFServiceDescription<T> where T: class, IPAFService
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Setter for the service - only for full-trust environments.
		/// </summary>
		void SetServiceInternal(T serviceObject);
	}
}

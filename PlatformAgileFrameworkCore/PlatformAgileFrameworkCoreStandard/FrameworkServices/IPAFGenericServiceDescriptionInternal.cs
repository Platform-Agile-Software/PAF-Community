namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// Provides a way to set the Generic service properties by
	/// internal callers.
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </summary>
	/// <typeparam name="T">
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </typeparam>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 25jan2018 </date>
	/// <description>
	/// Moved the default setter to internal, because it was being
	/// misused.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16apr2016 </date>
	/// <description>
	/// New - the internal interface was missing.
	/// </description>
	/// </contribution>
	/// </history>
	/// <typesafety>
	/// See <see cref="IPAFServiceDescription{T}"/>.
	/// </typesafety>
	// ReSharper disable PartialTypeWithSinglePart
	//Core
	internal partial interface IPAFServiceDescriptionInternal<T>
		: IPAFServiceDescription<T>, IPAFServiceDescriptionInternal where T: class, IPAFService
// ReSharper restore PartialTypeWithSinglePart
	{
		/// <summary>
		/// Setter for the service - only for full-trust environments.
		/// </summary>
		void SetServiceInternal(T serviceObject);
	}
}

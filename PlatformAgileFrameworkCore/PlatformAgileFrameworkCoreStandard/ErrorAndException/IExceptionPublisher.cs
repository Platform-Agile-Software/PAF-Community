using System;
namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	///	There are many, many times when exceptions should not be thrown, but
	/// must be transmitted to other parts of a system to make them aware of
	/// certain events.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03aug2019 </date>
	/// <description>
	/// New. Moved over from Golea and simplified for async/await support.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IExceptionPublisher
	{
		/// <summary>
		/// Just sends the exception out.
		/// </summary>
		event Action<Exception> TransmitException;
	}
}
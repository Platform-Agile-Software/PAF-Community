namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// <para>
	///	The base Generic exception interface from which all of our custom exceptions derive. 
	/// The exception is designed to be used in both low and elevated trust environments with
	/// serialization performed by the PAFSerializer.
	/// </para>
	/// <para>
	/// As Richter points out (pg. 481 of "CLR via C#, third edition), defining custom exceptions
	/// is tedious because of use of "ISerializable" on base ECMA exceptions. Custom exceptions
	/// normally incorporate some additional simple types like a filename or type information
	/// or something similar. It's quite a bit simpler to simply carry the additional information
	/// in a Generic payload. We use the payload for quite a bit more, but it is indeed quite
	/// helpful to create a Generic exception. A more important purpose for us is that
	/// the use of a Generic data paylod to specialize an exception allows exception types
	/// to be aggregated.
	/// </para>
	/// </summary>
	/// <typeparam name="T">
	/// This is an arbitrary type except for the fact that it must be serializable. The static
	/// constructor for the closed Generic type should make this check when any instance of the
	/// closed Generic type is first constructed.
	/// </typeparam>
	public interface IPAFExceptionBase<out T>: IPAFExceptionBase
	{
		/// <summary>
		/// Just grabs the Generic payload.
		/// </summary>
		/// <returns>
		/// The generic.
		/// </returns>
		T GetExceptionDataItem();

	}
}
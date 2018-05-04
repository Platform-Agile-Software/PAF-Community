namespace PlatformAgileFramework.ErrorAndException
{
	/// <summary>
	/// Interface applies a constraint on the Generic to carry our base payload.
	/// </summary>
	/// <typeparam name="T">
	/// Generic which is constrained in this class.
	/// </typeparam>
	public interface IPAFStandardException<T>:
		IPAFExceptionBase<T> where T : IPAFStandardExceptionData
	{
	}
}
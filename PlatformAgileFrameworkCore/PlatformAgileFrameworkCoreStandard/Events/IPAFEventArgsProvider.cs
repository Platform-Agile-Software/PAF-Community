namespace PlatformAgileFramework.Events
{
	/// <summary>
	/// Protocol for a container for an event's generic arguments.
	/// Renamed thusly to confusion with something in .Net.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args suport.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Implementations should be immutable.
	/// </remarks>
	public interface IPAFEventArgsProvider<out T>
	{
		/// <summary>
		/// Just gets the payload.
		/// </summary>
		T Value { get; }
	}
}
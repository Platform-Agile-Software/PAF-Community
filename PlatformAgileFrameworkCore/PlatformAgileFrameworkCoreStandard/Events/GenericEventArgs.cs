
using System;

namespace PlatformAgileFramework.Events
{
	/// <summary>
	/// Standard container for an event's generic arguments.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Built new event args support.
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// This is an immutable implementation.
	/// </remarks>
	public class PAFEventArgs<T> : EventArgs, IPAFEventArgsProvider<T>
	{
		/// <summary>
		/// <see cref="IPAFEventArgsProvider{T}"/>
		/// </summary>
		public T Value { get; set; }

		/// <summary>
		/// <see cref="IPAFEventArgsProvider{T}"/>
		/// </summary>
		public PAFEventArgs(T val)
		{
			Value = val;
		}
	}
}
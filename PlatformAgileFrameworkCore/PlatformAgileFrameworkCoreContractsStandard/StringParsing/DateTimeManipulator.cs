using System;

namespace PlatformAgileFramework.StringParsing
{
	/// <summary>
	/// Holds a pair of functions that imbed and extract date/time into/from strings.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>07nov2018 </date>
	/// <description>
	/// New. The original impetus for this code was to abstract the procedure for
	/// embedding dates in file names and extracting them. It has broad utility
	/// and was moved to the utilities folder.
	/// </description>
	/// </contribution>
	/// </history>
	public class DateTimeManipulator
	{
		/// <summary>
		/// Constructor just loads the properties.
		/// </summary>
		/// <param name="dateStamper"><see cref="DateStamper"/></param>
		/// <param name="dateParser"><see cref="DateParser"/></param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// Thrown for either parameter.
		/// </exception>
		/// </exceptions>
		public DateTimeManipulator(Func<string, DateTime, string> dateStamper,
			Func<string, DateTime?> dateParser)
		{
			DateStamper = dateStamper ?? throw new ArgumentNullException(nameof(dateStamper));
			DateParser = DateParser ?? throw new ArgumentNullException(nameof(DateParser));
		}
		/// <summary>
		/// Applies a date somehow to an incoming string and outputs the string again.
		/// </summary>
		public Func<string, DateTime, string> DateStamper { get; }
		/// <summary>
		/// Reads a date somehow from an incoming string. Provides an opportunity
		/// to return a <see langword="null"/> for no date or for a parsing error.
		/// Something needs to be put in front of this to detect badly-formed file
		/// names if this is a risk. This is outside the scope of this function.
		/// </summary>
		public Func<string, DateTime?> DateParser { get; }
	}

}



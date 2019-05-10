using System;

namespace PlatformAgileFramework.FileAndIO
{
	/// <summary>
	/// Holds a pair of functions that imbed and extract version information strings,
	/// from strings, which are often file names.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date>07nov2018</date>
	/// <description>
	/// New. The original impetus for this code was to abstract the procedure for
	/// embedding versions in file names and extracting them.
	/// </description>
	/// </contribution>
	/// </history>
	public class VersionManipulator
	{
		/// <summary>
		/// Constructor just loads the properties.
		/// </summary>
		/// <param name="versionStamper"><see cref="VersionStamper"/></param>
		/// <param name="versionParser"><see cref="VersionParser"/></param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">
		/// Thrown for either parameter.
		/// </exception>
		/// </exceptions>
		public VersionManipulator(Func<string, int, string> versionStamper,
			Func<string, int?> versionParser)
		{
			VersionStamper = versionStamper ?? throw new ArgumentNullException(nameof(versionStamper));
			VersionParser = versionParser ?? throw new ArgumentNullException(nameof(versionParser));
		}
		/// <summary>
		/// Applies a version number somehow to an incoming string and outputs the string again.
		/// </summary>
		public Func<string, int, string> VersionStamper { get; }
		/// <summary>
		/// Reads a version number somehow from an incoming string. Provides an opportunity
		/// to return a <see langword="null"/> for no version number or for a parsing error.
		/// Something needs to be put in front of this to detect badly-formed file
		/// names if this is a risk. This is outside the scope of this function.
		/// </summary>
		public Func<string, int?> VersionParser { get; }
	}

}



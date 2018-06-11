using System;

namespace PlatformAgileFramework.Manufacturing
{
	/// <summary>
	///	Useful extensions for objects.
	/// </summary>
	public static class ObjectExtensionMethods
	{
		#region Methods
		/// <summary>
		/// Just helps flag bad arg. Throws an exception if arg is <see langword="null"/>. Useful
		/// in constructors.
		/// </summary>
		/// <param name="argName">
		/// Name that is to be displayed in the <see cref="ArgumentNullException"/>.
		/// </param>
		/// <param name="obj">
		/// Incoming <see cref="Object"/>.
		/// </param>
		/// <returns>
		/// The <paramref name="obj"/>, if not <see langword="null"/>.
		/// </returns>
		public static object ExceptNullObject (this object obj, string argName)
		{
			if (obj == null) throw new ArgumentNullException(argName);
			return obj;
		}
		#endregion Methods
	}
}
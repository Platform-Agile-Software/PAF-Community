using System;
using Xamarin.Forms;

namespace PlatformAgileFramework.XamarinFormsHelpers
{
	/// <summary>
	/// The interface that is worn by a view or a containing view to
	/// receive information about click coordinates. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb18 </date>
	/// <description>
	/// Had to do this, since Xamarin STILL does not have a way to get
	/// gesture coordinates in Forms.
	/// </description>
	/// </contribution>
	/// </history>
	public interface ITouchCoordinatesCallback
	{
		/// <summary>
		/// Called when a click is made.
		/// </summary>
		/// <param name="clickPoint">
		/// Coordinates of the click.
		/// </param>
		void OnClickAction(Point clickPoint);
		/// <summary>
		/// Called when a double click is made.
		/// </summary>
		/// <param name="doubleClickPoint">
		/// Coordinates of the double click.
		/// </param>
		void OnDoubleClickAction(Point doubleClickPoint);
		/// <summary>
		/// Called when a selection is made.
		/// </summary>
		/// <param name="selectionPoint">
		/// Coordinates of the selection.
		/// </param>
		void OnSelectAction(Point selectionPoint);
	}
}

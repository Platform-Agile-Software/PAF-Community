using System;
using Xamarin.Forms;

namespace PlatformAgileFramework.XamarinFormsHelpers
{
	/// <summary>
	/// Just tells the direction of the swipe on the view
	/// </summary>
	public enum SwipeDirection
	{
		LeftSwipe = 0,
		RightSwipe = 1,
		UpSwipe = 2,
		DownSwipe = 3,
		Click = -1
	}
	/// <summary>
	/// The interface that is worn by a view or a containing view to
	/// receive information about swipes. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb18 </date>
	/// <description>
	/// Had to do this, since Xamarin STILL does not have a swipe recognizer in Forms.
	/// </description>
	/// </contribution>
	/// </history>
	public interface ISwipeCallBack
	{
		/// <summary>
		/// Called when a left swipe is detected.
		/// </summary>
		/// <param name="view">
		/// The <see cref="View"/> that generated the swipe. This may
		/// have to be located if in a <see cref="ListView"/> or other
		/// container.
		/// </param>
		/// <param name="direction">
		/// The direction the swipe.
		/// </param>
		void OnSwipe(View view, SwipeDirection direction);
	}
}

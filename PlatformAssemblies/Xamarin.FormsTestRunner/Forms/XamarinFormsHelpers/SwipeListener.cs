using System;
using Xamarin.Forms;

namespace PlatformAgileFramework.XamarinFormsHelpers
{
	/// <summary>
	/// Swipe capability - does left/right/up/down
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb18 </date>
	/// <description>
	/// Built because Xamarin.Forms did not have it.
	/// </description>
	/// </contribution>
	/// </history>
	public class SwipeListener : PanGestureRecognizer
	{
		/// <summary>
		/// The component we call back to to deliver the event.
		/// </summary>
		private readonly ISwipeCallBack m_ISwipeCallback;
		/// <summary>
		/// Use this and Y to figure out left/right/top/bottom.
		/// </summary>
		private double m_TranslatedX;
		/// <summary>
		/// Use this and X to figure out left/right/top/bottom.
		/// </summary>
		private double m_TranslatedY;

		/// <summary>
		/// This constructor adds the swipe listener to the view that is to be
		/// "swipeable". It is important to note here that this may not be
		/// the child within the containing view that the swipe was generated
		/// on. Thus the "view" in the callback is needed.
		/// </summary>
		/// <param name="view">
		/// The view that is "enabled" to generate gestures.
		/// </param>
		/// <param name="iSwipeCallBack">
		/// The receiver of the information, including the view that triggered
		/// the swipe as well as the type of swipe.
		/// </param>
		public SwipeListener(View view, ISwipeCallBack iSwipeCallBack)
		{
			m_ISwipeCallback = iSwipeCallBack;
			var panGesture = new PanGestureRecognizer();
			panGesture.PanUpdated += OnPanUpdated;
			view.GestureRecognizers.Add(panGesture);
		}

		/// <summary>
		/// The receiver for the pan event, which is to be interpreted as a
		/// swipe event. Calls the callback.
		/// </summary>
		/// <param name="sender">
		/// Always a <see cref="View"/>.
		/// </param>
		/// <param name="e">
		/// Coordinates of the pan.
		/// </param>
		void OnPanUpdated(object sender, PanUpdatedEventArgs e)
		{
			var swipedView = (View)sender;

			switch (e.StatusType)
			{

				case GestureStatus.Running:

					m_TranslatedX = e.TotalX;
					m_TranslatedY = e.TotalY;
					break;

				case GestureStatus.Completed:

					if (m_TranslatedX < 0 && Math.Abs(m_TranslatedX) > Math.Abs(m_TranslatedY))
					{
						m_ISwipeCallback.OnSwipe(swipedView, SwipeDirection.LeftSwipe);
					}
					else if (m_TranslatedX > 0 && m_TranslatedX > Math.Abs(m_TranslatedY))
					{
						m_ISwipeCallback.OnSwipe(swipedView, SwipeDirection.RightSwipe );
					}
					else if (m_TranslatedY < 0 && Math.Abs(m_TranslatedY) > Math.Abs(m_TranslatedX))
					{
						m_ISwipeCallback.OnSwipe(swipedView, SwipeDirection.UpSwipe);
					}
					else if (m_TranslatedY > 0 && m_TranslatedY > Math.Abs(m_TranslatedX))
					{
						m_ISwipeCallback.OnSwipe(swipedView, SwipeDirection.DownSwipe);
					}

					break;
			}
		}
	}
}

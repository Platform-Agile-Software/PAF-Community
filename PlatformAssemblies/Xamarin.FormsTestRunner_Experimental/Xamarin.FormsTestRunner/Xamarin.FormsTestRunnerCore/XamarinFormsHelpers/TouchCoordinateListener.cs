using Xamarin.Forms;

namespace PlatformAgileFramework.XamarinFormsHelpers
{
	/// <summary>
	/// Had to build this, since Xamarin, even at this late date
	/// doesn't have a way to get touch coordinates without writing
	/// custom renderers. This is only a partial solution, but is
	/// less "fragile" than custom renderers. We paint an absolute
	/// layout with a grid of buttons and examine the coordinates
	/// of the clicked button. Problem is that one needs a whole
	/// lot of buttons for an accurate hit resolution. We mostly
	/// don't. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 28feb18 </date>
	/// <description>
	/// Had to do this, since Xamarin STILL does not have a coordinate return in Forms.
	/// </description>
	/// </contribution>
	/// </history>
	public class TouchCoordinateListener : AbsoluteLayout
	{
		private readonly ITouchCoordinatesCallback m_CoordinatesCallback;

		public TouchCoordinateListener(int numCellsX, int numCellsY,
			ITouchCoordinatesCallback coordinatesCallback, Rectangle gridBounds)
			: base()
		{
			Layout(gridBounds);
			m_CoordinatesCallback = coordinatesCallback;
			int x = 0;
			int y = 0;

			double button_width = 1.0 / numCellsX;
			double button_height = 1.0 / numCellsY;

			for (y = 0; y < numCellsY; y++)
			{
				for (x = 0; x < numCellsX; x++)
				{
					Button touchButton = new Button();

					//// avoid x y delegate get last
					//int x2 = x;
					//int y2 = y;

					//touchButton.Clicked += (sender, args) =>
					//{
					//	Click((((double)x2) + 0.5) / ((double)touch_grid_resolution),
					//		(((double)y2) + 0.5) / ((double)touch_grid_resolution));
					//};					//touchButton.Clicked += (sender, args) =>
					touchButton.Clicked += (sender, args) =>
					{
						var button = (Button) sender;
						m_CoordinatesCallback.OnClickAction(new Point(button.AnchorX, button.AnchorY));
					};
					touchButton.Pressed += (sender, args) =>
					{
						var button = (Button) sender;
						m_CoordinatesCallback.OnSelectAction(new Point(button.AnchorX, button.AnchorY));
					};


					SetLayoutFlags(touchButton, AbsoluteLayoutFlags.All);

					// anchor is on center of button
					SetLayoutBounds(touchButton,

						new Rectangle(button_width * (x + 0.5), button_height * (y + 0.5), button_width,
							button_height)); // + 0.5 si for centered anchor

					Children.Add(touchButton);
				}
			}
		}
	}
}

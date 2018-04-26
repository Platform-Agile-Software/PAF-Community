using System;
using System.Threading;
using System.Threading.Tasks;

namespace PlatformAgileFramework.XamarinFormsHelpers
{
	/// <summary>
	/// Figures out if a selection is made with a double click.
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
	/// <threadsafety>
	/// Unsafe, but this is a GUI component, called only on the GUI thread.
	/// </threadsafety>
	public class SelectionListener
	{
		/// <summary>
		/// The delegate we call to deliver the event.
		/// </summary>
		/// <remarks>
		/// The <see cref="bool"/> indicates whether it was a double-click.
		/// The <see cref="object"/> just hands back the selected object,
		/// to make calling code neater.
		/// </remarks>
		private readonly Func<bool, object, Task<bool>> m_IsDoubleClickCallbackDelegate;

		/// <summary>
		/// Timing.
		/// </summary>
		private readonly int m_WaitTimeInMilliseconds;

		/// <summary>
		/// This retains the state of "clicking". Cleared when timer expires.
		/// </summary>
		private volatile bool m_IsListeningForSecondClick;

		/// <summary>
		/// Double clicks must be on the same object. The object may be
		/// <see langword="null"/>, but must be consistent.
		/// </summary>
		private object m_ClickSender;

		/// <summary>
		/// This constructor pushes in a callback method that tells
		/// the client if the selection was made with a double or
		/// single click.
		/// </summary>
		/// <param name="isDoubleClick">
		/// The callback method that reports whether <see langword="true"/>.
		/// </param>
		/// <param name="waitTimeInMilliseconds">
		/// The time to wait between clicks to declare that only a single
		/// click was made.
		/// </param>
		public SelectionListener(Func<bool, object, Task<bool>> isDoubleClick, int waitTimeInMilliseconds)
		{
			m_IsDoubleClickCallbackDelegate = isDoubleClick;
			m_WaitTimeInMilliseconds = waitTimeInMilliseconds;
		}

		/// <summary>
		/// This method contains an internal timer that waits on a separate
		/// thread, then declares either a single or a double click. After
		/// the callback is invoked, the internals are reset for another
		/// single/double detection.
		/// </summary>
		/// <param name="sender">
		/// The sender of the click, which must be the same on
		/// successive clicks for a double-click to be declared.
		/// Can be <see langword="null"/>.
		/// </param>
		/// <param name="sctx">
		/// Optional <see cref="SynchronizationContext"/> to post results to,
		/// if present.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if double-click, <see langword="null"/>
		/// if waiting
		/// </returns>
		public virtual void ReceiveClick(object sender,
			SynchronizationContext sctx = null)
		{
			var isDoubleClick = true;
			// Extra careful on the phones, despite NUMA.
			var localSender = m_ClickSender;

			if (m_IsListeningForSecondClick && (localSender == sender))
			{
				// Timer hasn't expired. This means it's a double click if it's
				// on the same object.
				//
				//
				// Reset for the next click event.
				m_IsListeningForSecondClick = false;
				m_ClickSender = null;
				// Transmit our status.
				if(sctx != null)
					sctx.Post((obj) => { m_IsDoubleClickCallbackDelegate(true, localSender); }, null);
				else
				{
					m_IsDoubleClickCallbackDelegate(true, localSender);
				}

				return;
			}

			// If we are at this point, we've received an isolated click.
			// We record this and set the timer so we can recognize a
			// potential double-click.
			m_ClickSender = sender;
			m_IsListeningForSecondClick = true;
			Task.Run(() =>
			{
				Task.Delay(m_WaitTimeInMilliseconds).Wait();
				if (!m_IsListeningForSecondClick)
					isDoubleClick = false;
				// Reset for the next click event.
				m_IsListeningForSecondClick = false;
				m_ClickSender = null;
			});
			// Transmit our status.
			if (sctx != null)
				sctx.Post((obj) => { m_IsDoubleClickCallbackDelegate(isDoubleClick, localSender); }, null);
			else
			{
				m_IsDoubleClickCallbackDelegate(isDoubleClick, localSender);
			}
		}
	}
}

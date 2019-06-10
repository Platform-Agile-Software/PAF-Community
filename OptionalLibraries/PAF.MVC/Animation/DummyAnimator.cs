//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2019 Icucom Corporation
//
//Permission is hereby granted, free of charge, to any person obtaining a copy
//of this software and associated documentation files (the "Software"), to deal
//in the Software without restriction, including without limitation the rights
//to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//copies of the Software, and to permit persons to whom the Software is
//furnished to do so, subject to the following conditions:
//
//The above copyright notice and this permission notice shall be included in
//all copies or substantial portions of the Software.
//
//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformAgileFramework.Collections.ExtensionMethods;
namespace PlatformAgileFramework.MVC.Animation
{
	/// <summary>
	/// Dummy animator that just sets to final value.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06jun19 </date>
	/// New.
	/// </contribution>
	/// </history>
	public class DummyAnimator : IAnimator
	{
		/// <summary>
		/// Backing for virtual prop.
		/// </summary>
		private IList<IAnimator> m_Children;
		/// <summary>
		/// Stored until fired.
		/// </summary>
		private readonly Action<double> m_Callback;
		/// <summary>
		/// Stored until fired.
		/// </summary>
		private readonly int m_TimeInMilliseconds;
		/// <summary>
		/// Stored until fired.
		/// </summary>
		private readonly double m_Start;
		/// <summary>
		/// Stored until fired.
		/// </summary>
		private readonly double m_End;
		/// <summary>
		/// Stored until fired.
		/// </summary>
		private readonly Action m_Finished;
		/// <remarks>
		/// See interface.
		/// </remarks>
		public DummyAnimator(Action<double> callback, IEnumerable<IAnimator> children = null,
			int timeInMilliseconds = 250, double start = 0, double end = 1, Action finished = null)
		{
			m_Children = new List<IAnimator>();
			if(children != null)
				m_Children.AddItems(children);
			m_Callback = callback ?? throw new ArgumentNullException(nameof(callback));
			m_TimeInMilliseconds = timeInMilliseconds;
			m_Start = start;
			m_End = end;
			m_Finished = finished;
		}
		/// <summary>
		/// Dummy method that just instantaneously sets client to end value
		/// and dispatches <see cref="m_Finished"/>.
		/// </summary>
		public virtual void Animate()
		{
			m_Callback(m_End);
			m_Finished?.Invoke();
		}
		/// <summary>
		/// <see cref="IAnimator"/>.
		/// </summary>
		public virtual IList<IAnimator> Children
		{
			get => m_Children;
			set => m_Children = value;
		}
		/// <summary>
		/// Stored until fired.
		/// </summary>
		public virtual Action<double> Callback
		{
			get { return m_Callback; }
		}
		/// <summary>
		/// Stored until fired.
		/// </summary>
		public virtual int TimeInMilliseconds
		{
			get { return m_TimeInMilliseconds; }
		}
		/// <summary>
		/// Stored until fired.
		/// </summary>
		public virtual double Start
		{
			get { return m_Start; }
		}
		/// <summary>
		/// Stored until fired.
		/// </summary>
		public virtual double End
		{
			get { return m_End; }
		}
		/// <summary>
		/// Stored until fired.
		/// </summary>
		public virtual Action Finished
		{
			get { return m_Finished; }
		}
		public virtual Task StartAnimationAsync()
		{
			throw new NotImplementedException();
		}
		public virtual IEnumerator<IAnimator> GetEnumerator()
		{
			return Children.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return Children.GetEnumerator();
		}
	}
}

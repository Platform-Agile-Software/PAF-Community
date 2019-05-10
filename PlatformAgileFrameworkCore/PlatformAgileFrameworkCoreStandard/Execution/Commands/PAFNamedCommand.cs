//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2018 Icucom Corporation
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
using PlatformAgileFramework.Events;
using PlatformAgileFramework.Events.Execution.Commands;
namespace PlatformAgileFramework.Execution.Commands
{
	/// <summary>
	/// A default implementation of <see cref="IPAFNamedCommand"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// New. Needed for console UI.
	/// </description>
	/// </contribution>
	/// </history>
	public abstract class PAFNamedCommand : IPAFNamedCommand
	{
		#region Fields and Autoproperties
		/// <summary>
		/// Backing for the prop.
		/// </summary>
		protected internal string m_CommandName;
		/// <summary>
		/// State-based indicator of executability. Needed for our use cases.
		/// If <see langword="null"/>, that means we never executed
		/// <see cref="CanExecute"/> to check it yet. 
		/// </summary>
		protected internal bool? m_IsExecutable;
		/// <summary>
		/// Eventargs doesn't vary.
		/// </summary>
		private readonly EventArgs m_Args = new EventArgs();
		/// <summary>
		/// <see langword = "bool?"/> is not atomic, so we need a lock.  
		/// </summary>
		private readonly object m_IsExecutableLockObject = new object();
		#endregion // Fields and Autoproperties
		#region Constructors
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPAFNamedCommand"/>
		/// </summary>
		public virtual string CommandName
		{
			get { return m_CommandName; }
		}

		/// <summary>
		/// <see cref="IPAFNamedCommand"/>. The setter raises the
		/// event if the value has changed.
		/// </summary>
		public virtual bool? IsExecutable
		{
			get
			{
				lock (m_IsExecutableLockObject)
				{
					return m_IsExecutable;
				}
			}
			set
			{
				// Get on the stack, so it's not shared.
				bool? oldIsExecutable;
				lock (m_IsExecutableLockObject)
				{
					oldIsExecutable = m_IsExecutable;
					m_IsExecutable = value;
				}
				// Raise is outside lock.
				if (oldIsExecutable != value)
					RaiseCanExecuteChangedEvent();
			}
		}

		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref="IPAFNamedCommand"/>
		/// </summary>
		/// <param name="parameter"><see cref="IPAFNamedCommand"/></param>
		/// <returns><see cref="IPAFNamedCommand"/></returns>
		public abstract bool CanExecute(object parameter);

		/// <summary>
		/// <see cref="IPAFNamedCommand"/>
		/// </summary>
		/// <param name="parameter"><see cref="IPAFNamedCommand"/></param>
		public abstract void Execute(object parameter);

		/// <summary>
		/// <see cref="IPAFNamedCommand"/>. This default implementation just
		/// provides exception service with a try/catch block.
		/// </summary>
		/// <param name="parameter"><see cref="IPAFNamedCommand"/></param>
		/// <returns>Exception from try/catch block.</returns>
		public virtual Exception ExecuteCommand(object parameter)
		{
			Exception returnedException = null;
			try
			{
				Execute(parameter);
			}
			catch (Exception ex)
			{
				returnedException = ex;
			}

			return returnedException;
		}

		/// <summary>
		/// Fired to signal a change in state.
		/// </summary>
		/// <threadsafety>
		/// Safe - atomically fired.
		/// </threadsafety>
		protected virtual void RaiseCanExecuteChangedEvent()
		{
			m_Args.RaiseEvent(ref CanExecuteChanged, this);
		}
		#endregion // Methods
		#region Events
		/// <summary>
		/// <see cref="IPAFNamedCommand"/>
		/// </summary>
		public event EventHandler CanExecuteChanged;
		#endregion // Events

	}
}
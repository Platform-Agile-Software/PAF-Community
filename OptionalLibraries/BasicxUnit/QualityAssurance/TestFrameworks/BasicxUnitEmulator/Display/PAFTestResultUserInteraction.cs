//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 -2017 Icucom Corporation
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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Collections.Generic;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display.Commands;
using PlatformAgileFramework.TypeHandling.AggregableObjectArguments;
using PlatformAgileFramework.UserInterface;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// Simple abstract User Interaction. 
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 16jan2018 </date>
	/// <description>
	/// New. Rebuilding the hieracrchical GUI.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// NOT thread-safe.
	/// </threadsafety>
	public class PAFTestResultUserInteraction
	{
		#region Fields and Autoproperties
		/// <remarks>
		/// Dictionary of navigation commnds.
		/// </remarks>
		protected internal IDictionary<string, IPAFTestResultInfoCommand>
			m_CommandDictionary = new Dictionary<string, IPAFTestResultInfoCommand>();
		/// <remarks>
		/// <see cref="IPAFTestElementResultInfo"/>.
		/// </remarks>
		protected internal IPAFTestElementResultInfo m_CurrentTestResultNode;
		/// <remarks>
		/// We have an integer parameter for some of our commands.
		/// </remarks>
		protected internal IPAFTestResultNavigationCommandArgumentHolder m_CommandArgument;
		/// <summary>
		/// Staple in the UI.
		/// </summary>
		protected internal IPAFUIService m_UIService
			= PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFUIService>();
		#endregion // Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// Default for construct and set style. This is protected so
		/// casual users won't call this and forget to set the node.
		/// </summary>
		protected PAFTestResultUserInteraction()
		{
			m_CommandArgument = new PAFTestResultNavigationCommandArgumentHolder();
		}
		/// <summary>
		/// Just wraps the info. and builds the commands.
		/// </summary>
		/// <param name="resultInfo">
		/// Wrapped info. Not <see langword="null"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException">"resultInfo"</exception>
		/// </exceptions>
		public  PAFTestResultUserInteraction(IPAFTestElementResultInfo resultInfo)
		:this()
		{
			m_CurrentTestResultNode
				= resultInfo ?? throw new ArgumentNullException("resultInfo");
            m_CommandDictionary.Add(PAFTestResultInfoDNCommand.COMMAND_NAME, new PAFTestResultInfoDNCommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoDNICommand.COMMAND_NAME, new PAFTestResultInfoDNICommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoORCommand.COMMAND_NAME, new PAFTestResultInfoORCommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoNXCommand.COMMAND_NAME, new PAFTestResultInfoNXCommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoOVICommand.COMMAND_NAME, new PAFTestResultInfoOVICommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoPRCommand.COMMAND_NAME, new PAFTestResultInfoPRCommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoQCommand.COMMAND_NAME, new PAFTestResultInfoQCommand(m_CurrentTestResultNode));
            m_CommandDictionary.Add(PAFTestResultInfoUPCommand.COMMAND_NAME, new PAFTestResultInfoUPCommand(m_CurrentTestResultNode));
		}
		#endregion // Constructors
		#region Properties
		/// <remarks>
		/// The current <see cref="IPAFTestElementResultInfo"/> we are on in the tree.
		/// </remarks>
		public IPAFTestElementResultInfo CurrentTestResultNode
		{
			get { return m_CurrentTestResultNode; }
			protected set { m_CurrentTestResultNode = value; }
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// We unfortunately cannot determine whether the index-based commands
		/// can run until we actually have the index. We have to let the command
		/// fail. Curently we just return an exception - the GUI will be able
		/// to do much better. We are not spending time on the console interaction.
		/// </summary>
		/// <returns>
		/// Dictionary of enabled commands. This should be called also to refresh
        /// the commands with the knowledge of where we are currently in the tree.
		/// </returns>
		public virtual IDictionary<string, IPAFTestResultInfoCommand> GetEnabledCommands
			(IPAFIntArgumentHolder intHolder = null)
		{
			var argument = ((IPAFIntArgumentHolder)m_CommandArgument);
			if (intHolder != null)
				argument = intHolder;

			var enabled = new Dictionary<string, IPAFTestResultInfoCommand>();
			foreach (var key in m_CommandDictionary.Keys)
			{
				// Update each command with the current node in the tree.
				var command = m_CommandDictionary[key];
				command.ResultInfo = CurrentTestResultNode;

				// We disable the command either by the arguments
				// or position of the node in the tree.
				if ((command.CanExecute(argument))
				    || (command.IsExecutable == true))
				{
					enabled.Add(key, command);
				}
			}

			return enabled;
		}

		/// <summary>
		/// Processes and performs a command.
		/// </summary>
		/// <param name="commandString">
		/// Command string to parse and execute. Currently we allow an integer
        /// index following the command.
		/// </param>
		/// <remarks>
		/// We could do a lot of verification and error trapping in
		/// this method, but this will wait until we put the parsing
		/// infrastructure back in.
		/// </remarks>
		public virtual void ProcessCommand(string commandString)
		{
			while (true)
			{
				// If it's just blank, we present the commands.
				if (string.IsNullOrEmpty(commandString))
					commandString = PrintEnabledCommandsAndGetCommand();
				// We short-circuit the quit command.
				else if (commandString == "Q")
					return;

				var strings = commandString.Split(' ');
				var commandIndex = 0;
				if (strings.Length > 1)
				{
					if (!int.TryParse(strings[1], out commandIndex))
					{
						PrintEnabledCommandsAndGetCommand("? format error " + PlatformUtils.LTRMN);
						continue;
					}
				}

                var commandTag = strings[0];

				((IPAFIntArgumentHolder)m_CommandArgument).Argument = commandIndex;

				var enabledCommands = GetEnabledCommands();

				if (!enabledCommands.ContainsKey(commandTag))
				{
					commandString = PrintEnabledCommandsAndGetCommand("? command not available " + PlatformUtils.LTRMN);
					continue;
				}

				var command = enabledCommands[commandTag];
				command.Execute(m_CommandArgument);

				// CurrentTestResultNode will not be modified if there is an exception.
				CurrentTestResultNode = command.ResultInfo;

				Exception exception;

				if ((exception = ((IPAFExceptionArgumentHolder)m_CommandArgument).Argument) != null)
				{
					commandString = PrintEnabledCommandsAndGetCommand
					("? problem with command: " + commandString + PlatformUtils.LTRMN
					 + exception.Message + PlatformUtils.LTRMN);

					// Clear the exception before continuing the loop.
					((IPAFExceptionArgumentHolder) m_CommandArgument).Argument = null;
					continue;
				}

                // Refresh the commands.
                enabledCommands = GetEnabledCommands();

                // If the user has NOT just done a "OR", print a summary at the node.
                if (commandTag != PAFTestResultInfoORCommand.COMMAND_NAME)
                {
                    // "OR" is always available - we print the summary at the current
                    // node we ended up on.
                    command = enabledCommands[PAFTestResultInfoORCommand.COMMAND_NAME];

                    // 0 for summary only.
                    ((IPAFIntArgumentHolder)m_CommandArgument).Argument = 0;
                    command.Execute(m_CommandArgument);
                }

                // Find out what the user wants to do next.
				commandString = PrintEnabledCommandsAndGetCommand();
			}
		}
		/// <summary>
		/// Prints a filtered list of enabled commands.
		/// </summary>
		/// <param name="prefixString">
		/// string prepended to the list of commands. We use it for
		/// a question mark, etc. if command is in error. Can be
		/// <see langword="null"/>.
		/// </param>
		public virtual string PrintEnabledCommandsAndGetCommand(string prefixString = null)
		{
			var outputString = "";
			if (!string.IsNullOrEmpty(prefixString))
				outputString = prefixString;

			var enabledCommands = GetEnabledCommands();

			foreach (var command in enabledCommands)
			{
				outputString += command.Key + ", ";
			}

			outputString += PlatformUtils.LTRMN;
			m_UIService.GetMessageAndStringResponse().PresentToUser(outputString, out var userResponseString);

			return userResponseString;
		}
		#endregion // Methods
	}
}

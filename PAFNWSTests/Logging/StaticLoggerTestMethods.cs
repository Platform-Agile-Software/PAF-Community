//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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
using System.Globalization;
using System.Threading.Tasks;
using PlatformAgileFramework.Annotations;
using PlatformAgileFramework.FileAndIO;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.Logging.Tests
{
	/// <summary>
	/// These are a set of test methods designed to subject an
	/// implementing class such as <see cref="RollingLogger"/>
	/// which implements the interfaces here to access by multiple
	/// threads. This allows "stochastic testing" of concurrent
	/// functionality.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 03may2019 </date>
	/// <description>
	/// New. Built to test file writer on multiple threads.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Specifically NOT thread-safe. These methods are typically run on
	/// multiple threads, accessing a shared payload, to test its locks.
	/// </threadsafety>
	public static class StaticLoggerTestMethods
	{
		#region Methods
		/// <summary>
		/// Logs the current time.
		/// </summary>
		/// <param name="logger">
		/// Must be a <see cref="IPAFLoggingService"/>.
		/// </param>
		public static void LogATime(object logger)
		{
			var genericLogger = (IPAFLoggingService) logger;
			LoggerLogATime(genericLogger);
		}
		/// <summary>
		/// Logs the current time.
		/// </summary>
		/// <param name="logger">
		/// The <see cref="IPAFLoggingService"/>.
		/// </param>
		public static void LoggerLogATime([NotNull] IPAFLoggingService logger)
		{
			var outputString = DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\r\n";
			outputString += Task.CurrentId + "\r\n";
			outputString += "Logger\r\n\r\n";
			logger.LogEntry(outputString);
		}
		/// <summary>
		/// Dispatches files to the dispatch directory or whatever we want to do with them.
		/// </summary>
		/// <param name="dispatcher">
		/// Must be a <see cref="IPAFFileDispatcher"/>.
		/// </param>
		public static void DispatchFiles(object dispatcher)
		{
			var genericDispatcher = (IPAFFileDispatcher)dispatcher;
			DispatcherDispatchFiles(genericDispatcher);
		}
		/// <summary>
		/// Dispatches files to the dispatch directory or whatever we want to do with them.
		/// </summary>
		/// <param name="dispatcher">
		/// The <see cref="IPAFFileDispatcher"/>.
		/// </param>
		public static void DispatcherDispatchFiles([NotNull] IPAFFileDispatcher dispatcher)
		{
			using (dispatcher.GetDisposableLock())
			{
				dispatcher.DispatchFilesIfNeeded();
			}

			// We have the local knowledge, so this terrible typecast is actually OK.
			var logger = (IPAFLoggingService)dispatcher;
			var outputString = DateTime.Now.ToString(CultureInfo.InvariantCulture) + "\r\n";
			outputString += Task.CurrentId + "\r\n";
			outputString += "Dispatcher\r\n\r\n";
			logger.LogEntry(outputString);
		}

		#endregion // Methods
	}
}
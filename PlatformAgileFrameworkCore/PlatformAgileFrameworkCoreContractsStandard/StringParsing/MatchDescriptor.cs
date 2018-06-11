//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2016 Icucom Corporation
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
#pragma warning disable 1587
///<file>
/// <Summary>
/// This file contains implementations of the match descriptor types used
/// within the PAF string parsing system. These types are implemented as
/// structs to allow very lightweight operation.
/// <file/>
/// </Summary>
#pragma warning restore 1587
using System;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.StringParsing
{
	#region MatchDescriptor Types
	/// <summary>
	/// This structure provides a description of the internal details of a match
	/// found by the parser service.
	/// </summary>
	[PAFSerializable]
	public struct MatchDescriptor
	{
		private int m_OffsetOfMatchStart;
		private int m_OffsetOfMatchEnd;
		private int m_NumMatches;

		/// <summary>
		/// Constructor loads the Type with the three search result parameters.
		/// </summary>
		/// <param name="numMatches">
		/// Loads <see cref="NumMatches"/>.
		/// </param>
		/// <param name="offsetOfMatchStart">
		/// Loads <see cref="OffsetOfMatchStart"/>.
		/// </param>
		/// <param name="offsetOfMatchEnd">
		/// Loads <see cref="OffsetOfMatchEnd"/>.
		/// </param>
		public MatchDescriptor(int numMatches, int offsetOfMatchStart,
			int offsetOfMatchEnd)
		{
			m_NumMatches = numMatches;
			m_OffsetOfMatchStart = offsetOfMatchStart;
			m_OffsetOfMatchEnd = offsetOfMatchEnd;
		}

		#region IMatchDescriptor implementation
		/// <summary>
		/// This is the offest into a string where the match starts. This is the
		/// first character of the located "match" string.
		/// </summary>
		public int OffsetOfMatchStart
		{
			get { return m_OffsetOfMatchStart; }
			set{ m_OffsetOfMatchStart = value; }
		}
		/// <summary>
		/// This is the offset into a string where the match starts. This is the
		/// first character after the end of the located "match" string. It may
		/// point off the end of the searched string if the match was at the very
		/// end. In this case, it will be equal to <c>searchedString.Length()</c>.
		/// </summary>
		public int OffsetOfMatchEnd
		{
			get { return m_OffsetOfMatchEnd; }
			set{ m_OffsetOfMatchEnd = value; }
		}
		/// <summary>
		/// This is the number of matches of a search pattern that have been found
		/// in the input string.
		/// </summary>
		public int NumMatches
		{
			get { return m_NumMatches; }
			set{ m_NumMatches = value; }
		}
		#endregion
	}
	#endregion
}

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
/// <file>
/// <summary>
/// This file contains implementations of the match partition classes used
/// within the PAF string parsing system.
/// </summary>
/// <file/>
#pragma warning restore 1587
using System;
using System.Collections.Generic;
using PlatformAgileFramework.Serializing.Attributes;

namespace PlatformAgileFramework.StringParsing
{
	#region MatchPartition Types
	/// <summary>
	/// This class provides a description of the internal details of a match
	/// partition found by the parser service.
	/// </summary>
	// This is a placeholder class that will be replaced by something of the same
	// name with pooling of the internals when we get PoolingService converted.
	[PAFSerializable]
	public class MatchPartition
	{
		/// <summary>
		/// Holds the matches.
		/// </summary>
		readonly List<MatchDescriptor> m_MDList;

		/// <summary>
		/// Constructor just initializes the list.
		/// </summary>
		public MatchPartition()
		{
			m_MDList = new List<MatchDescriptor>();
		}

		#region Properties
		/// <summary>
		/// This property returns the number of matches in the list.
		/// </summary>
		public int NumContainedMatches
		{
			get { return m_MDList.Count; }
		}
		/// <summary>
		/// This property returns a <see cref="MatchDescriptor"/> that describes
		/// the "boundary" of the matches contained within the partition. The
		/// <c>NumMatches</c> is loaded with the number of matches found and contained
		/// in the partition. The <c>OffsetOfMatchStart</c> is loaded with the
		/// <c>OffsetOfMatchStart</c> of the first match. The <c>OffsetOfMatchEnd</c>
		/// is loaded with the <c>OffsetOfMatchEnd</c> of the last match. These are
		/// both -1 if <c>NumMatches</c> is 0.
		/// </summary>
		public MatchDescriptor PartitionMatchBracket
		{
			get
			{
				// Assume we got none...
				var bracketMyMatches = new MatchDescriptor(0, -1, -1);
				// Bracket the matches if we've got any.
				if(m_MDList.Count > 0) {
					bracketMyMatches.NumMatches = m_MDList.Count;
					bracketMyMatches.OffsetOfMatchStart = this[0].OffsetOfMatchStart;
					bracketMyMatches.OffsetOfMatchEnd = this[m_MDList.Count - 1].OffsetOfMatchEnd;
				}
				return bracketMyMatches;
			}
		}
		#endregion
		#region Indexers
		/// <summary>
		/// Gets a <see cref="MatchDescriptor"/> at the index.
		/// </summary>
		/// <param name="matchIndex">
		/// The 0-based index of a match to extract.
		/// </param>
		/// <returns>
		/// The <see paramref="matchIndex"/>'th <see cref="MatchDescriptor"/> found
		/// in the search.
		/// </returns>
		/// <exception>
		/// Throws a standard exception from the internal list indexer if
		/// matchIndex is out of bounds.
		/// </exception>
		public MatchDescriptor this[int matchIndex]
		{
			get { return m_MDList[matchIndex]; }
		}
		#endregion
		#region Methods
		/// <summary>
		/// Clears the partition, resulting in <c>NumContainedMatches</c> = 0;
		/// </summary>
		public void ClearMatches()
		{
			m_MDList.Clear();
		}

		/// <summary>
		/// Adds a match to the internal collection at the end.
		/// </summary>
		/// <param name="matchDescriptor">
		/// A <see cref="MatchDescriptor"/> to be appended.
		/// </param>
		public void AddMatch(MatchDescriptor matchDescriptor)
		{
			m_MDList.Add(matchDescriptor);
		}
		#endregion
	}
	#endregion
}

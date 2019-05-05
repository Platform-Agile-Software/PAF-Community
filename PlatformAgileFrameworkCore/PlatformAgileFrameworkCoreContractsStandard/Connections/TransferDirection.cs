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
//FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region Using Directives

using PlatformAgileFramework.TypeHandling.PartialClassSupport;

#endregion // Using Directives

namespace PlatformAgileFramework.Connections
{
	/// <summary>
	/// Used pseudoenums here because of past problems in mono handling
	/// atomic setting of 32 bit enums.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 19mar2018 </date>
	/// <description>
	/// New. Redid things for dynamic control of transfer. Needed an atomic
	/// setting, so did it in terms of references to a pseudoenum.
	/// </description>
	/// </contribution>
	/// </history>
	public sealed class TransferDirection: ExtendablePseudoEnumInt32
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// Transfer is shut off in both directions.
		/// </summary>
		public static readonly TransferDirection NO_TRANSFER
		= new TransferDirection("NO_TRANSFER", 0, true);
		/// <summary>
		/// Transfer is from node 1 to node 2.
		/// </summary>
		public static readonly TransferDirection ONE_TO_TWO
			= new TransferDirection("ONE_TO_TWO", 1, true);
		/// <summary>
		/// Transfer is from node 2 to node 1.
		/// </summary>
		public static readonly TransferDirection TWO_TO_ONE
			= new TransferDirection("TWO_TO_ONE", 2, true);
		/// <summary>
		/// Transfer is in both directions.
		/// </summary>
		public static readonly TransferDirection TWO_WAY
			= new TransferDirection("TWO_WAY", 3, true);
		#endregion // Class Fields And Autoproperties
		/// <remarks>
		/// See base.
		/// </remarks>
		public TransferDirection(string name, int value)
			: base(name, value)
		{
		}
		/// <remarks>
		/// See base.
		/// </remarks>
		internal TransferDirection(string name, int value, bool addToDictonary)
			: base(name, value, addToDictonary)
		{
		}
	}
}

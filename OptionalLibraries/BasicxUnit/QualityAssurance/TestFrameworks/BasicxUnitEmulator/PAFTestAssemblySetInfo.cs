//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 Icucom Corporation
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
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator
{
	/// <summary>
	/// Subclass holds static information for each test assembly set. Can be late-bound.
	/// </summary>
	/// <threadsafety>
	/// This subclass contains essentially read-only information except that set
	/// at time of late binding. Late binding should only be done at startup
	/// on a single thread. Otherwise the design is probably wrong.
	/// </threadsafety>
	public class PAFTestAssemblySetInfo : PAFTestElementInfo<IPAFTestAssemblySetInfo>,
		IPAFTestAssemblySetInfo
	{
		#region Constructors
		/// <summary>
		/// Builds with necessary parameters for later construction of all members.
		/// </summary>
		/// <param name="name">
		/// Optional name for the <see cref="IPAFTestElementInfo"/> node. Defaults to <see langword="null"/>.
		/// </param>
		/// <param name="infos">
		/// Set of assembly infos to build with. Defaults to <see langword="null"/>. infos can be
		/// added later.
		/// </param>
		/// <param name="parent">
		/// The optional parent.
		/// </param>
		public PAFTestAssemblySetInfo(string name = null, IEnumerable<IPAFTestAssemblyInfo> infos = null,
			IPAFTestElementInfo parent = null) :base(name, infos.ConvertToElements(), parent)
		{
		}
		#endregion // Constructors
		#region Implementation of IPAFTestAssemblySetInfo
		/// <summary>
		/// See <see cref="IPAFTestAssemblySetInfo"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFTestAssemblySetInfo"/>.
		/// </returns>
		public virtual ICollection<IPAFTestAssemblyInfo> GetAssemblies()
		{
			return this.GetChildInfoSubtypesOfType<IPAFTestElementInfo,IPAFTestAssemblyInfo>();
		}
		/// <summary>
		/// See <see cref="IPAFTestAssemblySetInfo"/>.
		/// </summary>
		/// <returns>
		/// See <see cref="IPAFTestAssemblySetInfo"/>.
		/// </returns>
		public virtual ICollection<IPAFTestAssemblyInfo> GetActiveAssemblies()
		{
			if (!IsExePipelineInitialized) return null;
			var assys = GetAssemblies();

			var col = assys.GetActiveElements();
			return col;
		}
		#endregion // Implementation of IPAFTestAssemblySetInfo
	}
	/// <summary>
	/// Extension helpers for assy sets.
	/// </summary>
	public static class TestAssemblySetExtensionMethods
	{
		/// <summary>
		/// Just converts an array to it's base.
		/// </summary>
		/// <param name="infos">
		/// test assembly infos.
		/// </param>
		/// <returns>
		/// test element infos.
		/// </returns>
		public static IEnumerable<IPAFTestElementInfo> ConvertToElements
			(this IEnumerable<IPAFTestAssemblyInfo> infos)
		{
			if (infos == null) return null;
			var col = new Collection<IPAFTestElementInfo>();
			foreach (var info in infos)
			{
				col.Add(info);
			}
			return col;
		}
	}
}

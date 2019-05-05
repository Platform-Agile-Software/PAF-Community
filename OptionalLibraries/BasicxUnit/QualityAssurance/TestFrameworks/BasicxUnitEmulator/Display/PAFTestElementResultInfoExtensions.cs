//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2017 Icucom Corporation
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

using System.Collections.Generic;
using System.Linq;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.FrameworkServices;
using PlatformAgileFramework.StringParsing;
using PlatformAgileFramework.UserInterface;
using PlatformAgileFramework.ErrorAndException;

namespace PlatformAgileFramework.QualityAssurance.TestFrameworks.BasicxUnitEmulator.Display
{
	/// <summary>
	/// Class has a few helper extensions and statics for test element results. The test result tree
	/// is static, determined from the test info tree which is established at test construction
	/// time. In our model, result elements are thing which are attached to nodes in the
	/// test element tree.
	/// </summary>
	/// <threadsafety>
	/// NOT thread-safe.
	/// </threadsafety>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 06feb2018 </date>
	/// <description>
	/// Forgot about the "display elements" we now have and had to add
	/// a few methods to support them.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 22dec2017 </date>
	/// <description>
	/// New.
	/// </description>
	/// </contribution>
	/// </history>
	public static class PAFTestElementResultInfoExtensions
	{
		#region Fields and Autporoperties
		/// <summary>
		/// Indicator of whether we should print as we generate output results or
		/// just deliver the string. Static is fine - it is usually determined by
		/// what platform we are on.
		/// </summary>
		public static bool PrintInternally { get; set; } = true;
		#endregion // Fields and Autporoperties
		#region Helpers for the extension methods.
		/// <summary>
		/// Gets all children of a result node. 
		/// </summary>
		/// <param name="resultElementInfo">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// List of <see cref ="IPAFTestElementResultInfo"/>s
		/// </returns>
		public static IList<IPAFTestElementResultInfo> GetAllDisplayResultChildren
			(this IPAFTestElementResultInfo resultElementInfo)
		{
			var list = new List<IPAFTestElementResultInfo>();
			if (resultElementInfo == null) return list;

			var testElementInfo = resultElementInfo.ElementInfo;

			list.AddRange(testElementInfo.GetElementsToDisplay().Select(elementInfo => elementInfo.TestElementResultInfo));

			return list;
		}
		/// <summary>
		/// Gets all DISPLAY siblings of a result node and the result node, too. 
		/// </summary>
		/// <param name="resultElementInfo">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// List of <see cref ="IPAFTestElementResultInfo"/>s
		/// </returns>
		public static IList<IPAFTestElementResultInfo> GetAllDisplayResultSiblings
			(this IPAFTestElementResultInfo resultElementInfo)
		{
			var list = new List<IPAFTestElementResultInfo>();
			if (resultElementInfo == null) return list;

			var testElementInfo = resultElementInfo.ElementInfo.Parent;

			list.AddRange(testElementInfo.GetElementsToDisplay().Select(elementInfo => elementInfo.TestElementResultInfo));

			return list;
		}
		/// <summary>
		/// Gets all siblings of a result node and the result node, too. 
		/// </summary>
		/// <param name="resultElementInfo">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// List of <see cref ="IPAFTestElementResultInfo"/>s
		/// </returns>
		public static IList<IPAFTestElementResultInfo> GetAllResultSiblings
			(this IPAFTestElementResultInfo resultElementInfo)
		{
			var list = new List<IPAFTestElementResultInfo>();
			if (resultElementInfo == null) return list;

			var testElementInfo = resultElementInfo.ElementInfo.Parent;

			list.AddRange(testElementInfo.GetAllElementChildren().Select(elementInfo => elementInfo.TestElementResultInfo));

			return list;
		}
		/// <summary>
		/// Gets all children of a result node. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// List of <see cref ="IPAFTestElementResultInfo"/>s
		/// </returns>
		public static IList<IPAFTestElementResultInfo> GetAllResultChildren
			(this IPAFTestElementResultInfo info)
		{
			var list = new List<IPAFTestElementResultInfo>();
			if (info == null) return list;

			list.AddRange(info.ElementInfo.GetAllElementChildren().Select(elementInfo => elementInfo.TestElementResultInfo));

			return list;
		}
		#endregion // Helpers for the extension methods.

		/// <summary>
		/// Returns a textual tag for display, corresponding to the actual
		/// derived type of the interface.
		/// </summary>
		/// <param name="testElementResultInfo">One of us.</param>
		/// <returns>
		/// <c>"Method", "Fixture", "Assembly", "AssemblySet"</c>, etc.
		/// </returns>
		public static string GetElementTypeTag(this IPAFTestElementResultInfo testElementResultInfo)
		{
			switch (testElementResultInfo.ElementInfo)
			{
				case IPAFTestMethodInfo _:
					return "Method";
				case IPAFTestFixtureInfo _:
					return "Fixture";
				case IPAFTestFixtureWrapper _:
					return "FixtureWrapper";
				case IPAFTestAssemblyInfo _:
					return "Assembly";
				case IPAFTestAssemblySetInfo _:
					return "AssemblySet";
				case IPAFTestHarnessInfo _:
					return "Harness";
			}
			return "Element";
		}
		/// <summary>
		/// Goes down to the first child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// First child if we are not a leaf node. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="IsOnALeaf"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToFirstChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsOnALeaf())
				return info;
			var children
				= info.GetAllResultChildren();
			return children.First();
		}
		/// <summary>
		/// Goes down to the first DISPLAY child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// First child if we are not a leaf node. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="IsOnALeaf"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToFirstDisplayChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsOnALeaf())
				return info;
			var children = info.GetAllDisplayResultChildren();
			return children.First();
		}
		/// <summary>
		/// Goes down to the last child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// Last child if we are not a leaf node. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="IsOnALeaf"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToLastChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsOnALeaf())
				return info;
			var children
				= info.GetAllResultChildren();
			return children.Last();
		}
		/// <summary>
		/// Goes down to the last DISPLAY child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// Last child if we are not a leaf node. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="IsOnALeaf"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToLastDisplayChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsOnALeaf())
				return info;
			var children = info.GetAllDisplayResultChildren();
			return children.Last();
		}
		/// <summary>
		/// Goes to a node's "next" DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// The sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNextDisplaySibling"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToNextDisplaySibling
			(this IPAFTestElementResultInfo info)
		{
			if (!info.HasNextDisplaySibling())
				return info;
			var children = info.GetAllDisplayResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			return children[itemIndex + 1];
		}
		/// <summary>
		/// Goes to a node's "next" sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// The sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNextSibling"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToNextSibling
			(this IPAFTestElementResultInfo info)
		{
			if (!info.HasNextSibling())
				return info;
			var children = info.GetAllResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			return children[itemIndex + 1];
		}
		/// <summary>
		/// Goes to a node's "prior" DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// The sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasPriorDisplaySibling"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToPriorDisplaySibling
			(this IPAFTestElementResultInfo info)
		{
			if (!info.HasPriorDisplaySibling())
				return info;
			var children = info.GetAllDisplayResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			return children[itemIndex - 1];
		}
		/// <summary>
		/// Goes to a node's "prior" sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// The sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasPriorSibling"/> before
		/// the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToPriorSibling
			(this IPAFTestElementResultInfo info)
		{
			if (!info.HasPriorSibling())
				return info;
			var children = info.GetAllResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			return children[itemIndex - 1];
		}
		/// <summary>
		/// Goes down to a numbered child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="childNumber">
		/// The 0-based number of the child to seek.
		/// </param>
		/// <returns>
		/// Numbered child if the child exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNumberedChild"/>
		/// before the call.
		/// </returns>
		public static IPAFTestElementResultInfo GoDownToNumberedChild
			(this IPAFTestElementResultInfo info, int childNumber)
		{
			if (!info.HasNumberedChild(childNumber))
				return info;
			var children = info.GetAllResultChildren();
			return children[childNumber];
		}
		/// <summary>
		/// Goes down to a numbered DISPLAY child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="childNumber">
		/// The 0-based number of the child to seek.
		/// </param>
		/// <returns>
		/// Numbered child if the child exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNumberedDisplayChild"/>
		/// before the call.
		/// </returns>
		public static IPAFTestElementResultInfo GoDownToNumberedDisplayChild
			(this IPAFTestElementResultInfo info, int childNumber)
		{
			if (!info.HasNumberedDisplayChild(childNumber))
				return info;
			var children = info.GetAllDisplayResultChildren();
			return children[childNumber];
		}
		/// <summary>
		/// Goes down to a numbered DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="siblingNumber">
		/// The 0-based number of the child to seek.
		/// </param>
		/// <returns>
		/// Numbered sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNumberedDisplaySibling"/>
		/// before the call.
		/// </returns>
		public static IPAFTestElementResultInfo GoToNumberedDisplaySibling
			(this IPAFTestElementResultInfo info, int siblingNumber)
		{
			if (!info.HasNumberedDisplaySibling(siblingNumber))
				return info;
			var children = info.GetAllDisplayResultSiblings();
			return children[siblingNumber];
		}
		/// <summary>
		/// Goes down to a numbered sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="siblingNumber">
		/// The 0-based number of the child to seek.
		/// </param>
		/// <returns>
		/// Numbered sibling if the sibling exists. Same node
		/// otherwise, essentially rendering this as a no-op.
		/// Programmer must check <see cref="HasNumberedSibling"/>
		/// before the call.
		/// </returns>
		public static IPAFTestElementResultInfo GoToNumberedSibling
			(this IPAFTestElementResultInfo info, int siblingNumber)
		{
			if (!info.HasNumberedSibling(siblingNumber))
				return info;
			var children = info.GetAllResultSiblings();
			return children[siblingNumber];
		}
		/// <summary>
		/// Goes up to the parent. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// Parent node if we were not at the top of the
		/// results hierarchy. Returns same node if we were,
		/// making the call essentially a no-op. Programmer
		/// must check <see cref="IsAtRoot"/> before the call. 
		/// </returns>
		public static IPAFTestElementResultInfo GoToParent
			(this IPAFTestElementResultInfo info)
		{
			if (info.ElementInfo.Parent == null)
				return info;
			return info.ElementInfo.Parent.TestElementResultInfo;
		}

		/// <summary>
		/// Verifies that a node has a numbered child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="childNumber">The 0-based number of the child.</param>
		/// <returns>
		/// <see langword="true"/> if the child exists.
		/// </returns>
		public static bool HasNumberedChild
			(this IPAFTestElementResultInfo info, int childNumber)
		{
			if (info.IsOnALeaf())
				return false;
			var children
				= info.GetAllResultChildren();
			if ((children.Count > childNumber) && (childNumber >= 0))
				return true;
			return false;
		}
		/// <summary>
		/// Verifies that a node has a numbered DISPLAY child. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="childNumber">The 0-based number of the child.</param>
		/// <returns>
		/// <see langword="true"/> if the child exists.
		/// </returns>
		public static bool HasNumberedDisplayChild
			(this IPAFTestElementResultInfo info, int childNumber)
		{
			if (info.IsOnALeaf())
				return false;
			var children
				= info.GetAllDisplayResultChildren();
			if ((children.Count > childNumber) && (childNumber >= 0))
				return true;
			return false;
		}
		/// <summary>
		/// Verifies that a node has a numbered DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="siblingNumber">
		/// The 0-based number of the sibling. Less than 0 is OK.
		/// We just return <see langword="false"/>. We don't provide
		/// exception service in these extensions.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasNumberedDisplaySibling
			(this IPAFTestElementResultInfo info, int siblingNumber)
		{
			if (info.IsAtRoot())
				return false;
			var children = info.GetAllDisplayResultSiblings();
			if ((children.Count > siblingNumber) && (siblingNumber >= 0))
				return true;
			return false;
		}
		/// <summary>
		/// Verifies that a node has a numbered sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <param name="siblingNumber">
		/// The 0-based number of the sibling. Less than 0 is OK.
		/// We just return <see langword="false"/>. We don't provide
		/// exception service in these extensions.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasNumberedSibling
			(this IPAFTestElementResultInfo info, int siblingNumber)
		{
			if (info.IsAtRoot())
				return false;
			var children
				= info.GetAllResultSiblings();
			if ((children.Count > siblingNumber) && (siblingNumber >= 0))
				return true;
			return false;
		}
		/// <summary>
		/// Verifies that a node has a "prior" DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasPriorDisplaySibling
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return false;
			var children = info.GetAllDisplayResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			if (itemIndex < 1)
				return false;
			return true;
		}
		/// <summary>
		/// Verifies that a node has a "prior" sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasPriorSibling
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return false;
			var children
			= info.GetAllResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			if (itemIndex < 1)
				return false;
			return true;
		}
		/// <summary>
		/// Verifies that a node has a "next" DISPLAY sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasNextDisplaySibling
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return false;
			var children = info.GetAllDisplayResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			if (itemIndex == -1)
				return false;
			if (children.Count > itemIndex + 1)
				return true;
			return false;
		}
		/// <summary>
		/// Verifies that a node has a "next" sibling. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if the sibling exists.
		/// </returns>
		public static bool HasNextSibling
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return false;
			var children = info.GetAllResultSiblings();
			var itemIndex = children.ItemIndexInList(info);
			if (itemIndex == -1)
				return false;
			if (children.Count > itemIndex + 1)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the top of a hierarchy. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if at the top of the results hierarchy.
		/// </returns>
		public static bool IsAtRoot
			(this IPAFTestElementResultInfo info)
		{
			if (info.ElementInfo.Parent == null)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the bottom of a hierarchy or on a leaf node. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if at a leaf node.
		/// </returns>
		public static bool IsOnALeaf
			(this IPAFTestElementResultInfo info)
		{
			if (info.ElementInfo.GetAllElementChildren().Count == 0)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the first child of a parent. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if on a first child node. If we are the
		/// root node, this will be <see langword="true"/>.
		/// </returns>
		public static bool IsOnFirstChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return true;
			var children = info.GetAllResultSiblings();
			if (children[0] == info)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the first DISPLAY child of a parent. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if on a first child node. If we are the
		/// root node, this will be <see langword="true"/>.
		/// </returns>
		public static bool IsOnFirstDisplayChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return true;
			var children = info.GetAllDisplayResultSiblings();
			if (children[0] == info)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the last child of a parent. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if on the last child node. If we are the
		/// root node, this will be <see langword="true"/>.
		/// </returns>
		public static bool IsOnLastChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return true;
			var children = info.GetAllResultSiblings();
			if (children.Last() == info)
				return true;
			return false;
		}
		/// <summary>
		/// Determines if we are at the last DISPLAY child of a parent. 
		/// </summary>
		/// <param name="info">
		/// Any <see cref="IPAFTestElementResultInfo"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if on the last child node. If we are the
		/// root node, this will be <see langword="true"/>.
		/// </returns>
		public static bool IsOnLastDisplayChild
			(this IPAFTestElementResultInfo info)
		{
			if (info.IsAtRoot())
				return true;
			var children = info.GetAllDisplayResultSiblings();
			if (children.Last() == info)
				return true;
			return false;
		}
		/// <summary>
		/// Prints information about a single node in the test tree.
		/// It simply calls <see cref="PrependToInfoString"/> on each to get
		/// summary information. It prints each bit of information on
		/// a separate line.
		/// </summary>
		/// <param name="testElementResultInfo">One of us.</param>
		/// <param name="displayChildNumber">
		///     Set to display number of a child node. Useful for console output. If this is set at a node,
		///     all printouts of children will be numbered. In the GUI, child numbers can be determined
		///     just by enumerating the children. This is just a shortcut for the console interaction.
		/// </param>
		/// <param name="detailLevel">
		///     Detail level of the output. Curently 0, 1 and 2 are supported.
		/// </param>
		/// <param name="printHierarchy">
		/// <see langword="true"/> to print all nodes below us in the tree.
		/// </param>
		/// <remarks>
		/// The numbering is 0 - based.
		/// </remarks>
		public static string PrintResultAtNode<T>(this T testElementResultInfo,
			bool displayChildNumber = true, int detailLevel = 0, bool printHierarchy = true)
			where T : IPAFTestElementResultInfo
		{
			// We need to push in the child number if required.
			var prefixString = "";
			if ((displayChildNumber) && (testElementResultInfo.ChildElementNumber >= 0))
				prefixString = "(" + testElementResultInfo.ChildElementNumber + ")";

			var uiService = PAFServiceManagerContainer.ServiceManager.GetTypedService<IPAFUIService>();
			var outputStringWithoutTerminator = testElementResultInfo.PrependToInfoString(prefixString);
			var outputStringWithTerminator = outputStringWithoutTerminator.EnsureTerminator();

			// For details, we use exceptions.
			if (detailLevel >= 0)
			{
				foreach (var excep in testElementResultInfo.ElementInfo.Exceptions)
				{
					outputStringWithoutTerminator
						= outputStringWithTerminator + excep.RenderExceptionDetail(detailLevel);
					outputStringWithTerminator = outputStringWithoutTerminator.EnsureTerminator();
				}
			}

			var localOutputStringWithTerminator = outputStringWithTerminator;

			// Print just our node if required.
			if (PrintInternally)
				uiService.GetMessageWithNoResponse().PresentToUser(localOutputStringWithTerminator);


			if (printHierarchy)
			{
				var childResults = testElementResultInfo.DisplayChildResultGatherer(testElementResultInfo);
				if (childResults.Any())
				{
					outputStringWithoutTerminator = outputStringWithTerminator;
					// Let the children print themselves, each separated with a terminator.
					for (var childNum = 0; childNum < childResults.Count; childNum++)
					{
						outputStringWithoutTerminator = outputStringWithTerminator;
						var childResult = childResults[childNum];
						childResult.ChildElementNumber = childNum;
						outputStringWithoutTerminator
							+= childResult.PrintResultAtNode(displayChildNumber, detailLevel);
						outputStringWithTerminator = outputStringWithoutTerminator.EnsureTerminator();
					}
				}
			}

			outputStringWithTerminator = outputStringWithoutTerminator.EnsureTerminator();

			return outputStringWithTerminator;
		}
		/// <summary>
		/// Gathers the result infos associated with child node of the node
		/// that the result is attached to. These cannot be Generics, since
		/// there are no constraints on the type of children, except that they
		/// be <see cref="IPAFTestElementInfo"/>s
		/// </summary>
		/// <param name="testElementResultInfo">One of us.</param>
		/// <remarks>
		/// Set of result infos -never <seee langword="null"/>.
		/// </remarks>
		public static IList<IPAFTestElementResultInfo>
			GenerateChildResults(this IPAFTestElementResultInfo testElementResultInfo)
		{
			var testElementInfo = testElementResultInfo.ElementInfo;
			var list = new List<IPAFTestElementResultInfo>();

			var displayTestElementChildren = testElementInfo.GetElementsToDisplay();

			if (!displayTestElementChildren.Any())
				return list;

			foreach (var child in displayTestElementChildren)
			{
				list.Add(child.TestElementResultInfo);
			}
			return list;
		}
		/// <summary>
		/// This prefix to printed lines (and perhaps accessed by GUI) both tells
		/// us what kind of node we are on and allows navigation in the output.
		/// </summary>
		/// <returns> A prefix with left carets and element type/name.</returns>
		public static string GetPrefix(this IPAFTestElementResultInfo testElementResultInfo)
		{
			var depth = testElementResultInfo.ElementInfo.GetDepthDownFromRoot();
			var carets = "";

			while (depth-- != -1)
			{
				carets += "<";
			}

			var output = carets + " "
						 + testElementResultInfo.GetElementTypeTag() + "->"
						 + testElementResultInfo.ElementInfo.TestElementName;
			return output;

		}
		/// <summary>
		/// This method just provides a string to indicate test passed/failed/indeterminate.
		/// </summary>
		/// <param name="didPass">
		/// Status of the passing of this node.
		/// </param>
		/// <returns> "Passed"/"Failed"/"Indeterminate"</returns>
		public static string GetPassFailIndeterminateString(bool? didPass)
		{
			switch (didPass)
			{
				case null:
					return "Indeterminate";
				case true:
					return "Passed";
			}
			return "Failed";
		}

		/// <summary>
		/// This method outputs a header for each node with additional
		/// information possibly prepended to it.
		/// </summary>
		/// <param name="testElementResultInfo">One of us.</param>
		/// <param name="prefixString"/>
		/// Optional string prepended to the header. Can be <see langword="null"/>.
		/// <param/>
		/// <returns>Node Info string.</returns>
		public static string PrependToInfoString(this IPAFTestElementResultInfo testElementResultInfo, string prefixString)
		{
			if (testElementResultInfo.ShouldDisplay == false)
				return "";
			var workingString = "";
			if (!string.IsNullOrEmpty(prefixString))
			{
				workingString = prefixString;
			}
			workingString +=
				testElementResultInfo.GetPrefix()
				+ " (" +
				GetPassFailIndeterminateString(testElementResultInfo.ElementInfo.Passed)
				+ ")";
			return workingString;
		}

	}
}

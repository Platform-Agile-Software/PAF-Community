using System;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace PlatformAgileFramework.TypeHandling.TypeExtensionMethods.Helpers
{
	/// <summary>
	/// This interface represents a node in a Generic type tree. It is used
	/// in the hierarchical decomposition of Generic types into non-Generic
	/// types so that types can late-loaded. .Net Standard now has a
	/// <see cref="Type"/> class that is a token of the actual type,
	/// so the hierarchical stuff on our <see cref="IPAFTypeHolder"/>
	/// is gone.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 01feb2019 </date>
	/// <description>
	/// New. Reinvention of the type tree for the revised type system in
	/// .Net Standard.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFGenericTypeNode
	{
		/// <summary>
		/// Returns the set of actual wrapped types. <see langword="null"/> if
		/// this is a leaf node.
		/// </summary>
		IList<Type> ActualTypes { get; }
		/// <summary>
		/// Returns the set of child nodes. <see langword="null"/> if
		/// this is a leaf node.
		/// </summary>
		IList<IPAFGenericTypeNode> ChildTypeNodes { get; }
		/// <summary>
		/// Returns the type of the current node.
		/// </summary>
		Type NodeType { get; }
		/// <summary>
		/// Returns the parent node. <see langword="null"/> if we are at the
		/// root. A parent node is always Generic by construction.
		/// </summary>
		IPAFGenericTypeNode GenericParentNode { get; }
	}
}
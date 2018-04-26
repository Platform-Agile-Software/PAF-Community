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

using System;
using System.Collections.Generic;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using IPAFTypeMismatchExceptionData = PlatformAgileFramework.TypeHandling.Exceptions.IPAFTypeMismatchExceptionData;
using PAFTypeMismatchExceptionData = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
using PAFTypeMismatchExceptionMessageTags = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionMessageTags;

namespace PlatformAgileFramework.TypeHandling
{
	/// <summary>
	/// Extension methods make it easier to work with <see cref="IPAFTypeHolder"/>.
	/// </summary>
// ReSharper disable once PartialTypeWithSinglePart
	// Core part
	public static partial class PAFTypeHolderExtensions
	{
		#region Partial Methods
		/// <summary>
		/// Optional method will determine whether the type is consistent with the generic.
		/// Needed because all code generation is now moved out of core.
		/// </summary>
		/// <param name="exception">
		/// The parameter is initialized to <see langword="null"/> in this class. If the method
		/// returns a value, the exception is normally thrown or wrapped.
		/// </param>
		/// <param name="typeHolder">
		/// Type holder for type, which may or MAY NOT contain an actual type <see cref="Type"/>.
		/// </param>
		/// <threadsafety>
		/// The implementation of this method must be thread safe if the methods
		/// in which it is used are to be thread-safe. The argument is held locally
		/// (as it always should be), but the internals of the delegate must otherwise
		/// withstand concurrent calls.
		/// </threadsafety>
		/// <remarks>
		/// This method should not allow any exceptions to be thrown, instead returning
		/// caught exceptions and returning them.
		/// </remarks>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void IsGenericAssignableFromInterfaceTypePartial<U>(ref Exception exception,
			IPAFTypeHolder typeHolder);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		/// <summary>
		/// Gives a partial class defined by extenders a chance to resolutely determine
		/// whether a type is loadable BEFORE the logic in this class is called. This
		/// was specifically done for mono/Mac, since it's loader requires a version
		/// number.
		/// </summary>
		/// <param name="isTheTypeLoadable">
		/// Will be set to non - <see langword="null"/> if the linked
		/// partial has a definate idea yes/no wther we are loadable.
		/// </param>
		/// <returns>
		/// Value is unchanged if we have no vote.
		/// </returns>
// ReSharper disable once PartialMethodWithSinglePart
		static partial void IstheTypeLoadable(ref bool? isTheTypeLoadable);
		#endregion // Partial Methods
		/// <summary>
		/// Determines whether a <see cref="IPAFTypeHolder"/> has a namespace,
		/// unqualified name and an assembly name.
		/// </summary>
		/// <param name="holder">
		/// Incoming type holder. <see langword="null"/> gets <see langword="false"/>.
		/// </param>
		/// <returns>
		/// <see langword ="true"/> if conditions are satisfied.
		/// </returns>
		public static bool IsTypeLoadable(this IPAFTypeHolder holder)
		{
			if (holder == null) return false;

			bool? optionalVote = null;
// Resharper still doesn't quite get the partial method thing................
			// ReSharper disable once InvocationIsSkipped
			IstheTypeLoadable(ref optionalVote);
// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (optionalVote != null)
// ReSharper disable HeuristicUnreachableCode
			{
				return optionalVote.Value;
			}
// ReSharper restore HeuristicUnreachableCode

			var assyHolder = holder.GetAssemblyHolder();
		    if (assyHolder?.AssemblyNameString == null || (assyHolder.AssemblyNameString == "*"))
				return false;
			if ((string.IsNullOrEmpty(holder.Namespace)) || (string.IsNullOrEmpty(holder.SimpleTypeName)))
				return false;
			return true;
		}
		/// <summary>
		/// Converts a list of types to typeholders.
		/// </summary>
		/// <param name="types">
		/// Incoming types. <see langword="null"/> gets <see langword="null"/>.
		/// </param>
		/// <returns>Outgoing types.</returns>
		/// <remarks>
		/// This one is actually an enumerable extension. We don't want to pollute the
		/// enumerable extension class with bizarre types like a <see cref="PAFTypeHolder"/>.
		/// </remarks>
		public static IList<IPAFTypeHolder> TypesToTypeHolders(this IEnumerable<Type> types)
		{
			if (types == null) return null;

			var typeHolders = new List<IPAFTypeHolder>();
			foreach (var type in types) {
				typeHolders.Add(PAFTypeHolder.IHolder(type));
			}
			return typeHolders;
		}
		/// <summary>
		/// Converts a list of type names to typeholders.
		/// </summary>
		/// <param name="typeNames">
		/// Incoming names. <see langword="null"/> gets <see langword="null"/>.
		/// </param>
		/// <returns>Outgoing holders.</returns>
		/// <remarks>
		/// This one is actually an enumerable extension. We don't want to pollute the
		/// enumerable extension class with bizarre types like a <see cref="PAFTypeHolder"/>.
		/// </remarks>
		public static IList<IPAFTypeHolder> TypesNamesToTypeHolders(this IEnumerable<string> typeNames)
		{
			if (typeNames == null) return null;

			var typeHolders = new List<IPAFTypeHolder>();
			foreach (var name in typeNames) {
				typeHolders.Add(new PAFTypeHolder(name));
			}
			return typeHolders;
		}
		/// <summary>
		/// Method will determine whether an interface type is consistent with the Generic
		/// closure of this class. In core, it only checks a type's <see cref="Type"/> if
		/// it is not <see langword="null"/>. It wraps a partial method intended to be used
		/// in any extension of the framework that can do code generation for something
		/// like a proxy.
		/// </summary>
		/// <remarks>
		/// The exception that is thrown by this method can be expected to wrap other
		/// exceptions generated from the partial class, if it is active.
		/// </remarks>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}">
		/// <see cref="Notification.Exceptions.PAFTypeMismatchExceptionDataBase.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// is thrown if the Generic constraint is not satisfied. This exception may actually wrap
		/// other exceptions that are encountered during lazy loading of a type by its stringful
		/// description in framework extensions. 
		/// </exception>
		/// </exceptions>
		public static void ValidateGenericAssignableFromType<U>(this IPAFTypeHolder implementationTypeHolder)
		{
			if (implementationTypeHolder.TypeType != null)
			{
				// Check if service meets Generic constraint.
				if (!(typeof(U).IsTypeAssignableFrom(implementationTypeHolder.TypeType)))
				{
					var data = new PAFTypeMismatchExceptionData(implementationTypeHolder, new PAFTypeHolder(typeof(U)));
					throw new PAFStandardException<IPAFTypeMismatchExceptionData>(data, PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
				}
				return;
			}
			Exception exception = null;
			// ReSharper disable InvocationIsSkipped
			// Check if service interface type meets Generic constraint based on stringful representation
			// of the type.
			IsGenericAssignableFromInterfaceTypePartial<U>(ref exception, implementationTypeHolder);
			// ReSharper restore InvocationIsSkipped
			// ReSharper disable once ExpressionIsAlwaysNull
			// ReSharper disable once ConditionIsAlwaysTrueOrFalse
			if (exception != null)
			// ReSharper disable once HeuristicUnreachableCode
			{
				// ReSharper disable HeuristicUnreachableCode
				var data = new PAFTypeMismatchExceptionData(implementationTypeHolder, new PAFTypeHolder(typeof(U)));
				throw new PAFStandardException<IPAFTypeMismatchExceptionData>(
					data, PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
				// ReSharper restore HeuristicUnreachableCode
			}
		}
	}
}
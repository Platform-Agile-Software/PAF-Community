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

#region Using Directives

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

#endregion // Using Directives

namespace PlatformAgileFramework.TypeHandling.ParameterHelpers
{
	/// <summary>
	/// This class contains the collection of parameter attributes we use when doing
	/// method invokes.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 04jun2015 </date>
	/// <description>
	/// New.
	/// Clean up of the delegate verification stuff in our reflection classes.
	/// All of this stuff was originally built because  delegate stuff did
	/// not work correctly in all cases on mono.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Unsafe. Not intended for multi-threaded environmnts.
	/// </threadsafety>
	public class MethodParameters
	{
		#region Type fields and autoprops
		/// <summary>
		/// Passed parameters. This is a list in the correct order of appearance in
		/// method param list. <see langword = "null"/> for no params in the list "/>
		/// </summary>
		public IReadOnlyList<ParameterCharacteristics> Parameters { get; protected set; }
		/// <summary>
		/// Return value - <see langword="null"/> for void return.
		/// </summary>
		public ParameterCharacteristics Retval{ get; protected set; }
		#endregion // Type fields and autoprops
		#region Constructors
		/// <summary>
		/// Just constructs our representation from a set of <see cref="ParameterInfo"/>'s.
		/// </summary>
		/// <param name="parameters">
		/// <see langword="null"/> for a method with no parames and no return value.
		/// </param>
		public MethodParameters(IEnumerable<ParameterInfo> parameters)
		{
			IList<ParameterInfo> parameterInfos = null;

		    IComparer<ParameterInfo> isFirstParamGreater = null;

            if (parameters == null) return;
		    parameters = parameters.BuildCollection();

            if(parameters.Any()) isFirstParamGreater = new ParameterInfoPositionalComparer();

            foreach (var paramInfo in parameters)
			{
				if (paramInfo.IsRetval)
				{
					Retval = new ParameterCharacteristics(paramInfo);
					continue;
				}
				if (parameterInfos == null)
				{
					parameterInfos = new Collection<ParameterInfo>();
				}
				parameterInfos.AddItemInOrder(paramInfo, isFirstParamGreater);
			}
			if (parameterInfos == null) return;

			// Got 'em in order, now convert 'em.
			var chars = new Collection<ParameterCharacteristics>();
			foreach (ParameterInfo pInfo in parameterInfos)
			{
				chars.Add(new ParameterCharacteristics(pInfo));
			}
			Parameters = chars;
		}

		/// <summary>
		/// Constructs our representation from a set of <see cref="Type"/>'s and
		/// a return value. All parameters are considered to be "in" parameters.
		/// </summary>
		/// <param name="parameterTypes">
		/// Can be <see langword="null"/> for no params.
		/// </param>
		/// <param name="returnValueType">
		/// <see langword="null"/> for no return value.
		/// </param>
		public MethodParameters(IEnumerable<Type> parameterTypes, Type returnValueType = null)
		{
			if(returnValueType != null)
 				Retval = new ParameterCharacteristics(returnValueType);

			if (parameterTypes == null) return;

			// Convert 'em to "in" parameters.
			var chars = new Collection<ParameterCharacteristics>();
			foreach (var type in parameterTypes)
			{
				chars.Add(new ParameterCharacteristics(type, false));
			}
			Parameters = chars;
		}
		/// <summary>
		/// Just constructs our representation from a set of <see cref="ParameterCharacteristics"/>'s.
		/// </summary>
		/// <param name="parameters">
		/// <see langword="null"/> for a method with no params.
		/// </param>
		/// <param name="returnValue">
		/// Characteristics for the return value - <see langword="null"/> for a void method.
		/// </param>
		public MethodParameters(IEnumerable<ParameterCharacteristics> parameters,
			ParameterCharacteristics returnValue = null)
		{
			Retval = returnValue;
			if (parameters == null) return;
			Parameters = parameters.BuildCollection();
		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// Number of required parameters.
		/// </summary>
		public int NumRequiredParameters
		{
			get
			{
				if (Parameters == null) return 0;
				return Parameters.Count(parameter => !parameter.IsOptional);
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Determines if another parameter set can be substituted for this one
		/// in a method invoke.
		/// </summary>
		/// <param name="parametersToSubstitute">
		/// The actual parameters we'd like to pass, instead of the current set.
		/// <see langword="null"/> matches a method with no parameters returning void. 
		/// </param>
		/// <returns>
		/// <see langword="true"/> if we can make the call.
		/// </returns>
		/// <remarks>
		/// For "ref" parameters, types must match exactly. For "in" parameters,
		/// substituted parameter must be assignable to our parameter. For "out"
		/// parameters and return values, our parameter must be assignable to
		/// the substitute parameter.
		/// </remarks>
		public bool CanCallWith(MethodParameters parametersToSubstitute)
		{
            ////////////////////////////////////////////////////////////////////////////////////
            // Get easy stuff out of the way.
		    if ((parametersToSubstitute == null) && ((Parameters == null) || (Parameters.Count == 0)) && (Retval == null)) return true;
		    if (parametersToSubstitute == null) return false;
			if (parametersToSubstitute.Retval != null && (Retval == null)) return false;
			if ((Retval == null) && (parametersToSubstitute.Retval != null)) return false;
			if ((Retval != null) && (parametersToSubstitute.Retval != null))
			{
				if (!parametersToSubstitute.Retval.ParameterType.IsTypeAssignableFrom(Retval.ParameterType))
					return false;
			}

			// It's OK if nobody has any parameters.
			if (
				((Parameters == null) || (Parameters.Count == 0))
				&&
				((parametersToSubstitute.Parameters == null) || (parametersToSubstitute.Parameters.Count == 0))
				)
			{
				return true;
			}

			// See if substitute params will fit.
			if (
				(Parameters == null)
				||
				(Parameters.Count < parametersToSubstitute.Parameters.Count)
				)
			{
				return false;
			}

			////////////////////////////////////////////////////////////////////////////////////
			// Here's the real work - we've got to check the variance of each param.
			for (var parNum = 0; parNum < parametersToSubstitute.Parameters.Count; parNum ++)
			{
				// Exact type match for ref.
				if (parametersToSubstitute.Parameters[parNum].IsRef)
				{
					if (!Parameters[parNum].IsRef) return false;
					if (parametersToSubstitute.Parameters[parNum].ParameterType != Parameters[parNum].ParameterType)
						return false;
				}
				// One way.
				if (parametersToSubstitute.Parameters[parNum].IsIn)
				{
					if (!Parameters[parNum].IsIn) return false;
					if (!Parameters[parNum].ParameterType.IsTypeAssignableFrom(parametersToSubstitute.Parameters[parNum].ParameterType))
						return false;
				}
				// The other way.
				if (parametersToSubstitute.Parameters[parNum].IsOut)
				{
					if (!Parameters[parNum].IsOut) return false;
					if (!parametersToSubstitute.Parameters[parNum].ParameterType.IsTypeAssignableFrom(Parameters[parNum].ParameterType))
						return false;
				}				
			}
			return true;
		}
		#endregion // Methods
	}

}

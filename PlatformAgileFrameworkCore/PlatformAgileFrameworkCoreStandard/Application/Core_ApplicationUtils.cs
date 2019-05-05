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

// ReSharper disable once RedundantUsingDirective
using System;
using System.Security;
using PlatformAgileFramework.TypeHandling.PartialClassSupport;

namespace PlatformAgileFramework.Application
{
	/// <summary>
	/// <para>
	/// Helper classes/methods for overall application support.
	/// This part is Silverlight compatible (single AppDomain).
	/// </para>
	/// <para>
	/// Many of the facilities found here are present in platform-specific
	/// assemblies. Because we must keep the PAF core platform-agile and
	/// actually platform-independent, we implement those items here in an
	/// abstract way, allowing platform-specific extensions to provide
	/// concrete functionality. For example, the "System.Windows.Application"
	/// object contains methods for establishing current trust levels.
	/// We don't want anything to do with this assembly in core, since
	/// this assembly is Microsoft-specific. Framework extenders and/or
	/// users will connect to the platform-specific assemblies either
	/// through delegates or partial classes or through inheritance, which
	/// are our extensibility mechanisms.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 02sep2012 </date>
	/// <contribution>
	/// New.
	/// </contribution>
	/// </history>
// ReSharper disable PartialTypeWithSinglePart
	public partial class ApplicationUtils
// ReSharper restore PartialTypeWithSinglePart
	{
		#region Class Fields And Autoproperties
		/// <summary>
		/// This field is included for testing purposes. The assembly containing
		/// this class will normally have its internals exposed to the test
		/// assembly. Test setup methods can set this field. If this field has
		/// no value, the methods in this class operate normally. If this field
		/// is set, the application thinks it is in one mode or the other. On
		/// the other hand, if this is being used in a trusted ECMA/CLR environment
		/// the field can just be set.
		/// </summary>
		/// <threadsafety>
		/// No thread safety - designed to be set once at application initialization.
		/// </threadsafety>
		internal static bool? s_IsElevatedTrustTestOverride;
		/// <summary>
		/// In lieu of creating a second part to this class, the framework user/extender
		/// can simply provide a delegate that will tell us what security level we are at.
		/// If this is <see langword="null"/>, partial method can be used, or the <see cref="IsElevatedTrust"/>
		/// will return its default, which is <see langword="false"/>.
		/// </summary>
		/// <threadsafety>
		/// No thread safety - designed to be set once at application initialization. The
		/// implementation of the delegate, must however, be thread-safe.
		/// </threadsafety>
		internal static PartialClassUtils.TrueFalseOrIndifferent s_TrustSettingDelegate;
		#endregion // Class Fields And Autoproperties
		#region Methods
		#region Partial Methods

		/// <summary>
		/// Optional method will determine whether we are operating in an elevated-trust
		/// environment.
		/// </summary>
		/// <param name="isElevatedTrust">
		/// The parameter is initialized to <see langword="null"/> in this class. If the method
		/// is loaded and returns a definitive value, this value is accepted as the
		/// current trust level. If it returns <see langword="null"/>, the decision is made
		/// based on other criteria.
		/// </param>
		/// <threadsafety>
		/// The implementation of this method must be thread safe if the methods
		/// in which it is used are to be thread-safe. The argument is held locally
		/// (as it always should be), but the internals of the delegate must otherwise
		/// withstand concurrent calls.
		/// </threadsafety>
		// ReSharper disable UnusedMember.Local
		// ReSharper disable PartialMethodWithSinglePart
		static partial void IsElevatedTrust(ref bool? isElevatedTrust);
		// ReSharper restore PartialMethodWithSinglePart
		// ReSharper restore UnusedMember.Local
		#endregion // Partial Methods
		/// <summary>
		/// Method will determine whether we are operating in an elevated-trust
		/// environment. Clients typically call this method when deciding whether
		/// to call a full-featured method that is marked with
		/// <see cref="SecurityCriticalAttribute"/> or use a safe method. A simple
		/// example is deciding whether to store a file in the file system or in
		/// isolated storage.
		/// </summary>
		/// <remarks>
		/// The static variable is checked first for a definitive trust level.
		/// Next, the delegate is called, if it is installed.
		/// Next, the partial OPTIONAL method is called, if it is loaded.
		/// If none of these checks produces a definitive answer about the
		/// trust level (non-<see langword="null"/>), <see langword="false"/>
		/// is returned. This was an arbitrary decision, but it will result
		/// in less dramatic problems if apps are fielded without having the
		/// connections made correctly in this class.
		/// </remarks>
		/// <exceptions>
		/// Lots, potentially. We can't control what the user/extender links in
		/// here and/or what exceptions to expect, so we don't catch them.
		/// Well-designed extensions will not throw exceptions or pass them
		/// unless the program should stop. If the extensions cannot provide
		/// a definate T/F, they should return <see langword="null"/>.
		/// </exceptions>
		public static bool IsElevatedTrustEnvironment()
		{
			bool? trust = null;
			if(s_IsElevatedTrustTestOverride.HasValue) return s_IsElevatedTrustTestOverride.Value;
			if(s_TrustSettingDelegate != null)
			{
				s_TrustSettingDelegate(ref trust);
				if(trust.HasValue) return trust.Value;
			}
// ReSharper disable InvocationIsSkipped
			IsElevatedTrust(ref trust);
// ReSharper restore InvocationIsSkipped
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
			if(trust.HasValue) return trust.Value;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse
			return false;
		}
		#endregion // Methods
	}
}

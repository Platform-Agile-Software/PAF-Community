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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

#region using declarations
using System;
using System.Collections.Generic;
using System.Reflection;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Notification.Exceptions;
using PlatformAgileFramework.Properties;
using PlatformAgileFramework.TypeHandling.MethodHelpers;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.TypeHandling.WeakBindings;

#region exception shorthand
using PDED = PlatformAgileFramework.Notification.Exceptions.PAFDelegateExceptionData;
using IPDED = PlatformAgileFramework.Notification.Exceptions.IPAFDelegateExceptionData;
using PDEMT = PlatformAgileFramework.Notification.Exceptions.PAFDelegateExceptionMessageTags;
#endregion // exception shorthand
#endregion // using declarations


namespace PlatformAgileFramework.TypeHandling.Delegates
{
	/// <summary>
	/// Default implementation of "weak" <see cref="IPseudoDelegate{TDelegate}"/>.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 27dec2017 </date>
	/// <description>
	/// Changed this class name and behavior to reflect the use of
	/// BOTH weak and strong references. Put the pre-built delegate
	/// <typeparamref name="TDelegate"/> right on the class.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> BMC </author>
	/// <date> 25aug2013 </date>
	/// <description>
	/// Created weak pseudodelegates.
	/// </description>
	/// </contribution>
	/// </history>
	public class WeakablePseudoDelegate<TDelegate>
		: IEquatable<WeakablePseudoDelegate<TDelegate>>, IPseudoDelegate<TDelegate>
		where TDelegate: class
	{
		#region Class Fields
		/// <summary>
		/// Backing for <see cref="DelegateMethod"/> property.
		/// </summary>
		internal readonly MethodInfo m_DelegateMethodInfo;
		/// <summary>
		/// Backing for <see cref="Target"/> property. This
		/// will be the <see cref = "Type"/> of the declaring class
		/// for a static delegate.
		/// </summary>
		internal readonly IPAFWeakableReference<object> m_WeakableDelegateTarget;
		/// <summary>
		/// Backing for <see cref="IsStatic"/> property.
		/// </summary>
		internal bool m_IsStatic;
		/// <summary>
		/// Backing for <see cref="SubscriberState"/> property.
		/// </summary>
		internal SubscriberState m_SubscriberState;
		#endregion // Class Fields
		#region Constructors
		/// <summary>
		/// Unfortunately, the CLR does not even yet allow Generics
		/// to be constrained to be delegates, so we have to do the
		/// check at load time.
		/// </summary>
		/// <exceptions>
		/// <exception cref="InvalidOperationException">
		/// "TDelegate must derive from System.MulticastDelegate."
		/// </exception>
		/// </exceptions>
		static WeakablePseudoDelegate()
		{
			if (!typeof(TDelegate).IsTypeSubclassOf(typeof(MulticastDelegate)))
				throw new InvalidOperationException("TDelegate must derive from System.MulticastDelegate.");
		}

		/// <summary>
		/// Constructor just builds a <see cref="WeakablePseudoDelegate{TDelegate}"/> from a
		/// <typeparamref name="TDelegate"/>. The <typeparamref name="TDelegate"/> must have
		/// a target on it if it is not a static delegate. 
		/// </summary>
		/// <param name="del">The delegate to pull the method off.</param>
		/// <param name="isWeak">
		/// Tells whether a weak or strong reference should be held.
		/// Default = <see langword="true"/>.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFDelegateExceptionData}">
		/// <see cref="PAFDelegateExceptionMessageTags.DELEGATE_HAS_SUBSCRIBERS"/>
		/// A delegate must be formed by capturing a method and target. In this case, the
		/// delegate will only have one member on its invocation list.
		/// </exception>
		/// <exception cref="ArgumentNullException">"del"</exception>
		/// <exception cref="ArgumentNullException">"target"</exception>
		/// </exceptions>
		public WeakablePseudoDelegate([NotNull] TDelegate del, bool isWeak = true)
		{
			if (del == null) throw new ArgumentNullException(nameof(del));
			var mcd = del as MulticastDelegate;
			// ReSharper disable once PossibleNullReferenceException
			//// Always a MCD in our case, ReSharper.
			var invocationList = mcd.GetInvocationList();
			if ((invocationList != null) && (invocationList.Length > 1))
			{
				var data = new PDED(typeof(TDelegate).ToTypeholder());
				throw new PAFStandardException<IPDED>(data, PDEMT.DELEGATE_HAS_SUBSCRIBERS);
			}

			m_DelegateMethodInfo = ((Delegate)(object)del).GetMethodInfo();
			var target = ((Delegate)(object)del).Target;

			if (m_DelegateMethodInfo.IsStatic)
			{
				var staticClassType = m_DelegateMethodInfo.DeclaringType;
				m_IsStatic = true;
				m_WeakableDelegateTarget = new PAFWeakReference<object>(staticClassType, false);
				return;
			}
			if (target == null) throw new ArgumentNullException(nameof(target));
			m_WeakableDelegateTarget = new PAFWeakReference<object>(target, isWeak);
		}

		/// <summary>
		/// Constructor just builds a <see cref="WeakablePseudoDelegate{TDelegate}"/> from a
		/// named method on a type. The method may be either a static method or an
		/// instance method.
		/// </summary>
		/// <param name="target">
		/// The class or object to pull the method off. This argument
		/// can never be <see langword="null"/>. For a static method, this will
		/// be the <see cref="Type"/> of the class. For an instance method,
		/// this will be the arbitrary <see cref="object"/> instance reference.
		/// </param>
		/// <param name="methodName">
		/// Name of the method to look for - case sensitive.
		/// </param>
		/// <param name="isWeak">
		/// Tells whether a weak or strong reference should be held.
		/// Default = <see langword="true"/>. Not used for static methods.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFDelegateExceptionData}">
		/// <see cref="PAFDelegateExceptionMessageTags.NO_NAMED_METHOD_FOUND_ON_TYPE"/>
		/// When a named method cannot be found.
		/// </exception>
		/// <exception cref="ArgumentNullException">"target"</exception>
		/// <exception cref="ArgumentNullException">"methodName"</exception>
		/// </exceptions>
		/// <remarks>
		/// Note: this constructor will search up the type hierarchy and return
		/// the first named method found. It's best to target a non-virtual method
		/// on the lowest subclass to avoid confusion between overridden methods.
		/// Also best to choose a method which DOES NOT have optional parameters.
		/// If you need to do these things, build another constructor in a derived
		/// class which can use some of the facilities we provide for such things.
		/// </remarks>
		public WeakablePseudoDelegate([NotNull] object target, string methodName, bool isWeak = true)
		{
			if (target == null) throw new ArgumentNullException(nameof(target));
			Type targetType;
			var typeOfStaticTarget = target as Type;

			if (typeOfStaticTarget == null)
			{
				var typeofInstance = target.GetType();
				targetType = typeofInstance;
			}
			else
			{
				m_IsStatic = true;
				targetType = typeOfStaticTarget;
			}

			var methodInfos = (IList<MethodInfo>) targetType.GatherImplementedMethodsHierarchically().BuildCollection();

			methodInfos = methodInfos.FilterMethodsOnPI(null, !m_IsStatic);

			foreach (var methodInfo in methodInfos)
			{
				if (string.Equals(methodInfo.Name, methodName, StringComparison.Ordinal))
				{
					m_DelegateMethodInfo = methodInfo;
					break;
				}
			}

			if (m_DelegateMethodInfo.IsStatic) 
			{
				m_WeakableDelegateTarget = new PAFWeakReference<object>(targetType, false);

				return;
			}
			m_WeakableDelegateTarget = new PAFWeakReference<object>(target, isWeak);

		}
		#endregion // Constructors
		#region Properties
		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		public MethodInfo DelegateMethod
		{
			get { return m_DelegateMethodInfo; }
		}

		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		public bool IsStatic
		{
			get { return m_IsStatic; }
		}

		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		/// <remarks>
		/// Just looks at the wrapped <see cref="IPseudoDelegate{TDelegate}"/>.
		/// </remarks>
		public bool IsWeak
		{
			get
			{
				if (m_WeakableDelegateTarget == null)
					return false;
				return m_WeakableDelegateTarget.IsWeak;
			}
		}

		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		public SubscriberState State
		{
			get { return m_SubscriberState; }
			set { m_SubscriberState = value; }
		}
		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		[CanBeNull]
		public object Target
		{
			get
			{
				return m_WeakableDelegateTarget?.Target;
			}
		}

		/// <summary>
		/// <see cref="IPseudoDelegate{TDelegate}"/>
		/// </summary>
		public virtual TDelegate GetDelegate()
		{
			return this.GetDelegateIfAlive();
		}
		#endregion // Properties
		#region Methods
		/// <summary>Overridden for our customs equals.</summary>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		/// <param name="obj">The object to compare with the current object. </param>
		public override bool Equals(object obj)
		{
			return obj is WeakablePseudoDelegate<TDelegate> && Equals((WeakablePseudoDelegate<TDelegate>)obj);
		}

		/// <summary>Mixes in both method and target, if present. </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				return ((m_WeakableDelegateTarget?.Target?.GetHashCode() ?? 0) * 419) ^ (m_DelegateMethodInfo.GetHashCode());
			}
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type. Checks
		/// a bunch of stuff to see if everything is equal.
		/// </summary>
		/// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
		/// <param name="other">An object to compare with this object.</param>
		public bool Equals(WeakablePseudoDelegate<TDelegate> other)
		{
			// ReSharper disable once PossibleUnintendedReferenceComparison
			if (m_DelegateMethodInfo != other.m_DelegateMethodInfo) return false;
			if (m_WeakableDelegateTarget == null) return other.m_WeakableDelegateTarget == null;
			if (other.m_WeakableDelegateTarget == null) return false;
			var thisTarget = m_WeakableDelegateTarget.Target;
			var otherTarget = other.m_WeakableDelegateTarget.Target;
			if ((thisTarget == null) && (otherTarget != null)) return false;
			if ((thisTarget != null) && (otherTarget == null)) return false;
			if (thisTarget != otherTarget) return false;
			return true;
		}
		#endregion //Methods
	}
}

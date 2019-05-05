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

namespace PlatformAgileFramework.TypeHandling.Delegates
{
	/// <summary>
	/// Helpers for pseudodelegates.
	/// </summary>
	public static class PseudoDelegateExtensions
	{
		/// <summary>
		/// Returns a pseudodelegate (PD) from method info if the method is static
		/// or the target is still alive. This works perfectly well on weak
		/// or strong PDs. Strong PDs will never have a
		/// <see langword="null"/> target. The whole reason for this method is
		/// to provide a way to get a strong reference on something in order to
		/// count active PD's at a certain moment in time, without the need
		/// to generate full delegates, which is unneccessary.
		/// </summary>
		/// <returns>
		/// non - <see langword="null"/> if we haven't been nullified.
		/// </returns>
		/// <remarks>
		/// This method leaves a strong reference on the stack. If the caller doesn't
		/// staple it down somewhere, the PD can stiil be nullified if
		/// it's an weak instance PD.
		/// </remarks>
		public static object GetPseudoDelegateTargetIfAlive<TDelegate>(this IPseudoDelegate<TDelegate> pD)
			where TDelegate : class
		{
			return pD.Target;
		}
		/// <summary>
		/// Generates a delegate from method info if the method is static
		/// or the target is still alive. This works perfectly well on weak
		/// or strong delegates. Strong delegates will never have a
		/// <see langword="null"/> target.
		/// </summary>
		/// <returns>non - <see langword="null"/> if we can still be called.</returns>
		public static TDelegate GetDelegateIfAlive<TDelegate>(this IPseudoDelegate<TDelegate> pD)
			where TDelegate : class
		{
			if (pD.IsStatic)
				return (TDelegate)(object)pD.DelegateMethod.CreateDelegate(typeof(TDelegate));

			var target = pD.Target;

			if (target != null)
				return (TDelegate)(object)pD.DelegateMethod.CreateDelegate(typeof(TDelegate), target);
			return null;
		}
		/// <summary>
		/// Generates a delegate from method info if the method is non-static
		/// and the target is still alive. This works perfectly well on weak
		/// or strong delegates. Strong delegates will never have a
		/// <see langword="null"/> target.
		/// </summary>
		/// <returns>
		/// non - <see langword="null"/> if we are an instance method and we can still be called.
		/// </returns>
		public static TDelegate GetDelegateIfInstanceAlive<TDelegate>(this IPseudoDelegate<TDelegate> pD)
			where TDelegate : class
		{
			if (pD.IsStatic)
				return null;

			var target = pD.Target;

			if (target != null)
				return (TDelegate)(object)pD.DelegateMethod.CreateDelegate(typeof(TDelegate), target);
			return null;
		}

		/// <summary>
		/// Generates a <see cref="IPseudoDelegate{TDelegate}"/> by creating our default
		/// <see cref="WeakablePseudoDelegate{TDelegate}"/>.
		/// </summary>
		/// <param name="del">One of us.</param>
		/// <param name="isWeak"><se langword="true"/> to create a weak reference.</param>
		/// <returns>
		/// <see langword="null"/> if the incoming delegate is <see langword="null"/>
		/// </returns>
		public static IPseudoDelegate<TDelegate> GetPseudoDelegate<TDelegate>(this TDelegate del, bool isWeak = true)
			where TDelegate : class
		{
			if (del == null)
				return null;

			return new WeakablePseudoDelegate<TDelegate>(del, isWeak);
		}

	}
}

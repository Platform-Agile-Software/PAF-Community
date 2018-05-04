//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010-2011 Icucom Corporation
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
#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;

#endregion // Using directives

namespace PlatformAgileFramework.Collections.Comparers
{
	/// <summary>
	/// Comparer shell allows hooking of delegates. The characteristics of
	/// a comparer on a dictionary should generally not be changed while
	/// the dictionary is not empty. In the PAFFramework, we generally
	/// lock a dictionary, copy the elements out of it to another dictionary
	/// with the new comparer to make sure things go allright, then modify the
	/// comparer on the original dictionary after emptying it, and finally
	/// refill it. Other applications of the variable compare operation may
	/// require the same treatment.
	/// </summary>
	/// <typeparam name="T">
	/// Any Type that can be compared.
	/// </typeparam>
	public abstract class AbstractComparerShell<T> : IAllComparers<T>
	{
		#region Constructors
		/// <summary>
		/// This constructor just sets up with no hooked comparers - just calls the
		/// default overridden DefaultMainCompare.
		/// </summary>
		protected AbstractComparerShell() { }
		/// <summary>
		/// This constructor sets up with hooked comparers - any or all can be <see langword="null"/>.
		/// </summary>
		/// <param name="iPreComparer">
		/// A PreComparer or <see langword="null"/> for no pre-comparison.
		/// </param>
		/// <param name="iMainComparer">
		/// A MainComparer or <see langword="null"/> to use the DefaultMainComparer.
		/// </param>
		/// <param name="iPostComparer">
		/// A PostComparer or <see langword="null"/> for no post-comparison.
		/// </param>
		protected AbstractComparerShell(IComparer<T> iPreComparer, IComparer<T> iMainComparer, 
			IComparer<T> iPostComparer)
		{
			m_iPreComparer = iPreComparer;
			m_iMainComparer = iMainComparer;
			m_iPostComparer = iPostComparer;
		}
		#endregion // Constructors
		#region Class Fields
		/// <summary>
		/// This is the comparer that is used to pre-compare before the main comparison.
		/// </summary>
		IComparer<T> m_iPreComparer;
		/// <summary>
		/// This is the comparer that is used for the main comparison.
		/// </summary>
		IComparer<T> m_iMainComparer;
		/// <summary>
		/// This is the comparer that is used to post-compare after the main comparison.
		/// </summary>
		IComparer<T> m_iPostComparer;
		#endregion // Class Fields
		#region Properties
		/// <summary>
		/// This is the comparer that is used to pre-compare before the main comparison.
		/// </summary>
		public IComparer<T> PreComparer
		{ get { return m_iPreComparer; } set { m_iPreComparer = value; } }
		/// <summary>
		/// This is the comparer that is used for the main comparison.
		/// </summary>
		public IComparer<T> MainComparer
		{ get { return m_iMainComparer; } set { m_iMainComparer = value; } }
		/// <summary>
		/// This is the comparer that is used to post-compare after the main comparison.
		/// </summary>
		public IComparer<T> PostComparer
		{ get { return m_iPostComparer; } set { m_iPostComparer = value; } }
		#endregion // Properties
		#region Main Comparer
		/// <summary>
		/// This comparer must be overridden in a derived class. It has the same
		/// requirements as a normal compare method. The method can always return
		/// 0, in which case the client is expecting that the hooked delegates
		/// will do the work. If a delegate is hooked for MainCompare, this method
		/// will not get called.
		/// </summary>
		/// <param name="firstItem">
		/// The first object to compare.
		/// </param>
		/// <param name="secondItem">
		/// The other object to compare.
		/// </param>
		/// <returns>
		/// See <see cref="System.Collections.Generic.IComparer&lt;T&gt;"/>
		/// </returns>
		public abstract int DefaultMainCompare(T firstItem, T secondItem);
		#endregion // Main Comparer
		#region IComparable<T> Implementation
		/// <summary>
		/// This comparer implements the comparisaon method required by
		/// <see cref="IComparer{T}"/>. It
		/// optionally employs the hooked pre-compare, post-compare and main comparison
		/// operations to determine how two Generic Types compare.
		/// </summary>
		/// <param name="firstItem">
		/// The first item to compare.
		/// </param>
		/// <param name="secondItem">
		/// The other item to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <remarks>
		/// This method tries one compare method after another - Pre, Hooked main,
		/// default Main, then Post, in order to get a non-zero comparison for
		/// the result. If we never get a non-zero comparison, then we just return
		/// zero.
		/// </remarks>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public virtual int Compare(T firstItem, T secondItem)
		{
			// ReSharper disable CompareNonConstrainedGenericWithNull
			if ((!firstItem.GetType().IsTypeAValueType()) && (firstItem == null))
				throw new ArgumentNullException("firstItem");
			if ((!secondItem.GetType().IsTypeAValueType()) && (secondItem == null))
				throw new ArgumentNullException("secondItem");
			// ReSharper restore CompareNonConstrainedGenericWithNull
			int result;
			if ((m_iPreComparer != null) && ((result = m_iPreComparer.Compare(firstItem, secondItem)) != 0))
				return result;
			if ((m_iMainComparer != null) && ((result = m_iMainComparer.Compare(firstItem, secondItem)) != 0))
				return result;
			if ((result = DefaultMainCompare(firstItem, secondItem)) != 0)
				return result;
			if ((m_iPostComparer != null) && ((result = m_iPostComparer.Compare(firstItem, secondItem)) != 0))
				return result;
			return 0;
		}
		#endregion region // IComparable<T> Implementation
		#region IEqualityComparer<T> Implementation
		/// <summary>
		/// This comparer implements the comparison method required by
		/// <see cref="IEqualityComparer{T}"/>. It calls the <see cref="IComparable{T}"/>
		/// method to do it's work.
		/// </summary>
		/// <param name="firstItem">
		/// The first item to compare.
		/// </param>
		/// <param name="secondItem">
		/// The other item to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IEqualityComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public virtual bool Equals(T firstItem, T secondItem)
		{
			return Compare(firstItem, secondItem) == 0;
		}
		/// <summary>
		/// Base class just calls <see cref="GetHashCode(object)"/> on object.
		/// </summary>
		/// <param name="item">
		/// See <see cref="IEqualityComparer{T}"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IEqualityComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// <paramref name="item"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public virtual int GetHashCode(T item)
		{
			return GetHashCode((object)item);
		}

		#endregion region // IEqualityComparer<T> Implementation
		#region IComparable Implementation
		/// <summary>
		/// This comparer implements the comparison method required by
		/// <see cref="IComparer"/>. This implementation throws an exception
		/// if the types of the objects are not equal to <typeparamref name="T"/>
		/// and otherwise passes the objects into the generic method.
		/// </summary>
		/// <param name="firstObject">
		/// The first object to compare.
		/// </param>
		/// <param name="secondObject">
		/// The other object to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// either parameter is not of type <typeparamref name="T"/>.
		/// </exception>
		/// </exceptions>
		public virtual int Compare(object firstObject, object secondObject)
		{
			if (firstObject == null) throw new ArgumentNullException("firstObject");
			if (secondObject == null) throw new ArgumentNullException("secondObject");
			if (firstObject.GetType() != typeof(T))
				throw new InvalidOperationException("firstObject has bad type.");
			if (secondObject.GetType() != typeof(T))
				throw new InvalidOperationException("secondObject has bad type.");
			return Compare((T)firstObject, (T)secondObject);
		}
		#endregion region // IComparable Implementation
		#region IEqualityComparer Implementation
		/// <summary>
		/// This comparer implements the comparison method required by
		/// <see cref="IEqualityComparer"/>. It calls the <see cref="IComparable"/>
		/// method to do it's work.
		/// </summary>
		/// <param name="firstObject">
		/// The first object to compare.
		/// </param>
		/// <param name="secondObject">
		/// The other object to compare.
		/// </param>
		/// <returns>
		/// See <see cref="IComparer{T}"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// either parameter is <see langword="null"/>.
		/// </exception>
		/// <exception> <see cref="InvalidOperationException"/> is thrown if
		/// either parameter is not of type <typeparamref name="T"/>.
		/// </exception>
		/// </exceptions>
		bool IEqualityComparer.Equals(object firstObject, object secondObject)
		{
			return Compare(firstObject, secondObject) == 0;
		}
		/// <summary>
		/// Base class just calls <see cref="object.GetHashCode()"/> on object.
		/// </summary>
		/// <param name="obj">
		/// See <see cref="IEqualityComparer"/>.
		/// </param>
		/// <returns>
		/// See <see cref="IEqualityComparer"/>
		/// </returns>
		/// <exceptions>
		/// <exception> <see cref="ArgumentNullException"/> is thrown if
		/// <paramref name="obj"/> is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public virtual int GetHashCode(object obj)
		{
			if (obj == null) throw new ArgumentNullException("obj");
			return obj.GetHashCode();
		}
		#endregion region // IEqualityComparer Implementation
	}

}

//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2019 Icucom Corporation
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

// Exception shorthand.

using System;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.TypeHandling;

// ReSharper disable IdentifierTypo
using PAFTMED = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionData;
using IPAFTMED = PlatformAgileFramework.TypeHandling.Exceptions.IPAFTypeMismatchExceptionData;
using PAFTypeMismatchExceptionMessageTags = PlatformAgileFramework.TypeHandling.Exceptions.PAFTypeMismatchExceptionMessageTags;
// ReSharper restore IdentifierTypo

namespace PlatformAgileFramework.MultiProcessing.AsyncControl
{
	/// <summary>
	///	Class providing a method with the same signature as
	/// <see cref="System.Threading.WaitCallback"/> wrapping a contravariant
	/// delegate with a single input arg and no output args.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 31mar2019 </date>
	/// <contribution>
	/// New. Built to use a thread controller. Original motivation was
	/// for concurrent testing. 
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Thread-safe only if <see cref="ContravariantThreadMethod"/> is. The method
	/// exposed by this class is normally called by a single thread.
	/// </threadsafety>
	//public class ACOThreadMethodDelegator<T>
	//where T : class
	//{
	//	#region Class Fields and Autoproperties
	//	/// <summary>
	//	/// Holds the delegate we have been built with.
	//	/// </summary>
	//	protected Action<IAsyncGenericPayloadControlObject<T>> ContravariantThreadMethod
	//	{ get; set; }
	//	/// <summary>
	//	/// Holds a copy of the argument.
	//	/// </summary>
	//	protected IAsyncGenericPayloadControlObject<T> ThreadMethodArgument
	//	{ get; set; }
	//	#endregion // Class Fields and Autoproperties
	//	#region Constructors
	//	/// <summary>
	//	/// Builds with a delegate and argument.
	//	/// </summary>
	//	/// <param name="contravariantThreadMethod">
	//	/// The delegate.
	//	/// </param>
	//	/// <param name="threadMethodArgument">
	//	/// The argument, which wraps the Generic in a controller.
	//	/// </param>
	//	public ACOThreadMethodDelegator(
	//		Action<IAsyncGenericPayloadControlObject<T>> contravariantThreadMethod,
	//		IAsyncGenericPayloadControlObject<T> threadMethodArgument)
	//	{
	//		ContravariantThreadMethod = contravariantThreadMethod;
	//		ThreadMethodArgument = threadMethodArgument;
	//	}
	//	#endregion // Constructors
	//	#region Methods
	//	/// <summary>
	//	/// Method that is used to call the <see cref="ContravariantThreadMethod"/>.
	//	/// </summary>
	//	/// <param name="obj">
	//	/// Payload which must be a <typeparamref name="T"/> or <see langword="null"/>.
	//	/// If <see langword ="null"/>, the <see cref="ContravariantThreadMethod"/>
	//	/// is used as the argument.
	//	/// </param>
	//	/// <exception cref="PAFStandardException{IPAFTMED}">
	//	/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>.
	//	/// The incoming <paramref name="obj"/> must be castable to
	//	/// <see cref="IAsyncGenericPayloadControlObject{T}"/>.
	//	/// </exception>
	//	public virtual void ThreadMethod(object obj)
	//	{
	//		if (obj == null) obj = ThreadMethodArgument;
	//		if (!(obj is IAsyncGenericPayloadControlObject<T>))
	//		{
	//			var data =
	//				new PAFTMED(PAFTypeHolder.IHolder(obj.GetType()),
	//					PAFTypeHolder.IHolder(typeof(IAsyncGenericPayloadControlObject<T>)));
	//			throw new PAFStandardException<IPAFTMED>
	//				(data, PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE);
	//		}
	//		ContravariantThreadMethod((IAsyncGenericPayloadControlObject<T>)obj);
	//	}
	//	#endregion // Methods
	//}
}
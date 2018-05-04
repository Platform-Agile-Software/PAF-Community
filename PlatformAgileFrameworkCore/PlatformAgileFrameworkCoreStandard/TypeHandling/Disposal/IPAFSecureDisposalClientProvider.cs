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
using PlatformAgileFramework.Security;

namespace PlatformAgileFramework.TypeHandling.Disposal
{
	/// <summary>
	/// Interface supporting the PAF dispose pattern. Useful for
	/// class composition. Just aggregates two other interfaces.
	/// </summary>
	/// <history>
	/// <author> KRM </author>
	/// <date> 07mar2012 </date>
	/// <contribution>
	/// Built for the <see cref="DisposalRegistry"/>.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Not needed in implementations - only a get from a class that is normally immutable.
	/// </threadsafety>
	public interface IPAFSecureDisposalClientProvider: IPAFDisposalClientProvider, IPAFSecretKey
	{
	}
}


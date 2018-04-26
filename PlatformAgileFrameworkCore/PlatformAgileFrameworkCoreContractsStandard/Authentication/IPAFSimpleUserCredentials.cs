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


namespace PlatformAgileFramework.Authentication
{
	/// <summary>
	/// This is a small subset of the stuff on the entity framework model. It's
	/// generic, but we do keep the name <see cref="PasswordHash"/> so we don't
	/// have to make any translations.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> JWM </author>
	/// <date> 04jan2015 </date>
	/// <description>
	/// New. Made consistent with EF back-end.
	/// </description>
	/// </contribution>
	/// </history>
	public interface IPAFSimpleUserCredentials
	{
		/// <summary>
		/// Unique ID for automatic key generation in EF. For us, it's always
		/// forced to be the same as the <see cref="UserName"/>.
		/// </summary>
		string Id { get; set; }

		/// <summary>
		/// Unique user name.
		/// </summary>
		string UserName { get; set; }

		/// <summary>
		/// Appropriately hashed PW.
		/// </summary>
		string PasswordHash { get; set; }
	}
}
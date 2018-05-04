//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2005 - 2010 - 2016 Icucom Corporation
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

namespace PlatformAgileFramework.Platform
{
	/// <summary>
	/// This enumeration describes the platforms supported by a given method
	/// or assembly, etc.. It is a flags enum since multiple platforms can be
	/// supported by a given item. <see cref="long.MaxValue"/> indicates 
	/// non-platform-specific item.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 09jan2012 </date>
	/// <description>
	/// <para>
	/// Added history - original author unknown, probably Golea project....
	/// </para>
	/// <para>
	/// Added the "Custom" flag so we could escape the Enum for the extensibility model.
	/// We can't bag the Enum because there is too much legacy code (ours and clients)
	/// that relies on it. We do not want extenders to have to open up the source code
	/// and add more bits to the Enum.
	/// </para>
	/// <para>
	/// Added SL4 and eliminated other SL versions.
	/// </para>
	/// </description>
	/// </contribution>
	/// </history>
	/// <remarks>
	/// Noted we don't use anything from PlatformID, since it is not platform-independent.
	/// Silly, Huh? Also, we never want to support Win32s or anything lower
	/// than NT. We use a 64-bit integer. By the time we run out of bits, hopefully
	/// everybody working on the project will be dead or retired :-)
	/// </remarks>
	[Flags]
	public enum CLRPlatformsSupported : long
	{
		/// <summary>
		/// Indicates that this item is suitable for a CUSTOM platform. In this case, this
		/// enumeration is not used.
		/// </summary>
		Custom = 0,
		/// <summary>
		/// Indicates that this item is suitable for all platforms.
		/// </summary>
		AllPlatforms = long.MaxValue,
		/// <summary>
		/// Good old Microsoft Windows NT. Server and XP are same for now.
		/// Leave room for divergence later.....
		/// </summary>
		// (KRM) Prob'ly want to do Server-specific stuff the usual way in config.
		// files.
		MSWNT = 1,
		/// <summary>
		/// Microsoft CE.
		/// </summary>
		// We might do something with this if MS ever gets decent RT support
		// of more than managed c++. Look at Java RTS, Microsoft!!
		MSWCE = 4,
		/// <summary>
		/// For our courses.
		/// </summary>
		Rotor = 64,
		/// <summary>
		/// Microsoft running Mono.
		/// </summary>
		// We don't have any apps running the Mono CLI on Windows (we are not crazy),
		// but it sure is nice to have it there for preliminary testing of Mono stuff!!
		// We have finally settled the deep philosophical question: Is Mono's
		// port to MSWindows a "Windows" category? We decided no....
		MSMono = 128,
		/// <summary>
		/// Microsoft versions - 8 bits. Anything running on a MS OS.
		/// </summary>
		MicrosoftMask = 255,
		/// <summary>
		/// Microsoft Windows versions - 5 bits.
		/// </summary>
		MicrosoftWindowsMask = 31,
		/// <summary>
		/// Linux running Mono. Just dealing with WS for now.
		/// </summary>
		LinuxMono = 256,
		/// <summary>
		/// BSD's compilation/augmentation of Rotor.
		/// </summary>
		// (For Clem).
		BSDRotor = 512,
		/// <summary>
		/// OS 10 on Mac.
		/// </summary>
		MacOSX = 1024,
		/// <summary>
		/// Unix versions (including Linux, et. al.) - 10 bits.
		/// </summary>
		UnixMask = 1023 << 8,
		/// <summary>
		/// Unix versions except Mac.
		/// </summary>
		UnixNotMac = UnixMask & (~MacOSX),
		/// <summary>
		/// No more Silverlight versions less than 5.
		/// </summary>
		MSSilverlight4 = 2048
	}
}

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

#region Using Directives
using System;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.TypeExtensionMethods;
using PlatformAgileFramework.ErrorAndException.CoreCustomExceptions;
using PlatformAgileFramework.TypeHandling.Exceptions;

// Exception shorthand.
using PAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.PAFTypeExceptionData;
using IPAFTED = PlatformAgileFramework.ErrorAndException.CoreCustomExceptions.IPAFTypeExceptionData;


#endregion // Using Directives


namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// This class is an implementation of the <see cref="IPAFServiceDescription{T}"/>
	/// interface. Implements <see cref="IPAFNamedAndTypedObject{T}"/> so we can handle
	/// it our dictionaries.
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 12apr2015 </date>
	/// <description>
	/// Added support for the (new) internal interface.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 20aug2015 </date>
	/// <description>
	/// Touched up the arg to the exception just a bit so it would compile. This
	/// should not have been necessary.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> JAW(P) </author>
	/// <date> 09mar2015 </date>
	/// <description>
	/// Put in check for type compliance with the Generic. Build a partial method
	/// to call into stuff in extended.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 21feb2012 </date>
	/// <description>
	/// New - part of new service description.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// not safe.
	/// </threadsafety>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	// ReSharper disable once PartialTypeWithSinglePart
		// core
	public partial class PAFServiceDescription<T> : PAFServiceDescription,
		IPAFServiceDescriptionInternal<T> where T : class, IPAFService
	{
		#region Constructors
		/// <summary>
		/// Constructor throws an exception if <typeparamref name="T"/> is not an
		/// interface type.
		/// </summary>
		static PAFServiceDescription()
		{
			if (typeof(T).IsTypeAnInterfaceType()) return;
			var data = new PAFTED(PAFTypeHolder.IHolder(typeof(T)));
			throw new PAFStandardException<IPAFTED>(data, PAFTypeExceptionMessageTags.TYPE_NOT_AN_INTERFACE_TYPE);
		}
		/// <summary>
		/// For the serializer.
		/// </summary>
		protected internal PAFServiceDescription(){}

		/// <summary>
		/// This constructor loads props. but does not set service object. This is the
		/// legacy constructor.
		/// </summary>
		/// <param name="serviceInterfaceType">
		/// Loads <see cref="IPAFServiceDescription.ServiceInterfaceType"/>.
		/// Can be <see langword="null"/>.
		/// </param>
		/// <param name="serviceImplementationType">
		/// Loads <see cref="IPAFServiceDescription.ServiceImplementationType"/>.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <param name="serviceName">
		/// Loads <see cref="IPAFServiceDescription.ServiceName"/>.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <param name="isDefault">
		/// Is the service the default for its interface type? Used mostly
		/// for service collections.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="serviceInterfaceType"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}">
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// is thrown if the Generic constraint is not satisfied.
		/// </exception>
		/// No exceptions are caught.
		/// </exceptions>
		public PAFServiceDescription(IPAFTypeHolder serviceInterfaceType,
			IPAFTypeHolder serviceImplementationType = null, string serviceName = null,
			bool isDefault = false)
			: base(serviceInterfaceType, serviceImplementationType, serviceName)
		{

			m_IsDefault = isDefault;

		    serviceImplementationType?.ValidateGenericAssignableFromType<T>();
		}
		/// <summary>
		/// This constructor builds from a pre-constructed service.
		/// </summary>
		/// <param name="service">
		/// Generic service that defines everything.
		/// Can't be <see langword="null"/>.
		/// </param>
		/// <param name="serviceName">
		/// Loads <see cref="IPAFServiceDescription.ServiceName"/>.
		/// Default = <see langword="null"/>. If you don't name it and
		/// there is another service of the same type/name, an exception
		/// is thrown.
		/// </param>
		/// <param name="isDefault">
		/// Is the service the default for its interface type? Used mostly
		/// for service collections.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="service"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// No exceptions are caught.
		/// </exceptions>
		[SecuritySafeCritical]
		public PAFServiceDescription(T service, string serviceName = null,
			bool isDefault = false)
			: base(new PAFTypeHolder(typeof(T)), new PAFTypeHolder(service.GetType()), serviceName, isDefault)
		{
			Service = service;
		}
		#endregion
		#region Properties
		/// <summary>
		/// <see cref="IPAFServiceDescription{T}"/>.
		/// </summary>
		public T Service
		{
			get { return (T) ServiceObject; }
			[SecurityCritical]
			set{ServiceObject = value;}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// <see cref="IPAFServiceDescription{T}"/>.
		/// </summary>
		[SecurityCritical]
		public void SetService(T serviceObject)
		{
			ServiceObject = serviceObject;
		}
		#endregion // Methods
		#region IPAFServiceDescriptionInternal<T> Implementation
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal{T}"/>
		/// </summary>
		/// <param name="serviceObject"></param>
		void IPAFServiceDescriptionInternal<T>.SetServiceInternal(T serviceObject)
		{
			SetServiceObjectI(serviceObject);
		}
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal{T}"/>
		/// </summary>
		/// <param name="isDefault"></param>
		bool IPAFServiceDescriptionInternal.SetIsDefault(bool isDefault)
		{
			bool retval = isDefault != IsDefault;
			IsDefault = isDefault;
			return retval;
		}
		#endregion // IPAFServiceDescriptionInternal<T> Implementation
		#region IPAFNamedAndTypedObject Implementation
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public string ObjectName
		{
			get { return ServiceName; }
			[SecurityCritical]
			set { ServiceName = value; }
		}

		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public string AssemblyQualifiedObjectType
		{
			get { return ServiceInterfaceType.AssemblyQualifiedTypeName; }
			[SecurityCritical]
			set { ServiceInterfaceType.GetAssemblyHolder().AssemblyNameString = value; }
		}

		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public bool IsDefaultObject
		{
			get { return m_IsDefault; }
			[SecurityCritical]
			protected internal set { m_IsDefault = value; }
		}

		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public T ItemValue
		{
			get { return (T)ServiceObject; }
		}
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		public Type ObjectType
		{
			get { return ServiceInterfaceType.TypeType; }
			[SecurityCritical]
			set { m_ServiceInterfaceType.TypeType = value; }
		}

		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject{T}"/>.
		/// </summary>
		/// <exceptions>
		/// Value is validated in the set. See <see cref="PAFServiceDescription.ValidateServiceObject"/> for
		/// exceptions that are thrown.
		/// </exceptions>
		public object ObjectValue
		{
			get { return ServiceObject; }
			[SecurityCritical]
			set
			{
				if (value != null)
				{
					// This is a redundant call, but we want to make sure it is always done.
					ServiceObject = ValidateServiceObject(value);		
				}
			}
		}
		#endregion // IPAFNamedAndTypedObject Implementation
	}
}

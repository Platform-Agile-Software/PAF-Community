//@#$&+
//
//The MIT X11 License
//
//Copyright (c) 2010 - 2014 Icucom Corporation
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

using System;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Exceptions;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This class is an implementation of the <see cref="IPAFServiceDescription"/>.
	/// <see cref="IPAFNamedAndTypedObject"/> is mapped to the interface type for
	/// use in dictionary lookup.
	/// interface.
	/// </para>
	/// </summary>
	/// <history>
	/// <contribution>
	/// <author> KRM </author>
	/// <date> 21jan2019 </date>
	/// <description>
	/// Redirected <see cref="IPAFNamedAndTypedObject"/> to the interface
	/// for proper direct use in the dictionary.
	/// </description>
	/// </contribution>
	/// <contribution>
	/// <author> DAP </author>
	/// <date> 07jan2012 </date>
	/// <description>
	/// Rewrote the class based on type holders.
	/// </description>
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe. Secured with monitors - low traffic.
	/// </threadsafety>
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]

	// ReSharper disable once PartialTypeWithSinglePart
	public partial class PAFServiceDescription
		: IPAFServiceDescriptionInternal
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing field. Concrete type for serialization.
		/// </summary>
		protected internal PAFTypeHolder m_ServiceInterfaceType;
		/// <summary>
		/// Backing field. Concrete type for serialization.
		/// </summary>
		protected internal PAFTypeHolder m_ServiceImplementationType;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal string m_ServiceName;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal object m_ServiceObject;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal bool m_IsDefault;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// For the serializer.
		/// </summary>
		protected internal PAFServiceDescription()
		{
		}

		/// <summary>
		/// This constructor loads props.
		/// </summary>
		/// <param name="serviceInterfaceType">
		/// Loads <see cref="IPAFServiceDescription.ServiceInterfaceType"/>.
		/// Can not be <see langword="null"/>.
		/// </param>
		/// <param name="serviceImplementationType">
		/// Loads <see cref="IPAFServiceDescription.ServiceImplementationType"/>.
		/// Default = <see langword="null"/>.
		/// </param>
		/// <param name="serviceName">
		/// Loads <see cref="IPAFServiceDescription.ServiceName"/>.
		/// Default = <see langword="null"/>. Constructor turns null into blank.
		/// </param>
		/// <param name="isDefault">
		/// Allows a preset of this property. Rarely used, since this flag is normally
		/// set after the description is added to the service manager.
		/// </param>
		/// <param name="serviceObject">
		/// Service implementation instance that is validated in this constructor.
		/// If this parameter is present it overrides <paramref name="serviceImplementationType"/>
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="serviceInterfaceType"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// <exception cref="PAFStandardException{T}"> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message if the implementation type does not inherit from the interface.
		/// </exception>
		/// Additional exceptions are thrown from <see cref="ValidateServiceObject"/> if
		/// the incoming service object is not <see langword="null"/>. See that method for
		/// details.
		/// </exceptions>
		public PAFServiceDescription(IPAFTypeHolder serviceInterfaceType,
			IPAFTypeHolder serviceImplementationType = null, string serviceName = null,
			bool isDefault = false, object serviceObject = null)
		{
			if (serviceInterfaceType == null)
				throw (new ArgumentNullException(nameof(serviceInterfaceType)));

			m_ServiceInterfaceType = new PAFTypeHolder(serviceInterfaceType);
			if (serviceName == null) serviceName = "";
			m_ServiceName = serviceName;
			m_IsDefault = isDefault;

			// If we have a service object, we're all set.
			if (serviceObject != null)
				serviceImplementationType = new PAFTypeHolder(serviceObject.GetType());

			// We must ensure that the implementation type supports the interface.
			if (serviceImplementationType != null)
			{
				var exception = TypeHandlingUtils.TypeNotInheritedException(
					serviceImplementationType.TypeType, serviceInterfaceType.TypeType);
				if (exception != null) throw exception;
				m_ServiceImplementationType = new PAFTypeHolder(serviceImplementationType);
			}

			// Similarly, we must make sure any loaded implementation is valid.
			m_ServiceObject = ValidateServiceObject(serviceObject);
		}

		/// <summary>
		/// This constructor builds a description from a <see cref="IPAFNamedAndTypedObject"/>.
		/// In the cases where we don't need to specify an implementation, this is
		/// all we need. Name is transferred and the type info is transferred to the
		/// <see cref="IPAFServiceDescription.ServiceInterfaceType"/>.
		/// </summary>
		/// <param name="nto">
		/// Incoming object.
		/// </param>
		/// <exceptions>
		/// <exception cref="ArgumentNullException"> is thrown if
		/// <paramref name="nto"/>
		/// is <see langword="null"/>.
		/// </exception>
		/// </exceptions>
		public PAFServiceDescription(IPAFNamedAndTypedObject nto)
			: this(PAFTypeHolder.IHolder(nto.ObjectType), null, nto.ObjectName, nto.IsDefaultObject)
		{
			if (nto == null)
				throw (new ArgumentNullException(nameof(nto)));
			ServiceObject = nto.ObjectValue;
		}
		#endregion // Constructors
		#region Construction Helpers
		/// <summary>
		/// Purpose of this method is to throw an exception if an attempt is made
		/// to set a serviceObject (a type implementing the service that does not actually
		/// implement the service interface.) If the implementation type is set within
		/// the service description, an exception is thrown if the object type is not
		/// an exact match to that type. If the implementation type is <see langword="null"/>,
		/// this method will create it from the type of the object.
		/// </summary>
		/// <param name="serviceObject">
		/// Can be <see langword="null"/> when the serive is disposed (for example). That does
		/// not trigger the exception.
		/// </param>
		/// <exceptions>
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}"> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message is thrown if the object does not implement
		/// <see cref="IPAFServiceDescription.ServiceImplementationType.TypeType"/>.
		/// <see cref="IPAFServiceDescription.ServiceInterfaceType.TypeType"/>
		/// </exception">
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}"> with
		/// <see cref="PAFTypeMismatchExceptionMessageTags.TYPES_NOT_AN_EXACT_MATCH"/>
		/// message is thrown if the object does not exactly match the
		/// <see cref="IPAFServiceDescription.ServiceImplementationType.TypeType"/>.
		/// if it is here.
		/// </exception">
		/// </exceptions>
		protected internal object ValidateServiceObject(object serviceObject)
		{
			if (serviceObject == null) return null;
			var exception = TypeHandlingUtils.ObjectNotInheritedException(serviceObject,
				ServiceInterfaceType.TypeType);
			if (exception != null) throw exception;

			if (ServiceImplementationType == null)
			{
				ServiceImplementationType = new PAFTypeHolder(serviceObject.GetType());
				return serviceObject;
			}

			exception = TypeHandlingUtils.TypeMismatchException(serviceObject.GetType(),
				ServiceImplementationType.TypeType);
			if (exception != null) throw exception;
			return serviceObject;
		}
		#endregion // Construction Helpers
		#region Properties
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public bool IsDefault
		{
			get { return m_IsDefault; }
			protected internal set { m_IsDefault = value; }
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public IPAFTypeHolder ServiceInterfaceType
		{
			get
			{ return m_ServiceInterfaceType; }
			[SecurityCritical]
			set
			{
				if (value == null)
					throw (new ArgumentNullException(nameof(value)));

				m_ServiceInterfaceType = new PAFTypeHolder(value);
			}
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public IPAFTypeHolder ServiceImplementationType
		{
			get
			{
				return m_ServiceImplementationType;
			}
			[SecurityCritical]
			set
			{
				if (value == null)
				{
					m_ServiceImplementationType = null;
					return;
				}
				m_ServiceImplementationType
					= new PAFTypeHolder(value);
			}
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public string ServiceName
		{
			get
			{
				return m_ServiceName;
			}
			[SecurityCritical]
			set
			{
				m_ServiceName = value ?? string.Empty;
			}
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public object ServiceObject
		{
			get
			{
				return m_ServiceObject;
			}
			[SecurityCritical]
			set
			{
				SetServiceObjectI(value);
			}
		}
		#endregion // Properties
		#region Methods
		/// <summary>
		/// Helper to create one of us from a <see cref="IPAFNamedAndTypedObject"/>.
		/// </summary>
		/// <param name="ntod">The object to create from.</param>
		/// <returns>
		/// A service description without any implementation information.
		/// </returns>
		public static PAFServiceDescription GetDescriptionHelper(IPAFNamedAndTypedObject ntod)
		{
			return new PAFServiceDescription(
				PAFTypeHolder.IHolder(ntod.ObjectType), null, ntod.ObjectName, ntod.IsDefaultObject);
		}
		/// <summary>
		/// Little helper, appreciated by all.
		/// </summary>
		/// <param name="serviceObject">Sets the non-Generic.</param>
		internal void SetServiceObjectI(object serviceObject)
		{
			m_ServiceObject = ValidateServiceObject(serviceObject);
		}
		#endregion // Methods
		/////////////////////////////////////////////////////////////////////////////////////
		// This implementation of the interface sets fields directly, since it is used
		// for serialization. Extenders can develop extension methods for the interface
		// if additional functionality is needed.
		/////////////////////////////////////////////////////////////////////////////////////
		#region IPAFServiceDescriptionInternal Implementation
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </summary>
		/// <param name="isDefault">
		///     See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		bool IPAFServiceDescriptionInternal.SetIsDefault(bool isDefault)
		{
			var retval = isDefault != m_IsDefault;
			m_IsDefault = isDefault;
			return retval;
		}
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </summary>
		/// <param name="typeHolder">
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		/// <remarks>
		/// Since this implementation is serializable, we create an actual concrete
		/// container.
		/// </remarks>
		void IPAFServiceDescriptionInternal.SetServiceInterfaceType(IPAFTypeHolder typeHolder)
		{
			m_ServiceInterfaceType = new PAFTypeHolder(typeHolder);
		}
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </summary>
		/// <param name="typeHolder">
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		/// <remarks>
		/// Since this implementation is serializable, we create an actual concrete
		/// container.
		/// </remarks>
		void IPAFServiceDescriptionInternal.SetServiceImplementationType(IPAFTypeHolder typeHolder)
		{
			m_ServiceImplementationType = new PAFTypeHolder(typeHolder);
		}
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </summary>
		/// <param name="serviceName">
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		void IPAFServiceDescriptionInternal.SetServiceName(string serviceName)
		{
			m_ServiceName = serviceName;
		}
		/// <summary>
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </summary>
		/// <param name="obj">
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		void IPAFServiceDescriptionInternal.SetServiceObject(object obj)
		{
			m_ServiceObject = obj;
		}
		#endregion // IPAFServiceDescriptionInternal Implementation
		#region IPAFNamedAndTypedObject Implementation
		#region IPAFNamedObject Implementation
		/// <summary>
		/// <see cref="IPAFNamedObject"/> - set is <see cref="SecurityCriticalAttribute"/>
		/// </summary>
		string IPAFNamedObject.ObjectName
		{
			get { return ServiceName; }
			[SecurityCritical]
			set { ServiceName = value; }
		}
		/// <summary>
		/// <see cref="IPAFNamedObject"/> - set is <see cref="SecurityCriticalAttribute"/>
		/// </summary>
		object IPAFNamedObject.ObjectValue
		{
			get { return ServiceObject; }
			[SecurityCritical]
			set { ServiceObject = value; }
		}
		/// <summary>
		/// <see cref="IPAFNamedObject"/> - set is <see cref="SecurityCriticalAttribute"/>
		/// </summary>
		bool IPAFNamedObject.IsDefaultObject
		{
			get { return IsDefault; }
		}
		#endregion //IPAFNamedObject Implementation
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject"/> - set throws exception.
		/// </summary>
		string IPAFNamedAndTypedObject.AssemblyQualifiedObjectType
		{
			get { return ServiceInterfaceType.AssemblyQualifiedTypeName; }
			[SecurityCritical]
			set { throw new NotImplementedException(); }
		}
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject"/> - set throws exception.
		/// </summary>
		Type IPAFNamedAndTypedObject.ObjectType
		{
			get { return ServiceInterfaceType?.TypeType; }
			[SecurityCritical]
			set { throw new NotImplementedException(); }
		}
		#endregion //IPAFNamedAndTypedObject Implementation
	}
}

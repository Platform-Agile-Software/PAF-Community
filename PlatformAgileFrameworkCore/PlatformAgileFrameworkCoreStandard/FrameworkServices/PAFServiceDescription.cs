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
//FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
//THE SOFTWARE.
//@#$&-

using System;
using System.Security;
using PlatformAgileFramework.Collections;
using PlatformAgileFramework.ErrorAndException;
using PlatformAgileFramework.Notification.Exceptions;
using PlatformAgileFramework.Serializing;
using PlatformAgileFramework.Serializing.Attributes;
using PlatformAgileFramework.TypeHandling;
using PlatformAgileFramework.TypeHandling.Exceptions;

namespace PlatformAgileFramework.FrameworkServices
{
	/// <summary>
	/// <para>
	/// This class is an implementation of the <see cref="IPAFServiceDescription"/>
	/// interface.
	/// </para>
	/// </summary>
	/// <history>
	/// <author> DAP </author>
	/// <date> 07jan2012 </date>
	/// <contribution>
	/// Rewrote the class based on type holders.
	/// </contribution>
	/// </history>
	/// <threadsafety>
	/// Safe. Secured with monitors - low traffic.
	/// </threadsafety>
	//TODO - KRM make copy constructor for this class that accepts the interface.
	//TODO - also implement deepcopy - this should have been done when the class was touched.
	[PAFSerializable(PAFSerializationType.PAFSurrogate)]
	// ReSharper disable once PartialTypeWithSinglePart
	public partial class PAFServiceDescription : IPAFServiceDescriptionInternal
	{
		#region Class Fields and Autoproperties
		/// <summary>
		/// Backing field. Concrete type for serialization.
		/// </summary>
		protected internal PAFTypeHolder m_ServiceInterfaceType;
		/// <summary>
		/// Lock for the interface.
		/// </summary>
		private object m_ServiceInterfaceType_Lock;
		/// <summary>
		/// Backing field. Concrete type for serialization.
		/// </summary>
		protected internal PAFTypeHolder m_ServiceImplementationType;
		/// <summary>
		/// Lock for the implementation.
		/// </summary>
		private object m_ServiceImplementationType_Lock;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal string m_ServiceName;
		/// <summary>
		/// Lock for the name.
		/// </summary>
		private object m_ServiceName_Lock;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal object m_ServiceObject;
		/// <summary>
		/// Lock for the service object.
		/// </summary>
		private object m_ServiceObject_Lock;
		/// <summary>
		/// Backing field.
		/// </summary>
		protected internal bool m_IsDefault;
		/// <summary>
		/// Lock for the default flag.
		/// </summary>
		private object m_IsDefault_Lock;
		#endregion // Class Fields and Autoproperties
		#region Constructors
		/// <summary>
		/// For the serializer.
		/// </summary>
		protected internal PAFServiceDescription()
		{
			InitializePAFServiceDescription();
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
		/// Additional exceptios are thrown from <see cref="ValidateServiceObject"/> if
		/// the incoming service object is not <see langword="null"/>. See that method for
		/// details.
		/// </exceptions>
		public PAFServiceDescription(IPAFTypeHolder serviceInterfaceType,
			IPAFTypeHolder serviceImplementationType = null, string serviceName = null,
			bool isDefault = false, object serviceObject = null)
		{
			if (serviceInterfaceType == null)
				throw (new ArgumentNullException(nameof(serviceInterfaceType)));

			InitializePAFServiceDescription();

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
			InitializePAFServiceDescription();
			ServiceObject = nto.ObjectValue;
		}
		#endregion // Constructors
		#region Construction Helpers
		/// <summary>
		/// Initializes basic stuff in the class. Can be called multiple times.
		/// </summary>
		protected internal void InitializePAFServiceDescription()
		{
			if (m_ServiceInterfaceType_Lock == null)
				m_ServiceInterfaceType_Lock = new object();
			if (m_ServiceImplementationType_Lock == null)
				m_ServiceImplementationType_Lock = new object();
			if (m_ServiceName_Lock == null)
				m_ServiceName_Lock = new object();
			if (m_IsDefault_Lock == null)
				m_IsDefault_Lock = new object();
			if (m_ServiceObject_Lock == null)
				m_ServiceObject_Lock = new object();
		}
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
		/// <see cref="PAFTypeMismatchExceptionDataBase.FIRST_TYPE_NOT_CASTABLE_TO_SECOND_TYPE"/>
		/// message is thrown if the object does not implement
		/// <see cref="IPAFServiceDescription.ServiceImplementationType.TypeType"/>.
		/// <see cref="IPAFServiceDescription.ServiceInterfaceType.TypeType"/>
		/// </exception">
		/// <exception cref="PAFStandardException{IPAFTypeMismatchExceptionData}"> with
		/// <see cref="PAFTypeMismatchExceptionDataBase.TYPES_NOT_AN_EXACT_MATCH"/>
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
			get
			{
				lock (m_IsDefault_Lock) { return m_IsDefault; }
			}
			[SecurityCritical]
			set { lock (m_IsDefault_Lock) { m_IsDefault = value; } }
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public IPAFTypeHolder ServiceInterfaceType
		{
			get
			{
				lock (m_ServiceInterfaceType_Lock)
				{
					return m_ServiceInterfaceType;
				}
			}
			[SecurityCritical]
			set
			{
				lock (m_ServiceInterfaceType_Lock)
				{
					if (value == null)
						throw (new ArgumentNullException("value"));

					m_ServiceInterfaceType = new PAFTypeHolder(value);
				}
			}
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public IPAFTypeHolder ServiceImplementationType
		{
			get
			{
				lock (m_ServiceImplementationType_Lock)
				{
					return m_ServiceImplementationType;
				}
			}
			[SecurityCritical]
			set
			{
				lock (m_ServiceImplementationType_Lock)
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
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public string ServiceName
		{
			get
			{
				lock (m_ServiceName_Lock)
				{
					return m_ServiceName;
				}
			}
			[SecurityCritical]
			set
			{
				lock (m_ServiceName_Lock)
				{
					m_ServiceName = value ?? string.Empty;
				}
			}
		}
		/// <summary>
		/// <see cref="IPAFServiceDescription"/>.
		/// </summary>
		public object ServiceObject
		{
			get
			{
				lock (m_ServiceObject_Lock)
				{
					return m_ServiceObject;
				}
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
			lock (m_ServiceObject_Lock)
			{
				m_ServiceObject = ValidateServiceObject(serviceObject);
			}
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
		/// See <see cref="IPAFServiceDescriptionInternal"/>.
		/// </param>
		void IPAFServiceDescriptionInternal.SetIsDefault(bool isDefault)
		{
			m_IsDefault = isDefault;
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
			[SecurityCritical]
			set { IsDefault = value; }
		}
		#endregion //IPAFNamedObject Implementation
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject"/> - set throws exception.
		/// </summary>
		string IPAFNamedAndTypedObject.AssemblyQualifiedObjectType
		{
			get { return ServiceImplementationType.AssemblyQualifiedTypeName; }
			[SecurityCritical]
			set { throw new NotImplementedException(); }
		}
		/// <summary>
		/// <see cref="IPAFNamedAndTypedObject"/> - set throws exception.
		/// </summary>
		Type IPAFNamedAndTypedObject.ObjectType
		{
			get { return ServiceImplementationType.TypeType; }
			[SecurityCritical]
			set { throw new NotImplementedException(); }
		}
		#endregion //IPAFNamedAndTypedObject Implementation
	}
}

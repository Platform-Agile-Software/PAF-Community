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

using System.Collections.Generic;
using System.Security;
using PlatformAgileFramework.Collections.ExtensionMethods;
using PlatformAgileFramework.Execution.Pipeline;
using PlatformAgileFramework.Platform;
using PlatformAgileFramework.XML;
using PlatformAgileFramework.XML.Linq;

namespace PlatformAgileFramework.FileAndIO.SymbolicDirectories
{
	/// <summary>
	/// This is the default implementation of <see cref="ISymbolicDirectoryMappingDictionary"/>.
	/// It provides an instance dictionary and a static dictionary. Instance dictionary is checked
	/// first (if the class is instantiated), then a static dictionary is checked. This
	/// allows the framework to save emergency files to drive letter mapped file paths
	/// before the full framework has been set up. This is useful for emergency log
	/// files and the like.
	/// </summary>
	/// <threadsafety>
	/// safe - uses monitor locks.
	/// </threadsafety>
	/// <history>
	/// <description>
	/// <author> KRM </author>
	/// <date> 10apr2016 </date>
	/// <contribution>
	/// Just added some DOCs down in remarks. This facility has been around since
	/// .Net 1.1 in various forms.
	/// </contribution>
	/// </description>
	/// </history>
	/// <remarks>
	/// The instance piece of this was developed for/with a customer who wanted to overload
	/// mappings on the fly. We don't use it in core. The interface allows it to be
	/// used as a service within the service manager.
	/// </remarks>
	public class SymbolicDirectoryMappingDictionary : ISymbolicDirectoryMappingDictionaryInternal
	{
		#region Class Fields and AutoProperties
		/// <summary>
		/// The dictionary containing the mappings.
		/// </summary>
		private static readonly IDictionary<string, string> s_DirectoryMappingDictionary;

		/// <summary>
		/// The dictionary containing the mappings. This is the instance version,
		/// which can augment and/or override the static version. Access is thread-safe
		/// if access rules (below) are followed.
		/// </summary>
		private readonly IDictionary<string, string> m_DirectoryMappingDictionary;
		#endregion // Class Fields and AutoProperties

		#region Constructors
		/// <summary>
		/// Static constructor just builds with hardwired constants that are set
		/// in <see cref="PlatformUtils"/>.
		/// </summary>
		static SymbolicDirectoryMappingDictionary()
		{
			s_DirectoryMappingDictionary = new Dictionary<string, string>
			{
				{"c", PlatformUtils.s_C_DriveMapping},
				{"C", PlatformUtils.s_C_DriveMapping},
				{"d", PlatformUtils.s_D_DriveMapping},
				{"D", PlatformUtils.s_D_DriveMapping}
			};
		}
		/// <summary>
		/// Default constructor just creates the instance dictionary.
		/// </summary>
		public SymbolicDirectoryMappingDictionary()
		{
			m_DirectoryMappingDictionary = new Dictionary<string, string>();
		}
		/// <summary>
		/// Constructor builds with a pre-loaded dictionary.
		/// </summary>
		public SymbolicDirectoryMappingDictionary(IDictionary<string, string> directoryMappingDictionary = null)
		{
			m_DirectoryMappingDictionary = directoryMappingDictionary ?? new Dictionary<string, string>();
		}
		#endregion // Constructors
		#region Methods
		/// <summary>
		/// Adds a mapping to the static mapping dictionary.
		/// </summary>
		/// <param name="token">
		/// The token for the drive or other string key.
		/// </param>
		/// <param name="directory">
		/// The directory that this token should be replaced with in
		/// file/directory operations.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if another thread snuck in before us and
		/// added the mapping we are trying to add.
		/// </returns>
		[SecurityCritical]
		public static bool AddStaticMapping(string token, string directory)
		{
			return AddStaticMappingInternal(token, directory);
		}
		/// <summary>
		/// Adds a mapping to the static mapping dictionary.
		/// </summary>
		/// <param name="token">
		/// The token for the drive or other string key.
		/// </param>
		/// <param name="directory">
		/// The directory that this token should be replaced with in
		/// file/directory operations.
		/// </param>
		/// <returns>
		/// <see langword="false"/> if another thread snuck in before us and
		/// </returns>
		internal static bool AddStaticMappingInternal(string token, string directory)
		{
			lock (s_DirectoryMappingDictionary)
			{
				if (s_DirectoryMappingDictionary.ContainsKey(token)) return false;
				s_DirectoryMappingDictionary.Add(token, directory);
				return true;
			}
		}
		/// <summary>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </summary>
		/// <param name="token">
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </param>
		/// <param name="directory">
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </param>
		/// <returns>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </returns>
		[SecurityCritical]
		public bool AddMapping(string token, string directory)
		{
			return AddMappingPIV(token, directory);
		}
        /// <summary>
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </summary>
        /// <param name="token">
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </param>
        /// <param name="directory">
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </param>
        /// <returns>
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </returns>
        bool ISymbolicDirectoryMappingDictionaryInternal.AddMappingInternal(string token, string directory)
        {
            return AddMappingPIV(token, directory); 
        }
		/// <summary>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </summary>
		/// <param name="token">
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
        /// </param>
		/// <returns>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </returns>
		[SecurityCritical]
		public static string GetStaticMapping(string token)
		{
			return GetStaticMappingInternal(token);
		}

		/// <summary>
		/// Gets a mapping from the static mapping dictionary.
		/// </summary>
		/// <param name="token">The token for the drive or other string key.</param>
		/// <returns>
		/// The mapping if found or <see langword="null"/>.
		/// </returns>
		internal static string GetStaticMappingInternal(string token)
		{
			lock (s_DirectoryMappingDictionary) {
				if (!s_DirectoryMappingDictionary.ContainsKey(token)) return null;
				return s_DirectoryMappingDictionary[token];
			}
		}
		/// <summary>
		/// Gets a mapping from the mapping dictionaries. Instance mapping dictionary
		/// is checked first, followed by the static dictionary.
		/// </summary>
		/// <param name="token">
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
        /// </param>
		/// <returns>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </returns>
		[SecurityCritical]
		public virtual string GetMapping(string token)
		{
			return GetMappingPIV(token);
		}
        /// <summary>
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// Gets a mapping from the mapping dictionaries. Instance mapping dictionary
        /// is checked first, followed by the static dictionary.
        /// </summary>
        /// <param name="token">
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </param>
        /// <returns>
        /// <see cref="ISymbolicDirectoryMappingDictionaryInternal"/>
        /// </returns>
        string ISymbolicDirectoryMappingDictionaryInternal.GetMappingInternal(string token)
        {
            return GetMappingPIV(token);
        }

		/// <summary>
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </summary>
		/// <param name="filePath">
        /// <see cref="ISymbolicDirectoryMappingDictionary"/>
		/// </param>
		/// <remarks>
		/// For privileged callers. This method needs to be called AFTER core
        /// services are loaded, since it uses the file system.
		/// </remarks>
		[SecurityCritical]
		public void PopulateStaticDictionaryFromXML(string filePath)
		{
			PopulateStaticDictionaryFromXMLPIV(filePath);
		}

        void ISymbolicDirectoryMappingDictionaryInternal.PopulateStaticDictionaryFromXMLInternal(string filePath)
        {
            PopulateStaticDictionaryFromXMLPIV(filePath);
        }
        #region PIV methods
        //////////////////////////////////////////////////////////////
        // This is where the work gets done.
        //////////////////////////////////////////////////////////////
        /// <remarks>
        /// Virtual implementation for extenders.
        /// </remarks>
        protected internal virtual bool AddMappingPIV(string token, string directory)
        {
            lock (m_DirectoryMappingDictionary)
            {
                if (m_DirectoryMappingDictionary.ContainsKey(token)) return false;
                m_DirectoryMappingDictionary.Add(token, directory);
                return true;
            }
        }
        /// <remarks>
        /// Virtual implementation for extenders.
        /// </remarks>
        protected internal virtual string GetMappingPIV(string token)
        {
            lock (m_DirectoryMappingDictionary)
            {
                if (m_DirectoryMappingDictionary.ContainsKey(token))
                    return m_DirectoryMappingDictionary[token];
            }
            return GetStaticMappingInternal(token);
        }
        /// <remarks>
        /// Virtual implementation for extenders.
        /// </remarks>
        protected internal virtual void PopulateStaticDictionaryFromXMLPIV(string filePath)
        {
            var xmlParameters = new XMLExaminerParams { XMLInputFilePath = filePath };
            var pipelineParams = new PAFPipelineParams<IXMLExaminerParams>(xmlParameters);
            var examiner = new XMLExaminer();
            examiner.InitializeExePipeline(pipelineParams);
            var xElement = examiner.ReadXMLDocumentLinq();
            xElement = xElement.MovePastRoot();
            var mappingsNode = xElement.NamedChildElement(SymbolicDirectoryMappingConstants.MAPPING_SECTION_ELEMENT_NAME);
            var mappings = mappingsNode.NamedChildElements(SymbolicDirectoryMappingConstants.MAPPING_ELEMENT_NAME);
            var entries = mappings.KeyValuePairsFromElements(SymbolicDirectoryMappingConstants.DIRECTORY_SYMBOL_ATTRIBUTE_NAME,
                SymbolicDirectoryMappingConstants.PHYSICAL_DIRECTORY_ATTRIBUTE_NAME);
            var dictionary = entries.BuildDictionaryIfKeysUnique();
            // TODO - krm need an exception here.

            // Now just simply push them in.
            foreach (var entry in dictionary)
            {
                AddStaticMappingInternal(entry.Key, entry.Value);
            }

        }
        #endregion // PIV methods
		#endregion // Methods
		}
	}

////////////////////////////////////////////////////////
/// File: FileHelper.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Helpers
{
    #region Using Statements
    
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.IO;

    #endregion

    /// <summary>
    /// Singleton Helper class for file operations. 
    /// </summary>
    public class FileHelper
    {
        #region Private Static Members

        /// <summary>
        /// Stores the static instance of this class
        /// </summary>
        private static FileHelper _instance = new FileHelper();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        private FileHelper()
        {
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the static instance of this class
        /// </summary>
        public static FileHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Tests if a file exists. Will not throw an exception.
        /// </summary>
        /// <param name="filePath">File to test existence of</param>
        /// <returns>True if file exists, otherwise false</returns>
        public bool FileExists(string filePath)
        {
            bool exists = false;

            try
            {
                exists = File.Exists(filePath);
            }
            catch { }

            return exists;
        }

        #endregion
    }
}

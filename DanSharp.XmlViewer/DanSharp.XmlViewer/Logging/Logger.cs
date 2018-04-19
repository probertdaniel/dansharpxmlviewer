////////////////////////////////////////////////////////
/// File: Logger.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Logging
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    #endregion

    /// <summary>
    /// Singleton logging helper used to log errors/information for this application.
    /// </summary>
    public class Logger
    {
        #region Private Static Members

        /// <summary>
        /// Stores the static instance of this class
        /// </summary>
        private static Logger _instance = new Logger();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the static instance of this class
        /// </summary>
        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Writes a log entry
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="logType">Type of log event</param>
        public void Write(string message, LogType logType)
        {
            // Needs to be updated to use kernel level logging or log4net.
            // Using Debug.WriteLine() for now (won't work in release versions)
            Debug.WriteLine(string.Format("{0}: {1}", logType, message));
        }

        #endregion
    }

    #region LogType Enum

    /// <summary>
    /// Enum of log events
    /// </summary>
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    #endregion
}

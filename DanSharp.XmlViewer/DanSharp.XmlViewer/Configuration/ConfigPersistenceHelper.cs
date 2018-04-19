////////////////////////////////////////////////////////
/// File: ConfigPersistenceHelper.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Configuration
{
    #region Using Statements

    using System;
    using System.Globalization;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Security;
    using DanSharp.XmlViewer.Logging;

    #endregion

    /// <summary>
    /// Helper class for persisting Config to disk
    /// </summary>
    public class ConfigPersistenceHelper
    {
        #region Private Instance Fields

        /// <summary>
        /// Contains the current config to persist
        /// </summary>
        private Config _settings = null;

        /// <summary>
        /// Flag indicating if SettingsChanged events should be ignored
        /// </summary>
        private bool _ignoreSettingsChangedEvents = false;

        /// <summary>
        /// Name of the file to persist config to
        /// </summary>
        private const string FileName = "XmlViewer_settings.xml";

        /// <summary>
        /// IsolatedStorage file to save config to
        /// </summary>
        private IsolatedStorageFile _storageFile = null;

        #endregion

        #region Private Static Members

        /// <summary>
        /// Persistence helper used to persist config
        /// </summary>
        private static ConfigPersistenceHelper _instance = new ConfigPersistenceHelper();

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public ConfigPersistenceHelper()
        {
            try
            {
                _storageFile = IsolatedStorageFile.GetUserStoreForAssembly();
            }
            catch (SecurityException sex)
            {
                // Unable to get a FileStore
                // Log an exception
                Logger.Instance.Write(string.Format(CultureInfo.CurrentUICulture, "Unable to get an IsolatedFileStore for the XmlViewer for the current user {0}/{1} because of this error: {2}", Environment.UserDomainName, Environment.UserName, sex.ToString()), LogType.Error);

                _storageFile = null;
                throw;
            }
        }

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the instance of this class
        /// </summary>
        public static ConfigPersistenceHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the settings for the TestApp
        /// </summary>
        public Config Settings
        {
            get
            {
                if (_settings == null)
                {
                    LoadSettings();
                }
                return _settings;
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Loads in the Config settings from disk
        /// </summary>
        private void LoadSettings()
        {
            // Unhook events for any existing settings object
            if (_settings != null)
            {
                _settings.SettingsChanged -= new EventHandler(_settings_settingsChanged);
            }
           
            string serialisedSettingsData = null;
            IsolatedStorageFileStream fs;
            StreamReader sr;
            _settings = new Config();

            // Check if we have a storage file
            if (_storageFile == null)
            {
                    return;
            }

            lock (this)
            {
                try
                {
                    // Check if the settings file exists
                    if (_storageFile.GetFileNames(FileName).Length > 0)
                    {
                        // Create a new FileStream to read the data
                        fs = new IsolatedStorageFileStream(FileName, FileMode.OpenOrCreate, FileAccess.Read, _storageFile);

                        sr = new StreamReader(fs);
                        serialisedSettingsData = sr.ReadToEnd();

                        // Close the streams
                        sr.Close();
                        fs.Close();

                        // Deserialise the settings
                        if ((serialisedSettingsData != null) && (serialisedSettingsData.Length > 0))
                        {
                            _settings = new ConfigFactory().Deserialise(serialisedSettingsData);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Unable to read from the file store
                    // Log an exception
                    Logger.Instance.Write(string.Format(CultureInfo.CurrentUICulture, "Unable to read from an IsolatedFileStore for the XmlViewer settings for the current user {0}/{1} because of this error: {2}", Environment.UserDomainName, Environment.UserName, ex.ToString()), LogType.Error);
                }
            }

            _settings.SetupEventHandlers();

            // Hookup events for this object
            _settings.SettingsChanged += new EventHandler(_settings_settingsChanged);
        }

        /// <summary>
        /// Saves the Config settings to disk
        /// </summary>
        private void SaveSettings()
        {
            string serialisedSettingsData = null;
            IsolatedStorageFileStream fs;
            StreamWriter sw;

            // Check if we have a storage file
            if (_storageFile == null)
            {
                return;
            }

            lock (this)
            {
                try
                {
                    // Serialise the settings
                    serialisedSettingsData = new ConfigFactory().SerialiseObject(_settings);

                    // Create a new FileStream to write the settings data
                    fs = new IsolatedStorageFileStream(FileName, FileMode.Create, FileAccess.Write, _storageFile);

                    sw = new StreamWriter(fs);

                    // Write the data
                    sw.Write(serialisedSettingsData);

                    // Flush and close the streams
                    sw.Flush();
                    sw.Close();
                    fs.Close();
                }
                catch (Exception ex)
                {
                    // Unable to write to the file store
                    // Log an exception
                    Logger.Instance.Write(string.Format(CultureInfo.CurrentUICulture, "Unable to write to an IsolatedFileStore for the XmlViewer settings for the current user {0}/{1} because of this error: {2}", Environment.UserDomainName, Environment.UserName, ex.ToString()), LogType.Error);
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles event raised when settings are changed
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void _settings_settingsChanged(object sender, EventArgs e)
        {
            if (_ignoreSettingsChangedEvents) return;

            // Save the settings class
            SaveSettings();
        }

        #endregion
    }
}

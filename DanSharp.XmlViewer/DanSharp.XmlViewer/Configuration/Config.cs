////////////////////////////////////////////////////////
/// File: Config.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Configuration
{
    #region Using Statements

    using System;
    using System.Xml.Serialization;
    using System.Collections.Generic;

    #endregion

    /// <summary>
    /// Class used for serialising/deserialising application settings
    /// </summary>
    [Serializable]
    public class Config
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the maximum number of history items that we store
        /// </summary>
        private const int HistoryLength = 10;

        /// <summary>
        /// Stores the history of xml files we have opened
        /// </summary>
        private FileHistoryItemCollection _xmlFiles = new FileHistoryItemCollection();

        /// <summary>
        /// Stores the hostory of xsd files we have opened
        /// </summary>
        private FileHistoryItemCollection _xsdFiles = new FileHistoryItemCollection();

        /// <summary>
        /// Stores the current XPath query
        /// </summary>
        private string _currentXPathQuery = null;

        /// <summary>
        /// Flag that indicates if we should ignore the SettingsChanged event
        /// </summary>
        private bool _ignoreSettingsChangedEvent = false;

        #endregion

        #region Public Events

        /// <summary>
        /// Event raised when one of the settings in this class is changed
        /// </summary>
        public event EventHandler SettingsChanged;

        #endregion

        #region Constructors
        /// <summary>
        /// Default constructor
        /// </summary>
        public Config()
        {
            SetupEventHandlers();
        }
        #endregion

        #region Public Properties

        /// <summary>
        /// Gets/Sets the collection of Xml files
        /// </summary>
        public FileHistoryItemCollection XmlFiles
        {
            get
            {
                return _xmlFiles;
            }
            set
            {
                _xmlFiles = value;
            }
        }

        /// <summary>
        /// Gets/Sets the collection of Xsd file
        /// </summary>
        public FileHistoryItemCollection XsdFiles
        {
            get
            {
                return _xsdFiles;
            }
            set
            {
                _xsdFiles = value;
            }
        }

        /// <summary>
        /// Gets/Sets the current XPath query
        /// </summary>
        public string CurrentXPathQuery
        {
            get
            {
                return _currentXPathQuery;
            }
            set
            {
                if (string.Compare(value, _currentXPathQuery, true) != 0)
                {
                    _currentXPathQuery = value;
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// Gets the first Xml file in the history
        /// </summary>
        [XmlIgnore]
        public FileHistoryItem FirstXmlFile
        {
            get
            {
                if (_xmlFiles.Count == 0)
                {
                    return null;
                }

                return _xmlFiles[0];
            }
        }

        /// <summary>
        /// Gets the first Xsd file in the history
        /// </summary>
        public FileHistoryItem FirstXsdFile
        {
            get
            {
                if (_xsdFiles.Count == 0)
                {
                    return null;
                }

                return _xsdFiles[0];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets up event handlers for the collections
        /// </summary>
        public void SetupEventHandlers()
        {
            _xmlFiles.SettingsChanged += new EventHandler(Files_SettingsChanged);
            _xsdFiles.SettingsChanged += new EventHandler(Files_SettingsChanged);
        }

        /// <summary>
        /// Clears all settings
        /// </summary>
        public void Reset()
        {
            _xmlFiles.Clear();
            _xsdFiles.Clear();
        }
        #endregion

        #region Private Methods

        /// <summary>
        /// Raises the SettingsChanged event
        /// </summary>
        private void OnSettingsChanged()
        {
            if (_ignoreSettingsChangedEvent) return;

            // Raise the SettingsChanged event
            if (SettingsChanged != null)
            {
                    SettingsChanged(this, new EventArgs());
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the event raised when a collection is modified
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        void Files_SettingsChanged(object sender, EventArgs e)
        {
            OnSettingsChanged();
        }

        #endregion
    }
}

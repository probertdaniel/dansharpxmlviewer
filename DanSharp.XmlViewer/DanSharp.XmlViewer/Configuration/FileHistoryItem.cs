////////////////////////////////////////////////////////
/// File: FileHistoryItem.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Configuration
{
    #region Using Statements

    using System;
    using System.Globalization;

    #endregion

    /// <summary>
    /// Class used to represent the location of an XmlFile
    /// </summary>
    [Serializable]
    public class FileHistoryItem
    {
        #region Private Instance Fields

        /// <summary>
        /// FilePath that we're storing
        /// </summary>
        private string _filePath = null;

        /// <summary>
        /// Display name for the file path
        /// </summary>
        private string _displayName = null;

        /// <summary>
        /// Max length of the display name
        /// </summary>
        private const int MaxDisplayFileName = 80;

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
        /// Creates an instance of the class with no file path
        /// </summary>
        public FileHistoryItem()
        {
        }

        /// <summary>
        /// Creates an instance of the class using the given file path
        /// </summary>
        /// <param name="filePath">Path of the history item</param>
        public FileHistoryItem(string filePath)
        {
            _filePath = filePath;
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Gets/Sets the location of the file
        /// </summary>
        public string FilePath
        {
            get
            {
                return _filePath;
            }
            set
            {
                if (string.Compare(_filePath, value, true) != 0)
                {
                    _filePath = value;
                    OnSettingsChanged();
                }
            }
        }

        /// <summary>
        /// Gets the display name of the file
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (_displayName == null)
                {
                    try
                    {
                        // Check if the total length is greater than the max length
                        if (_filePath.Length > MaxDisplayFileName)
                        {
                            // Convert the string to an arry, splitting at a backspace
                            string[] parts = _filePath.Split('\\');

                            // Check if the last (or only) part is greater than the max length
                            if (parts[parts.Length - 1].Length > MaxDisplayFileName)
                            {
                                string value = parts[parts.Length - 1];
                                _displayName = string.Format("{0}...{1}", value.Substring(0, (MaxDisplayFileName / 2) - 3), value.Substring(value.Length - (MaxDisplayFileName / 2)));
                            }
                            else
                            {
                                string value;
                                if (parts.Length > 2)
                                {
                                    value = parts[parts.Length - 2] + "\\" + parts[parts.Length - 1];
                                }
                                else
                                {
                                    value = parts[parts.Length - 1];
                                }

                                if (value.Length > (MaxDisplayFileName - 4))
                                {
                                    _displayName = string.Format("{0}...{1}", value.Substring(0, (MaxDisplayFileName / 2) - 3), value.Substring(value.Length - (MaxDisplayFileName / 2)));
                                }
                                else
                                {
                                    int firstPartLength = MaxDisplayFileName - value.Length - 4;
                                    _displayName = string.Format("{0}...\\{1}", _filePath.Substring(0, firstPartLength), value);
                                }

                            }
                        }
                        else
                        {
                            _displayName = _filePath;
                        }
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format(CultureInfo.CurrentUICulture, "Error occurred retrieving FileHistoryItem with path {0}: {1}", _filePath, ex.Message));
                    }
                }

                return _displayName;
            }
        }
        #endregion

        #region Public Overridden Methods

        /// <summary>
        /// Gets a string representation of this instance
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return DisplayName;
        }

        /// <summary>
        /// Checks if this instance is equal to another instance
        /// </summary>
        /// <param name="obj">Instance to compare to</param>
        /// <returns>-1 if less than, 0 if equal, 1 if greater than</returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || (obj is System.DBNull))
            {
                return false;
            }

            FileHistoryItem item = (FileHistoryItem)obj;
            if (string.Compare(item.FilePath, _filePath) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets a hashcode for this instance
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
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
    }
}

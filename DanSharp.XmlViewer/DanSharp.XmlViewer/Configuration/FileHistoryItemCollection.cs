////////////////////////////////////////////////////////
/// File: FileHistoryItemCollection.cs
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
    /// Represents a collection of FileHistory items
    /// </summary>
    [Serializable]
    public class FileHistoryItemCollection
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the maximum number of history items that we store
        /// </summary>
        private const int MaxNumberOfItems = 10;

        /// <summary>
        /// List of items in this collection
        /// </summary>
        List<FileHistoryItem> _files = new List<FileHistoryItem>();

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
        public FileHistoryItemCollection()
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the count of items in the collection
        /// </summary>
        public int Count
        {
            get
            {
                return _files.Count;
            }
        }

        /// <summary>
        /// Gets the item at the specified index
        /// </summary>
        /// <param name="index">Index to get item at</param>
        /// <returns>FileHistoryItem at this index</returns>
        public FileHistoryItem this[int index]
        {
            get
            {
                return _files[index];
            }

            set
            {
                if (index > (_files.Count - 1))
                {
                    throw new ArgumentOutOfRangeException(string.Format("Can't add an element at index {0} as there are only {1} elements in the collection", index, _files.Count));

                }
                _files[index] = value;
            }
        }

        /// <summary>
        /// Gets the collection of FileHistory items
        /// </summary>
        public List<FileHistoryItem> Files
        {
            get
            {
                return _files;
            }

            set
            {
                _files = value;
            }
        }

        /// <summary>
        /// Gets the first item in the collection
        /// </summary>
        [XmlIgnore]
        public FileHistoryItem FirstFile
        {
            get
            {
                if (_files.Count == 0)
                {
                    return null;
                }
                return _files[0];
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Clears the collection
        /// </summary>
        public void Clear()
        {
            _files.Clear();
            OnSettingsChanged();
        }

        /// <summary>
        /// Resets the collection
        /// </summary>
        public void Reset()
        {
            Clear();
        }

        /// <summary>
        /// Removes the given item from the collection
        /// </summary>
        /// <param name="item">Item to remove</param>
        public void Remove(FileHistoryItem item)
        {
            _files.Remove(item);
            OnSettingsChanged();
        }

        /// <summary>
        /// Adds an item to the collection
        /// </summary>
        /// <param name="filePath">Item to add</param>
        public void AddItem(string filePath)
        {

            FileHistoryItem item = new FileHistoryItem(filePath);
            if (!Exists(item))
            {
                if (_files.Count >= MaxNumberOfItems)
                {
                    _files.RemoveAt(0);
                }

                _files.Add(item);
                OnSettingsChanged();
            }
        }

        /// <summary>
        /// Checks if an item exists in the collection
        /// </summary>
        /// <param name="item">Item to check for</param>
        /// <returns>True if the item exists</returns>
        public bool Exists(FileHistoryItem item)
        {
            return (IndexOf(item) > -1);
        }

        /// <summary>
        /// Gets the index of an item in the collection
        /// </summary>
        /// <param name="item">Item to get the index of</param>
        /// <returns>Index of the item, or -1 if the item doesn't exist</returns>
        public int IndexOf(FileHistoryItem item)
        {
            int returnValue = -1;
            for (int index = 0; index < _files.Count; index++)
            {
                if ((_files[index] != null) && (_files[index].Equals(item)))
                {
                    returnValue = index;
                    break;
                }
            }

            return returnValue;
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

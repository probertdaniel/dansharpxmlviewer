////////////////////////////////////////////////////////
/// File: XPathQuery.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Text;

    #endregion

    /// <summary>
    /// Representation of an xpath query
    /// </summary>
    public class XPathQuery
    {
        #region Private Instance Fields

        /// <summary>
        /// Staores the name of the query
        /// </summary>
        private string _name = null;

        /// <summary>
        /// Stores the text of the query
        /// </summary>
        private string _query = null;

        /// <summary>
        /// Stores the value of the query
        /// </summary>
        private string _value = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the XPathQuery class using default values
        /// </summary>
        public XPathQuery()
        {
        }

        /// <summary>
        /// Creates an instance of the XPathQuery class initialized using using specified values
        /// </summary>
        /// <param name="name">Name to initialize with</param>
        /// <param name="query">Query to initialize with</param>
        /// <param name="value">Value to initialize with</param>
        public XPathQuery(string name, string query, string value)
        {
            _name = name;
            _query = query;
            _value = value;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the name of the query
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        /// <summary>
        /// Gets or sets the text of the query
        /// </summary>
        public string Query
        {
            get
            {
                return _query;
            }
            set
            {
                _query = value;
            }
        }

        /// <summary>
        /// Gets or sets the value of the query
        /// </summary>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        #endregion
    }
}

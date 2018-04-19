////////////////////////////////////////////////////////
/// File: XsdValidationResult.cs
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
    /// Represents the results of an Xsd validation
    /// </summary>
    public class XsdValidationResult
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the current state of the valiation
        /// </summary>
        private ValidationState _state = ValidationState.Success;

        /// <summary>
        /// Stores the results of the validation
        /// </summary>
        private StringBuilder _results = new StringBuilder();

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the state of the validation
        /// </summary>
        public ValidationState State
        {
            get
            {
                return _state;
            }
            set
            {
                _state = value;
            }
        }

        /// <summary>
        /// Gets the results of the validation.
        /// results are returned as a StringBuilder, which each line
        /// in the builder being a separate validation result, pre-formatted for display.
        /// </summary>
        public StringBuilder Results
        {
            get
            {
                return _results;
            }
        }

        #endregion
    }

    #region ValidationState Enum

    /// <summary>
    /// Enum of validation states
    /// </summary>
    public enum ValidationState
    {
        Success,
        Warning,
        ValidationError,
        OtherError
    }

    #endregion
}

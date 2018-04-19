////////////////////////////////////////////////////////
/// File: XsdValidationHelper.cs
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
    using System.Xml;
    using System.Xml.Schema;
    using System.IO;

    #endregion

    /// <summary>
    /// Singleton Helper class for Xsd operations. 
    /// </summary>
    public class XsdValidationHelper
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the result of an Xsd validation
        /// </summary>
        private XsdValidationResult _result = null;

        #endregion

        #region Private Static Members

        /// <summary>
        /// Stores the static instance of this class
        /// </summary>
        private static XsdValidationHelper _instance = new XsdValidationHelper();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the static instance of this class
        /// </summary>
        public static XsdValidationHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Validates an xml file against an xsd file
        /// </summary>
        /// <param name="xsdFilePath">Path to the xsd file</param>
        /// <param name="xmlFilePath">Path to the xml file</param>
        /// <returns>Results of the validation</returns>
        public XsdValidationResult ValidateInstance(string xsdFilePath, string xmlFilePath)
        {
            // Sanity check parameters
            if (string.IsNullOrEmpty(xsdFilePath))
            {
                throw new ArgumentNullException("xsdFilePath", "An Xsd File Path must be given");
            }

            if (string.IsNullOrEmpty(xmlFilePath))
            {
                throw new ArgumentNullException("xmlFilePath", "An Xml File Path must be given");
            }

            // Check existence of Xml File
            if (!FileHelper.Instance.FileExists(xmlFilePath))
            {
                throw new ArgumentException(string.Format("The Xml file '{0}' dooes not exist. Please specify a valid file.", xmlFilePath));
            }

            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(xmlFilePath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Supplied instance file is not valid XML: " + ex.Message, ex);
            }

            // Call the overload
            return ValidateInstance(xsdFilePath, doc);
        }

        /// <summary>
        /// Validates an xml document against an xsd file
        /// </summary>
        /// <param name="xsdFilePath">Path to the xsd file</param>
        /// <param name="xmlFilePath">Xml document to be validated</param>
        /// <returns>Results of the validation</returns>
        public XsdValidationResult ValidateInstance(string xsdFilePath, XmlDocument instanceDoc)
        {
            // Sanity check parameters
            if (string.IsNullOrEmpty(xsdFilePath))
            {
                throw new ArgumentNullException("xsdFilePath", "An Xsd File Path must be given");
            }
            if (instanceDoc == null)
            {
                throw new ArgumentNullException("instanceDoc", "A valid instance XmlDocument must be supplied");
            }

            // Check existence of Xsd File
            if (!FileHelper.Instance.FileExists(xsdFilePath))
            {
                throw new ArgumentException(string.Format("The Xsd file '{0}' dooes not exist. Please specify a valid file.", xsdFilePath));
            }

            // Load in the schema to a stream
            StreamReader xsdReader = null;
            FileInfo fi = new FileInfo(xsdFilePath);

            // Change the current diretcory to the schema location
            // in case there are relative schema locations (for import/include)
            string currentDirectory = Environment.CurrentDirectory;
            Environment.CurrentDirectory = fi.DirectoryName;

            // Load in the schema
            xsdReader = new StreamReader(xsdFilePath);
            XmlTextReader trSchema = new XmlTextReader(xsdReader.BaseStream);

            // Call the overload
            XsdValidationResult result = ValidateInstance(trSchema, instanceDoc);

            // Reset the current directory
            Environment.CurrentDirectory = currentDirectory;

            return result;
        }

        /// <summary>
        /// Validates an xml document against an xsd file
        /// </summary>
        /// <param name="xsdFilePath">XmlTextReader containing an xsd file</param>
        /// <param name="xmlFilePath">Xml document to be validated</param>
        /// <returns>Results of the validation</returns>
        public XsdValidationResult ValidateInstance(XmlTextReader reader, XmlDocument instanceDoc)
        {
            _result = new XsdValidationResult();

            try
            {
                // Create an XmlReaderSettings object and load the schema into it.
                // This will also load in any imported/included schemas.
                XmlReaderSettings settings = new XmlReaderSettings();
                settings.ValidationType = ValidationType.Schema;
                settings.ValidationFlags = XmlSchemaValidationFlags.ProcessSchemaLocation;
                settings.Schemas.Add(null, reader);

                // Setup the event handler to handle errors/warnings
                settings.ValidationEventHandler += new ValidationEventHandler(settings_ValidationEventHandler);

                // Get the list of all targetNamespaces from the loaded schemas
                List<string> namespaces = new List<string>();
                foreach (XmlSchema s in settings.Schemas.Schemas())
                {
                    if (!string.IsNullOrEmpty(s.TargetNamespace))
                    {
                        namespaces.Add(s.TargetNamespace);
                    }
                }

                // Load the current Xml Document into a stream
                MemoryStream stream = new MemoryStream();
                instanceDoc.Save(stream);
                stream.Position = 0L;

                // Create the validating reader
                XmlReader vr = XmlReader.Create(stream, settings);

                // Read the document stream - this will validate as it reads
                while (vr.Read()) { }

                // Check if we successfully validated the document
                if (_result.State == ValidationState.Success)
                {
                    _result.Results.AppendLine("No validation errors found.");

                    // Check if the document namespace matches any of the schema namespaces
                    if (!namespaces.Contains(instanceDoc.DocumentElement.NamespaceURI))
                    {
                        // Store the schema namespaces in a single string
                        StringBuilder schemaNSs = new StringBuilder();
                        if (namespaces.Count == 0)
                        {
                            schemaNSs.Append("\t");
                            schemaNSs.AppendLine("(None)");
                        }
                        else
                        {
                            for (int nsIndex = 0; nsIndex < namespaces.Count; nsIndex++)
                            {
                                schemaNSs.Append("\t");
                                schemaNSs.AppendLine(namespaces[nsIndex]);
                            }
                        }

                        // Add a warning that namespaces don't match
                        _result.Results.AppendLine();
                        _result.Results.AppendLine("Warning: The namespace of the current document does not match any of the target namespaces for the loaded schemas.");
                        _result.Results.AppendLine("Have you specified the correct schema for the current document?");
                        _result.Results.AppendLine();
                        _result.Results.AppendLine("Document Namespace:");
                        _result.Results.Append("\t");
                        _result.Results.AppendLine(instanceDoc.DocumentElement.NamespaceURI);
                        _result.Results.AppendLine();
                        _result.Results.AppendLine("Schema Target Namespace(s):");
                        _result.Results.AppendLine(schemaNSs.ToString());
                        _result.State = ValidationState.Warning;
                    }
                }
            }
            catch (Exception ex)
            {
                _result.Results.AppendLine("An error occurred: " + ex.Message);
                _result.State = ValidationState.OtherError;
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
            }

            return _result;
        }

        #endregion

        #region Private Event Handlers

        /// <summary>
        /// Handles the event raised when a validation error is found
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void settings_ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            _result.Results.Append("Validation error: ");
            _result.Results.AppendLine(e.Message);
            _result.Results.AppendLine("-----------------------------------------------------------");
            _result.State = ValidationState.ValidationError;
        }

        #endregion
    }
}

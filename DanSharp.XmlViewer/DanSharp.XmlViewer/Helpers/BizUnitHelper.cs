////////////////////////////////////////////////////////
/// File: BizUnitHelper.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer.Helpers
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;
    using System.Xml;
    using System.IO;
    using DanSharp.XmlViewer;

    #endregion

    /// <summary>
    /// Singleton Helper class for generating BizUnit test cases. 
    /// Generates test cases for v3.x of BizUnit.
    /// </summary>
    public class BizUnitHelper
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the index of the xpath
        /// </summary>
        private int _xpathIndex = 0;

        #endregion

        #region Private Static Members

        /// <summary>
        /// Stores the static instance of this class
        /// </summary>
        private static BizUnitHelper _instance = new BizUnitHelper();

        #endregion

        #region Public Static Properties

        /// <summary>
        /// Gets the static instance of this class
        /// </summary>
        public static BizUnitHelper Instance
        {
            get
            {
                return _instance;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Generates a test case and saves it
        /// </summary>
        /// <param name="inputFile">Path to input file to generate test case from</param>
        /// <param name="outputFile">Path to output file name to write to</param>
        public void GenerateTestCaseAndSave(string inputFile, string outputFile)
        {
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(inputFile);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Supplied input file is not valid Xml: " + ex.Message, ex);
            }

            // Check if we need to convert the input file name from
            // a short name to a long name
            if (inputFile.Contains("~"))
            {
                FileInfo fi = new FileInfo(inputFile);
                inputFile = fi.FullName;
            }

            // Check if we need to generate the output file name
            if (string.IsNullOrEmpty(outputFile))
            {
                FileInfo fi = new FileInfo(inputFile);
                fi = new FileInfo(fi.FullName);
                outputFile = fi.DirectoryName + "\\TestCaseFor_" + fi.Name;
            }

            StreamReader rd = GenerateTestCase(inputFile, doc, null);
            try
            {
                StreamWriter wr = new StreamWriter(outputFile, false);
                wr.Write(rd.ReadToEnd());
                wr.Close();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred trying to save the output file: " + ex.Message, ex);
            }

            rd.Close();
        }

        /// <summary>
        /// Generates a test case
        /// </summary>
        /// <param name="inputFile">Path to input file to generate test case from</param>
        /// <param name="doc">XmlDocument containing data to generate test case from</param>
        /// <param name="schemaPath">Path to schema file</param>
        /// <returns>A StreamReader containing the test case</returns>
        public StreamReader GenerateTestCase(string inputFile, XmlDocument doc, string schemaPath)
        {
            ViewerNode rootNode = new ViewerNode(doc.DocumentElement);

            // Call the method overload
            return GenerateTestCase(inputFile, rootNode, schemaPath);
        }

        /// <summary>
        /// Generates a test case
        /// </summary>
        /// <param name="inputFile">Path to input file to generate test case from</param>
        /// <param name="rootNode">RootNode to generate test case data from</param>
        /// <param name="schemaPath">Path to schema file</param>
        /// <returns>A StreamReader containing the test case</returns>
        public StreamReader GenerateTestCase(string fileName, ViewerNode rootNode, string schemaPath)
        {
            try
            {
                _xpathIndex = 0;
                FileInfo currentFile = new FileInfo(fileName);
                MemoryStream stream = new MemoryStream();
                XmlTextWriter xWriter = new XmlTextWriter(stream, Encoding.UTF8);
                xWriter.Formatting = Formatting.Indented;

                string testName = currentFile.Name.Replace(" ", "_");
                if (testName.ToLower().EndsWith(".xml"))
                {
                    testName = testName.Substring(0, testName.Length - 4);
                }

                testName += "_Test";

                // Write the XML Header
                xWriter.WriteStartDocument();
                // Write version/date and url comment
                xWriter.WriteComment(string.Format("Generated with DanSharp XmlViewer v{0}.{1}.{2} on {3} {4}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor, Assembly.GetEntryAssembly().GetName().Version.Build, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString()));
                xWriter.WriteComment("http://dansharpxmlviewer.codeplex.com");
                // Write original filename comment
                xWriter.WriteComment(string.Format("Original Xml file: {0}", fileName));
                // Open the TestCase element
                xWriter.WriteStartElement("TestCase");
                // Write attributes for TestCase
                xWriter.WriteAttributeString("testName", testName);
                // Write an empty TestSetup element
                xWriter.WriteStartElement("TestSetup");
                xWriter.WriteEndElement();
                // Open the TestExecution element
                xWriter.WriteStartElement("TestExecution");
                xWriter.WriteComment("Start: Test " + testName);
                // Open the TestStep element for file copying
                xWriter.WriteStartElement("TestStep");
                // Write attributes for TestStep
                xWriter.WriteAttributeString("assemblyPath", "");
                xWriter.WriteAttributeString("typeName", "Microsoft.Services.BizTalkApplicationFramework.BizUnit.FileCreateStep");
                // Open the SourcePath element
                xWriter.WriteStartElement("SourcePath");
                // Write the current file path as the source file
                xWriter.WriteValue(fileName);
                // Close the SourcePath element
                xWriter.WriteEndElement();
                // Open the CreationPath element
                xWriter.WriteStartElement("CreationPath");
                // Write the current file path (with "Copy_" at the front of th file name) as the creation file
                xWriter.WriteValue(currentFile.DirectoryName + "\\Copy_" + currentFile.Name);
                // Close the CreationPath element
                xWriter.WriteEndElement();
                // Close the TestStep element for file copying
                xWriter.WriteEndElement();

                // Open the TestStep element for validation
                xWriter.WriteStartElement("TestStep");
                // Write attributes for TestStep
                xWriter.WriteAttributeString("assemblyPath", "");
                xWriter.WriteAttributeString("typeName", "Microsoft.Services.BizTalkApplicationFramework.BizUnit.FileValidateStep");
                // Open the Timeout element
                xWriter.WriteStartElement("Timeout");
                // Write 10000 as the value
                xWriter.WriteValue("10000");
                // Close the Timeout element
                xWriter.WriteEndElement();
                // Open the Directory element
                xWriter.WriteStartElement("Directory");
                // Write the current file path
                xWriter.WriteValue(currentFile.DirectoryName);
                // Close the Directory element
                xWriter.WriteEndElement();
                // Open the SearchPattern element
                xWriter.WriteStartElement("SearchPattern");
                // Write "Copy_*.xml" as the value
                xWriter.WriteValue("Out_Copy_" + currentFile.Name);
                // Close the SearchPattern element
                xWriter.WriteEndElement();
                // Open the DeleteFile element
                xWriter.WriteStartElement("DeleteFile");
                // Write true as the value
                xWriter.WriteValue("true");
                // Close the DeleteFile element
                xWriter.WriteEndElement();
                // Open the ValidationStep element
                xWriter.WriteStartElement("ValidationStep");
                // Write attributes for ValidationStep
                xWriter.WriteAttributeString("assemblyPath", "");
                xWriter.WriteAttributeString("typeName", "Microsoft.Services.BizTalkApplicationFramework.BizUnit.XmlValidationStepEx");
                // Open the XmlSchemaPath element
                xWriter.WriteStartElement("XmlSchemaPath");
                // Write a schema name as the value
                if (string.IsNullOrEmpty(schemaPath))
                {
                    schemaPath = fileName.Replace(".xml", ".xsd");
                }
                xWriter.WriteValue(schemaPath);
                // Close the XmlSchemaPath element
                xWriter.WriteEndElement();
                // Open the XmlSchemaNameSpace element
                xWriter.WriteStartElement("XmlSchemaNameSpace");
                // Write the namespace of the root node
                xWriter.WriteValue(rootNode.Namespace);
                // Close the XmlSchemaNameSpace element
                xWriter.WriteEndElement();
                // Open the XPathList element
                xWriter.WriteStartElement("XPathList");
                // Write a list of XPaths generated from the current ViewerNode
                WriteXPathListItems(xWriter, rootNode);
                // Close the XPathList element
                xWriter.WriteEndElement();
                // Close the ValidationStep element
                xWriter.WriteEndElement();
                // Close the TestStep element for validation
                xWriter.WriteEndElement();

                xWriter.WriteComment("End: Test " + testName);
                // Close the TestExecution element
                xWriter.WriteEndElement();

                // Write an empty TestCleanup element
                xWriter.WriteComment("Test cleanup: test cases should always leave the system in the state they found it");
                xWriter.WriteStartElement("TestCleanup");
                xWriter.WriteEndElement();

                // Close the TestCase element
                xWriter.WriteEndElement();
                // Close the document
                xWriter.WriteEndDocument();
                // Flush what's written to the stream
                xWriter.Flush();
                // Reset the stream for reading
                stream.Position = 0;
                // Convert the stream to a string and store in the text box
                return new StreamReader(stream, Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred creating the Test Case: " + ex.Message, ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Writes tests for all nodes under the given root node
        /// </summary>
        /// <param name="xWriter">XmlWriter to write tests to</param>
        /// <param name="rootNode">RootNode to start writing from</param>
        private void WriteXPathListItems(XmlTextWriter xWriter, ViewerNode rootNode)
        {
            // Find all elements and attributes in the document which have a value, and write them
            _xpathIndex = 0;
            WriteXPathListItemsRec(xWriter, rootNode);
        }

        /// <summary>
        /// Writes out a test for the current node
        /// </summary>
        /// <param name="xWriter">XmlWriter to write test to</param>
        /// <param name="node">Node to write test for</param>
        private void WriteXPathListItemsRec(XmlTextWriter xWriter, ViewerNode node)
        {
            // Check if this node has a value
            if (!string.IsNullOrEmpty(node.Value))
            {
                // Check the type of node
                if (node.NodeType == NodeType.Element)
                {
                    // Add comment
                    xWriter.WriteComment(string.Format(CultureInfo.CurrentUICulture, " {0}: Element {1} must equal '{2}'", ++_xpathIndex, node.LocalName, node.Value));
                    // Open the XPathList element
                    xWriter.WriteStartElement("XPathValidation");
                    // Write attributes for XPathValidation
                    xWriter.WriteAttributeString("query", node.XPath);
                    // Write node value
                    xWriter.WriteValue(node.Value);
                    // Close the XPathList element
                    xWriter.WriteEndElement();
                }
                else if ((node.NodeType == NodeType.Attribute) && (node.AttributeType == AttributeType.None))
                {
                    // Add comment
                    xWriter.WriteComment(string.Format(CultureInfo.CurrentUICulture, " {0}: Attribute {1} must equal '{2}'", ++_xpathIndex, node.LocalName, node.Value));
                    // Open the XPathList element
                    xWriter.WriteStartElement("XPathValidation");
                    // Write attributes for XPathValidation
                    xWriter.WriteAttributeString("query", node.XPath);
                    // Write node value
                    xWriter.WriteValue(node.Value);
                    // Close the XPathList element
                    xWriter.WriteEndElement();
                }
            }

            // Process attributes for this node
            for (int attrIndex = 0; attrIndex < node.Attributes.Count; attrIndex++)
            {
                WriteXPathListItemsRec(xWriter, node.Attributes[attrIndex]);
            }

            List<string> processedChildNodes = new List<string>();
            List<XPathQuery> nodeCountXPathQuery = new List<XPathQuery>();

            // Process child elements for this node
            for (int childIndex = 0; childIndex < node.ChildNodes.Count; childIndex++)
            {
                WriteXPathListItemsRec(xWriter, node.ChildNodes[childIndex]);
                // Check if we've processed this node as a repeatign node
                if (!processedChildNodes.Contains(node.ChildNodes[childIndex].Name))
                {
                    // Check if this node is a repeating node
                    if (node.ChildNodes[childIndex].OccurrenceIndex > -1)
                    {
                        // Get the count of the number of times this node repeats
                        int nodeCount = node.GetCountOfRepeatingChildNodes(node.ChildNodes[childIndex]);
                        if (nodeCount > 0)
                        {
                            // Create an xpath statement to verify the count of these nodes
                            string nodeCountXPath = string.Format(CultureInfo.CurrentUICulture, "count({0}{1})", node.XPath, node.ChildNodes[childIndex].NonRecurringNodePath, nodeCount);
                            XPathQuery query = new XPathQuery(node.ChildNodes[childIndex].LocalName, nodeCountXPath, nodeCount.ToString());
                            // Add the xpath query to a list
                            nodeCountXPathQuery.Add(query);
                        }
                    }
                    // Add the node to the list of processed nodes
                    processedChildNodes.Add(node.ChildNodes[childIndex].Name);
                }
            }

            // Process any Node Count statements
            if (nodeCountXPathQuery.Count > 0)
            {
                for (int queryIndex = 0; queryIndex < nodeCountXPathQuery.Count; queryIndex++)
                {
                    // Add comment
                    xWriter.WriteComment(string.Format(CultureInfo.CurrentUICulture, " {0}: Count of Element {1} must equal '{2}'", ++_xpathIndex, nodeCountXPathQuery[queryIndex].Name, nodeCountXPathQuery[queryIndex].Value));
                    // Open the XPathList element
                    xWriter.WriteStartElement("XPathValidation");
                    // Write attributes for XPathValidation
                    xWriter.WriteAttributeString("query", nodeCountXPathQuery[queryIndex].Query);
                    // Write node value
                    xWriter.WriteValue(nodeCountXPathQuery[queryIndex].Value);
                    // Close the XPathList element
                    xWriter.WriteEndElement();
                }
            }
        }

        #endregion
    }
}

////////////////////////////////////////////////////////
/// File: Main.cs
/// Author: Daniel Probert
/// Date: 27-07-2008
/// Version: 1.0
////////////////////////////////////////////////////////
namespace DanSharp.XmlViewer
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Drawing;
    using System.Text;
    using System.Windows.Forms;
    using System.IO;
    using System.Xml;
    using DanSharp.XmlViewer.Helpers;

    #endregion

    /// <summary>
    /// Form used to generate BizUnit test cases
    /// </summary>
    public partial class BizUnitTestCase : Form
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the root node to generate the test case from
        /// </summary>
        private ViewerNode _rootNode = null;

        /// <summary>
        /// Stores the path to the file to generate the test case from
        /// </summary>
        private string _currentFileName = null;

        /// <summary>
        /// Stores the file to generate the test case from
        /// </summary>
        private FileInfo _currentFile = null;

        /// <summary>
        /// Stores the path to the Xsd file to generate the test case from
        /// </summary>
        private string _schemaPath = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the BizUnitTestCase class using default values
        /// </summary>
        public BizUnitTestCase()
        {
            InitializeComponent();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Shows this form as a modal dialog
        /// </summary>
        /// <param name="rootNode">Root node to generate test case from</param>
        /// <param name="currentFileName">The path to the xml file to use</param>
        /// <param name="schemaPath">The path to the xsd file to use</param>
        public void ShowDialog(ViewerNode rootNode, string currentFileName, string schemaPath)
        {
            _rootNode = rootNode;
            _currentFileName = currentFileName;
            _schemaPath = schemaPath;

            try
            {
                _currentFile = new FileInfo(_currentFileName);
                CreateBizUnitTestCase();
                ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to show the Test Case dialog because: " + ex.Message, "Unable to show the Test Case dialog ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the event raised when the Save button is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            dlgSaveFile.Title = "Select Location to save file";
            dlgSaveFile.Filter = "Xml Files (*.xml)|*.xml|All Files (*.*)|*.*";
            dlgSaveFile.FileName = "BizUnit Test Case for " + _currentFile.Name;

            if (!dlgSaveFile.FileName.ToLower().EndsWith(".xml"))
            {
                dlgSaveFile.FileName += ".xml";
            }

            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    StreamWriter wr = new StreamWriter(dlgSaveFile.FileName, false);
                    wr.Write(txtUnitCase.Text);
                    wr.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Unable to save the file because: " + ex.Message, "Unable to save file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Handles the event raised text is changed in the UnitCase text box
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void txtUnitCase_TextChanged(object sender, EventArgs e)
        {
            SetButtonStates();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Sets the state of buttons on the form
        /// </summary>
        private void SetButtonStates()
        {
            btnSave.Enabled = txtUnitCase.Text.Length > 0;
        }

        /// <summary>
        /// Creates the BizUnit test case
        /// </summary>
        private void CreateBizUnitTestCase()
        {
            StreamReader rd = BizUnitHelper.Instance.GenerateTestCase(_currentFileName, _rootNode, _schemaPath);
            txtUnitCase.Text = rd.ReadToEnd();
        }

        #endregion
    }
}
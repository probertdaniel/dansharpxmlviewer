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
    using System.Xml.Schema;
    using System.Xml.XPath;
    using DanSharp.XmlViewer.Configuration;
    using DanSharp.XmlViewer.Helpers;
    using System.Reflection;
    using System.Diagnostics;

    #endregion

    /// <summary>
    /// Main form for the application
    /// </summary>
    public partial class Main : Form
    {
        #region Private Instance Fields

        /// <summary>
        /// Stores the file/path of the file we're currently processing
        /// </summary>
        private string _currentFileName = null;

        /// <summary>
        /// Stores the file we're currently processing
        /// </summary>
        private FileInfo _currentFile = null;

        /// <summary>
        /// Stores the root node of the file we're processing
        /// </summary>
        private ViewerNode _rootNode = null;

        /// <summary>
        /// Stores the currently selected node
        /// </summary>
        private ViewerNode _selectedNode = null;

        /// <summary>
        /// Stores the current file as an xml document
        /// </summary>
        private XmlDocument _doc = null;

        /// <summary>
        /// Stores the document we use for checking syntax
        /// </summary>
        private XmlDocument _syntaxCheckDoc = new XmlDocument();

        /// <summary>
        /// Stores an XPathNavigator used to check syntax
        /// </summary>
        private XPathNavigator _syntaxCheckXNav = null;

        /// <summary>
        /// File system watcher used to check for changes to the current file
        /// </summary>
        private FileSystemWatcher _watcher = null;

        /// <summary>
        /// Stores the current colour of the XPath text
        /// </summary>
        private Color _xpathTextColor;

        /// <summary>
        /// Stores the current context tree node
        /// </summary>
        private TreeNode _contextNode = null;

        /// <summary>
        /// Delegate used for updates
        /// </summary>
        private delegate void UpdateDelegate();

        /// <summary>
        /// Delegate used for string updates
        /// </summary>
        /// <param name="value"></param>
        private delegate void UpdateWithStringDelegate(string value);

        /// <summary>
        /// Delegate used for TabIndex updates
        /// </summary>
        /// <param name="index"></param>
        private delegate void UpdateTabIndexDelegate(int index);

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public Main()
        {
            try
            {
                InitializeComponent();
                SetButtonStates();
                PopulateRecentlyUsedXmlFileList();
                LoadXSDComboBox();
                _syntaxCheckXNav = _syntaxCheckDoc.CreateNavigator();
                _xpathTextColor = txtXPathQuery.ForeColor;
                txtXPathQuery.Text = ConfigPersistenceHelper.Instance.Settings.CurrentXPathQuery;
                labVersion.Text = string.Format("v {0}.{1}.{2}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor, Assembly.GetEntryAssembly().GetName().Version.Build);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "An error has occurred", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Handles the event raised when the version/url link is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void labLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("http://dansharpxmlviewer.codeplex.com");
        }

        /// <summary>
        /// Handles the event raised when the File\Open menu option is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dlgOpenFile.Filter = "Xml Files (*.xml)|*.xml";
            dlgOpenFile.Title = "Select an Xml File to open";

            if (string.IsNullOrEmpty(_currentFileName))
            {
                dlgOpenFile.FileName = null;
            }
            else
            {
                dlgOpenFile.FileName = _currentFileName;
            }

            // Show the FileOpen dialog
            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                if (string.Compare(_currentFileName, dlgOpenFile.FileName, true) != 0)
                {
                    OpenFile(dlgOpenFile.FileName);
                    ConfigPersistenceHelper.Instance.Settings.XmlFiles.AddItem(dlgOpenFile.FileName);
                    PopulateRecentlyUsedXmlFileList();
                }
                SetButtonStates();
            }
        }

        /// <summary>
        /// Handles the event raised when a recent file menu option is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void recentFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
            if (menuItem != null)
            {
                FileHistoryItem item = menuItem.Tag as FileHistoryItem;
                if (item != null)
                {
                    // Check if the item exists
                    if (!FileHelper.Instance.FileExists(item.FilePath))
                    {
                        MessageBox.Show(this, string.Format("The file {0} does not exist", item.FilePath), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        // Remove the file from the list
                        ConfigPersistenceHelper.Instance.Settings.XmlFiles.Remove(item);
                        // Rebuild the menu
                        PopulateRecentlyUsedXmlFileList();
                    }
                    else
                    {
                        OpenFile(item.FilePath);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the event raised when the File\Exit menu option is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the event raised after a new tree node has been selected
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void trvXml_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetTreeViewContextMenuStates(sender as TreeView);
            if ((e.Node != null) && (e.Node.Tag != null))
            {
                _selectedNode = (ViewerNode)e.Node.Tag;
                txtXPath.Text = _selectedNode.XPath;
            }
        }

        /// <summary>
        /// Handles the event raised when a key is pressed in the XPath text box
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void txtXPath_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.A))
            {
                txtXPath.SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Handles the event raised when a key is pressed in the tree view
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void treeView_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.C))
            {
                CopyTreeViewNodeText(((TreeView)sender).SelectedNode);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Handles the event raised when a key is pressed in the XPathQuery text box
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void txtXPathQuery_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Control) && (e.KeyCode == Keys.A))
            {
                txtXPathQuery.SelectAll();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        /// <summary>
        /// Handles the event raised when the SelectXSD button is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void btnSelectXSD_Click(object sender, EventArgs e)
        {
            dlgOpenFile.Filter = "Xsd Files (*.xsd)|*.xsd";
            dlgOpenFile.Title = "Select an Xsd File to open";
            if ((string.IsNullOrEmpty(cboXsdFile.Text)) || (!FileHelper.Instance.FileExists(cboXsdFile.Text)))
            {
                dlgOpenFile.FileName = null;
            }
            else
            {
                dlgOpenFile.FileName = cboXsdFile.Text;
            }

            if (dlgOpenFile.ShowDialog() == DialogResult.OK)
            {
                if (string.Compare(cboXsdFile.Text, dlgOpenFile.FileName, true) != 0)
                {
                    FileHistoryItem item = new FileHistoryItem(dlgOpenFile.FileName);
                    cboXsdFile.Items.Add(item);
                    cboXsdFile.SelectedItem = item;
                }
            }
        }

        /// <summary>
        /// Handles the event raised when the Validate button is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void btnValidate_Click(object sender, EventArgs e)
        {
            FileHistoryItem item = cboXsdFile.SelectedItem as FileHistoryItem;
            string xsdPath = cboXsdFile.Text;
            if (item != null)
            {
                xsdPath = item.FilePath;
            }

            // Check that the file exists
            if (!FileHelper.Instance.FileExists(xsdPath))
            {
                MessageBox.Show(this, string.Format("The file '{0}' does not exist, or the path is invalid", xsdPath), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ConfigPersistenceHelper.Instance.Settings.XsdFiles.AddItem(xsdPath);
            txtValidationEvents.Clear();
            XsdValidationResult result = XsdValidationHelper.Instance.ValidateInstance(xsdPath, _doc);
            txtValidationEvents.Text = result.Results.ToString();
            switch (result.State)
            {
                case ValidationState.Success:
                    {
                        txtValidationEvents.ForeColor = Color.Black;
                        break;
                    }
                case ValidationState.OtherError:
                    {
                        txtValidationEvents.ForeColor = Color.Red;
                        break;
                    }
                case ValidationState.ValidationError:
                    {
                        txtValidationEvents.ForeColor = Color.Green;
                        break;
                    }
                case ValidationState.Warning:
                    {
                        txtValidationEvents.ForeColor = Color.Brown;
                        break;
                    }
            }

            // Refresh the combo box
            string currentPath = cboXsdFile.Text;
            LoadXSDComboBox();
            cboXsdFile.Text = currentPath;
        }

        /// <summary>
        /// Handles the event raised when the ExecuteQuery button is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void btnExecuteQuery_Click(object sender, EventArgs e)
        {
            if (_doc != null)
            {
                trvXpathResult.Nodes.Clear();
                string queryText = txtXPathQuery.Text;
                if (txtXPathQuery.SelectionLength > 0)
                {
                    queryText = txtXPathQuery.SelectedText;
                }

                XPathNavigator xnav = _doc.CreateNavigator();
                try
                {
                    XPathNodeIterator xpi = xnav.Select(queryText);
                    DisplayNavigator(xpi);
                }
                catch (XPathException)
                {
                    try
                    {
                        object result = xnav.Evaluate(queryText);

                        if (result.GetType().Name == "XPathSelectionIterator")
                        {
                            XPathNodeIterator xpi = result as XPathNodeIterator;
                            DisplayNavigator(xpi);
                        }
                        else
                        {
                            TreeNode node = new TreeNode("Result: " + result.ToString());
                            node.ForeColor = Color.Brown;
                            node.ContextMenuStrip = mnuTreeView;
                            node.ToolTipText = node.Text;
                            trvXpathResult.Nodes.Add(node);
                        }
                    }
                    catch (Exception ex)
                    {
                        TreeNode node = new TreeNode("Error: " + ex.Message);
                        node.ForeColor = Color.Red;
                        node.ToolTipText = ex.Message;
                        node.ContextMenuStrip = mnuTreeView;
                        trvXpathResult.Nodes.Add(node);
                    }
                }
                catch (Exception ex)
                {
                    TreeNode node = new TreeNode("Error: " + ex.Message);
                    node.ForeColor = Color.Red;
                    node.ToolTipText = ex.Message;
                    node.ContextMenuStrip = mnuTreeView;
                    trvXpathResult.Nodes.Add(node);
                }
            }
        }

        /// <summary>
        /// Handles the event raised when the GenerateBizUnitTestCase menu item is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void generateBizUnitTestCaseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileHistoryItem item = cboXsdFile.SelectedItem as FileHistoryItem;
            string xsdPath = cboXsdFile.Text;
            if (item != null)
            {
                xsdPath = item.FilePath;
            }
            new BizUnitTestCase().ShowDialog(_rootNode, _currentFileName, xsdPath);
        }

        /// <summary>
        /// Handles the event raised when text is changed in the XPathQuery text box
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void txtXPathQuery_TextChanged(object sender, EventArgs e)
        {
            SetButtonStates();
            ConfigPersistenceHelper.Instance.Settings.CurrentXPathQuery = txtXPathQuery.Text;
            try
            {
                // Attempt to validate the current text
                _syntaxCheckXNav.Evaluate(txtXPathQuery.Text);
                // Text valiXdated successfully
                if (txtXPathQuery.ForeColor != _xpathTextColor)
                {
                    txtXPathQuery.ForeColor = _xpathTextColor;
                }

                if (labelToolTip.GetToolTip(txtXPathQuery) != string.Empty)
                {
                    labelToolTip.SetToolTip(txtXPathQuery, string.Empty);
                }
            }
            catch (Exception ex)
            {
                // Text is invalid
                if (txtXPathQuery.ForeColor != Color.Red)
                {
                    txtXPathQuery.ForeColor = Color.Red;
                }

                if (labelToolTip.GetToolTip(txtXPathQuery) != "Invalid XPath Query: " + ex.Message)
                {
                    labelToolTip.SetToolTip(txtXPathQuery, "Invalid XPath Query: " + ex.Message);
                }
            }
        }

        /// <summary>
        /// Handles the event raised when text is changed in the Xsd combo box
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void cboXsdFile_TextChanged(object sender, EventArgs e)
        {
            SetButtonStates();
            labelToolTip.SetToolTip(cboXsdFile, cboXsdFile.Text);
        }

        /// <summary>
        /// Handles the event raised when the ResetRUF menu item is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void resetRecentlyUsedFileListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConfigPersistenceHelper.Instance.Settings.XmlFiles.Reset();
            ConfigPersistenceHelper.Instance.Settings.XsdFiles.Reset();
            PopulateRecentlyUsedXmlFileList();
            LoadXSDComboBox();
        }

        /// <summary>
        /// Handles the event raised when the ReloadFile menu item is clicked
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void reloadFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFile(_currentFileName);
        }

        /// <summary>
        /// Handles the event raised when an item is dragged onto the Main form
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            bool accept = false;
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                Array fileNames = (Array)e.Data.GetData(DataFormats.FileDrop);
                if ((fileNames != null) && (fileNames.Length > 0))
                {
                    string fileName = fileNames.GetValue(0).ToString();
                    accept = fileName.ToLower().EndsWith(".xml");
                }
            }

            if (accept)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        /// <summary>
        /// Handles the event raised when an item is droppped onto the Main form
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            Array fileNames = (Array)e.Data.GetData(DataFormats.FileDrop);
            if ((fileNames != null) && (fileNames.Length > 0))
            {
                string fileName = fileNames.GetValue(0).ToString();
                if (!string.IsNullOrEmpty(fileName))
                {
                    OpenFile(fileName);
                    ConfigPersistenceHelper.Instance.Settings.XmlFiles.AddItem(fileName);
                    PopulateRecentlyUsedXmlFileList();
                }
            }
        }

        /// <summary>
        /// Handles the event raised a context menu item is clicked on the treeview
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void mnuTreeView_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip strip = sender as ContextMenuStrip;
            // Get the tree view which was selected
            TreeView view = strip.SourceControl as TreeView;
            if (view != null)
            {
                // Check which menu item was selected
                if (e.ClickedItem == copyToolStripMenuItem)
                {
                    CopyTreeViewNodeText(_contextNode);
                }
                else if (e.ClickedItem == expandNodeToolStripMenuItem)
                {
                    if (_contextNode != null)
                    {
                        _contextNode.ExpandAll();
                        _contextNode.EnsureVisible();
                    }
                }
                else if (e.ClickedItem == collapseNodeToolStripMenuItem)
                {
                    if (_contextNode != null)
                    {
                        _contextNode.Collapse();
                        _contextNode.EnsureVisible();
                    }
                }
                else if (e.ClickedItem == expandAllNodesToolStripMenuItem)
                {
                    view.ExpandAll();
                    if (_contextNode != null)
                    {
                        _contextNode.EnsureVisible();
                    }
                }
                else if (e.ClickedItem == collapseAllNodesToolStripMenuItem)
                {
                    view.CollapseAll();
                }
            }
        }

        /// <summary>
        /// Handles the event raised after an item is slected in the XPathResult Treeview
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void trvXpathResult_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SetTreeViewContextMenuStates(sender as TreeView);
        }

        /// <summary>
        /// Handles event raised when a watched file is changed
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void _watcher_Changed(object sender, FileSystemEventArgs e)
        {
            // Prompt to reload the file
            Activate();
            _watcher.EnableRaisingEvents = false;
            if (MessageBox.Show(this, string.Format("The file '{0}' has changed.\r\nWould you like to reload it?", _currentFileName), "File has changed", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                //_watcher.Changed -= new FileSystemEventHandler(_watcher_Changed);
                //_watcher.Dispose();
                OpenFile(_currentFileName);
            }
            else
            {
                _watcher.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Handles the event raised when the mouse if clicked on a tree view node
        /// </summary>
        /// <param name="sender">Object raising the event</param>
        /// <param name="e">Event-specific parameters</param>
        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                _contextNode = e.Node;
                SetTreeViewContextMenuStates(sender as TreeView);
            }
            else
            {
                _contextNode = null;
                SetTreeViewContextMenuStates(sender as TreeView);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Updates the index of the currently selected tab
        /// </summary>
        /// <param name="index">New index</param>
        private void UpdateTabIndex(int index)
        {
            ctlTab.SelectedIndex = index;
        }

        /// <summary>
        /// Updates the text for the current file name
        /// </summary>
        /// <param name="value">New text to use</param>
        private void UpdateFileNameLabel(string value)
        {
            labFileName.Text = value;
            labelToolTip.SetToolTip(labFileName, _currentFileName);
        }

        /// <summary>
        /// Opens a file and loads into an XmlDocument
        /// </summary>
        /// <param name="filePath">Path to file to open</param>
        private void OpenFile(string filePath)
        {
            if (!FileHelper.Instance.FileExists(filePath))
            {
                MessageBox.Show(this, string.Format("The file '{0}' does not exist", filePath), "File does not exist", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            _currentFileName = filePath;
            _doc = new XmlDocument();

            try
            {
                _doc.Load(_currentFileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, string.Format("The file '{0}' does not appear to be valid Xml:\r\n{1}", _currentFileName, ex.Message), "Invalid Xml File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (_watcher != null)
                {
                    _watcher.EnableRaisingEvents = true;
                }
                return;
            }

            try
            {
                _currentFile = new FileInfo(_currentFileName);
                if (InvokeRequired)
                {
                    trvXml.Invoke(new UpdateDelegate(BuildTreeView));
                    webBrowser.Invoke(new UpdateDelegate(BuildXmlView));
                    labFileName.Invoke(new UpdateWithStringDelegate(UpdateFileNameLabel), _currentFile.Name);
                    ctlTab.Invoke(new UpdateTabIndexDelegate(UpdateTabIndex), 0);
                }
                else
                {
                    BuildTreeView();
                    BuildXmlView();
                    UpdateFileNameLabel(_currentFile.Name);
                    UpdateTabIndex(0);
                }

                if (_watcher != null)
                {
                    _watcher.Dispose();
                }
                _watcher = new FileSystemWatcher(_currentFile.DirectoryName, _currentFile.Name);
                _watcher.NotifyFilter = NotifyFilters.LastWrite;
                _watcher.Changed += new FileSystemEventHandler(_watcher_Changed);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error has occurred: " + ex.Message, "An error occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (_watcher != null)
            {
                _watcher.EnableRaisingEvents = true;
            }
            SetButtonStates();
        }

        /// <summary>
        /// Sets the states of the various buttons on the form
        /// </summary>
        private void SetButtonStates()
        {
            btnExecuteQuery.Enabled = ((txtXPathQuery.Text.Length > 0) && (_doc != null));
            btnValidate.Enabled = ((cboXsdFile.Text.Length > 0) && (_doc != null));
            reloadFileToolStripMenuItem.Enabled = (_doc != null);
            generateBizUnitTestCaseToolStripMenuItem.Enabled = (_doc != null);
        }

        /// <summary>
        /// Sets up the XmlView tab
        /// </summary>
        private void BuildXmlView()
        {
            webBrowser.Url = new Uri(_currentFileName);
        }

        /// <summary>
        /// Builds the tree view from the current XmlDocument
        /// </summary>
        private void BuildTreeView()
        {
            trvXml.Nodes.Clear();
            _rootNode = new ViewerNode(_doc.DocumentElement, null);
            BuildTreeView(trvXml, _rootNode, null);
            trvXml.Nodes[0].Expand();
        }

        /// <summary>
        /// Add a node to the tree view and processes any children
        /// </summary>
        /// <param name="vNode">ViewerNode to add</param>
        /// <param name="parentNode">Parent TreeView node to add new node to</param>
        private void BuildTreeView(TreeView trv, ViewerNode vNode, TreeNode parentNode)
        {
            if (vNode.NodeType == NodeType.Unknown) return;

            TreeNode node = null;
            if (vNode.NodeType == NodeType.Attribute)
            {
                if (vNode.AttributeType == AttributeType.None)
                {
                    node = new TreeNode(vNode.ToString());
                    node.ForeColor = Color.Brown;
                    node.ToolTipText = vNode.ToDetailsString();
                }
                else
                {
                    return;
                }
            }
            else if (vNode.NodeType == NodeType.Element)
            {
                node = new TreeNode(vNode.ToString());
                node.ToolTipText = vNode.ToDetailsString();
            }

            node.Tag = vNode;
            node.ContextMenuStrip = mnuTreeView;

            // Add attributes
            for (int attrIndex = 0; attrIndex < vNode.Attributes.Count; attrIndex++)
            {
                BuildTreeView(trv, vNode.Attributes[attrIndex], node);
            }

            // Add children
            for (int childIndex = 0; childIndex < vNode.ChildNodes.Count; childIndex++)
            {
                BuildTreeView(trv, vNode.ChildNodes[childIndex], node);
            }

            if (parentNode == null)
            {
                trv.Nodes.Add(node);
            }
            else
            {
                parentNode.Nodes.Add(node);
            }
        }

        /// <summary>
        /// Displays the results of the XPath Navigator in the Xpath results tree view
        /// </summary>
        /// <param name="xpi">XPathNodeIterator created using the XPath query</param>
        private void DisplayNavigator(XPathNodeIterator xpi)
        {
            if ((xpi != null) && (xpi.Count > 0))
            {
                bool moreNodes = xpi.MoveNext();
                while (moreNodes)
                {
                    if (xpi.Current.NodeType == XPathNodeType.Text)
                    {
                        TreeNode node = new TreeNode("Result: " + xpi.Current.Value);
                        node.ForeColor = Color.Brown;
                        node.ContextMenuStrip = mnuTreeView;
                        node.ToolTipText = node.Text;
                        trvXpathResult.Nodes.Add(node);
                    }
                    else if (xpi.Current.NodeType == XPathNodeType.Attribute)
                    {
                        TreeNode node = new TreeNode("@" + xpi.Current.Name + ": " + xpi.Current.Value);
                        node.ForeColor = Color.Brown;
                        node.ContextMenuStrip = mnuTreeView;
                        node.ToolTipText = node.Text;
                        trvXpathResult.Nodes.Add(node);
                    }
                    else if (xpi.Current.NodeType == XPathNodeType.Element)
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.LoadXml(xpi.Current.OuterXml);
                        ViewerNode vNode = new ViewerNode(doc.DocumentElement, null);
                        BuildTreeView(trvXpathResult, vNode, null);
                    }
                    moreNodes = xpi.MoveNext();
                }
            }
            else
            {
                trvXpathResult.Nodes.Add("Nothing found.");
            }
        }

        /// <summary>
        /// Updates the menu list of recently used files
        /// </summary>
        private void PopulateRecentlyUsedXmlFileList()
        {
            List<ToolStripItem> itemsToRemove = new List<ToolStripItem>();

            // Loop through the current menu items and find items to remove
            for (int menuIndex = 0; menuIndex < fileToolStripMenuItem.DropDownItems.Count; menuIndex++)
            {
                if (fileToolStripMenuItem.DropDownItems[menuIndex].Name.StartsWith("mnuRecentFiles"))
                {
                    itemsToRemove.Add(fileToolStripMenuItem.DropDownItems[menuIndex]);
                }
            }

            // Remove existing entries
            for (int removeIndex = 0; removeIndex < itemsToRemove.Count; removeIndex++)
            {
                fileToolStripMenuItem.DropDownItems.Remove(itemsToRemove[removeIndex]);
            }

            int lowerBarIndex = fileToolStripMenuItem.DropDownItems.IndexOf(mnuRecentListLowerBar);

            // Check if we have any recent files
            if (ConfigPersistenceHelper.Instance.Settings.XmlFiles.Count == 0)
            {
                ToolStripMenuItem item = new ToolStripMenuItem("(No Recent Files)");
                item.Name = "mnuRecentFiles_None";
                item.Enabled = false;
                item.ForeColor = Color.Gray;
                fileToolStripMenuItem.DropDownItems.Insert(lowerBarIndex, item);
            }
            else
            {
                // Loop through the recent files
                for (int fileIndex = ConfigPersistenceHelper.Instance.Settings.XmlFiles.Count - 1; fileIndex >= 0; fileIndex--)
                {
                    // Create a new menu item for this file
                    ToolStripMenuItem item = new ToolStripMenuItem(string.Format("&{0} {1}", fileIndex + 1, ConfigPersistenceHelper.Instance.Settings.XmlFiles[fileIndex].DisplayName));
                    item.Click += new EventHandler(recentFileToolStripMenuItem_Click);
                    item.Tag = ConfigPersistenceHelper.Instance.Settings.XmlFiles[fileIndex];
                    item.Name = "mnuRecentFiles_" + fileIndex.ToString();

                    // Add this menu item to the list of recently used files
                    fileToolStripMenuItem.DropDownItems.Insert(lowerBarIndex, item);
                }
            }
        }

        /// <summary>
        /// Load the XSD combo box with recently used xsd files
        /// </summary>
        private void LoadXSDComboBox()
        {
            try
            {
                cboXsdFile.Items.Clear();
                for (int index = 0; index < ConfigPersistenceHelper.Instance.Settings.XsdFiles.Count; index++)
                {
                    cboXsdFile.Items.Add(ConfigPersistenceHelper.Instance.Settings.XsdFiles[index]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred loading the XSD File Combo Box: " + ex.Message, "An error has occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Copies the treeview node text to the clipboard
        /// </summary>
        /// <param name="node"></param>
        private void CopyTreeViewNodeText(TreeNode node)
        {
            if ((node != null) && (!string.IsNullOrEmpty(node.Text)))
            {
                try
                {
                    Clipboard.SetDataObject(node.Text, true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error occurred trying to copy information to the clipboard: " + ex.Message, "Unable to copy to clipboard", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        /// <summary>
        /// Sets the state of the TreeView context menu
        /// </summary>
        private void SetTreeViewContextMenuStates(TreeView view)
        {
            copyToolStripMenuItem.Enabled = (_contextNode != null);
            expandNodeToolStripMenuItem.Enabled = ((_contextNode != null) && (_contextNode.Nodes.Count > 0) && (!_contextNode.IsExpanded));
            collapseNodeToolStripMenuItem.Enabled = ((_contextNode != null) && (_contextNode.Nodes.Count > 0) && (_contextNode.IsExpanded));
            expandAllNodesToolStripMenuItem.Enabled = ((view.Nodes.Count > 0) && ((view.Nodes.Count > 1) || (view.Nodes[0].Nodes.Count > 0)));
            collapseAllNodesToolStripMenuItem.Enabled = ((view.Nodes.Count > 0) && ((view.Nodes.Count > 1) || (view.Nodes[0].Nodes.Count > 0)));
        }

        #endregion
    }
}

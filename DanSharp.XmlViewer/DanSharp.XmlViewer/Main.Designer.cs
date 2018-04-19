namespace DanSharp.XmlViewer
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.mnuStrip = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecentListUpperBar = new System.Windows.Forms.ToolStripSeparator();
            this.mnuRecentFiles_None = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecentListLowerBar = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateBizUnitTestCaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.resetRecentlyUsedFileListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.labFileName = new System.Windows.Forms.Label();
            this.trvXml = new System.Windows.Forms.TreeView();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.dlgOpenFile = new System.Windows.Forms.OpenFileDialog();
            this.panTreeView = new System.Windows.Forms.SplitContainer();
            this.ctlTab = new System.Windows.Forms.TabControl();
            this.pageTrv = new System.Windows.Forms.TabPage();
            this.pageXml = new System.Windows.Forms.TabPage();
            this.webBrowser = new System.Windows.Forms.WebBrowser();
            this.pageXPath = new System.Windows.Forms.TabPage();
            this.panXPath = new System.Windows.Forms.SplitContainer();
            this.btnExecuteQuery = new System.Windows.Forms.Button();
            this.txtXPathQuery = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.trvXpathResult = new System.Windows.Forms.TreeView();
            this.pageXSD = new System.Windows.Forms.TabPage();
            this.panXsdVal = new System.Windows.Forms.SplitContainer();
            this.cboXsdFile = new System.Windows.Forms.ComboBox();
            this.btnValidate = new System.Windows.Forms.Button();
            this.btnSelectXSD = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtValidationEvents = new System.Windows.Forms.RichTextBox();
            this.labelToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.labLink = new System.Windows.Forms.LinkLabel();
            this.labVersion = new System.Windows.Forms.Label();
            this.mnuTreeView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.copyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.expandNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseNodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.expandAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.collapseAllNodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStrip.SuspendLayout();
            this.panTreeView.Panel1.SuspendLayout();
            this.panTreeView.Panel2.SuspendLayout();
            this.panTreeView.SuspendLayout();
            this.ctlTab.SuspendLayout();
            this.pageTrv.SuspendLayout();
            this.pageXml.SuspendLayout();
            this.pageXPath.SuspendLayout();
            this.panXPath.Panel1.SuspendLayout();
            this.panXPath.Panel2.SuspendLayout();
            this.panXPath.SuspendLayout();
            this.pageXSD.SuspendLayout();
            this.panXsdVal.Panel1.SuspendLayout();
            this.panXsdVal.Panel2.SuspendLayout();
            this.panXsdVal.SuspendLayout();
            this.mnuTreeView.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "Xml File:";
            // 
            // mnuStrip
            // 
            this.mnuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.toolsToolStripMenuItem});
            this.mnuStrip.Location = new System.Drawing.Point(0, 0);
            this.mnuStrip.Name = "mnuStrip";
            this.mnuStrip.Size = new System.Drawing.Size(643, 24);
            this.mnuStrip.TabIndex = 0;
            this.mnuStrip.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.reloadFileToolStripMenuItem,
            this.mnuRecentListUpperBar,
            this.mnuRecentFiles_None,
            this.mnuRecentListLowerBar,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.openToolStripMenuItem.Text = "&Open...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // reloadFileToolStripMenuItem
            // 
            this.reloadFileToolStripMenuItem.Enabled = false;
            this.reloadFileToolStripMenuItem.Name = "reloadFileToolStripMenuItem";
            this.reloadFileToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.reloadFileToolStripMenuItem.Text = "Re&load File";
            this.reloadFileToolStripMenuItem.Click += new System.EventHandler(this.reloadFileToolStripMenuItem_Click);
            // 
            // mnuRecentListUpperBar
            // 
            this.mnuRecentListUpperBar.Name = "mnuRecentListUpperBar";
            this.mnuRecentListUpperBar.Size = new System.Drawing.Size(148, 6);
            // 
            // mnuRecentFiles_None
            // 
            this.mnuRecentFiles_None.Enabled = false;
            this.mnuRecentFiles_None.ForeColor = System.Drawing.SystemColors.GrayText;
            this.mnuRecentFiles_None.Name = "mnuRecentFiles_None";
            this.mnuRecentFiles_None.Size = new System.Drawing.Size(151, 22);
            this.mnuRecentFiles_None.Text = "(No recent files)";
            // 
            // mnuRecentListLowerBar
            // 
            this.mnuRecentListLowerBar.Name = "mnuRecentListLowerBar";
            this.mnuRecentListLowerBar.Size = new System.Drawing.Size(148, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(151, 22);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateBizUnitTestCaseToolStripMenuItem,
            this.toolStripMenuItem1,
            this.resetRecentlyUsedFileListToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.toolsToolStripMenuItem.Text = "&Tools";
            // 
            // generateBizUnitTestCaseToolStripMenuItem
            // 
            this.generateBizUnitTestCaseToolStripMenuItem.Enabled = false;
            this.generateBizUnitTestCaseToolStripMenuItem.Name = "generateBizUnitTestCaseToolStripMenuItem";
            this.generateBizUnitTestCaseToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.generateBizUnitTestCaseToolStripMenuItem.Text = "Generate &BizUnit Test Case...";
            this.generateBizUnitTestCaseToolStripMenuItem.Click += new System.EventHandler(this.generateBizUnitTestCaseToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(214, 6);
            // 
            // resetRecentlyUsedFileListToolStripMenuItem
            // 
            this.resetRecentlyUsedFileListToolStripMenuItem.Name = "resetRecentlyUsedFileListToolStripMenuItem";
            this.resetRecentlyUsedFileListToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.resetRecentlyUsedFileListToolStripMenuItem.Text = "&Reset Recent File List";
            this.resetRecentlyUsedFileListToolStripMenuItem.Click += new System.EventHandler(this.resetRecentlyUsedFileListToolStripMenuItem_Click);
            // 
            // labFileName
            // 
            this.labFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.labFileName.AutoEllipsis = true;
            this.labFileName.AutoSize = true;
            this.labFileName.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labFileName.Location = new System.Drawing.Point(77, 30);
            this.labFileName.Name = "labFileName";
            this.labFileName.Size = new System.Drawing.Size(0, 14);
            this.labFileName.TabIndex = 4;
            // 
            // trvXml
            // 
            this.trvXml.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvXml.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvXml.FullRowSelect = true;
            this.trvXml.HotTracking = true;
            this.trvXml.Location = new System.Drawing.Point(0, 0);
            this.trvXml.Name = "trvXml";
            this.trvXml.ShowNodeToolTips = true;
            this.trvXml.Size = new System.Drawing.Size(605, 375);
            this.trvXml.TabIndex = 2;
            this.trvXml.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvXml_AfterSelect);
            this.trvXml.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.trvXml.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            // 
            // txtXPath
            // 
            this.txtXPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXPath.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXPath.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtXPath.Location = new System.Drawing.Point(0, 0);
            this.txtXPath.Multiline = true;
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtXPath.Size = new System.Drawing.Size(605, 155);
            this.txtXPath.TabIndex = 3;
            this.txtXPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXPath_KeyDown);
            // 
            // dlgOpenFile
            // 
            this.dlgOpenFile.FileName = "openFileDialog1";
            // 
            // panTreeView
            // 
            this.panTreeView.BackColor = System.Drawing.SystemColors.Control;
            this.panTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panTreeView.Location = new System.Drawing.Point(3, 3);
            this.panTreeView.Name = "panTreeView";
            this.panTreeView.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panTreeView.Panel1
            // 
            this.panTreeView.Panel1.Controls.Add(this.trvXml);
            // 
            // panTreeView.Panel2
            // 
            this.panTreeView.Panel2.Controls.Add(this.txtXPath);
            this.panTreeView.Size = new System.Drawing.Size(605, 534);
            this.panTreeView.SplitterDistance = 375;
            this.panTreeView.TabIndex = 7;
            // 
            // ctlTab
            // 
            this.ctlTab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlTab.Controls.Add(this.pageTrv);
            this.ctlTab.Controls.Add(this.pageXml);
            this.ctlTab.Controls.Add(this.pageXPath);
            this.ctlTab.Controls.Add(this.pageXSD);
            this.ctlTab.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ctlTab.HotTrack = true;
            this.ctlTab.Location = new System.Drawing.Point(12, 46);
            this.ctlTab.Name = "ctlTab";
            this.ctlTab.SelectedIndex = 0;
            this.ctlTab.Size = new System.Drawing.Size(619, 567);
            this.ctlTab.TabIndex = 1;
            // 
            // pageTrv
            // 
            this.pageTrv.Controls.Add(this.panTreeView);
            this.pageTrv.Location = new System.Drawing.Point(4, 23);
            this.pageTrv.Name = "pageTrv";
            this.pageTrv.Padding = new System.Windows.Forms.Padding(3);
            this.pageTrv.Size = new System.Drawing.Size(611, 540);
            this.pageTrv.TabIndex = 0;
            this.pageTrv.Text = "Tree View";
            this.pageTrv.UseVisualStyleBackColor = true;
            // 
            // pageXml
            // 
            this.pageXml.Controls.Add(this.webBrowser);
            this.pageXml.Location = new System.Drawing.Point(4, 23);
            this.pageXml.Name = "pageXml";
            this.pageXml.Padding = new System.Windows.Forms.Padding(3);
            this.pageXml.Size = new System.Drawing.Size(611, 540);
            this.pageXml.TabIndex = 1;
            this.pageXml.Text = "Xml View";
            this.pageXml.UseVisualStyleBackColor = true;
            // 
            // webBrowser
            // 
            this.webBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser.Location = new System.Drawing.Point(3, 3);
            this.webBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser.Name = "webBrowser";
            this.webBrowser.Size = new System.Drawing.Size(605, 534);
            this.webBrowser.TabIndex = 4;
            // 
            // pageXPath
            // 
            this.pageXPath.Controls.Add(this.panXPath);
            this.pageXPath.Location = new System.Drawing.Point(4, 23);
            this.pageXPath.Name = "pageXPath";
            this.pageXPath.Size = new System.Drawing.Size(611, 540);
            this.pageXPath.TabIndex = 2;
            this.pageXPath.Text = "XPath Queries";
            this.pageXPath.UseVisualStyleBackColor = true;
            // 
            // panXPath
            // 
            this.panXPath.BackColor = System.Drawing.SystemColors.Control;
            this.panXPath.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panXPath.Location = new System.Drawing.Point(0, 0);
            this.panXPath.Name = "panXPath";
            this.panXPath.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panXPath.Panel1
            // 
            this.panXPath.Panel1.Controls.Add(this.btnExecuteQuery);
            this.panXPath.Panel1.Controls.Add(this.txtXPathQuery);
            this.panXPath.Panel1.Controls.Add(this.label2);
            // 
            // panXPath.Panel2
            // 
            this.panXPath.Panel2.Controls.Add(this.trvXpathResult);
            this.panXPath.Size = new System.Drawing.Size(611, 540);
            this.panXPath.SplitterDistance = 187;
            this.panXPath.TabIndex = 0;
            // 
            // btnExecuteQuery
            // 
            this.btnExecuteQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExecuteQuery.Location = new System.Drawing.Point(533, 3);
            this.btnExecuteQuery.Name = "btnExecuteQuery";
            this.btnExecuteQuery.Size = new System.Drawing.Size(75, 23);
            this.btnExecuteQuery.TabIndex = 5;
            this.btnExecuteQuery.Text = "Execute";
            this.btnExecuteQuery.UseVisualStyleBackColor = true;
            this.btnExecuteQuery.Click += new System.EventHandler(this.btnExecuteQuery_Click);
            // 
            // txtXPathQuery
            // 
            this.txtXPathQuery.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtXPathQuery.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXPathQuery.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.txtXPathQuery.HideSelection = false;
            this.txtXPathQuery.Location = new System.Drawing.Point(0, 29);
            this.txtXPathQuery.Multiline = true;
            this.txtXPathQuery.Name = "txtXPathQuery";
            this.txtXPathQuery.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtXPathQuery.Size = new System.Drawing.Size(611, 158);
            this.txtXPathQuery.TabIndex = 6;
            this.txtXPathQuery.TextChanged += new System.EventHandler(this.txtXPathQuery_TextChanged);
            this.txtXPathQuery.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtXPathQuery_KeyDown);
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Top;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(611, 29);
            this.label2.TabIndex = 0;
            this.label2.Text = "XPath Query:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // trvXpathResult
            // 
            this.trvXpathResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trvXpathResult.Font = new System.Drawing.Font("Verdana", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trvXpathResult.FullRowSelect = true;
            this.trvXpathResult.HotTracking = true;
            this.trvXpathResult.Location = new System.Drawing.Point(0, 0);
            this.trvXpathResult.Name = "trvXpathResult";
            this.trvXpathResult.ShowNodeToolTips = true;
            this.trvXpathResult.Size = new System.Drawing.Size(611, 349);
            this.trvXpathResult.TabIndex = 7;
            this.trvXpathResult.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trvXpathResult_AfterSelect);
            this.trvXpathResult.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView_NodeMouseClick);
            this.trvXpathResult.KeyDown += new System.Windows.Forms.KeyEventHandler(this.treeView_KeyDown);
            // 
            // pageXSD
            // 
            this.pageXSD.Controls.Add(this.panXsdVal);
            this.pageXSD.Location = new System.Drawing.Point(4, 23);
            this.pageXSD.Name = "pageXSD";
            this.pageXSD.Size = new System.Drawing.Size(611, 540);
            this.pageXSD.TabIndex = 3;
            this.pageXSD.Text = "XSD Validation";
            this.pageXSD.UseVisualStyleBackColor = true;
            // 
            // panXsdVal
            // 
            this.panXsdVal.BackColor = System.Drawing.SystemColors.Control;
            this.panXsdVal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panXsdVal.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.panXsdVal.IsSplitterFixed = true;
            this.panXsdVal.Location = new System.Drawing.Point(0, 0);
            this.panXsdVal.Name = "panXsdVal";
            this.panXsdVal.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // panXsdVal.Panel1
            // 
            this.panXsdVal.Panel1.Controls.Add(this.cboXsdFile);
            this.panXsdVal.Panel1.Controls.Add(this.btnValidate);
            this.panXsdVal.Panel1.Controls.Add(this.btnSelectXSD);
            this.panXsdVal.Panel1.Controls.Add(this.label3);
            // 
            // panXsdVal.Panel2
            // 
            this.panXsdVal.Panel2.Controls.Add(this.txtValidationEvents);
            this.panXsdVal.Size = new System.Drawing.Size(611, 540);
            this.panXsdVal.SplitterDistance = 54;
            this.panXsdVal.TabIndex = 0;
            // 
            // cboXsdFile
            // 
            this.cboXsdFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cboXsdFile.FormattingEnabled = true;
            this.cboXsdFile.Location = new System.Drawing.Point(63, 5);
            this.cboXsdFile.Name = "cboXsdFile";
            this.cboXsdFile.Size = new System.Drawing.Size(545, 22);
            this.cboXsdFile.TabIndex = 8;
            this.cboXsdFile.TextChanged += new System.EventHandler(this.cboXsdFile_TextChanged);
            // 
            // btnValidate
            // 
            this.btnValidate.Location = new System.Drawing.Point(165, 28);
            this.btnValidate.Name = "btnValidate";
            this.btnValidate.Size = new System.Drawing.Size(96, 23);
            this.btnValidate.TabIndex = 10;
            this.btnValidate.Text = "Validate";
            this.btnValidate.UseVisualStyleBackColor = true;
            this.btnValidate.Click += new System.EventHandler(this.btnValidate_Click);
            // 
            // btnSelectXSD
            // 
            this.btnSelectXSD.Location = new System.Drawing.Point(63, 28);
            this.btnSelectXSD.Name = "btnSelectXSD";
            this.btnSelectXSD.Size = new System.Drawing.Size(96, 23);
            this.btnSelectXSD.TabIndex = 9;
            this.btnSelectXSD.Text = "Select Xsd...";
            this.btnSelectXSD.UseVisualStyleBackColor = true;
            this.btnSelectXSD.Click += new System.EventHandler(this.btnSelectXSD_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "XSD File:";
            // 
            // txtValidationEvents
            // 
            this.txtValidationEvents.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtValidationEvents.Location = new System.Drawing.Point(0, 0);
            this.txtValidationEvents.Name = "txtValidationEvents";
            this.txtValidationEvents.Size = new System.Drawing.Size(611, 482);
            this.txtValidationEvents.TabIndex = 11;
            this.txtValidationEvents.Text = "";
            // 
            // labLink
            // 
            this.labLink.ActiveLinkColor = System.Drawing.Color.Gray;
            this.labLink.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labLink.AutoSize = true;
            this.labLink.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.labLink.LinkColor = System.Drawing.Color.Gray;
            this.labLink.Location = new System.Drawing.Point(8, 614);
            this.labLink.Name = "labLink";
            this.labLink.Size = new System.Drawing.Size(239, 13);
            this.labLink.TabIndex = 12;
            this.labLink.TabStop = true;
            this.labLink.Text = "http://dansharpxmlviewer.codeplex.com";
            this.labLink.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.labelToolTip.SetToolTip(this.labLink, "Click here to visit the web site for this utility");
            this.labLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.labLink_LinkClicked);
            // 
            // labVersion
            // 
            this.labVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labVersion.BackColor = System.Drawing.Color.Transparent;
            this.labVersion.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labVersion.ForeColor = System.Drawing.Color.Gray;
            this.labVersion.Location = new System.Drawing.Point(534, 615);
            this.labVersion.Margin = new System.Windows.Forms.Padding(0);
            this.labVersion.Name = "labVersion";
            this.labVersion.Size = new System.Drawing.Size(100, 13);
            this.labVersion.TabIndex = 9;
            this.labVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // mnuTreeView
            // 
            this.mnuTreeView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToolStripMenuItem,
            this.toolStripMenuItem3,
            this.expandNodeToolStripMenuItem,
            this.collapseNodeToolStripMenuItem,
            this.toolStripMenuItem2,
            this.expandAllNodesToolStripMenuItem,
            this.collapseAllNodesToolStripMenuItem});
            this.mnuTreeView.Name = "mnuTreeView";
            this.mnuTreeView.Size = new System.Drawing.Size(162, 126);
            this.mnuTreeView.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.mnuTreeView_ItemClicked);
            // 
            // copyToolStripMenuItem
            // 
            this.copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            this.copyToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.copyToolStripMenuItem.Text = "Copy";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(158, 6);
            // 
            // expandNodeToolStripMenuItem
            // 
            this.expandNodeToolStripMenuItem.Name = "expandNodeToolStripMenuItem";
            this.expandNodeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.expandNodeToolStripMenuItem.Text = "Expand Node";
            // 
            // collapseNodeToolStripMenuItem
            // 
            this.collapseNodeToolStripMenuItem.Name = "collapseNodeToolStripMenuItem";
            this.collapseNodeToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.collapseNodeToolStripMenuItem.Text = "Collapse Node";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 6);
            // 
            // expandAllNodesToolStripMenuItem
            // 
            this.expandAllNodesToolStripMenuItem.Name = "expandAllNodesToolStripMenuItem";
            this.expandAllNodesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.expandAllNodesToolStripMenuItem.Text = "Expand All Nodes";
            // 
            // collapseAllNodesToolStripMenuItem
            // 
            this.collapseAllNodesToolStripMenuItem.Name = "collapseAllNodesToolStripMenuItem";
            this.collapseAllNodesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.collapseAllNodesToolStripMenuItem.Text = "Collapse All Nodes";
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 630);
            this.Controls.Add(this.labVersion);
            this.Controls.Add(this.labLink);
            this.Controls.Add(this.ctlTab);
            this.Controls.Add(this.labFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.mnuStrip);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.mnuStrip;
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DanSharp Xml Viewer";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Main_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Main_DragEnter);
            this.mnuStrip.ResumeLayout(false);
            this.mnuStrip.PerformLayout();
            this.panTreeView.Panel1.ResumeLayout(false);
            this.panTreeView.Panel2.ResumeLayout(false);
            this.panTreeView.Panel2.PerformLayout();
            this.panTreeView.ResumeLayout(false);
            this.ctlTab.ResumeLayout(false);
            this.pageTrv.ResumeLayout(false);
            this.pageXml.ResumeLayout(false);
            this.pageXPath.ResumeLayout(false);
            this.panXPath.Panel1.ResumeLayout(false);
            this.panXPath.Panel1.PerformLayout();
            this.panXPath.Panel2.ResumeLayout(false);
            this.panXPath.ResumeLayout(false);
            this.pageXSD.ResumeLayout(false);
            this.panXsdVal.Panel1.ResumeLayout(false);
            this.panXsdVal.Panel1.PerformLayout();
            this.panXsdVal.Panel2.ResumeLayout(false);
            this.panXsdVal.ResumeLayout(false);
            this.mnuTreeView.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip mnuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.Label labFileName;
        private System.Windows.Forms.TreeView trvXml;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.OpenFileDialog dlgOpenFile;
        private System.Windows.Forms.SplitContainer panTreeView;
        private System.Windows.Forms.TabControl ctlTab;
        private System.Windows.Forms.TabPage pageTrv;
        private System.Windows.Forms.TabPage pageXml;
        private System.Windows.Forms.TabPage pageXPath;
        private System.Windows.Forms.TabPage pageXSD;
        private System.Windows.Forms.SplitContainer panXPath;
        private System.Windows.Forms.WebBrowser webBrowser;
        private System.Windows.Forms.SplitContainer panXsdVal;
        private System.Windows.Forms.TreeView trvXpathResult;
        private System.Windows.Forms.RichTextBox txtValidationEvents;
        private System.Windows.Forms.TextBox txtXPathQuery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSelectXSD;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnValidate;
        private System.Windows.Forms.Button btnExecuteQuery;
        private System.Windows.Forms.ToolStripSeparator mnuRecentListUpperBar;
        private System.Windows.Forms.ToolStripMenuItem mnuRecentFiles_None;
        private System.Windows.Forms.ToolStripSeparator mnuRecentListLowerBar;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resetRecentlyUsedFileListToolStripMenuItem;
        private System.Windows.Forms.ToolTip labelToolTip;
        private System.Windows.Forms.ComboBox cboXsdFile;
        private System.Windows.Forms.ToolStripMenuItem reloadFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateBizUnitTestCaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.Label labVersion;
        private System.Windows.Forms.LinkLabel labLink;
        private System.Windows.Forms.ContextMenuStrip mnuTreeView;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem expandNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseNodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem expandAllNodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem collapseAllNodesToolStripMenuItem;
    }
}

namespace InfoService.GUIConfiguration
{
    partial class AdvancedConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedConfigForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.gbLogging = new System.Windows.Forms.GroupBox();
            this.btnOpenLog = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnOpenLogFolder = new System.Windows.Forms.Button();
            this.cbDebug = new System.Windows.Forms.CheckBox();
            this.cbError = new System.Windows.Forms.CheckBox();
            this.cbWarning = new System.Windows.Forms.CheckBox();
            this.cbInfo = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.gbCaching = new System.Windows.Forms.GroupBox();
            this.btnBrowseTwitterCacheFolder = new System.Windows.Forms.Button();
            this.txtTwitterCacheFolder = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseFeedCacheFolder = new System.Windows.Forms.Button();
            this.txtFeedCacheFolder = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFeedDeleteCache = new System.Windows.Forms.Button();
            this.btnTwitterDeleteCache = new System.Windows.Forms.Button();
            this.coTwitterDeleteCache = new System.Windows.Forms.ComboBox();
            this.coFeedDeleteCache = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.cbDeveloperMode = new System.Windows.Forms.CheckBox();
            this.txtPluginName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbWebBrowser = new System.Windows.Forms.GroupBox();
            this.laWebBrowserPlaceholders = new System.Windows.Forms.Label();
            this.txtWebBrowserWindowID = new System.Windows.Forms.TextBox();
            this.laWebBrowserWindowID = new System.Windows.Forms.Label();
            this.dgWebBrowserData = new System.Windows.Forms.DataGridView();
            this.Property = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cbWebBrowserSelector = new System.Windows.Forms.ComboBox();
            this.ttWebBrowserValueHelp = new System.Windows.Forms.ToolTip(this.components);
            this.CacheFolderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btnExportOPML = new System.Windows.Forms.Button();
            this.btnFeedItemsFilters = new System.Windows.Forms.Button();
            this.saveOPMLFile = new System.Windows.Forms.SaveFileDialog();
            this.gbLogging.SuspendLayout();
            this.gbCaching.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.gbWebBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWebBrowserData)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbLogging
            // 
            this.gbLogging.Controls.Add(this.btnOpenLog);
            this.gbLogging.Controls.Add(this.btnOpenLogFolder);
            this.gbLogging.Controls.Add(this.cbDebug);
            this.gbLogging.Controls.Add(this.cbError);
            this.gbLogging.Controls.Add(this.cbWarning);
            this.gbLogging.Controls.Add(this.cbInfo);
            this.gbLogging.Controls.Add(this.label1);
            this.gbLogging.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbLogging.Location = new System.Drawing.Point(6, 180);
            this.gbLogging.Name = "gbLogging";
            this.gbLogging.Size = new System.Drawing.Size(462, 119);
            this.gbLogging.TabIndex = 1;
            this.gbLogging.TabStop = false;
            this.gbLogging.Text = "Logging";
            // 
            // btnOpenLog
            // 
            this.btnOpenLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenLog.ImageKey = "page_white_text.png";
            this.btnOpenLog.ImageList = this.imgList;
            this.btnOpenLog.Location = new System.Drawing.Point(337, 47);
            this.btnOpenLog.Name = "btnOpenLog";
            this.btnOpenLog.Size = new System.Drawing.Size(114, 23);
            this.btnOpenLog.TabIndex = 6;
            this.btnOpenLog.Text = "Open log";
            this.btnOpenLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenLog.UseVisualStyleBackColor = true;
            this.btnOpenLog.Click += new System.EventHandler(this.btnOpenLog_Click);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "page_white_text.png");
            this.imgList.Images.SetKeyName(1, "folder_page_white.png");
            this.imgList.Images.SetKeyName(2, "database_delete.png");
            this.imgList.Images.SetKeyName(3, "folder_explore.png");
            this.imgList.Images.SetKeyName(4, "feed_magnify.png");
            this.imgList.Images.SetKeyName(5, "feed_disk.png");
            // 
            // btnOpenLogFolder
            // 
            this.btnOpenLogFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenLogFolder.ImageKey = "folder_page_white.png";
            this.btnOpenLogFolder.ImageList = this.imgList;
            this.btnOpenLogFolder.Location = new System.Drawing.Point(337, 19);
            this.btnOpenLogFolder.Name = "btnOpenLogFolder";
            this.btnOpenLogFolder.Size = new System.Drawing.Size(114, 23);
            this.btnOpenLogFolder.TabIndex = 5;
            this.btnOpenLogFolder.Text = "Open log folder";
            this.btnOpenLogFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnOpenLogFolder.UseVisualStyleBackColor = true;
            this.btnOpenLogFolder.Click += new System.EventHandler(this.btnOpenLogFolder_Click);
            // 
            // cbDebug
            // 
            this.cbDebug.AutoSize = true;
            this.cbDebug.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDebug.Location = new System.Drawing.Point(87, 93);
            this.cbDebug.Name = "cbDebug";
            this.cbDebug.Size = new System.Drawing.Size(58, 17);
            this.cbDebug.TabIndex = 4;
            this.cbDebug.Text = "Debug";
            this.cbDebug.UseVisualStyleBackColor = true;
            this.cbDebug.CheckedChanged += new System.EventHandler(this.cbDebug_CheckedChanged);
            // 
            // cbError
            // 
            this.cbError.AutoSize = true;
            this.cbError.Checked = true;
            this.cbError.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbError.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbError.Location = new System.Drawing.Point(87, 70);
            this.cbError.Name = "cbError";
            this.cbError.Size = new System.Drawing.Size(48, 17);
            this.cbError.TabIndex = 3;
            this.cbError.Text = "Error";
            this.cbError.UseVisualStyleBackColor = true;
            // 
            // cbWarning
            // 
            this.cbWarning.AutoSize = true;
            this.cbWarning.Checked = true;
            this.cbWarning.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbWarning.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWarning.Location = new System.Drawing.Point(87, 47);
            this.cbWarning.Name = "cbWarning";
            this.cbWarning.Size = new System.Drawing.Size(66, 17);
            this.cbWarning.TabIndex = 2;
            this.cbWarning.Text = "Warning";
            this.cbWarning.UseVisualStyleBackColor = true;
            // 
            // cbInfo
            // 
            this.cbInfo.AutoSize = true;
            this.cbInfo.Checked = true;
            this.cbInfo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbInfo.Enabled = false;
            this.cbInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbInfo.Location = new System.Drawing.Point(87, 24);
            this.cbInfo.Name = "cbInfo";
            this.cbInfo.Size = new System.Drawing.Size(44, 17);
            this.cbInfo.TabIndex = 1;
            this.cbInfo.Text = "Info";
            this.cbInfo.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Log verbosity:";
            // 
            // gbCaching
            // 
            this.gbCaching.Controls.Add(this.btnBrowseTwitterCacheFolder);
            this.gbCaching.Controls.Add(this.txtTwitterCacheFolder);
            this.gbCaching.Controls.Add(this.label6);
            this.gbCaching.Controls.Add(this.btnBrowseFeedCacheFolder);
            this.gbCaching.Controls.Add(this.txtFeedCacheFolder);
            this.gbCaching.Controls.Add(this.label5);
            this.gbCaching.Controls.Add(this.btnFeedDeleteCache);
            this.gbCaching.Controls.Add(this.btnTwitterDeleteCache);
            this.gbCaching.Controls.Add(this.coTwitterDeleteCache);
            this.gbCaching.Controls.Add(this.coFeedDeleteCache);
            this.gbCaching.Controls.Add(this.label3);
            this.gbCaching.Controls.Add(this.label2);
            this.gbCaching.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbCaching.Location = new System.Drawing.Point(6, 305);
            this.gbCaching.Name = "gbCaching";
            this.gbCaching.Size = new System.Drawing.Size(462, 152);
            this.gbCaching.TabIndex = 2;
            this.gbCaching.TabStop = false;
            this.gbCaching.Text = "Caching";
            // 
            // btnBrowseTwitterCacheFolder
            // 
            this.btnBrowseTwitterCacheFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseTwitterCacheFolder.ImageKey = "folder_explore.png";
            this.btnBrowseTwitterCacheFolder.ImageList = this.imgList;
            this.btnBrowseTwitterCacheFolder.Location = new System.Drawing.Point(421, 91);
            this.btnBrowseTwitterCacheFolder.Name = "btnBrowseTwitterCacheFolder";
            this.btnBrowseTwitterCacheFolder.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseTwitterCacheFolder.TabIndex = 35;
            this.btnBrowseTwitterCacheFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseTwitterCacheFolder.UseVisualStyleBackColor = true;
            this.btnBrowseTwitterCacheFolder.Click += new System.EventHandler(this.btnBrowseTwitterCacheFolder_Click);
            // 
            // txtTwitterCacheFolder
            // 
            this.txtTwitterCacheFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTwitterCacheFolder.Location = new System.Drawing.Point(122, 93);
            this.txtTwitterCacheFolder.Name = "txtTwitterCacheFolder";
            this.txtTwitterCacheFolder.Size = new System.Drawing.Size(293, 20);
            this.txtTwitterCacheFolder.TabIndex = 34;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(9, 96);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(107, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "Twitter cache folder: ";
            // 
            // btnBrowseFeedCacheFolder
            // 
            this.btnBrowseFeedCacheFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFeedCacheFolder.ImageKey = "folder_explore.png";
            this.btnBrowseFeedCacheFolder.ImageList = this.imgList;
            this.btnBrowseFeedCacheFolder.Location = new System.Drawing.Point(421, 24);
            this.btnBrowseFeedCacheFolder.Name = "btnBrowseFeedCacheFolder";
            this.btnBrowseFeedCacheFolder.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseFeedCacheFolder.TabIndex = 32;
            this.btnBrowseFeedCacheFolder.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseFeedCacheFolder.UseVisualStyleBackColor = true;
            this.btnBrowseFeedCacheFolder.Click += new System.EventHandler(this.btnBrowseFeedCacheFolder_Click);
            // 
            // txtFeedCacheFolder
            // 
            this.txtFeedCacheFolder.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFeedCacheFolder.Location = new System.Drawing.Point(122, 26);
            this.txtFeedCacheFolder.Name = "txtFeedCacheFolder";
            this.txtFeedCacheFolder.Size = new System.Drawing.Size(293, 20);
            this.txtFeedCacheFolder.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(9, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(99, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Feed cache folder: ";
            // 
            // btnFeedDeleteCache
            // 
            this.btnFeedDeleteCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFeedDeleteCache.ImageKey = "database_delete.png";
            this.btnFeedDeleteCache.ImageList = this.imgList;
            this.btnFeedDeleteCache.Location = new System.Drawing.Point(326, 52);
            this.btnFeedDeleteCache.Name = "btnFeedDeleteCache";
            this.btnFeedDeleteCache.Size = new System.Drawing.Size(125, 23);
            this.btnFeedDeleteCache.TabIndex = 5;
            this.btnFeedDeleteCache.Text = "Clear feed cache";
            this.btnFeedDeleteCache.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFeedDeleteCache.UseVisualStyleBackColor = true;
            this.btnFeedDeleteCache.Click += new System.EventHandler(this.btnFeedDeleteCache_Click);
            // 
            // btnTwitterDeleteCache
            // 
            this.btnTwitterDeleteCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTwitterDeleteCache.ImageKey = "database_delete.png";
            this.btnTwitterDeleteCache.ImageList = this.imgList;
            this.btnTwitterDeleteCache.Location = new System.Drawing.Point(326, 119);
            this.btnTwitterDeleteCache.Name = "btnTwitterDeleteCache";
            this.btnTwitterDeleteCache.Size = new System.Drawing.Size(125, 23);
            this.btnTwitterDeleteCache.TabIndex = 6;
            this.btnTwitterDeleteCache.Text = "Clear twitter cache";
            this.btnTwitterDeleteCache.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnTwitterDeleteCache.UseVisualStyleBackColor = true;
            this.btnTwitterDeleteCache.Click += new System.EventHandler(this.btnTwitterDeleteCache_Click);
            // 
            // coTwitterDeleteCache
            // 
            this.coTwitterDeleteCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coTwitterDeleteCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coTwitterDeleteCache.FormattingEnabled = true;
            this.coTwitterDeleteCache.Items.AddRange(new object[] {
            "Day",
            "Week",
            "Month"});
            this.coTwitterDeleteCache.Location = new System.Drawing.Point(148, 119);
            this.coTwitterDeleteCache.Name = "coTwitterDeleteCache";
            this.coTwitterDeleteCache.Size = new System.Drawing.Size(146, 21);
            this.coTwitterDeleteCache.TabIndex = 3;
            // 
            // coFeedDeleteCache
            // 
            this.coFeedDeleteCache.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.coFeedDeleteCache.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.coFeedDeleteCache.FormattingEnabled = true;
            this.coFeedDeleteCache.Items.AddRange(new object[] {
            "Day",
            "Week",
            "Month"});
            this.coFeedDeleteCache.Location = new System.Drawing.Point(148, 52);
            this.coFeedDeleteCache.Name = "coFeedDeleteCache";
            this.coFeedDeleteCache.Size = new System.Drawing.Size(146, 21);
            this.coFeedDeleteCache.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(8, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(134, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Delete twitter cache every:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(9, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(127, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Delete feed cache every:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(6, 652);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(84, 652);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbGeneral
            // 
            this.gbGeneral.Controls.Add(this.cbDeveloperMode);
            this.gbGeneral.Controls.Add(this.txtPluginName);
            this.gbGeneral.Controls.Add(this.label4);
            this.gbGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbGeneral.Location = new System.Drawing.Point(6, 3);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.Size = new System.Drawing.Size(462, 86);
            this.gbGeneral.TabIndex = 0;
            this.gbGeneral.TabStop = false;
            this.gbGeneral.Text = "General";
            // 
            // cbDeveloperMode
            // 
            this.cbDeveloperMode.AutoSize = true;
            this.cbDeveloperMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbDeveloperMode.Location = new System.Drawing.Point(12, 58);
            this.cbDeveloperMode.Name = "cbDeveloperMode";
            this.cbDeveloperMode.Size = new System.Drawing.Size(138, 17);
            this.cbDeveloperMode.TabIndex = 2;
            this.cbDeveloperMode.Text = "Enable developer mode";
            this.cbDeveloperMode.UseVisualStyleBackColor = true;
            // 
            // txtPluginName
            // 
            this.txtPluginName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPluginName.Location = new System.Drawing.Point(161, 26);
            this.txtPluginName.Name = "txtPluginName";
            this.txtPluginName.Size = new System.Drawing.Size(290, 20);
            this.txtPluginName.TabIndex = 1;
            this.txtPluginName.Text = "InfoService";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(147, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Plugin name on home-screen:";
            // 
            // gbWebBrowser
            // 
            this.gbWebBrowser.Controls.Add(this.laWebBrowserPlaceholders);
            this.gbWebBrowser.Controls.Add(this.txtWebBrowserWindowID);
            this.gbWebBrowser.Controls.Add(this.laWebBrowserWindowID);
            this.gbWebBrowser.Controls.Add(this.dgWebBrowserData);
            this.gbWebBrowser.Controls.Add(this.cbWebBrowserSelector);
            this.gbWebBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbWebBrowser.Location = new System.Drawing.Point(6, 463);
            this.gbWebBrowser.Name = "gbWebBrowser";
            this.gbWebBrowser.Size = new System.Drawing.Size(462, 183);
            this.gbWebBrowser.TabIndex = 3;
            this.gbWebBrowser.TabStop = false;
            this.gbWebBrowser.Text = "Webbrowser";
            // 
            // laWebBrowserPlaceholders
            // 
            this.laWebBrowserPlaceholders.AutoSize = true;
            this.laWebBrowserPlaceholders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laWebBrowserPlaceholders.Location = new System.Drawing.Point(9, 162);
            this.laWebBrowserPlaceholders.Name = "laWebBrowserPlaceholders";
            this.laWebBrowserPlaceholders.Size = new System.Drawing.Size(357, 13);
            this.laWebBrowserPlaceholders.TabIndex = 4;
            this.laWebBrowserPlaceholders.Text = "Move your mouse over the Value header to see the available placeholders";
            // 
            // txtWebBrowserWindowID
            // 
            this.txtWebBrowserWindowID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWebBrowserWindowID.Location = new System.Drawing.Point(354, 23);
            this.txtWebBrowserWindowID.Name = "txtWebBrowserWindowID";
            this.txtWebBrowserWindowID.Size = new System.Drawing.Size(97, 20);
            this.txtWebBrowserWindowID.TabIndex = 2;
            this.txtWebBrowserWindowID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // laWebBrowserWindowID
            // 
            this.laWebBrowserWindowID.AutoSize = true;
            this.laWebBrowserWindowID.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.laWebBrowserWindowID.Location = new System.Drawing.Point(285, 26);
            this.laWebBrowserWindowID.Name = "laWebBrowserWindowID";
            this.laWebBrowserWindowID.Size = new System.Drawing.Size(63, 13);
            this.laWebBrowserWindowID.TabIndex = 1;
            this.laWebBrowserWindowID.Text = "Window ID:";
            // 
            // dgWebBrowserData
            // 
            this.dgWebBrowserData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWebBrowserData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgWebBrowserData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgWebBrowserData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Property,
            this.Value});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgWebBrowserData.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgWebBrowserData.Location = new System.Drawing.Point(12, 50);
            this.dgWebBrowserData.Name = "dgWebBrowserData";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgWebBrowserData.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgWebBrowserData.RowHeadersWidth = 10;
            this.dgWebBrowserData.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgWebBrowserData.ShowCellToolTips = false;
            this.dgWebBrowserData.Size = new System.Drawing.Size(439, 106);
            this.dgWebBrowserData.TabIndex = 3;
            this.dgWebBrowserData.CellMouseEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgWebBrowserData_CellMouseEnter);
            // 
            // Property
            // 
            this.Property.HeaderText = "Property";
            this.Property.MaxInputLength = 512;
            this.Property.Name = "Property";
            this.Property.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Property.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Property.Width = 210;
            // 
            // Value
            // 
            this.Value.HeaderText = "Value";
            this.Value.MaxInputLength = 512;
            this.Value.Name = "Value";
            this.Value.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Value.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Value.Width = 210;
            // 
            // cbWebBrowserSelector
            // 
            this.cbWebBrowserSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWebBrowserSelector.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbWebBrowserSelector.FormattingEnabled = true;
            this.cbWebBrowserSelector.Items.AddRange(new object[] {
            "WebBrowser",
            "GeckoBrowser",
            "Other"});
            this.cbWebBrowserSelector.Location = new System.Drawing.Point(12, 23);
            this.cbWebBrowserSelector.Name = "cbWebBrowserSelector";
            this.cbWebBrowserSelector.Size = new System.Drawing.Size(141, 21);
            this.cbWebBrowserSelector.TabIndex = 0;
            this.cbWebBrowserSelector.SelectedIndexChanged += new System.EventHandler(this.cbWebBrowserSelector_SelectedIndexChanged);
            // 
            // ttWebBrowserValueHelp
            // 
            this.ttWebBrowserValueHelp.AutomaticDelay = 50;
            this.ttWebBrowserValueHelp.AutoPopDelay = 20000;
            this.ttWebBrowserValueHelp.InitialDelay = 50;
            this.ttWebBrowserValueHelp.ReshowDelay = 10;
            this.ttWebBrowserValueHelp.ShowAlways = true;
            this.ttWebBrowserValueHelp.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttWebBrowserValueHelp.ToolTipTitle = "Available Placeholders";
            // 
            // CacheFolderBrowserDialog
            // 
            this.CacheFolderBrowserDialog.Description = "Please select a new cache folder for twitter cache files.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnExportOPML);
            this.groupBox1.Controls.Add(this.btnFeedItemsFilters);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(6, 95);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(462, 79);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Feeds";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(9, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(318, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Export current feed list to a OPML file and import the list elsewhere";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(9, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(326, 13);
            this.label7.TabIndex = 10;
            this.label7.Text = "Edit feed item filters, to replace unwanted chars in title or description";
            // 
            // btnExportOPML
            // 
            this.btnExportOPML.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExportOPML.ImageKey = "feed_disk.png";
            this.btnExportOPML.ImageList = this.imgList;
            this.btnExportOPML.Location = new System.Drawing.Point(367, 48);
            this.btnExportOPML.Name = "btnExportOPML";
            this.btnExportOPML.Size = new System.Drawing.Size(84, 23);
            this.btnExportOPML.TabIndex = 9;
            this.btnExportOPML.Text = "&Export...";
            this.btnExportOPML.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnExportOPML.UseVisualStyleBackColor = true;
            this.btnExportOPML.Click += new System.EventHandler(this.btnExportOPML_Click);
            // 
            // btnFeedItemsFilters
            // 
            this.btnFeedItemsFilters.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btnFeedItemsFilters.ImageKey = "feed_magnify.png";
            this.btnFeedItemsFilters.ImageList = this.imgList;
            this.btnFeedItemsFilters.Location = new System.Drawing.Point(367, 19);
            this.btnFeedItemsFilters.Name = "btnFeedItemsFilters";
            this.btnFeedItemsFilters.Size = new System.Drawing.Size(84, 23);
            this.btnFeedItemsFilters.TabIndex = 8;
            this.btnFeedItemsFilters.Text = "E&dit...";
            this.btnFeedItemsFilters.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFeedItemsFilters.UseVisualStyleBackColor = true;
            this.btnFeedItemsFilters.Click += new System.EventHandler(this.btnFeedItemsFilters_Click);
            // 
            // saveOPMLFile
            // 
            this.saveOPMLFile.DefaultExt = "*.xml";
            this.saveOPMLFile.FileName = "infoservice_feed_export";
            this.saveOPMLFile.Filter = "XML-File|*.xml";
            // 
            // AdvancedConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 684);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.gbWebBrowser);
            this.Controls.Add(this.gbGeneral);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.gbCaching);
            this.Controls.Add(this.gbLogging);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InfoService advanced configuration";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.AdvancedConfigForm_HelpButtonClicked);
            this.Load += new System.EventHandler(this.AdvancedConfigForm_Load);
            this.gbLogging.ResumeLayout(false);
            this.gbLogging.PerformLayout();
            this.gbCaching.ResumeLayout(false);
            this.gbCaching.PerformLayout();
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbWebBrowser.ResumeLayout(false);
            this.gbWebBrowser.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgWebBrowserData)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbLogging;
        private System.Windows.Forms.GroupBox gbCaching;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox cbDebug;
        private System.Windows.Forms.CheckBox cbError;
        private System.Windows.Forms.CheckBox cbWarning;
        private System.Windows.Forms.CheckBox cbInfo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenLog;
        private System.Windows.Forms.Button btnOpenLogFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox coFeedDeleteCache;
        private System.Windows.Forms.ComboBox coTwitterDeleteCache;
        private System.Windows.Forms.Button btnFeedDeleteCache;
        private System.Windows.Forms.Button btnTwitterDeleteCache;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.TextBox txtPluginName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbWebBrowser;
        private System.Windows.Forms.ComboBox cbWebBrowserSelector;
        private System.Windows.Forms.DataGridView dgWebBrowserData;
        private System.Windows.Forms.TextBox txtWebBrowserWindowID;
        private System.Windows.Forms.Label laWebBrowserWindowID;
        private System.Windows.Forms.Label laWebBrowserPlaceholders;
        private System.Windows.Forms.ToolTip ttWebBrowserValueHelp;
        private System.Windows.Forms.TextBox txtFeedCacheFolder;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnBrowseFeedCacheFolder;
        private System.Windows.Forms.Button btnBrowseTwitterCacheFolder;
        private System.Windows.Forms.TextBox txtTwitterCacheFolder;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.FolderBrowserDialog CacheFolderBrowserDialog;
        private System.Windows.Forms.DataGridViewTextBoxColumn Property;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFeedItemsFilters;
        private System.Windows.Forms.Button btnExportOPML;
        private System.Windows.Forms.SaveFileDialog saveOPMLFile;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox cbDeveloperMode;
    }
}
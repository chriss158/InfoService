namespace InfoService.GUIConfiguration
{
    partial class FilterConfigForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilterConfigForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.gbWebBrowser = new System.Windows.Forms.GroupBox();
            this.btnRem = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.dgFeedItemsFilters = new System.Windows.Forms.DataGridView();
            this.IsEnabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.IsRegEx = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ReplaceThis = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReplaceWith = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UseInTitle = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.UseInBody = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CleanBefore = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.gbWebBrowser.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgFeedItemsFilters)).BeginInit();
            this.SuspendLayout();
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "page_white_text.png");
            this.imgList.Images.SetKeyName(1, "folder_page_white.png");
            this.imgList.Images.SetKeyName(2, "database_delete.png");
            this.imgList.Images.SetKeyName(3, "folder_explore.png");
            this.imgList.Images.SetKeyName(4, "script_add.png");
            this.imgList.Images.SetKeyName(5, "script_delete.png");
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSave.Location = new System.Drawing.Point(6, 397);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "&Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(84, 397);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // gbWebBrowser
            // 
            this.gbWebBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbWebBrowser.Controls.Add(this.btnRem);
            this.gbWebBrowser.Controls.Add(this.btnAdd);
            this.gbWebBrowser.Controls.Add(this.dgFeedItemsFilters);
            this.gbWebBrowser.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbWebBrowser.Location = new System.Drawing.Point(6, 3);
            this.gbWebBrowser.Name = "gbWebBrowser";
            this.gbWebBrowser.Size = new System.Drawing.Size(810, 387);
            this.gbWebBrowser.TabIndex = 3;
            this.gbWebBrowser.TabStop = false;
            this.gbWebBrowser.Text = "Feed item filters";
            // 
            // btnRem
            // 
            this.btnRem.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRem.Enabled = false;
            this.btnRem.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRem.ImageKey = "script_delete.png";
            this.btnRem.ImageList = this.imgList;
            this.btnRem.Location = new System.Drawing.Point(720, 358);
            this.btnRem.Name = "btnRem";
            this.btnRem.Size = new System.Drawing.Size(84, 23);
            this.btnRem.TabIndex = 5;
            this.btnRem.Text = "&Remove..";
            this.btnRem.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnRem.UseVisualStyleBackColor = true;
            this.btnRem.Click += new System.EventHandler(this.btnRem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdd.ImageKey = "script_add.png";
            this.btnAdd.ImageList = this.imgList;
            this.btnAdd.Location = new System.Drawing.Point(633, 358);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(81, 23);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "&Add..";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // dgFeedItemsFilters
            // 
            this.dgFeedItemsFilters.AllowUserToAddRows = false;
            this.dgFeedItemsFilters.AllowUserToDeleteRows = false;
            this.dgFeedItemsFilters.AllowUserToResizeColumns = false;
            this.dgFeedItemsFilters.AllowUserToResizeRows = false;
            this.dgFeedItemsFilters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgFeedItemsFilters.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgFeedItemsFilters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgFeedItemsFilters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.IsEnabled,
            this.IsRegEx,
            this.ReplaceThis,
            this.ReplaceWith,
            this.UseInTitle,
            this.UseInBody,
            this.CleanBefore});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgFeedItemsFilters.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgFeedItemsFilters.Location = new System.Drawing.Point(6, 19);
            this.dgFeedItemsFilters.MultiSelect = false;
            this.dgFeedItemsFilters.Name = "dgFeedItemsFilters";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgFeedItemsFilters.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgFeedItemsFilters.RowHeadersWidth = 10;
            this.dgFeedItemsFilters.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgFeedItemsFilters.ShowCellToolTips = false;
            this.dgFeedItemsFilters.Size = new System.Drawing.Size(798, 333);
            this.dgFeedItemsFilters.TabIndex = 3;
            this.dgFeedItemsFilters.SelectionChanged += new System.EventHandler(this.dgFeedItemsFilters_SelectionChanged);
            // 
            // IsEnabled
            // 
            this.IsEnabled.HeaderText = "Enabled";
            this.IsEnabled.Name = "IsEnabled";
            this.IsEnabled.Width = 65;
            // 
            // IsRegEx
            // 
            this.IsRegEx.HeaderText = "Is RegEx";
            this.IsRegEx.Name = "IsRegEx";
            this.IsRegEx.Width = 65;
            // 
            // ReplaceThis
            // 
            this.ReplaceThis.HeaderText = "Replace this...";
            this.ReplaceThis.MaxInputLength = 512;
            this.ReplaceThis.Name = "ReplaceThis";
            this.ReplaceThis.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ReplaceThis.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReplaceThis.Width = 210;
            // 
            // ReplaceWith
            // 
            this.ReplaceWith.HeaderText = "...with this";
            this.ReplaceWith.MaxInputLength = 512;
            this.ReplaceWith.Name = "ReplaceWith";
            this.ReplaceWith.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ReplaceWith.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ReplaceWith.Width = 210;
            // 
            // UseInTitle
            // 
            this.UseInTitle.HeaderText = "...in title";
            this.UseInTitle.Name = "UseInTitle";
            this.UseInTitle.Width = 70;
            // 
            // UseInBody
            // 
            this.UseInBody.HeaderText = "...in body";
            this.UseInBody.Name = "UseInBody";
            this.UseInBody.Width = 70;
            // 
            // CleanBefore
            // 
            this.CleanBefore.HeaderText = "..before HTML parse";
            this.CleanBefore.Name = "CleanBefore";
            this.CleanBefore.Width = 70;
            // 
            // FilterConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(823, 425);
            this.Controls.Add(this.gbWebBrowser);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterConfigForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "InfoService feed items filters";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FilterConfigForm_HelpButtonClicked);
            this.Load += new System.EventHandler(this.FilterConfigForm_Load);
            this.gbWebBrowser.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgFeedItemsFilters)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.GroupBox gbWebBrowser;
        private System.Windows.Forms.DataGridView dgFeedItemsFilters;
        private System.Windows.Forms.Button btnRem;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsEnabled;
        private System.Windows.Forms.DataGridViewCheckBoxColumn IsRegEx;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplaceThis;
        private System.Windows.Forms.DataGridViewTextBoxColumn ReplaceWith;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseInTitle;
        private System.Windows.Forms.DataGridViewCheckBoxColumn UseInBody;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CleanBefore;
    }
}
namespace InfoService.GUIConfiguration
{
    partial class FeedForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedForm));
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAddEdit = new System.Windows.Forms.Button();
            this.openImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cbShowPopupForFeed = new System.Windows.Forms.CheckBox();
            this.btnBrowseFeed = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnDownloadDefaultName = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.btnBrowseImage = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtDefaultZoom = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.txtFeedURL_PATH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtImage = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.openFeedDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDefaultZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(85, 179);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAddEdit
            // 
            this.btnAddEdit.Location = new System.Drawing.Point(4, 179);
            this.btnAddEdit.Name = "btnAddEdit";
            this.btnAddEdit.Size = new System.Drawing.Size(75, 23);
            this.btnAddEdit.TabIndex = 4;
            this.btnAddEdit.Text = "&Add..";
            this.btnAddEdit.UseVisualStyleBackColor = true;
            this.btnAddEdit.Click += new System.EventHandler(this.btnAddEdit_Click);
            // 
            // openImageDialog
            // 
            this.openImageDialog.Filter = "Jpg files|*.jpg|Png files|*.png|Bmp files|*.bmp";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cbShowPopupForFeed);
            this.groupBox1.Controls.Add(this.btnBrowseFeed);
            this.groupBox1.Controls.Add(this.btnDownloadDefaultName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnBrowseImage);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtDefaultZoom);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtFeedURL_PATH);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtImage);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTitle);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(498, 169);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Feed informations";
            // 
            // cbShowPopupForFeed
            // 
            this.cbShowPopupForFeed.AutoSize = true;
            this.cbShowPopupForFeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbShowPopupForFeed.Location = new System.Drawing.Point(13, 143);
            this.cbShowPopupForFeed.Name = "cbShowPopupForFeed";
            this.cbShowPopupForFeed.Size = new System.Drawing.Size(144, 17);
            this.cbShowPopupForFeed.TabIndex = 32;
            this.cbShowPopupForFeed.Text = "Show popup for this feed";
            this.cbShowPopupForFeed.UseVisualStyleBackColor = true;
            // 
            // btnBrowseFeed
            // 
            this.btnBrowseFeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFeed.ImageKey = "folder_explore.png";
            this.btnBrowseFeed.ImageList = this.imgList;
            this.btnBrowseFeed.Location = new System.Drawing.Point(461, 28);
            this.btnBrowseFeed.Name = "btnBrowseFeed";
            this.btnBrowseFeed.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseFeed.TabIndex = 31;
            this.btnBrowseFeed.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseFeed.UseVisualStyleBackColor = true;
            this.btnBrowseFeed.Click += new System.EventHandler(this.btnBrowseFeed_Click);
            // 
            // imgList
            // 
            this.imgList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgList.ImageStream")));
            this.imgList.TransparentColor = System.Drawing.Color.Transparent;
            this.imgList.Images.SetKeyName(0, "feed_go.png");
            this.imgList.Images.SetKeyName(1, "folder_explore.png");
            // 
            // btnDownloadDefaultName
            // 
            this.btnDownloadDefaultName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDownloadDefaultName.ImageKey = "feed_go.png";
            this.btnDownloadDefaultName.ImageList = this.imgList;
            this.btnDownloadDefaultName.Location = new System.Drawing.Point(358, 55);
            this.btnDownloadDefaultName.Name = "btnDownloadDefaultName";
            this.btnDownloadDefaultName.Size = new System.Drawing.Size(133, 23);
            this.btnDownloadDefaultName.TabIndex = 30;
            this.btnDownloadDefaultName.Text = "Get default feed title";
            this.btnDownloadDefaultName.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnDownloadDefaultName.UseVisualStyleBackColor = true;
            this.btnDownloadDefaultName.Click += new System.EventHandler(this.btnDownloadDefaultName_Click);
            // 
            // label7
            // 
            this.label7.Enabled = false;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(163, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(328, 32);
            this.label7.TabIndex = 28;
            this.label7.Text = "To use this option, please setup your webbrowser in the advance configuration of " +
    "InfoService.";
            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseImage.ImageKey = "folder_explore.png";
            this.btnBrowseImage.ImageList = this.imgList;
            this.btnBrowseImage.Location = new System.Drawing.Point(461, 82);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(30, 23);
            this.btnBrowseImage.TabIndex = 18;
            this.btnBrowseImage.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnBrowseImage.UseVisualStyleBackColor = true;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(142, 115);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 27;
            this.label6.Text = "%";
            // 
            // txtDefaultZoom
            // 
            this.txtDefaultZoom.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDefaultZoom.Location = new System.Drawing.Point(99, 110);
            this.txtDefaultZoom.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.txtDefaultZoom.Name = "txtDefaultZoom";
            this.txtDefaultZoom.Size = new System.Drawing.Size(43, 20);
            this.txtDefaultZoom.TabIndex = 26;
            this.txtDefaultZoom.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 115);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Default zoom:";
            // 
            // txtFeedURL_PATH
            // 
            this.txtFeedURL_PATH.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFeedURL_PATH.Location = new System.Drawing.Point(99, 30);
            this.txtFeedURL_PATH.Name = "txtFeedURL_PATH";
            this.txtFeedURL_PATH.Size = new System.Drawing.Size(356, 20);
            this.txtFeedURL_PATH.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Feed URL/Path:";
            // 
            // txtImage
            // 
            this.txtImage.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtImage.Location = new System.Drawing.Point(99, 84);
            this.txtImage.Name = "txtImage";
            this.txtImage.Size = new System.Drawing.Size(356, 20);
            this.txtImage.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Image:";
            // 
            // txtTitle
            // 
            this.txtTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTitle.Location = new System.Drawing.Point(99, 57);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(253, 20);
            this.txtTitle.TabIndex = 16;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Title:";
            // 
            // openFeedDialog
            // 
            this.openFeedDialog.AddExtension = false;
            this.openFeedDialog.DefaultExt = "feed";
            this.openFeedDialog.Filter = "All-Files|*.*";
            // 
            // FeedForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 210);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAddEdit);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeedForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Feed...";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FeedForm_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtDefaultZoom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAddEdit;
        private System.Windows.Forms.OpenFileDialog openImageDialog;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtFeedURL_PATH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.TextBox txtImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown txtDefaultZoom;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnDownloadDefaultName;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.Button btnBrowseFeed;
        private System.Windows.Forms.OpenFileDialog openFeedDialog;
        private System.Windows.Forms.CheckBox cbShowPopupForFeed;
    }
}
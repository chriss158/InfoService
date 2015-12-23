namespace InfoService.GUIConfiguration
{
    partial class FeedFormOPML
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FeedFormOPML));
            this.txtOPMLURL_PATH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseFeed = new System.Windows.Forms.Button();
            this.imgList = new System.Windows.Forms.ImageList(this.components);
            this.btnAddEdit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.openOPMLDialog = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtOPMLURL_PATH
            // 
            this.txtOPMLURL_PATH.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOPMLURL_PATH.Location = new System.Drawing.Point(105, 30);
            this.txtOPMLURL_PATH.Name = "txtOPMLURL_PATH";
            this.txtOPMLURL_PATH.Size = new System.Drawing.Size(317, 20);
            this.txtOPMLURL_PATH.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "OPML URL/Path";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnBrowseFeed);
            this.groupBox1.Controls.Add(this.txtOPMLURL_PATH);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(4, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 99);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "OPML informations";
            // 
            // btnBrowseFeed
            // 
            this.btnBrowseFeed.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowseFeed.ImageKey = "folder_explore.png";
            this.btnBrowseFeed.ImageList = this.imgList;
            this.btnBrowseFeed.Location = new System.Drawing.Point(428, 29);
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
            this.imgList.Images.SetKeyName(0, "folder_explore.png");
            // 
            // btnAddEdit
            // 
            this.btnAddEdit.Location = new System.Drawing.Point(4, 109);
            this.btnAddEdit.Name = "btnAddEdit";
            this.btnAddEdit.Size = new System.Drawing.Size(75, 23);
            this.btnAddEdit.TabIndex = 17;
            this.btnAddEdit.Text = "&Add..";
            this.btnAddEdit.UseVisualStyleBackColor = true;
            this.btnAddEdit.Click += new System.EventHandler(this.btnAddEdit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(85, 109);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 18;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // openOPMLDialog
            // 
            this.openOPMLDialog.AddExtension = false;
            this.openOPMLDialog.DefaultExt = "opml";
            this.openOPMLDialog.Filter = "All-Files|*.*";
            // 
            // label1
            // 
            this.label1.Enabled = false;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(10, 64);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(448, 32);
            this.label1.TabIndex = 32;
            this.label1.Text = "Attention: Every feed in the OPML file/url will be downloaded an checked if it is" +
    " a valid feed. If the OPML list is long, then the adding process can take a whil" +
    "e.";
            // 
            // FeedFormOPML
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(475, 140);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnAddEdit);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FeedFormOPML";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add multiple feeds...";
            this.HelpButtonClicked += new System.ComponentModel.CancelEventHandler(this.FeedFormOPML_HelpButtonClicked);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtOPMLURL_PATH;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnBrowseFeed;
        private System.Windows.Forms.Button btnAddEdit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ImageList imgList;
        private System.Windows.Forms.OpenFileDialog openOPMLDialog;
        private System.Windows.Forms.Label label1;
    }
}
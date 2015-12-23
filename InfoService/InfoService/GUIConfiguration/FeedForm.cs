using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using InfoService.Feeds;
using FeedReader;

namespace InfoService.GUIConfiguration
{
    public partial class FeedForm : Form
    {
        private string _feedUrlPath;
        private string _title;
        private string _image;
        private int _defaultzoom;
        private bool _popupShow;

        public string FeedUrlPath
        {
            get { return _feedUrlPath; }
            set { _feedUrlPath = value; }
        }
        public string FeedTitle
        {
            get { return _title; }
            set { _title = value; }
        }
        public string FeedImage
        {
            get { return _image; }
            set { _image = value; }
        }
        public int FeedDefaultZoom
        {
            get { return _defaultzoom; }
            set { _defaultzoom = value; }
        }

        public bool FeedPopupShow
        {
            get { return _popupShow; }
            set { _popupShow = value; }
        }

        public FeedForm(bool showPopupAllowed)
        {
            InitializeComponent();
            this.Text = "Add Feed...";
            btnAddEdit.Text = "Add...";
            cbShowPopupForFeed.Enabled = showPopupAllowed;
        }
        public FeedForm(string Feed, string Title, string Image, int DefaultZoom, bool showPopup, bool showPopupAllowed)
        {

            InitializeComponent();
            Text = "Edit Feed...";
            btnAddEdit.Text = "Save...";
            txtFeedURL_PATH.Text = Feed;
            txtTitle.Text = Title;
            txtImage.Text = Image;
            txtDefaultZoom.Value = Convert.ToDecimal(DefaultZoom);
            cbShowPopupForFeed.Checked = showPopup;
            cbShowPopupForFeed.Enabled = showPopupAllowed;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            if (openImageDialog.ShowDialog() == DialogResult.OK)
            {
                txtImage.Text = openImageDialog.FileName;
            }
        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            if (txtFeedURL_PATH.Text != String.Empty && txtTitle.Text != String.Empty)
            {

                  /*  XDocument feed = XDocument.Load(txtFeedURL.Text);
    

                XElement element = feed.Element("rss");
                if (element == null)
                {
                    XNamespace rdf = "http://www.w3.org/1999/02/22-rdf-syntax-ns#";
                    element = feed.Element(rdf + "RDF");
                    if (element == null)
                    {
                        XNamespace atom = "http://www.w3.org/2005/Atom";
                        element = feed.Element(atom + "feed");
                        if (element == null)
                        {
                            MessageBox.Show("The entered URL is no RDF, RSS or Atom feed. Please check your Feed URL!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }*/


                if (Utils.InfoServiceUtils.IsValidUrl(txtFeedURL_PATH.Text) || Utils.InfoServiceUtils.IsValidPath(txtFeedURL_PATH.Text))
                {
                    if (txtImage.Text.Length > 0)
                    {
                        try
                        {
                            Image testImage = Image.FromFile(txtImage.Text);
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("The selected image is not a valid image. Please select a valid image file or leave the text field empty.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    ExFeed testFeed = new ExFeed(txtFeedURL_PATH.Text);
                    Cursor.Current = Cursors.WaitCursor;
                    if (testFeed.Update(false))
                    {
                        Cursor.Current = Cursors.Default;
                        this.DialogResult = DialogResult.OK;
                        _feedUrlPath = txtFeedURL_PATH.Text;
                        _image = txtImage.Text;
                        _title = txtTitle.Text;
                        _defaultzoom = Convert.ToInt32(txtDefaultZoom.Value);
                        _popupShow = cbShowPopupForFeed.Checked;
                        Close();
                    }
                    else
                    {
                        MessageBox.Show(txtFeedURL_PATH.Text + " is not a valid feed.\n\n Please select a valid feed file or feed url!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(txtFeedURL_PATH.Text + " is not a valid URL/Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please fill out all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRemImage_Click(object sender, EventArgs e)
        {
            txtImage.Text = String.Empty;
        }
        private void llblWebBrowserLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://forum.team-mediaportal.com/mediaportal-plugins-47/webbrowser-v0-5-04-09-2009-new-70005/");
        }

        private void btnDownloadDefaultName_Click(object sender, EventArgs e)
        {
            if (Utils.InfoServiceUtils.IsValidUrl(txtFeedURL_PATH.Text) || Utils.InfoServiceUtils.IsValidPath(txtFeedURL_PATH.Text))
            {
                Cursor.Current = Cursors.WaitCursor;
                bool error = false;
                string msg = string.Empty;
                XDocument feed = new XDocument();
                try
                {
                    feed = XDocument.Load(txtFeedURL_PATH.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error downloading/loading from URL/Path: " + txtFeedURL_PATH.Text + "\n\n" + ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                try
                {

                    txtTitle.Text = (from ele in feed.Descendants("channel").Elements("title")
                                     select ele.Value).Single();

                }
                catch (Exception ex)
                {
                    error = true;
                    msg = ex.Message;
                }

                if (error)
                {
                    error = false;
                    try
                    {
                        XNamespace d = "http://purl.org/rss/1.0/";
                        txtTitle.Text = (from ele in feed.Descendants(d + "channel").Elements(d + "title") select ele.Value).Single();
                    }
                    catch (Exception ex)
                    {
                        error = true;
                        msg = ex.Message;
                    }

                    if(error)
                    {
                        error = false;
                        try
                        {
                            XNamespace atom = "http://www.w3.org/2005/Atom";
                            txtTitle.Text = (from ele in feed.Descendants(atom + "feed").Elements(atom + "title")
                                             select ele.Value).Single();
                        }
                        catch (Exception ex)
                        {
                            error = true;
                            msg = ex.Message;
                        }
                    }
                }

                if (error)
                {
                    MessageBox.Show("Error downloading/loading from URL/Path: " + txtFeedURL_PATH.Text + "\n\n" + msg, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                Cursor.Current = Cursors.Default;
            }
        }

        private void FeedForm_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Add%2fEdit%20Feed&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void btnBrowseFeed_Click(object sender, EventArgs e)
        {
            if (openFeedDialog.ShowDialog() == DialogResult.OK)
            {
                txtFeedURL_PATH.Text = openFeedDialog.FileName;
            }
        }

    }
}

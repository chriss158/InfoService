using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FeedReader;

namespace InfoService.GUIConfiguration
{
    public partial class FeedFormOPML : Form
    {

        public List<Feed> AddedFeeds { get; set; }
        public FeedFormOPML()
        {
            InitializeComponent();
            AddedFeeds = new List<Feed>();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void FeedFormOPML_HelpButtonClicked(object sender, CancelEventArgs e)
        {
            System.Diagnostics.Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Add%2fEdit%20Feed&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void btnBrowseFeed_Click(object sender, EventArgs e)
        {
            if (openOPMLDialog.ShowDialog() == DialogResult.OK)
            {
                txtOPMLURL_PATH.Text = openOPMLDialog.FileName;
            }
        }

        private void btnAddEdit_Click(object sender, EventArgs e)
        {
            bool errorOccured = false;
            string errorMessage = string.Empty;
            if (txtOPMLURL_PATH.Text != String.Empty)
            {
                if (Utils.InfoServiceUtils.IsValidUrl(txtOPMLURL_PATH.Text) || Utils.InfoServiceUtils.IsValidPath(txtOPMLURL_PATH.Text))
                {
                    try
                    {
                        Cursor.Current = Cursors.WaitCursor;
                        OPMLManager.OPMLManager opml = new OPMLManager.OPMLManager();
                        List<OPMLManager.Data.OPMLFeedItem> opmlFeeds = opml.ParseOPML(txtOPMLURL_PATH.Text);

                        if (opmlFeeds != null)
                        {
                            int i = 0;
                            foreach (OPMLManager.Data.OPMLFeedItem feed in opmlFeeds)
                            {
                                AddedFeeds.Add(new Feed(feed.XmlUrl));
                                if (AddedFeeds[i].Update(false))
                                {
                                    if (String.IsNullOrEmpty(AddedFeeds[i].Title))
                                    {
                                        if (String.IsNullOrEmpty(feed.Title))
                                        {
                                            AddedFeeds[i].Title = System.IO.Path.GetFileNameWithoutExtension(feed.XmlUrl);
                                        }
                                        else AddedFeeds[i].Title = feed.Title; 
                                    }
                                }
                                else
                                {
                                    errorOccured = true;
                                    errorMessage += feed.XmlUrl + "\n";
                                    AddedFeeds.RemoveAt(i);
                                    i--;
                                }
                                i++;
                            }

                            if (errorOccured)
                            {
                                MessageBox.Show("Following feeds could not be downloaded:\n\n" + errorMessage + "\nPlease check if the url is a valid RSS/RSS2/ATOM feed. The above mentioned feeds will not be added!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            Cursor.Current = Cursors.Default;
                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("The selected OPML file/url is empty or not a valid OPML file. Please select another OPML file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("An error occured while adding/downloading feeds.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show(txtOPMLURL_PATH.Text + " is not a valid URL/Path", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }            
            else
            {
                MessageBox.Show("Please fill out all fields!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}

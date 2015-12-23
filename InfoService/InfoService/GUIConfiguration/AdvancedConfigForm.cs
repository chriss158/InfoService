using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using InfoService.Enums;
using InfoService.Settings;
using InfoService.Utils;
using MediaPortal.Configuration;

namespace InfoService.GUIConfiguration
{
    public partial class AdvancedConfigForm : Form
    {
        public AdvancedConfigForm()
        {
            InitializeComponent();
        }

        private void btnTwitterDeleteCache_Click(object sender, EventArgs e)
        {
            string twitterDir = txtTwitterCacheFolder.Text;
            if(Directory.Exists(twitterDir))
            {
                try
                {
                    Directory.Delete(twitterDir, true);
                    MessageBox.Show(this, "Twitter cache cleared successfully.", "Twitter cache", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error occured while clearing the twitter cache.\n\n" + ex.Message, "Twitter cache", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else MessageBox.Show(this, "Twitter cache is already cleared.", "Twitter cache", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnFeedDeleteCache_Click(object sender, EventArgs e)
        {
            string feedsDir = txtFeedCacheFolder.Text;
            if(Directory.Exists(feedsDir))
            {
                try
                {
                    Directory.Delete(feedsDir, true);
                    MessageBox.Show(this, "Feed cache cleared successfully.", "Feed cache", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, "An error occured while clearing the feed cache.\n\n" + ex.Message, "Feed cache", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                
            }
            else MessageBox.Show(this, "Feed cache is already cleared.", "Feed cache", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        public static bool IsValidPath(string path)
        {
            if (!string.IsNullOrEmpty(path))
            {
                Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
                return r.IsMatch(path);
            }
            return false;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!InfoServiceUtils.IsValidPath(txtFeedCacheFolder.Text) || string.IsNullOrEmpty(txtFeedCacheFolder.Text))
            {
                MessageBox.Show("The entered feed cache folder path is not a valid or empty. Please enter a valid path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!InfoServiceUtils.IsValidPath(txtTwitterCacheFolder.Text) || string.IsNullOrEmpty(txtTwitterCacheFolder.Text))
            {
                MessageBox.Show("The entered twitter cache folder path is not a valid or empty. Please enter a valid path!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ((WebBrowserType)cbWebBrowserSelector.SelectedIndex == WebBrowserType.Other)
            {
                List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> propertyValueList = GetValidPropertyValueLines();

                if (txtWebBrowserWindowID.Text == string.Empty || propertyValueList.Count < 1)
                {
                    MessageBox.Show("Please enter valid Window ID and Property/Value pairs.", "Web browser", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                SettingsManager.Properties.WebBrowserSettings.WindowID = txtWebBrowserWindowID.Text;
                SettingsManager.Properties.WebBrowserSettings.GUIProperties = propertyValueList;
            }

            SettingsManager.Properties.FeedSettings.DeletionInterval = (DeleteCache) Enum.Parse(typeof (DeleteCache), coFeedDeleteCache.Text);
            SettingsManager.Properties.TwitterSettings.DeletionInterval = (DeleteCache)Enum.Parse(typeof(DeleteCache), coTwitterDeleteCache.Text);
            SettingsManager.Properties.GeneralSettings.FeedCacheFolder = txtFeedCacheFolder.Text;
            SettingsManager.Properties.GeneralSettings.TwitterCacheFolder = txtTwitterCacheFolder.Text;

            if (!cbDebug.Checked)
            {
                SettingsManager.Properties.GeneralSettings.LogWarning = cbWarning.Checked;
                SettingsManager.Properties.GeneralSettings.LogError = cbError.Checked;
                SettingsManager.Properties.GeneralSettings.LogDebug = false;
            }
            else
            {
                SettingsManager.Properties.GeneralSettings.LogDebug = true;
            }

            SettingsManager.Properties.GeneralSettings.PluginName = txtPluginName.Text;

            SettingsManager.Properties.WebBrowserSettings.BrowserType = (WebBrowserType)cbWebBrowserSelector.SelectedIndex;

            Close();
        }

        private void AdvancedConfigForm_Load(object sender, EventArgs e)
        {
            cbWebBrowserSelector.Items.Clear();
            foreach (WebBrowserType value in Enum.GetValues(typeof(WebBrowserType)))
            {
                cbWebBrowserSelector.Items.Add(StringEnum.GetStringValue(value) ?? string.Empty);
            }
            cbWebBrowserSelector.SelectedIndex = (int)SettingsManager.Properties.WebBrowserSettings.BrowserType;
            cbWebBrowserSelector_SelectedIndexChanged(cbWebBrowserSelector, new EventArgs());

            coFeedDeleteCache.Text = SettingsManager.Properties.FeedSettings.DeletionInterval.ToString();
            coTwitterDeleteCache.Text = SettingsManager.Properties.TwitterSettings.DeletionInterval.ToString();
            txtFeedCacheFolder.Text = SettingsManager.Properties.GeneralSettings.FeedCacheFolder;
            txtTwitterCacheFolder.Text = SettingsManager.Properties.GeneralSettings.TwitterCacheFolder;

            if (!SettingsManager.Properties.GeneralSettings.LogDebug)
            {
                cbWarning.Checked = SettingsManager.Properties.GeneralSettings.LogWarning;
                cbError.Checked = SettingsManager.Properties.GeneralSettings.LogError;
                cbDebug.Checked = false;
            }
            else
            {
                cbWarning.Checked = true;
                cbError.Checked = true;
                cbDebug.Checked = true;
            }

            txtPluginName.Text = SettingsManager.Properties.GeneralSettings.PluginName;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnOpenLogFolder_Click(object sender, EventArgs e)
        {
            string dir = Config.GetFolder(Config.Dir.Log);
            if (Directory.Exists(dir))
            {
                Process.Start(dir);
            }
            else
            {
                MessageBox.Show("Error opening " + dir + ". Directory doesn't exist. It seems that the are some MediaPortal configuration problems.", "Error opening folder!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnOpenLog_Click(object sender, EventArgs e)
        {
            string logFile = Config.GetFolder(Config.Dir.Log) + @"\InfoService.log";
            if (File.Exists(logFile))
            {
                Process.Start(logFile);
            }
            else
            {
                MessageBox.Show("Error opening " + logFile + ". The log file doesn't exist. Please start MediaPortal first, to show the log file.", "Error opening file!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AdvancedConfigForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Advanced%20configuration&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            int isNumber = 0;
            e.Handled = !int.TryParse(e.KeyChar.ToString(), out isNumber);
        }

        private void cbWebBrowserSelector_SelectedIndexChanged(object sender, EventArgs e)
        {
            laWebBrowserWindowID.Enabled = (sender as ComboBox).SelectedIndex == (int)WebBrowserType.Other;
            txtWebBrowserWindowID.Enabled = (sender as ComboBox).SelectedIndex == (int)WebBrowserType.Other;
            dgWebBrowserData.Enabled = (sender as ComboBox).SelectedIndex == (int)WebBrowserType.Other;
            laWebBrowserPlaceholders.Enabled = (sender as ComboBox).SelectedIndex == (int)WebBrowserType.Other;

            txtWebBrowserWindowID.Text = ((WebBrowserType)(sender as ComboBox).SelectedIndex).GetWindowIDFromEnum();
            List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> propertyValueList = null;
            propertyValueList = ((WebBrowserType)(sender as ComboBox).SelectedIndex).GetPropertyValueListFromEnum();

            ClearDataGrid();

            foreach (InfoService.Settings.Data.SettingsWebBrowserGUIProperty propertyValue in propertyValueList)
            {
                dgWebBrowserData.Rows.Add();
                dgWebBrowserData.Rows[dgWebBrowserData.Rows.Count - 2].Cells[0].Value = propertyValue.Property;
                dgWebBrowserData.Rows[dgWebBrowserData.Rows.Count - 2].Cells[1].Value = propertyValue.Value;
            }

            dgWebBrowserData.Rows[dgWebBrowserData.Rows.Count - 1].Cells[0].Selected = true;
        }

        private void ClearDataGrid()
        {
            if (dgWebBrowserData == null || dgWebBrowserData.Rows == null)
                return;

            foreach (DataGridViewRow row in dgWebBrowserData.Rows)
                foreach (DataGridViewCell cell in row.Cells)
                    cell.Value = string.Empty;
            while (dgWebBrowserData.Rows.Count > 0 && !(dgWebBrowserData.Rows.Count == 1 && dgWebBrowserData.Rows[0].IsNewRow))
                dgWebBrowserData.Rows.RemoveAt(0);
        }

        private void dgWebBrowserData_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1 && e.RowIndex == -1)
            {
                if (!ttWebBrowserValueHelp.Active)
                {
                    //dgWebBrowserData.CurrentCell = DataGridView1.Rows(e.RowIndex).Cells(e.ColumnIndex);
                    ttWebBrowserValueHelp.SetToolTip(dgWebBrowserData, "\n%link% – Item link\n%zoom% – Default zoom level (in % without percent sign)");
                    ttWebBrowserValueHelp.Active = true;
                }
            }
            else
            {
                ttWebBrowserValueHelp.Active = false;
            }
        }

        private List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> GetValidPropertyValueLines()
        {
            List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty> resultList = new List<InfoService.Settings.Data.SettingsWebBrowserGUIProperty>();
            
            if (dgWebBrowserData != null && dgWebBrowserData.Rows != null)
            {
                foreach (DataGridViewRow row in dgWebBrowserData.Rows)
                {
                    if (row.Cells[0].Value != null && !string.IsNullOrEmpty(row.Cells[0].Value.ToString().Trim()) && row.Cells[0].Value.ToString().StartsWith("#") &&
                        row.Cells[1].Value != null && !string.IsNullOrEmpty(row.Cells[1].Value.ToString()))
                    {
                        InfoService.Settings.Data.SettingsWebBrowserGUIProperty guiProperty = new InfoService.Settings.Data.SettingsWebBrowserGUIProperty();
                        guiProperty.Property = row.Cells[0].Value.ToString().Trim();
                        guiProperty.Value = row.Cells[1].Value.ToString();

                        resultList.Add(guiProperty);
                    }
                }
            }
            return resultList;

        }

        private void btnBrowseTwitterCacheFolder_Click(object sender, EventArgs e)
        {
            CacheFolderBrowserDialog.Description = "Please select a new cache folder for twitter cache files.";
            if(CacheFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtTwitterCacheFolder.Text = CacheFolderBrowserDialog.SelectedPath;
            }
        }

        private void btnBrowseFeedCacheFolder_Click(object sender, EventArgs e)
        {
            CacheFolderBrowserDialog.Description = "Please select a new cache folder for feed cache files.";
            if (CacheFolderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtFeedCacheFolder.Text = CacheFolderBrowserDialog.SelectedPath;
            }
        }

        private void btnFeedItemsFilters_Click(object sender, EventArgs e)
        {
            FilterConfigForm fcf = new FilterConfigForm();
            fcf.Show();
        }

        private void cbDebug_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDebug.Checked)
            {
                cbWarning.Enabled = false;
                cbError.Enabled = false;
                cbWarning.Checked = true;
                cbError.Checked = true;
            }
            else
            {
                cbWarning.Enabled = true;
                cbError.Enabled = true;
                cbWarning.Checked = SettingsManager.Properties.GeneralSettings.LogWarning;
                cbError.Checked = SettingsManager.Properties.GeneralSettings.LogError;
            }

        }

        private void btnExportOPML_Click(object sender, EventArgs e)
        {
            if (saveOPMLFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                OPMLManager.OPMLManager opml = new OPMLManager.OPMLManager();
                opml.OpmlTitle = "InfoService Feeds";
                foreach (Settings.Data.SettingsFeed feed in Settings.SettingsManager.Properties.FeedSettings.Feeds)
                {
                    if(!string.IsNullOrEmpty(feed.UrlPath)) opml.AddFeed(feed.Title, "", "", feed.UrlPath);
                }
                try
                {
                    opml.Save(saveOPMLFile.FileName);
                    MessageBox.Show("Succesfull exported feeds to OPML file \"" + saveOPMLFile.FileName + "\".", "Export successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving OPML file.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
       
    }
}

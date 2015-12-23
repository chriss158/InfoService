#region Usings

using System;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Text.RegularExpressions;
using InfoService.Feeds;
using InfoService.Settings;
using InfoService.Settings.Data;
using InfoService.Utils;
using InfoService.Enums;
using MediaPortal.Configuration;
using MediaPortal.Profile;
using TwitterConnector.OAuth;

#endregion

namespace InfoService.GUIConfiguration
{
    
    public partial class ConfigForm : Form
    {
        private ExFeed exFeed;
        private bool _authSuccessful;
        private AccessToken _accessToken;

        public ConfigForm()
        {
            InitializeComponent();
        }

        #region Save/Load
        private void ConfigForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            cbTempUnit.SelectedIndex = 0;

            //if (!System.IO.File.Exists(Config.GetFile(Config.Dir.Config, "InfoService.xml")))
            //{
            //    SettingsFeed sf = new SettingsFeed();
            //    sf.UrlPath = "http://www.team-mediaportal.com/rss-feeds";
            //    sf.ImagePath = "";
            //    sf.Title = "MediaPortal News";
            //    sf.DefaultZoom = 100;
            //    SettingsManager.Properties.FeedSettings.Feeds.Clear();
            //    SettingsManager.Properties.FeedSettings.Feeds.Add(sf);
            //    SettingsManager.Properties.FeedSettings.Enabled = true;
            //    SettingsManager.Properties.FeedSettings.RandomFeedOnUpdate = false;
            //    SettingsManager.Properties.FeedSettings.RandomFeedOnStartup = false;
            //    SettingsManager.Properties.FeedSettings.DeletionInterval = Enums.DeleteCache.Week;
            //    SettingsManager.Properties.FeedSettings.Separator = "+++";
            //    SettingsManager.Properties.FeedSettings.ItemsCountPerFeed = 3;
            //    SettingsManager.Properties.FeedSettings.ItemsCount = 10;
            //    SettingsManager.Properties.FeedSettings.RefreshInterval = 30;
            //    SettingsManager.Properties.FeedSettings.UpdateOnStartup = true;
            //    SettingsManager.Properties.FeedSettings.ShowItemPublishTime = true;
            //    SettingsManager.Properties.FeedSettings.TickerMask = "%itemtitle%";
            //    SettingsManager.Properties.FeedSettings.StartupFeedIndex = 0;
            //    SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.FeedName;
            //    SettingsManager.Properties.WeatherSettings.Enabled = true;
            //    SettingsManager.Properties.WeatherSettings.RefreshInterval = 60;
            //    SettingsManager.Properties.WeatherSettings.UpdateOnStartup = true;
            //    SettingsManager.Properties.TwitterSettings.ItemsCount = 10;
            //    SettingsManager.Properties.TwitterSettings.RefreshInterval = 45;
            //    SettingsManager.Properties.TwitterSettings.TickerMask = "%message%";
            //    SettingsManager.Properties.TwitterSettings.Separator = "+++";
            //    SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchMask = "I'm just watching %video% on my MediaPortal HTPC!";
            //    SettingsManager.Properties.GeneralSettings.LogDebug = false;
            //    SettingsManager.Properties.GeneralSettings.LogError = true;
            //    SettingsManager.Properties.GeneralSettings.LogWarning = true;
            //    SettingsManager.Properties.GeneralSettings.PluginName = "InfoService";
            //    SettingsManager.Properties.GeneralSettings.FeedCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Feeds\";
            //    SettingsManager.Properties.GeneralSettings.TwitterCacheFolder = Config.GetFolder(Config.Dir.Thumbs) + @"\InfoService\Twitter\";
            //    SettingsManager.Save(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
            //}
            try
            {
                SettingsManager.Load(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error when loading the settings file from " +
                                Config.GetFile(Config.Dir.Config, "InfoService.xml") +
                                ". Please check if the file has no errors or delete this file and try again.\n\n" + ex.Message, "Error loading settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close();
            }
            cbFeedEnabled.Checked = SettingsManager.Properties.FeedSettings.Enabled;
            cobFeedStartupFeed.Items.Add("All Feeds");

            #region Skin Settings
            string skin = string.Empty;
            using (MediaPortal.Profile.Settings settings = new MediaPortal.Profile.MPSettings())
            {
                skin = settings.GetValueAsString("skin", "name", "Blue3wide");
                skin = Config.GetSubFolder(Config.Dir.Skin, skin);
            }

            InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.loadSkinSettings(skin + @"\infoservice.xml");
            #endregion

            foreach (SettingsFeed settingsFeed in SettingsManager.Properties.FeedSettings.Feeds)
            {
                lvFeeds.Items.Add(new ListViewItem(new[] { settingsFeed.Title, settingsFeed.UrlPath }));
                cobFeedStartupFeed.Items.Add(settingsFeed.Title);
            }

            SettingsFeed allSf = new SettingsFeed();
            allSf.Title = "All Feeds";
            SettingsManager.Properties.FeedSettings.Feeds.Insert(0, allSf);

            exFeed = new ExFeed("http://www.team-mediaportal.com/rss-feeds");
            exFeed.Update(false);

            if (SettingsManager.Properties.FeedSettings.RandomFeedOnStartup) rbFeedShowRandomFeedOnStartup.PerformClick();
            else rbFeedShowThisFeedOnStartup.PerformClick();
            switch (SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds)
            {
                case ItemPublishTimeAllFeedsType.PublishTime:
                    rbItemPublishTimeAllFeeds.PerformClick();
                    break;
                case ItemPublishTimeAllFeedsType.FeedName:
                    rbFeedNameAllFeeds.PerformClick();
                    break;
                case ItemPublishTimeAllFeedsType.Both:
                    rbBothAllFeeds.PerformClick();
                    break;
            }
            cbFeedRandomFeedOnUpdate.Checked = SettingsManager.Properties.FeedSettings.RandomFeedOnUpdate;
            cobFeedStartupFeed.SelectedIndex = SettingsManager.Properties.FeedSettings.StartupFeedIndex;
            txtFeedSeparator.Text = SettingsManager.Properties.FeedSettings.Separator;
            txtFeedAllSeparator.Text = SettingsManager.Properties.FeedSettings.SeparatorAll;
            txtFeedItems.Value = SettingsManager.Properties.FeedSettings.ItemsCount;
            txtFeedItemsPerFeed.Value = SettingsManager.Properties.FeedSettings.ItemsCountPerFeed;
            txtFeedRefreshInt.Value = SettingsManager.Properties.FeedSettings.RefreshInterval;
            cbFeedUpdateOnStartup.Checked = SettingsManager.Properties.FeedSettings.UpdateOnStartup;
            cbFeedShowItemPublishTime.Checked = SettingsManager.Properties.FeedSettings.ShowItemPublishTime;
            txtFeedTickerLayout.Text = SettingsManager.Properties.FeedSettings.TickerMask;
            txtFeedTickerAllLayout.Text = SettingsManager.Properties.FeedSettings.TickerAllMask;
            cbNewsPopup.Checked = SettingsManager.Properties.FeedSettings.ShowPopup;
            txtPopupTimeout.Value = SettingsManager.Properties.FeedSettings.PopupTimeout;
            cbPopupWhileVideoPlaying.Checked = SettingsManager.Properties.FeedSettings.PopupWhileFullScreenVideo;
            cbWeatherEnabled.Checked = SettingsManager.Properties.WeatherSettings.Enabled;
            txtWeatherRefreshInt.Value = SettingsManager.Properties.WeatherSettings.RefreshInterval;
            cbWeatherUpdateOnStartup.Checked = SettingsManager.Properties.WeatherSettings.UpdateOnStartup;

            cbTwitterEnabled.Checked = SettingsManager.Properties.TwitterSettings.Enabled;
            txtTwitterPIN.Text = SettingsManager.Properties.TwitterSettings.Pin;
            if (string.IsNullOrEmpty(txtTwitterPIN.Text))
            {
                txtTwitterPIN.Enabled = true;
                btnTwitterAuthorize.Enabled = true;
                _authSuccessful = false;
            }
            else
            {
                _authSuccessful = true;
                txtTwitterPIN.Enabled = false;
                btnTwitterAuthorize.Enabled = false;
            }
            _accessToken = new AccessToken(SettingsManager.Properties.TwitterSettings.TokenValue, SettingsManager.Properties.TwitterSettings.TokenSecret);
            txtTwitterSeparator.Text = SettingsManager.Properties.TwitterSettings.Separator;
            txtTwitterItems.Value = SettingsManager.Properties.TwitterSettings.ItemsCount;
            txtTwitterRefreshInt.Value = SettingsManager.Properties.TwitterSettings.RefreshInterval;
            cbTwitterUpdateOnStartup.Checked = SettingsManager.Properties.TwitterSettings.UpdateOnStartup;
            cbTwitterHomeTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.HomeTimeline;
            //cbTwitterPublicTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.PublicTimeline;
            cbTwitterUserTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.UserTimeline;
            cbTwitterFriendsTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.FriendsTimeline;
            cbTwitterMentionsTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.MentionsTimeline;
            cbTwitterRetweetedByMeTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedByMeTimeline;
            cbTwitterRetweetedToMeTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedToMeTimeline;
            cbTwitterRetweetsOfMeTimeline.Checked = SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetsOfMeTimeline;
            txtTwitterTickerLayout.Text = SettingsManager.Properties.TwitterSettings.TickerMask;
            cbTwitterPostWatchingVideos.Checked = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.Enabled;
            cbTwitterPostWatchingVideos_CheckedChanged(cbTwitterPostWatchingVideos, new EventArgs());
            cbTwitterUseMovingPictures.Checked = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMovingPictures;
            cbTwitterUserTVSeries.Checked = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMPTVSeries;
            cbTwitterUseMyVideo.Checked = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMyVideo;
            txtTwitterWatchingMoviesMask.Text = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchMoviesMask;
            txtTwitterWatchingSeriesMask.Text = SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchSeriesMask;
            SetExampleTicker();
            //lblProductVersion.Text = "v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            lblProductVersion.Text = "v" + FileVersionInfo.GetVersionInfo(System.Reflection.Assembly.GetExecutingAssembly().Location).ProductVersion;

            SkinSettingsControlsUpdate(this, new EventArgs());

            Cursor.Current = Cursors.Default;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            /*else if (txtCity.Text == String.Empty || txtCityCode.Text == String.Empty)
            {
                MessageBox.Show("Please add a city for weather service or disable the weather service.", "No weather location", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }*/
            if ((cbFeedEnabled.Checked && lvFeeds.Items.Count <= 1))
            {
                MessageBox.Show("Please add at least one Feed or disable Feed ticker.", "No Feeds", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            if (cbTwitterEnabled.Checked && (String.IsNullOrEmpty(txtTwitterPIN.Text)))
            {
                MessageBox.Show("Please enter a pin or disable Twitter ticker.", "No pin entered", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cbTwitterEnabled.Checked)
            {
                if (!cbTwitterFriendsTimeline.Checked && !cbTwitterUserTimeline.Checked &&
                    //!cbTwitterPublicTimeline.Checked && !cbTwitterHomeTimeline.Checked &&
                    !cbTwitterRetweetedByMeTimeline.Checked && !cbTwitterRetweetedToMeTimeline.Checked &&
                    !cbTwitterRetweetsOfMeTimeline.Checked && !cbTwitterMentionsTimeline.Checked)
                {
                    DialogResult dr = MessageBox.Show(
                        "You selected no timeline to update. InfoService will disable the twitter ticker if you continue. Do you want continue?",
                        "Disable twitter ticker?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dr == DialogResult.Yes)
                    {
                        cbTwitterEnabled.Checked = false;
                        SettingsManager.Properties.TwitterSettings.Enabled = cbTwitterEnabled.Checked ? true : false;
                    }
                    else
                    {
                        return;
                    }
                }
            }
            
            if (cbTwitterEnabled.Checked && (!_authSuccessful))
            {
                DialogResult dr = MessageBox.Show("The last authorization has failed. InfoService will disable the twitter ticker if you continue. Do you want continue?", "Auth unsuccessful", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dr == DialogResult.Yes)
                {
                    cbTwitterEnabled.Checked = false;
                    SettingsManager.Properties.TwitterSettings.Enabled = cbTwitterEnabled.Checked ? true : false;
                }
                else
                {
                    return;
                }
            }

            SettingsManager.Properties.FeedSettings.Enabled = cbFeedEnabled.Checked ? true : false;
            SettingsManager.Properties.FeedSettings.RandomFeedOnStartup = rbFeedShowRandomFeedOnStartup.Checked;
            SettingsManager.Properties.FeedSettings.RandomFeedOnUpdate = cbFeedRandomFeedOnUpdate.Checked;
            SettingsManager.Properties.FeedSettings.StartupFeedIndex = cobFeedStartupFeed.SelectedIndex;
            SettingsManager.Properties.FeedSettings.Separator = txtFeedSeparator.Text;
            SettingsManager.Properties.FeedSettings.SeparatorAll = txtFeedAllSeparator.Text;
            SettingsManager.Properties.FeedSettings.ItemsCountPerFeed = txtFeedItemsPerFeed.Value;
            SettingsManager.Properties.FeedSettings.ItemsCount = txtFeedItems.Value;
            SettingsManager.Properties.FeedSettings.RefreshInterval = txtFeedRefreshInt.Value;
            SettingsManager.Properties.FeedSettings.UpdateOnStartup = cbFeedUpdateOnStartup.Checked;
            SettingsManager.Properties.FeedSettings.ShowItemPublishTime = cbFeedShowItemPublishTime.Checked;
            SettingsManager.Properties.FeedSettings.TickerMask = txtFeedTickerLayout.Text;
            SettingsManager.Properties.FeedSettings.TickerAllMask = txtFeedTickerAllLayout.Text;
            SettingsManager.Properties.FeedSettings.ShowPopup = cbNewsPopup.Checked;
            SettingsManager.Properties.FeedSettings.PopupTimeout = txtPopupTimeout.Value;
            SettingsManager.Properties.FeedSettings.PopupWhileFullScreenVideo = cbPopupWhileVideoPlaying.Checked;
            if (rbItemPublishTimeAllFeeds.Checked)
                SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.PublishTime;
            else if (rbFeedNameAllFeeds.Checked)
                SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.FeedName;
            else if (rbBothAllFeeds.Checked)
                SettingsManager.Properties.FeedSettings.ItemPublishTimeAllFeeds = ItemPublishTimeAllFeedsType.Both;
            SettingsManager.Properties.FeedSettings.Feeds.RemoveAt(0);

            SettingsManager.Properties.WeatherSettings.Enabled = cbWeatherEnabled.Checked ? true : false;
            SettingsManager.Properties.WeatherSettings.RefreshInterval = txtWeatherRefreshInt.Value;
            SettingsManager.Properties.WeatherSettings.UpdateOnStartup = cbWeatherUpdateOnStartup.Checked;

            SettingsManager.Properties.TwitterSettings.Enabled = cbTwitterEnabled.Checked ? true : false;
            if (_authSuccessful)
            {
                if (_accessToken != null) SettingsManager.Properties.TwitterSettings.TokenValue = _accessToken.TokenValue;
                if (_accessToken != null) SettingsManager.Properties.TwitterSettings.TokenSecret = _accessToken.TokenSecret;
                SettingsManager.Properties.TwitterSettings.Pin = txtTwitterPIN.Text;
            }
            else
            {
                if (_accessToken != null) SettingsManager.Properties.TwitterSettings.TokenValue = string.Empty;
                if (_accessToken != null) SettingsManager.Properties.TwitterSettings.TokenSecret = string.Empty;
                SettingsManager.Properties.TwitterSettings.Pin = string.Empty;
            }
            SettingsManager.Properties.TwitterSettings.Separator = txtTwitterSeparator.Text;
            SettingsManager.Properties.TwitterSettings.ItemsCount = txtTwitterItems.Value;
            SettingsManager.Properties.TwitterSettings.RefreshInterval = txtTwitterRefreshInt.Value;
            SettingsManager.Properties.TwitterSettings.UpdateOnStartup = cbTwitterUpdateOnStartup.Checked;
            //SettingsManager.Properties.TwitterSettings.UsedTimelines.PublicTimeline = cbTwitterPublicTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.HomeTimeline = cbTwitterHomeTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.UserTimeline = cbTwitterUserTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.FriendsTimeline = cbTwitterFriendsTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.MentionsTimeline = cbTwitterMentionsTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedByMeTimeline = cbTwitterRetweetedByMeTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetedToMeTimeline = cbTwitterRetweetedToMeTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.UsedTimelines.RetweetsOfMeTimeline = cbTwitterRetweetsOfMeTimeline.Checked;
            SettingsManager.Properties.TwitterSettings.TickerMask = txtTwitterTickerLayout.Text;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.Enabled = cbTwitterPostWatchingVideos.Checked;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMovingPictures = cbTwitterUseMovingPictures.Checked;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMPTVSeries = cbTwitterUserTVSeries.Checked;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WithMyVideo = cbTwitterUseMyVideo.Checked;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchMoviesMask = txtTwitterWatchingMoviesMask.Text;
            SettingsManager.Properties.TwitterSettings.TwitterStatusUpdate.WatchSeriesMask = txtTwitterWatchingSeriesMask.Text;

            try
            {
                SettingsManager.Save(Config.GetFile(Config.Dir.Config, "InfoService.xml"));
            }
            catch (Exception ex)
            {
                string logMessage = "Saving settings to InfoService.xml unsuccessfull..." + "\n\t\t\t\t\t\t" + ex.Message + "\n\t\t\t\t\t\t" + ex.StackTrace;
                MessageBox.Show("There was an error when saving the settings file to " +
                Config.GetFile(Config.Dir.Config, "InfoService.xml") + ".\n\n" + ex.Message, "Error loading settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Close();
        }
        #endregion

        #region Events
        private void btnFeedItemFilters_Click(object sender, EventArgs e)
        {
            FilterConfigForm fcf = new FilterConfigForm();
            fcf.Show();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cbFeedEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbFeedEnabled.Checked)
            {
                gbFeedConf.Enabled = true;
                gbFeedGeneral.Enabled = true;
                gbFeedTickerLayout.Enabled = true;
                gbFeedLayout.Enabled = true;
            }
            else
            {
                gbFeedConf.Enabled = false;
                gbFeedGeneral.Enabled = false;
                gbFeedTickerLayout.Enabled = false;
                gbFeedLayout.Enabled = false;
            }

            SkinSettingsControlsUpdate(sender, e);
        }

        private void cbWeatherEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbWeatherEnabled.Checked)
            {
                gbWeatherGeneral.Enabled = true;
                gbWeatherCity.Enabled = true;
            }
            else
            {
                gbWeatherGeneral.Enabled = false;
                gbWeatherCity.Enabled = false;
            }

            SkinSettingsControlsUpdate(sender, e);
        }

        private void cbTwitterEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTwitterEnabled.Checked)
            {
                gbTwitterConf.Enabled = true;
                gbTwitterGeneral.Enabled = true;
                gbTwitterTickerLayout.Enabled = true;
                gbTwitterTimelines.Enabled = true;
                gbTwitterUpdateStatus.Enabled = true;
                gbTwitterLayout.Enabled = true;

            }
            else
            {
                gbTwitterConf.Enabled = false;
                gbTwitterGeneral.Enabled = false;
                gbTwitterTickerLayout.Enabled = false;
                gbTwitterTimelines.Enabled = false;
                gbTwitterUpdateStatus.Enabled = false;
                gbTwitterLayout.Enabled = false;
            }

            SkinSettingsControlsUpdate(sender, e);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            FeedForm feedform = new FeedForm(cbNewsPopup.Checked);
            if (feedform.ShowDialog() == DialogResult.OK)
            {
                if (!IsFeedAlreadyAdded(feedform.FeedUrlPath))
                {
                    lvFeeds.Items.Add(new ListViewItem(new[] { feedform.FeedTitle, feedform.FeedUrlPath }));
                    cobFeedStartupFeed.Items.Add(feedform.FeedTitle);
                    SettingsFeed sf = new SettingsFeed { UrlPath = feedform.FeedUrlPath, DefaultZoom = feedform.FeedDefaultZoom, ImagePath = feedform.FeedImage, Title = feedform.FeedTitle, ShowPopup = feedform.FeedPopupShow };
                    SettingsManager.Properties.FeedSettings.Feeds.Add(sf);
                }
                else
                {
                    MessageBox.Show("The feed \"" + feedform.FeedTitle + "\" (" + feedform.FeedUrlPath + ") has been already added to the feed list.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                //feeds.Add(new Feed(feedform.FeedUrlPath, feedform.FeedTitle, feedform.FeedImage, feedform.FeedDefaultZoom));
                //lvFeeds.SeSelectedIndex = lv.Items.Count - 1;
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditFeed();
        }

        private void btnRem_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Properties.FeedSettings.Feeds.Count > 0 && lvFeeds.SelectedIndices.Count > 0)
            {
                int index = lvFeeds.SelectedIndices[0];
                SettingsManager.Properties.FeedSettings.Feeds.RemoveAt(index);
                lvFeeds.Items.RemoveAt(index);
                if ((index - 1) >= 0)
                {
                    if(cobFeedStartupFeed.SelectedIndex == index) cobFeedStartupFeed.SelectedIndex = index - 1;
                    lvFeeds.Items[index - 1].Selected = true;
                }
                cobFeedStartupFeed.Items.RemoveAt(index);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchCity.Text != String.Empty)
            {
                WeatherForm wform = new WeatherForm(txtSearchCity.Text);
                wform.ShowDialog();
                txtCityCode.Text = wform.WeatherLocationCode;
                txtCity.Text = wform.WeatherLocation;
            }
        }
        
        private void lvFeeds_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lvFeeds.SelectedIndices.Count > 0)
            {
                if (lvFeeds.SelectedIndices[0] >= 1)
                {
                    btnEdit.Enabled = true;
                    btnRem.Enabled = true;
                    btnUp.Enabled = lvFeeds.SelectedIndices[0] != 1;
                    btnDown.Enabled = lvFeeds.SelectedIndices[0] != lvFeeds.Items.Count - 1;
                }
                else
                {
                    btnEdit.Enabled = false;
                    btnRem.Enabled = false;
                    btnUp.Enabled = false;
                    btnDown.Enabled = false;
                }
            }
            else
            {
                btnEdit.Enabled = false;
                btnRem.Enabled = false;
                btnUp.Enabled = false;
                btnDown.Enabled = false;
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Properties.FeedSettings.Feeds.Count > 0 && lvFeeds.SelectedIndices.Count > 0)
            {
                int index = lvFeeds.SelectedIndices[0];
                if ((index - 1) >= 0)
                {
                    ListViewItem itemToMove = lvFeeds.Items[index];
                    object cobItemToMove = cobFeedStartupFeed.Items[index];
                    SettingsFeed feedToMove = SettingsManager.Properties.FeedSettings.Feeds[index];
                    SettingsManager.Properties.FeedSettings.Feeds.RemoveAt(index);
                    SettingsManager.Properties.FeedSettings.Feeds.Insert(index - 1, feedToMove);
                    lvFeeds.Items.RemoveAt(index);
                    lvFeeds.Items.Insert(index - 1, itemToMove);
                    lvFeeds.Items[index - 1].Selected = true;
                    lvFeeds.EnsureVisible(index - 1);
                    int cobOldSelectedIndex = cobFeedStartupFeed.SelectedIndex;
                    cobFeedStartupFeed.Items.RemoveAt(index);
                    cobFeedStartupFeed.Items.Insert(index - 1, cobItemToMove);
                    if (cobOldSelectedIndex == index) cobFeedStartupFeed.SelectedIndex = index - 1;
                    
                }
                lvFeeds.Select();
                
            }
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            if (SettingsManager.Properties.FeedSettings.Feeds.Count > 0 && lvFeeds.SelectedIndices.Count > 0)
            {
                int index = lvFeeds.SelectedIndices[0];
                if ((index + 1) < lvFeeds.Items.Count && (index + 1) < SettingsManager.Properties.FeedSettings.Feeds.Count)
                {
                    ListViewItem itemToMove = lvFeeds.Items[index];
                    object cobItemToMove = cobFeedStartupFeed.Items[index];
                    SettingsFeed feedToMove = SettingsManager.Properties.FeedSettings.Feeds[index];
                    SettingsManager.Properties.FeedSettings.Feeds.RemoveAt(index);
                    SettingsManager.Properties.FeedSettings.Feeds.Insert(index + 1, feedToMove);
                    lvFeeds.Items.RemoveAt(index);
                    lvFeeds.Items.Insert(index + 1, itemToMove);
                    lvFeeds.Items[index + 1].Selected = true;
                    lvFeeds.EnsureVisible(index + 1);
                    int cobOldSelectedIndex = cobFeedStartupFeed.SelectedIndex;
                    cobFeedStartupFeed.Items.RemoveAt(index);
                    cobFeedStartupFeed.Items.Insert(index + 1, cobItemToMove);
                    if (cobOldSelectedIndex == index) cobFeedStartupFeed.SelectedIndex = index + 1;
                }
                lvFeeds.Select();
            }
        }

        private void txtFeedTickerLayout_TextChanged(object sender, EventArgs e)
        {
            SetExampleTicker();

            SkinSettingsControlsUpdate(sender, e);
        }

        private void txtFeedSeparator_TextChanged(object sender, EventArgs e)
        {
            SetExampleTicker();

            SkinSettingsControlsUpdate(sender, e);
        }

        private void txtFeedAllSeparator_TextChanged(object sender, EventArgs e)
        {
            SetExampleTicker(true);

            SkinSettingsControlsUpdate(sender, e);
        }

        private void txtFeedTickerAllLayout_TextChanged(object sender, EventArgs e)
        {
            SetExampleTicker(true);

            SkinSettingsControlsUpdate(sender, e);
        }

        private void txtFeedTickerAllLayout_Enter(object sender, EventArgs e)
        {
            SetExampleTicker(true);
        }

        private void txtFeedTickerAllLayout_Leave(object sender, EventArgs e)
        {
            SetExampleTicker();
        }

        private void txtFeedAllSeparator_Enter(object sender, EventArgs e)
        {
            SetExampleTicker(true);
        }

        private void txtFeedAllSeparator_Leave(object sender, EventArgs e)
        {
            SetExampleTicker();
        }

        private void btnGetPin_Click(object sender, EventArgs e)
        {
            try
            {
                if (DialogResult.Cancel ==
                    MessageBox.Show(this, "InfoService will now direct you to the Twitter website where " +
                                          "you will be assigned a 7-digit PIN for this application.",
                                            "InfoService", MessageBoxButtons.OKCancel, MessageBoxIcon.Information))
                    return;
                RequestToken requestToken;
                TwitterConnector.Twitter.GetRequestToken(out requestToken);
                Process.Start(TwitterConnector.Twitter.GetAuthUrl());
                _authSuccessful = false;
                btnTwitterAuthorize.Enabled = true;
                txtTwitterPIN.Enabled = true;

            }
            catch (TwitterConnector.Expections.TwitterAuthExpection tae)
            {
                MessageBox.Show(this, "An error occurred during authorization. Error:\n\n" + tae.Message, "InfoService", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "An error occurred during authorization. Error:\n\n" + ex.Message, "InfoService", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void pbOAuthLogo_Click(object sender, EventArgs e)
        {
            Process.Start("http://oauth.net/");
        }

        private void btnAuthorize_Click(object sender, EventArgs e)
        {
            try
            {
                txtTwitterPIN.Text = txtTwitterPIN.Text.Trim();
                if (TwitterConnector.Twitter.GetAuthToken(txtTwitterPIN.Text, out _accessToken))
                {
                    _authSuccessful = true;
                    MessageBox.Show(this, "Successfully authed! You're ready to start tweeting with MediaPortal!", "InfoService", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (TwitterConnector.Expections.TwitterAuthExpection tae)
            {
                MessageBox.Show(this, "An error occurred during authorization. Error: " + tae.Message + "\n\n" + tae.InnerException.Message, "InfoService", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _authSuccessful = false;
            }
            btnTwitterAuthorize.Enabled = false;
            txtTwitterPIN.Enabled = false;
            btnTwitterGetPin.Enabled = true;
        }

        private void cbTwitterPostWatchingVideos_CheckedChanged(object sender, EventArgs e)
        {
            cbTwitterUseMovingPictures.Enabled = cbTwitterPostWatchingVideos.Checked;
            cbTwitterUserTVSeries.Enabled = cbTwitterPostWatchingVideos.Checked;
            cbTwitterUseMyVideo.Enabled = cbTwitterPostWatchingVideos.Checked;
            txtTwitterWatchingMoviesMask.Enabled = cbTwitterPostWatchingVideos.Checked;
            txtTwitterWatchingSeriesMask.Enabled = cbTwitterPostWatchingVideos.Checked;
            laTwitterWatchingMoviesMask.Enabled = cbTwitterPostWatchingVideos.Checked;
            laTwitterWatchingSeriesMask.Enabled = cbTwitterPostWatchingVideos.Checked;
        }

        private void rbFeedShowThisFeedOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            cobFeedStartupFeed.Enabled = true;
        }

        private void rbFeedShowRandomFeedOnStartup_CheckedChanged(object sender, EventArgs e)
        {
            cobFeedStartupFeed.Enabled = false;
        }

        private void btnAdvancedConfig_Click(object sender, EventArgs e)
        {
            AdvancedConfigForm acf = new AdvancedConfigForm();
            acf.ShowDialog();
        }

        private void llblAboutCodeplex_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/");
        }

        private void llblAboutManual_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/documentation");
        }

        private void llblAboutForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://forum.team-mediaportal.com/mediaportal-plugins-47/infoservice-v1-32-5-day-weather-feeds-twitter-basic-home-04-01-2010-planning-60206/");
        }

        private void btnFeedHelp_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Feeds%20configuration&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void btnWeatherHelp_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Weather%20configuration&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void btnTwitterHelp_Click(object sender, EventArgs e)
        {
            Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Twitter%20configuration&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void llblFamFamFam_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(@"http://www.famfamfam.com/");
        }

        private void cbFeedShowItemPublishTime_CheckedChanged(object sender, EventArgs e)
        {
            rbItemPublishTimeAllFeeds.Enabled = cbFeedShowItemPublishTime.Checked;
            rbFeedNameAllFeeds.Enabled = cbFeedShowItemPublishTime.Checked;
            rbBothAllFeeds.Enabled = cbFeedShowItemPublishTime.Checked;

            SkinSettingsControlsUpdate(sender, e);
        }

        private void SkinSettingsControlEnabledChanged(object sender, EventArgs e)
        {
            if ((sender as Control).Enabled)
            {
                (sender as Control).Text = Regex.Replace((sender as Control).Text, @"^\* ", "");
            }
            else if ((sender as Control).ForeColor == System.Drawing.Color.DarkRed)
            {
                if (!Regex.Match((sender as Control).Text, @"^\* ").Success)
                    (sender as Control).Text = "* " + (sender as Control).Text;
            }
        }

        private void SkinSettingsControlChanged(object sender, EventArgs e)
        {
            SkinSettingsControlsUpdate(sender, e);
        }
        #endregion

        #region Helpers
        private void SetExampleTicker()
        {
            SetExampleTicker(false);
        }

        private void SetExampleTicker(bool bAll)
        {
            if (!bAll) mlFeedTickerExample.Text = FeedUtils.MakeFeedLine(exFeed, txtFeedTickerLayout.Text, 3, txtFeedSeparator.Text);
            else mlFeedTickerExample.Text = FeedUtils.MakeFeedMixLine(exFeed, txtFeedTickerAllLayout.Text, 3, txtFeedAllSeparator.Text);
        }

        private void SkinSettingsControlsUpdate(object sender, EventArgs e)
        {
            #region Feeds
            labelSkinSettingsFeeds.Visible = false;
            if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.HasFeedsSkinSettings)
            {
                bool isAnyFeedSettingDifferent = false;

                // Feeds enabled
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Enabled_Val) && cbFeedEnabled.Checked != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Enabled)
                {
                    cbFeedEnabled.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(cbFeedEnabled, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    cbFeedEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Feeds refresh every
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_General_RefreshFeedEvery_Val) && txtFeedRefreshInt.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_General_RefreshFeedEvery)
                {
                    laFeedRefresh.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedRefresh, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Show item publish time
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ShowFeedItemPublishTime_Val) && cbFeedShowItemPublishTime.Checked != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ShowFeedItemPublishTime)
                {
                    cbFeedShowItemPublishTime.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(cbFeedShowItemPublishTime, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    cbFeedShowItemPublishTime.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Item publish time for all feeds
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds_Val) && 
                     ( 
                       (rbItemPublishTimeAllFeeds.Checked && InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds != ItemPublishTimeAllFeedsType.PublishTime) ||
                       (rbFeedNameAllFeeds.Checked && InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds != ItemPublishTimeAllFeedsType.FeedName) ||
                       (rbBothAllFeeds.Checked && InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_ItemPublishTimeAllFeeds != ItemPublishTimeAllFeedsType.Both)
                     )
                   )
                {
                    rbItemPublishTimeAllFeeds.ForeColor = System.Drawing.Color.DarkRed;
                    rbFeedNameAllFeeds.ForeColor = System.Drawing.Color.DarkRed;
                    rbBothAllFeeds.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(rbItemPublishTimeAllFeeds, new EventArgs());
                    SkinSettingsControlEnabledChanged(rbFeedNameAllFeeds, new EventArgs());
                    SkinSettingsControlEnabledChanged(rbBothAllFeeds, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    rbItemPublishTimeAllFeeds.ForeColor = System.Drawing.SystemColors.ControlText;
                    rbFeedNameAllFeeds.ForeColor = System.Drawing.SystemColors.ControlText;
                    rbBothAllFeeds.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Max items per feed for all feeds
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsPerFeedForAllFeeds_Val) && txtFeedItemsPerFeed.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsPerFeedForAllFeeds)
                {
                    laFeedItemsPerFeed.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedItemsPerFeed, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedItemsPerFeed.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Max items for feed ticker
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsForFeedTicker_Val) && txtFeedItems.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_Layout_MaxItemsForFeedTicker)
                {
                    laFeedItems.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedItems, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedItems.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Feed ticker mask
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerMask_Val) && txtFeedTickerLayout.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerMask)
                {
                    laFeedTickerLayout.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedTickerLayout, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedTickerLayout.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Feed ticker all mask
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerAllMask_Val) && txtFeedTickerAllLayout.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_TickerAllMask)
                {
                    laFeedTickerAllLayout.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedTickerAllLayout, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedTickerAllLayout.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Feed separator
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_Separator_Val) && txtFeedSeparator.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_Separator)
                {
                    laFeedSeparator.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedSeparator, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Feed separator all
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_SeparatorAll_Val) && txtFeedAllSeparator.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Feeds_TickerLayout_SeparatorAll)
                {
                    laFeedAllSeparator.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laFeedAllSeparator, new EventArgs());
                    isAnyFeedSettingDifferent = true;
                }
                else
                {
                    laFeedAllSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                if (isAnyFeedSettingDifferent)
                    labelSkinSettingsFeeds.Visible = true;
            }
            #endregion

            #region Weather
            labelSkinSettingsWeather.Visible = false;
            if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.HasWeatherSkinSettings)
            {
                bool isAnyWeatherSettingDifferent = false;

                // Weather enabled
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_Enabled_Val) && cbWeatherEnabled.Checked != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_Enabled)
                {
                    cbWeatherEnabled.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(cbWeatherEnabled, new EventArgs());
                    isAnyWeatherSettingDifferent = true;
                }
                else
                {
                    cbWeatherEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Weather refresh every
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_General_RefreshWeatherEvery_Val) && txtWeatherRefreshInt.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Weather_General_RefreshWeatherEvery)
                {
                    laWeatherRefresh.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laWeatherRefresh, new EventArgs());
                    isAnyWeatherSettingDifferent = true;
                }
                else
                {
                    laWeatherRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                if (isAnyWeatherSettingDifferent)
                    labelSkinSettingsWeather.Visible = true;
            }
            #endregion

            #region Twitter
            labelSkinSettingsTwitter.Visible = false;
            if (InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.HasTwitterSkinSettings)
            {
                bool isAnyTwitterSettingDifferent = false;

                // Twitter enabled
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Enabled_Val) && cbTwitterEnabled.Checked != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Enabled)
                {
                    cbTwitterEnabled.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(cbTwitterEnabled, new EventArgs());
                    isAnyTwitterSettingDifferent = true;
                }
                else
                {
                    cbTwitterEnabled.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Twitter refresh every
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_General_RefreshTwitterEvery_Val) && txtTwitterRefreshInt.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_General_RefreshTwitterEvery)
                {
                    laTwitterRefresh.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laTwitterRefresh, new EventArgs());
                    isAnyTwitterSettingDifferent = true;
                }
                else
                {
                    laTwitterRefresh.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Max items for twitter ticker
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Layout_MaxItemsForTwitterTicker_Val) && txtTwitterItems.Value != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_Layout_MaxItemsForTwitterTicker)
                {
                    laTwitterItems.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laTwitterItems, new EventArgs());
                    isAnyTwitterSettingDifferent = true;
                }
                else
                {
                    laTwitterItems.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Twitter ticker mask
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_TickerMask_Val) && txtTwitterTickerLayout.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_TickerMask)
                {
                    laTwitterTickerLayout.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laTwitterTickerLayout, new EventArgs());
                    isAnyTwitterSettingDifferent = true;
                }
                else
                {
                    laTwitterTickerLayout.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                // Twitter separator
                if (!string.IsNullOrEmpty(InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_Separator_Val) && txtTwitterSeparator.Text != InfoServiceSkinSettings.InfoServiceSkinSettingsInstance.Twitter_TickerLayout_Separator)
                {
                    laTwitterSeparator.ForeColor = System.Drawing.Color.DarkRed;
                    SkinSettingsControlEnabledChanged(laTwitterSeparator, new EventArgs());
                    isAnyTwitterSettingDifferent = true;
                }
                else
                {
                    laTwitterSeparator.ForeColor = System.Drawing.SystemColors.ControlText;
                }

                if (isAnyTwitterSettingDifferent)
                    labelSkinSettingsTwitter.Visible = true;
            }
            #endregion
        }
        #endregion

        private void btnAddMultiple_Click(object sender, EventArgs e)
        {
            FeedFormOPML feedform = new FeedFormOPML();
            if (feedform.ShowDialog() == DialogResult.OK)
            {
                foreach (FeedReader.Feed feed in feedform.AddedFeeds)
                {
                    if (!IsFeedAlreadyAdded(feed.UrlPath))
                    {
                        lvFeeds.Items.Add(new ListViewItem(new[] { feed.Title, feed.UrlPath }));
                        cobFeedStartupFeed.Items.Add(feed.Title);
                        SettingsFeed sf = new SettingsFeed { UrlPath = feed.UrlPath, DefaultZoom = 100, ImagePath = "", Title = feed.Title, ShowPopup = true };
                        SettingsManager.Properties.FeedSettings.Feeds.Add(sf);
                    }
                }
            }
        }

        private bool IsFeedAlreadyAdded(string url)
        {
            foreach (ListViewItem item in lvFeeds.Items)
            {
                if (item.SubItems[1].Text == url)
                {
                    return true;
                }
            }
            return false;
        }

        private void EditFeed()
        {
            if (SettingsManager.Properties.FeedSettings.Feeds.Count > 0 && lvFeeds.SelectedIndices.Count > 0)
            {
                int index = lvFeeds.SelectedIndices[0];
                FeedForm feedform = new FeedForm(SettingsManager.Properties.FeedSettings.Feeds[index].UrlPath,
                                                 SettingsManager.Properties.FeedSettings.Feeds[index].Title,
                                                 SettingsManager.Properties.FeedSettings.Feeds[index].ImagePath,
                                                 SettingsManager.Properties.FeedSettings.Feeds[index].DefaultZoom,
                                                 SettingsManager.Properties.FeedSettings.Feeds[index].ShowPopup,
                                                 cbNewsPopup.Checked);
                if (feedform.ShowDialog() == DialogResult.OK)
                {
                    SettingsManager.Properties.FeedSettings.Feeds.RemoveAt(index);
                    SettingsFeed sf = new SettingsFeed { UrlPath = feedform.FeedUrlPath, DefaultZoom = feedform.FeedDefaultZoom, ImagePath = feedform.FeedImage, Title = feedform.FeedTitle, ShowPopup = feedform.FeedPopupShow };
                    SettingsManager.Properties.FeedSettings.Feeds.Insert(index, sf);
                    lvFeeds.Items.RemoveAt(index);
                    lvFeeds.Items.Insert(index, new ListViewItem(new[] { feedform.FeedTitle, feedform.FeedUrlPath }));
                    //lvFeeds.SelectedIndex = index;
                }
            }
        }

        private void lvFeeds_DoubleClick(object sender, EventArgs e)
        {
            EditFeed();
        }

        private void cbNewsPopup_CheckedChanged(object sender, EventArgs e)
        {
            txtPopupTimeout.Enabled = cbNewsPopup.Checked;
            cbPopupWhileVideoPlaying.Enabled = cbNewsPopup.Checked;
        
            SkinSettingsControlsUpdate(sender, e);
        }

        private void cbTwitterPublicTimeline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbTwitterUserTimeline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbTwitterRetweetedToMeTimeline_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void cbTwitterHomeTimeline_CheckedChanged(object sender, EventArgs e)
        {

        }

    }
}

using System;
using System.Windows.Forms;
using InfoService.Settings;

namespace InfoService.GUIConfiguration
{
    public partial class FilterConfigForm : Form
    {
        public FilterConfigForm()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SettingsManager.Properties.FeedItemsFiltersSettings.FeedItemFilters.Clear();
            foreach (DataGridViewRow row in dgFeedItemsFilters.Rows)
            {
                Settings.Data.SettingsFeedItemsFilter filter = new InfoService.Settings.Data.SettingsFeedItemsFilter();

                filter.IsEnabled = (bool)(row.Cells["IsEnabled"].Value ?? false);
                filter.IsRegEx = (bool)(row.Cells["IsRegEx"].Value ?? false);
                filter.ReplaceThis = (string)(row.Cells["ReplaceThis"].Value ?? string.Empty);
                filter.ReplaceWith = (string)(row.Cells["ReplaceWith"].Value ?? string.Empty);
                filter.UseInTitle = (bool)(row.Cells["UseInTitle"].Value ?? false);
                filter.UseInBody = (bool)(row.Cells["UseInBody"].Value ?? false);
                filter.CleanBefore = (bool)(row.Cells["CleanBefore"].Value ?? false);

                SettingsManager.Properties.FeedItemsFiltersSettings.FeedItemFilters.Add(filter);
            }
            Close();
        }

        private void FilterConfigForm_Load(object sender, EventArgs e)
        {
            foreach (Settings.Data.SettingsFeedItemsFilter filter in SettingsManager.Properties.FeedItemsFiltersSettings.FeedItemFilters)
            {
                dgFeedItemsFilters.Rows.Add();
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["IsEnabled"].Value = filter.IsEnabled;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["IsRegEx"].Value = filter.IsRegEx;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["ReplaceThis"].Value = filter.ReplaceThis;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["ReplaceWith"].Value = filter.ReplaceWith;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["UseInTitle"].Value = filter.UseInTitle;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["UseInBody"].Value = filter.UseInBody;
                dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["CleanBefore"].Value = filter.CleanBefore;
            }
        }

        private void FilterConfigForm_HelpButtonClicked(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // TODO MS
            //Process.Start(@"http://infoservice.codeplex.com/wikipage?title=Advanced%20configuration&referringTitle=How%20to%20use%20%28Users%29");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            dgFeedItemsFilters.Rows.Add();
            dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["IsEnabled"].Value = true;
            dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["UseInBody"].Value = true;
            dgFeedItemsFilters.Rows[dgFeedItemsFilters.Rows.Count - 1].Cells["ReplaceThis"].Selected = true;
            dgFeedItemsFilters.Focus();
        }

        private void btnRem_Click(object sender, EventArgs e)
        {
            if (dgFeedItemsFilters.CurrentRow != null)
            {
                dgFeedItemsFilters.Rows.Remove(dgFeedItemsFilters.CurrentRow);
                if (dgFeedItemsFilters.Rows.Count > 0)
                    dgFeedItemsFilters.Focus();
            }
        }

        private void dgFeedItemsFilters_SelectionChanged(object sender, EventArgs e)
        {
            btnRem.Enabled = dgFeedItemsFilters.CurrentRow != null;
        }
    }
}

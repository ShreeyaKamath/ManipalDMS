using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class DashboardForm : Form
    {
        private readonly string currentUser;
        private readonly string userRole;

        public DashboardForm(string username, string role)
        {
            InitializeComponent();
            currentUser = username;
            userRole = role;
            labelRole.Text = "Signed in: " + currentUser + " | " + userRole;
            StartPosition = FormStartPosition.CenterScreen;
            ApplyRoleAccess();
            LoadSummary();
        }

        private void LoadSummary()
        {
            try
            {
                labelDocsCount.Text = GetCount("SELECT COUNT(*) FROM dbo.Documents");
                labelAdsCount.Text = GetCount("SELECT COUNT(*) FROM dbo.Advertisements");
                labelPubCount.Text = GetCount("SELECT COUNT(*) FROM dbo.Publishing");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Unable to load dashboard summary: {0}", ex);
                labelDocsCount.Text = labelAdsCount.Text = labelPubCount.Text = "--";
            }
        }

        private static string GetCount(string query)
        {
            DataTable table = DatabaseHelper.ExecuteQuery(query);
            return table.Rows.Count == 0 || table.Rows[0][0] == DBNull.Value ? "0" : table.Rows[0][0].ToString();
        }

        private void ApplyRoleAccess()
        {
            buttonDocs.Enabled = SecurityPolicy.CanManageDocuments(userRole);
            buttonAds.Enabled = SecurityPolicy.CanManageAds(userRole);
            buttonPublish.Enabled = SecurityPolicy.CanManagePublishing(userRole);
            buttonAds.BackColor = buttonAds.Enabled ? SystemColors.Control : Color.FromArgb(180, 180, 180);
            buttonPublish.BackColor = buttonPublish.Enabled ? SystemColors.Control : Color.FromArgb(180, 180, 180);
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManageDocuments(userRole)) { Deny(); return; }
            using (DocumentsForm form = new DocumentsForm(currentUser)) form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonAds_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManageAds(userRole)) { Deny(); return; }
            using (AdsForm form = new AdsForm(currentUser, userRole)) form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManagePublishing(userRole)) { Deny(); return; }
            using (PublishingForm form = new PublishingForm(currentUser, userRole)) form.ShowDialog(this);
            LoadSummary();
        }

        private static void Deny()
        {
            MessageBox.Show("You do not have permission to perform this action.", "Authorization", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

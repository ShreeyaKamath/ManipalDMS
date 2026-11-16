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
                labelDocsCount.Text = GetCount("SELECT COUNT(*) FROM Documents");
                labelAdsCount.Text = GetCount("SELECT COUNT(*) FROM Advertisements");
                labelPubCount.Text = GetCount("SELECT COUNT(*) FROM Publishing");
            }
            catch (Exception)
            {
                labelDocsCount.Text = "--";
                labelAdsCount.Text = "--";
                labelPubCount.Text = "--";
            }
        }

        private static string GetCount(string query)
        {
            DataTable table = DatabaseHelper.ExecuteQuery(query);
            if (table.Rows.Count == 0 || table.Rows[0][0] == null || table.Rows[0][0] == DBNull.Value)
                return "0";
            return table.Rows[0][0].ToString();
        }

        private void ApplyRoleAccess()
        {
            bool canViewDocuments = SecurityPolicy.CanManageDocuments(userRole);
            bool canManageAds = SecurityPolicy.CanManageAds(userRole);
            bool canManagePublishing = SecurityPolicy.CanManagePublishing(userRole);

            buttonDocs.Enabled = canViewDocuments;
            buttonAds.Enabled = canManageAds;
            buttonPublish.Enabled = canManagePublishing;

            buttonAds.BackColor = canManageAds ? SystemColors.Control : Color.FromArgb(180, 180, 180);
            buttonPublish.BackColor = canManagePublishing ? SystemColors.Control : Color.FromArgb(180, 180, 180);
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            using (DocumentsForm form = new DocumentsForm(currentUser))
                form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonAds_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManageAds(userRole))
            {
                MessageBox.Show("Access denied. Only administrators can manage advertisements.", "Authorization",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (AdsForm form = new AdsForm(currentUser))
                form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManagePublishing(userRole))
            {
                MessageBox.Show("Access denied. Only administrators can manage publishing records.", "Authorization",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (PublishingForm form = new PublishingForm(currentUser))
                form.ShowDialog(this);
            LoadSummary();
        }
    }
}

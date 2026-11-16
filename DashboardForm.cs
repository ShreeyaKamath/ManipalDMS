using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class DashboardForm : Form
    {
        private readonly string currentUser;
        private readonly string userRole;
        private readonly Button buttonUsers;
        private readonly Button buttonAudit;

        public DashboardForm(string username, string role)
        {
            InitializeComponent();
            currentUser = username;
            userRole = role;
            labelRole.Text = "Signed in: " + currentUser + " | " + userRole;
            StartPosition = FormStartPosition.CenterScreen;

            buttonUsers = CreateDashboardButton("User Management", new Point(55, 350), Color.FromArgb(108, 117, 125));
            buttonUsers.Click += buttonUsers_Click;
            panelContent.Controls.Add(buttonUsers);
            buttonAudit = CreateDashboardButton("Audit Log", new Point(660, 350), Color.FromArgb(65, 85, 110));
            buttonAudit.Click += buttonAudit_Click;
            panelContent.Controls.Add(buttonAudit);

            ApplyRoleAccess();
            LoadSummary();
        }

        private static Button CreateDashboardButton(string text, Point location, Color backColor)
        {
            return new Button
            {
                Text = text, Width = 230, Height = 48, Location = location, BackColor = backColor,
                ForeColor = Color.White, FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 12F, FontStyle.Bold)
            };
        }

        private void LoadSummary()
        {
            try
            {
                if (SecurityPolicy.HasPermission(userRole, Permission.DocumentsRead))
                {
                    DataTable docs = DatabaseHelper.ExecuteQuery(
                        @"SELECT COUNT(*) FROM dbo.Documents
                          WHERE (@CanViewAll = 1 OR UploadedBy = @CurrentUser)",
                        new SqlParameter("@CanViewAll", SqlDbType.Bit) { Value = SecurityPolicy.CanViewAllDocuments(userRole) },
                        new SqlParameter("@CurrentUser", SqlDbType.NVarChar, 50) { Value = currentUser });
                    labelDocsCount.Text = docs.Rows.Count == 0 ? "0" : Convert.ToString(docs.Rows[0][0]);
                }
                else labelDocsCount.Text = "—";

                labelAdsCount.Text = SecurityPolicy.HasPermission(userRole, Permission.AdsRead)
                    ? GetCount("SELECT COUNT(*) FROM dbo.Advertisements") : "—";
                labelPubCount.Text = SecurityPolicy.HasPermission(userRole, Permission.PublishingRead)
                    ? GetCount("SELECT COUNT(*) FROM dbo.Publishing") : "—";
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
            bool docs = SecurityPolicy.HasPermission(userRole, Permission.DocumentsRead);
            bool ads = SecurityPolicy.HasPermission(userRole, Permission.AdsRead);
            bool publishing = SecurityPolicy.HasPermission(userRole, Permission.PublishingRead);
            bool users = SecurityPolicy.CanManageUsers(userRole);
            bool audit = SecurityPolicy.CanViewAuditLog(userRole);
            buttonDocs.Enabled = buttonDocs.Visible = docs;
            buttonAds.Enabled = buttonAds.Visible = ads;
            buttonPublish.Enabled = buttonPublish.Visible = publishing;
            buttonUsers.Enabled = buttonUsers.Visible = users;
            buttonAudit.Enabled = buttonAudit.Visible = audit;
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.HasPermission(userRole, Permission.DocumentsRead)) { Deny(); return; }
            using (DocumentsForm form = new DocumentsForm(currentUser, userRole)) form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonAds_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.HasPermission(userRole, Permission.AdsRead)) { Deny(); return; }
            using (AdsForm form = new AdsForm(currentUser, userRole)) form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.HasPermission(userRole, Permission.PublishingRead)) { Deny(); return; }
            using (PublishingForm form = new PublishingForm(currentUser, userRole)) form.ShowDialog(this);
            LoadSummary();
        }

        private void buttonUsers_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanManageUsers(userRole)) { Deny(); return; }
            using (UserManagementForm form = new UserManagementForm(currentUser, userRole)) form.ShowDialog(this);
        }

        private void buttonAudit_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.CanViewAuditLog(userRole)) { Deny(); return; }
            using (AuditLogForm form = new AuditLogForm(userRole)) form.ShowDialog(this);
        }

        private static void Deny()
        {
            MessageBox.Show("You do not have permission to perform this action.", "Authorization", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

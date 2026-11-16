using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class AdsForm : Form
    {
        private readonly string currentUser;
        private readonly string currentRole;

        public AdsForm(string username, string role)
        {
            if (!SecurityPolicy.HasPermission(role, Permission.AdsRead)) throw new UnauthorizedAccessException("Role is not authorized to view advertisements.");
            InitializeComponent();
            currentUser = username;
            currentRole = role;
            dataGridViewAds.ReadOnly = true;
            dataGridViewAds.AllowUserToAddRows = false;
            dataGridViewAds.AllowUserToDeleteRows = false;
            dataGridViewAds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            buttonAddAd.Enabled = SecurityPolicy.HasPermission(currentRole, Permission.AdsWrite);
            LoadAds();
        }

        private void LoadAds()
        {
            if (!SecurityPolicy.HasPermission(currentRole, Permission.AdsRead)) throw new UnauthorizedAccessException("Advertisement read permission is required.");
            try { dataGridViewAds.DataSource = DatabaseHelper.ExecuteQuery("SELECT AdID, ClientName, AdType, Cost, DatePosted FROM dbo.Advertisements ORDER BY DatePosted DESC"); }
            catch (Exception ex) { ShowError(ex); }
        }

        private void buttonAddAd_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.HasPermission(currentRole, Permission.AdsWrite)) { Deny(); return; }
            string client = textBoxClient.Text.Trim();
            string type = textBoxType.Text.Trim();
            decimal cost;
            if (client.Length == 0 || type.Length == 0 || client.Length > 100 || type.Length > 50 ||
                !decimal.TryParse(textBoxCost.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out cost) || cost < 0 || cost >= 100000000m)
            { MessageBox.Show("Enter valid advertisement details and a non-negative cost.", "Advertisement details", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            try
            {
                DatabaseHelper.ExecuteNonQuery("INSERT INTO dbo.Advertisements (ClientName, AdType, Cost) VALUES (@ClientName, @AdType, @Cost)",
                    new SqlParameter("@ClientName", SqlDbType.NVarChar, 100) { Value = client },
                    new SqlParameter("@AdType", SqlDbType.NVarChar, 50) { Value = type },
                    new SqlParameter("@Cost", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = cost });
                DatabaseHelper.LogActivity(currentUser, "Create", "Advertisements", "Client: " + client + "; Type: " + type);
                textBoxClient.Clear(); textBoxType.Clear(); textBoxCost.Clear(); LoadAds();
                MessageBox.Show("Advertisement added.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private static void Deny() { MessageBox.Show("You do not have permission to perform this action.", "Authorization", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
        private static void ShowError(Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Advertisement operation failed: {0}", ex);
            MessageBox.Show("Unable to load or save advertisements. Check the database connection.", "Manipal DMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

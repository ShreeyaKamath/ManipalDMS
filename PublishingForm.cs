using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class PublishingForm : Form
    {
        private readonly string currentUser;
        private readonly string currentRole;

        public PublishingForm(string username, string role)
        {
            if (!SecurityPolicy.HasPermission(role, Permission.PublishingRead)) throw new UnauthorizedAccessException("Role is not authorized to view publishing records.");
            InitializeComponent();
            currentUser = username;
            currentRole = role;
            dataGridViewPub.ReadOnly = true;
            dataGridViewPub.AllowUserToAddRows = false;
            dataGridViewPub.AllowUserToDeleteRows = false;
            dataGridViewPub.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPub.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            buttonAdd.Enabled = SecurityPolicy.HasPermission(currentRole, Permission.PublishingWrite);
            LoadPub();
        }

        private void LoadPub()
        {
            if (!SecurityPolicy.HasPermission(currentRole, Permission.PublishingRead)) throw new UnauthorizedAccessException("Publishing read permission is required.");
            try { dataGridViewPub.DataSource = DatabaseHelper.ExecuteQuery("SELECT PubID, ArticleTitle, PublishDate, Cost, Revenue FROM dbo.Publishing ORDER BY PublishDate DESC"); }
            catch (Exception ex) { ShowError(ex); }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            if (!SecurityPolicy.HasPermission(currentRole, Permission.PublishingWrite))
            { MessageBox.Show("You do not have permission to perform this action.", "Authorization", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string title = textBoxTitle.Text.Trim();
            decimal cost, revenue;
            if (title.Length == 0 || title.Length > 100 ||
                !decimal.TryParse(textBoxCost.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out cost) || cost < 0 || cost >= 100000000m ||
                !decimal.TryParse(textBoxRevenue.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out revenue) || revenue < 0 || revenue >= 100000000m)
            { MessageBox.Show("Enter an article title and valid non-negative cost and revenue amounts.", "Publishing details", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            try
            {
                DatabaseHelper.ExecuteNonQuery("INSERT INTO dbo.Publishing (ArticleTitle, Cost, Revenue) VALUES (@ArticleTitle, @Cost, @Revenue)",
                    new SqlParameter("@ArticleTitle", SqlDbType.NVarChar, 100) { Value = title },
                    new SqlParameter("@Cost", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = cost },
                    new SqlParameter("@Revenue", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = revenue });
                DatabaseHelper.LogActivity(currentUser, "Create", "Publishing", "Article: " + title);
                textBoxTitle.Clear(); textBoxCost.Clear(); textBoxRevenue.Clear(); LoadPub();
                MessageBox.Show("Publishing record added.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { ShowError(ex); }
        }

        private static void ShowError(Exception ex)
        {
            System.Diagnostics.Trace.TraceError("Publishing operation failed: {0}", ex);
            MessageBox.Show("Unable to load or save publishing records. Check the database connection.", "Manipal DMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

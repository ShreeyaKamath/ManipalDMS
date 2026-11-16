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

        public AdsForm(string username)
        {
            InitializeComponent();
            currentUser = username;
            dataGridViewAds.ReadOnly = true;
            dataGridViewAds.AllowUserToAddRows = false;
            dataGridViewAds.AllowUserToDeleteRows = false;
            dataGridViewAds.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewAds.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadAds();
        }

        private void LoadAds()
        {
            try
            {
                dataGridViewAds.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Advertisements ORDER BY DatePosted DESC");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonAddAd_Click(object sender, EventArgs e)
        {
            string client = textBoxClient.Text.Trim();
            string type = textBoxType.Text.Trim();
            decimal cost;
            if (client.Length == 0 || type.Length == 0 ||
                !decimal.TryParse(textBoxCost.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out cost) || cost < 0)
            {
                MessageBox.Show("Enter a client, advertisement type, and a valid non-negative cost.", "Advertisement details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "INSERT INTO Advertisements (ClientName, AdType, Cost) VALUES (@ClientName, @AdType, @Cost)",
                    new SqlParameter("@ClientName", SqlDbType.NVarChar, 100) { Value = client },
                    new SqlParameter("@AdType", SqlDbType.NVarChar, 50) { Value = type },
                    new SqlParameter("@Cost", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = cost });
                DatabaseHelper.LogActivity(currentUser, "Create", "Advertisements", "Client: " + client + "; Type: " + type);
                textBoxClient.Clear();
                textBoxType.Clear();
                textBoxCost.Clear();
                LoadAds();
                MessageBox.Show("Advertisement added.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private static void ShowError(Exception ex)
        {
            MessageBox.Show("Unable to load or save advertisements. Check the database connection.\n\n" + ex.Message,
                "Manipal DMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

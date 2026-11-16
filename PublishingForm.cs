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

        public PublishingForm(string username)
        {
            InitializeComponent();
            currentUser = username;
            dataGridViewPub.ReadOnly = true;
            dataGridViewPub.AllowUserToAddRows = false;
            dataGridViewPub.AllowUserToDeleteRows = false;
            dataGridViewPub.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPub.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadPub();
        }

        private void LoadPub()
        {
            try
            {
                dataGridViewPub.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Publishing ORDER BY PublishDate DESC");
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text.Trim();
            decimal cost;
            decimal revenue;
            if (title.Length == 0 ||
                !decimal.TryParse(textBoxCost.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out cost) || cost < 0 ||
                !decimal.TryParse(textBoxRevenue.Text, NumberStyles.Currency, CultureInfo.CurrentCulture, out revenue) || revenue < 0)
            {
                MessageBox.Show("Enter an article title and valid non-negative cost and revenue amounts.", "Publishing details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "INSERT INTO Publishing (ArticleTitle, Cost, Revenue) VALUES (@ArticleTitle, @Cost, @Revenue)",
                    new SqlParameter("@ArticleTitle", SqlDbType.NVarChar, 100) { Value = title },
                    new SqlParameter("@Cost", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = cost },
                    new SqlParameter("@Revenue", SqlDbType.Decimal) { Precision = 10, Scale = 2, Value = revenue });
                DatabaseHelper.LogActivity(currentUser, "Create", "Publishing", "Article: " + title);
                textBoxTitle.Clear();
                textBoxCost.Clear();
                textBoxRevenue.Clear();
                LoadPub();
                MessageBox.Show("Publishing record added.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError(ex);
            }
        }

        private static void ShowError(Exception ex)
        {
            MessageBox.Show("Unable to load or save publishing records. Check the database connection.\n\n" + ex.Message,
                "Manipal DMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

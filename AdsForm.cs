using System;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class AdsForm : Form
    {
        public AdsForm()
        {
            InitializeComponent();
            LoadAds();
        }

        void LoadAds()
        {
            dataGridViewAds.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Advertisements");
        }

        private void buttonAddAd_Click(object sender, EventArgs e)
        {
            string client = textBoxClient.Text;
            string type = textBoxType.Text;
            decimal cost = Convert.ToDecimal(textBoxCost.Text);

            string query = $"INSERT INTO Advertisements (ClientName, AdType, Cost) VALUES " +
                           $"('{client}', '{type}', {cost})";
            DatabaseHelper.ExecuteNonQuery(query);
            LoadAds();
        }
    }
}

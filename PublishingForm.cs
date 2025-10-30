using System;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class PublishingForm : Form
    {
        public PublishingForm()
        {
            InitializeComponent();
            LoadPub();
        }

        void LoadPub()
        {
            dataGridViewPub.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Publishing");
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text;
            decimal cost = Convert.ToDecimal(textBoxCost.Text);
            decimal revenue = Convert.ToDecimal(textBoxRevenue.Text);

            string query = $"INSERT INTO Publishing (ArticleTitle, Cost, Revenue) VALUES " +
                           $"('{title}', {cost}, {revenue})";
            DatabaseHelper.ExecuteNonQuery(query);
            LoadPub();
        }
    }
}



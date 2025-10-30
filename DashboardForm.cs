using System;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class DashboardForm : Form
    {
        string userRole;

        public DashboardForm(string role)
        {
            InitializeComponent();
            labelRole.Text = $"Logged in as: {role}";
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            new DocumentsForm().ShowDialog();
        }

        private void buttonAds_Click(object sender, EventArgs e)
        {
            new AdsForm().ShowDialog();
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            new PublishingForm().ShowDialog();
        }
    }
}




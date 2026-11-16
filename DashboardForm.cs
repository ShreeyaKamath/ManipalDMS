using System;
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
            labelRole.Text = "Signed in: " + currentUser + "  |  " + userRole;
            buttonDocs.Click += buttonDocs_Click;
            buttonAds.Click += buttonAds_Click;
            buttonPublish.Click += buttonPublish_Click;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void buttonDocs_Click(object sender, EventArgs e)
        {
            using (DocumentsForm form = new DocumentsForm(currentUser))
                form.ShowDialog(this);
        }

        private void buttonAds_Click(object sender, EventArgs e)
        {
            using (AdsForm form = new AdsForm(currentUser))
                form.ShowDialog(this);
        }

        private void buttonPublish_Click(object sender, EventArgs e)
        {
            using (PublishingForm form = new PublishingForm(currentUser))
                form.ShowDialog(this);
        }
    }
}

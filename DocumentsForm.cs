using System;
using System.IO;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class DocumentsForm : Form
    {
        private string docsDirectory = @"C:\ManipalDMSFiles\";  // Directory for storing uploaded files
        private string currentUser = "admin"; // Ideally set dynamically on login

        public DocumentsForm()
        {
            InitializeComponent();

            // Event handler wiring
            this.Load += DocumentsForm_Load;
            buttonUpload.Click += buttonUpload_Click;
            buttonDownload.Click += buttonDownload_Click;
            buttonSearch.Click += buttonSearch_Click;
            buttonRefresh.Click += buttonRefresh_Click;

            LoadDocs();
        }

        private void DocumentsForm_Load(object sender, EventArgs e)
        {
            // Optional initialization on form load
        }

        private void LoadDocs()
        {
            dataGridView1.DataSource = DatabaseHelper.ExecuteQuery("SELECT * FROM Documents");
        }

        private void LogActivity(string action, string details)
        {
            DatabaseHelper.LogActivity(currentUser, action, "Documents", details);
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "All Files|*.*";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string sourcePath = ofd.FileName;
                string fileName = Path.GetFileName(sourcePath);
                string targetPath = Path.Combine(docsDirectory, fileName);

                if (!Directory.Exists(docsDirectory))
                    Directory.CreateDirectory(docsDirectory);

                File.Copy(sourcePath, targetPath, true);

                string title = textBoxTitle.Text.Trim();
                string category = textBoxCategory.Text.Trim();
                string uploadedBy = textBoxUploadedBy.Text.Trim();

                string query = $"INSERT INTO Documents (Title, Category, FilePath, StoredFileName, UploadedBy) VALUES " +
                               $"('{title}', '{category}', '{targetPath}', '{fileName}', '{uploadedBy}')";

                DatabaseHelper.ExecuteNonQuery(query);

                LogActivity("Upload", $"File:{fileName}; Title:{title}");

                LoadDocs();
                MessageBox.Show("Document uploaded and saved.");
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;

            string storedFileName = dataGridView1.SelectedRows[0].Cells["StoredFileName"].Value.ToString();
            string fullFilePath = Path.Combine(docsDirectory, storedFileName);

            if (!File.Exists(fullFilePath))
            {
                MessageBox.Show("File not found in storage.");
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.FileName = storedFileName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.Copy(fullFilePath, sfd.FileName, true);
                LogActivity("Download", $"File:{storedFileName}");
                MessageBox.Show("File downloaded successfully.");
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string title = textBoxTitleSearch.Text.Trim();
            string category = textBoxCategorySearch.Text.Trim();
            string uploader = textBoxUploaderSearch.Text.Trim();

            DateTime from = dateTimeFrom.Value.Date;
            DateTime to = dateTimeTo.Value.Date;
            string query = "SELECT * FROM Documents WHERE 1=1";

            if (!string.IsNullOrEmpty(title))
                query += $" AND Title LIKE '%{title}%'";

            if (!string.IsNullOrEmpty(category))
                query += $" AND Category LIKE '%{category}%'";

            if (!string.IsNullOrEmpty(uploader))
                query += $" AND UploadedBy LIKE '%{uploader}%'";

            query += $" AND UploadDate >= '{from:yyyy-MM-dd}' AND UploadDate <= '{to:yyyy-MM-dd}'";

            dataGridView1.DataSource = DatabaseHelper.ExecuteQuery(query);

            LogActivity("Search", $"Title:{title}, Category:{category}, Uploader:{uploader}, From:{from}, To:{to}");
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            LoadDocs();
        }
    }
}


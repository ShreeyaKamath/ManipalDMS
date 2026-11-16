using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class DocumentsForm : Form
    {
        private const long MaxDocumentSizeBytes = 25L * 1024L * 1024L;
        private static readonly string[] AllowedExtensions = { ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".txt", ".csv", ".png", ".jpg", ".jpeg" };

        private readonly string docsDirectory = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "ManipalDMS", "Documents");
        private readonly string currentUser;

        public DocumentsForm(string username)
        {
            InitializeComponent();
            currentUser = username;
            textBoxUploadedBy.Text = currentUser;
            textBoxUploadedBy.ReadOnly = true;
            dateTimeFrom.Value = DateTime.Today.AddYears(-10);
            dateTimeTo.Value = DateTime.Today;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            LoadDocs();
        }

        private void LoadDocs()
        {
            try
            {
                dataGridView1.DataSource = DatabaseHelper.ExecuteQuery(
                    "SELECT DocumentID, Title, Category, UploadedBy, UploadDate, StoredFileName FROM Documents ORDER BY UploadDate DESC");
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private void LogActivity(string action, string details)
        {
            DatabaseHelper.LogActivity(currentUser, action, "Documents", details);
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            string title = textBoxTitle.Text.Trim();
            string category = textBoxCategory.Text.Trim();
            if (title.Length == 0 || category.Length == 0)
            {
                MessageBox.Show("Enter a document title and category before uploading.", "Document details",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Choose a document to upload";
                dialog.Filter = "Allowed files|" + string.Join(";", AllowedExtensions.Select(ext => "*" + ext).ToArray());
                if (dialog.ShowDialog(this) != DialogResult.OK) return;

                FileInfo selectedFile = new FileInfo(dialog.FileName);
                if (!selectedFile.Exists)
                {
                    MessageBox.Show("The selected file could not be found.", "Upload failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string extension = Path.GetExtension(dialog.FileName);
                if (!AllowedExtensions.Any(ext => string.Equals(ext, extension, StringComparison.OrdinalIgnoreCase)))
                {
                    MessageBox.Show("The selected file type is not allowed for this application.", "Upload failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (selectedFile.Length == 0 || selectedFile.Length > MaxDocumentSizeBytes)
                {
                    MessageBox.Show("The file size must be between 1 byte and 25 MB.", "Upload failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string storedFileName = Guid.NewGuid().ToString("N") + extension;
                string targetPath = Path.Combine(docsDirectory, storedFileName);
                try
                {
                    Directory.CreateDirectory(docsDirectory);
                    File.Copy(dialog.FileName, targetPath, false);
                    DatabaseHelper.ExecuteNonQuery(
                        "INSERT INTO Documents (Title, Category, FilePath, StoredFileName, UploadedBy) " +
                        "VALUES (@Title, @Category, @FilePath, @StoredFileName, @UploadedBy)",
                        new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = title },
                        new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = category },
                        new SqlParameter("@FilePath", SqlDbType.NVarChar, 255) { Value = targetPath },
                        new SqlParameter("@StoredFileName", SqlDbType.NVarChar, 255) { Value = storedFileName },
                        new SqlParameter("@UploadedBy", SqlDbType.NVarChar, 50) { Value = currentUser });
                    LogActivity("Upload", "File: " + storedFileName + "; Title: " + title);
                    textBoxTitle.Clear();
                    textBoxCategory.Clear();
                    LoadDocs();
                    MessageBox.Show("Document uploaded successfully.", "Upload complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    if (File.Exists(targetPath)) File.Delete(targetPath);
                    ShowDatabaseError(ex);
                }
            }
        }

        private void buttonDownload_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a document to download.", "Download", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];
            string storedFileName = Convert.ToString(row.Cells["StoredFileName"].Value);
            if (string.IsNullOrWhiteSpace(storedFileName))
            {
                MessageBox.Show("This document does not have a stored file name. Refresh after configuring file storage.",
                    "File unavailable", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string sourcePath = Path.Combine(docsDirectory, Path.GetFileName(storedFileName));
            if (!File.Exists(sourcePath))
            {
                MessageBox.Show("The document file is not present in the storage folder.", "File unavailable",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.FileName = Path.GetFileName(sourcePath);
                dialog.Title = "Save downloaded document";
                if (dialog.ShowDialog(this) != DialogResult.OK) return;
                try
                {
                    File.Copy(sourcePath, dialog.FileName, true);
                    LogActivity("Download", "File: " + storedFileName);
                    MessageBox.Show("Document downloaded successfully.", "Download complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    ShowDatabaseError(ex);
                }
            }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            DateTime from = dateTimeFrom.Value.Date;
            DateTime to = dateTimeTo.Value.Date;
            if (from > to)
            {
                MessageBox.Show("The start date must be on or before the end date.", "Search dates",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string query = "SELECT DocumentID, Title, Category, UploadedBy, UploadDate, StoredFileName FROM Documents WHERE UploadDate >= @From AND UploadDate < @To ORDER BY UploadDate DESC";
            SqlParameter[] parameters = {
                new SqlParameter("@From", SqlDbType.DateTime) { Value = from },
                new SqlParameter("@To", SqlDbType.DateTime) { Value = to.AddDays(1) }
            };
            string title = textBoxTitleSearch.Text.Trim();
            string category = textBoxCategorySearch.Text.Trim();
            string uploader = textBoxUploaderSearch.Text.Trim();
            if (title.Length > 0)
            {
                query = query.Replace(" ORDER BY", " AND Title LIKE @Title ORDER BY");
                parameters = AddParameter(parameters, new SqlParameter("@Title", SqlDbType.NVarChar, 100) { Value = "%" + title + "%" });
            }
            if (category.Length > 0)
            {
                query = query.Replace(" ORDER BY", " AND Category LIKE @Category ORDER BY");
                parameters = AddParameter(parameters, new SqlParameter("@Category", SqlDbType.NVarChar, 50) { Value = "%" + category + "%" });
            }
            if (uploader.Length > 0)
            {
                query = query.Replace(" ORDER BY", " AND UploadedBy LIKE @Uploader ORDER BY");
                parameters = AddParameter(parameters, new SqlParameter("@Uploader", SqlDbType.NVarChar, 50) { Value = "%" + uploader + "%" });
            }

            try
            {
                dataGridView1.DataSource = DatabaseHelper.ExecuteQuery(query, parameters);
                LogActivity("Search", string.Format(CultureInfo.CurrentCulture,
                    "Title:{0}, Category:{1}, Uploader:{2}, From:{3:d}, To:{4:d}", title, category, uploader, from, to));
            }
            catch (Exception ex)
            {
                ShowDatabaseError(ex);
            }
        }

        private static SqlParameter[] AddParameter(SqlParameter[] current, SqlParameter parameter)
        {
            SqlParameter[] updated = new SqlParameter[current.Length + 1];
            Array.Copy(current, updated, current.Length);
            updated[current.Length] = parameter;
            return updated;
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            textBoxTitleSearch.Clear();
            textBoxCategorySearch.Clear();
            textBoxUploaderSearch.Clear();
            LoadDocs();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                dataGridView1.Rows[e.RowIndex].Selected = true;
                buttonDownload_Click(sender, EventArgs.Empty);
            }
        }

        private static void ShowDatabaseError(Exception ex)
        {
            MessageBox.Show("The operation could not be completed. Verify the database settings and document folder permissions.\n\n" + ex.Message,
                "Manipal DMS", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}

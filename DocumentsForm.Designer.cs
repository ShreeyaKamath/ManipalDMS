namespace ManipalDMS
{
    partial class DocumentsForm
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCategory;
        private System.Windows.Forms.Label labelUploadedBy;

        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxCategory;
        private System.Windows.Forms.TextBox textBoxUploadedBy;

        private System.Windows.Forms.Label labelSearchTitle;
        private System.Windows.Forms.Label labelSearchCategory;
        private System.Windows.Forms.Label labelSearchUploader;
        private System.Windows.Forms.Label labelSearchFrom;
        private System.Windows.Forms.Label labelSearchTo;

        private System.Windows.Forms.TextBox textBoxTitleSearch;
        private System.Windows.Forms.TextBox textBoxCategorySearch;
        private System.Windows.Forms.TextBox textBoxUploaderSearch;

        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonDownload;
        private System.Windows.Forms.Button buttonSearch;
        private System.Windows.Forms.Button buttonRefresh;

        private System.Windows.Forms.DataGridView dataGridView1;

        private System.Windows.Forms.DateTimePicker dateTimeFrom;
        private System.Windows.Forms.DateTimePicker dateTimeTo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCategory = new System.Windows.Forms.Label();
            this.labelUploadedBy = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxCategory = new System.Windows.Forms.TextBox();
            this.textBoxUploadedBy = new System.Windows.Forms.TextBox();
            this.labelSearchTitle = new System.Windows.Forms.Label();
            this.labelSearchCategory = new System.Windows.Forms.Label();
            this.labelSearchUploader = new System.Windows.Forms.Label();
            this.labelSearchFrom = new System.Windows.Forms.Label();
            this.labelSearchTo = new System.Windows.Forms.Label();
            this.textBoxTitleSearch = new System.Windows.Forms.TextBox();
            this.textBoxCategorySearch = new System.Windows.Forms.TextBox();
            this.textBoxUploaderSearch = new System.Windows.Forms.TextBox();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonDownload = new System.Windows.Forms.Button();
            this.buttonSearch = new System.Windows.Forms.Button();
            this.buttonRefresh = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dateTimeFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimeTo = new System.Windows.Forms.DateTimePicker();

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();

            // labelTitle
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(20, 10);
            this.labelTitle.Text = "Document Information";

            // labelCategory
            this.labelCategory.AutoSize = true;
            this.labelCategory.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelCategory.Location = new System.Drawing.Point(21, 120);
            this.labelCategory.Text = "Category:";

            // labelUploadedBy
            this.labelUploadedBy.AutoSize = true;
            this.labelUploadedBy.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.labelUploadedBy.Location = new System.Drawing.Point(21, 188);
            this.labelUploadedBy.Text = "Uploaded By:";

            // textBoxTitle
            this.textBoxTitle.Location = new System.Drawing.Point(20, 84);
            this.textBoxTitle.Size = new System.Drawing.Size(200, 22);

            // textBoxCategory
            this.textBoxCategory.Location = new System.Drawing.Point(20, 163);
            this.textBoxCategory.Size = new System.Drawing.Size(200, 22);

            // textBoxUploadedBy
            this.textBoxUploadedBy.Location = new System.Drawing.Point(20, 235);
            this.textBoxUploadedBy.Size = new System.Drawing.Size(200, 22);

            // labelSearchTitle
            this.labelSearchTitle.AutoSize = true;
            this.labelSearchTitle.Location = new System.Drawing.Point(260, 40);
            this.labelSearchTitle.Text = "Title:";

            // labelSearchCategory
            this.labelSearchCategory.AutoSize = true;
            this.labelSearchCategory.Location = new System.Drawing.Point(400, 40);
            this.labelSearchCategory.Text = "Category:";

            // labelSearchUploader
            this.labelSearchUploader.AutoSize = true;
            this.labelSearchUploader.Location = new System.Drawing.Point(560, 40);
            this.labelSearchUploader.Text = "Uploaded By:";

            // labelSearchFrom
            this.labelSearchFrom.AutoSize = true;
            this.labelSearchFrom.Location = new System.Drawing.Point(720, 15);
            this.labelSearchFrom.Text = "From:";

            // labelSearchTo
            this.labelSearchTo.AutoSize = true;
            this.labelSearchTo.Location = new System.Drawing.Point(720, 45);
            this.labelSearchTo.Text = "To:";

            // textBoxTitleSearch
            this.textBoxTitleSearch.Location = new System.Drawing.Point(260, 60);
            this.textBoxTitleSearch.Size = new System.Drawing.Size(120, 22);

            // textBoxCategorySearch
            this.textBoxCategorySearch.Location = new System.Drawing.Point(400, 60);
            this.textBoxCategorySearch.Size = new System.Drawing.Size(120, 22);

            // textBoxUploaderSearch
            this.textBoxUploaderSearch.Location = new System.Drawing.Point(560, 60);
            this.textBoxUploaderSearch.Size = new System.Drawing.Size(120, 22);

            // buttonUpload
            this.buttonUpload.BackColor = System.Drawing.Color.RoyalBlue;
            this.buttonUpload.ForeColor = System.Drawing.Color.White;
            this.buttonUpload.Location = new System.Drawing.Point(45, 286);
            this.buttonUpload.Size = new System.Drawing.Size(140, 35);
            this.buttonUpload.Text = "Upload Document";
            this.buttonUpload.UseVisualStyleBackColor = false;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);

            // buttonDownload
            this.buttonDownload.BackColor = System.Drawing.Color.LightSteelBlue;
            this.buttonDownload.ForeColor = System.Drawing.Color.Black;
            this.buttonDownload.Location = new System.Drawing.Point(45, 346);
            this.buttonDownload.Size = new System.Drawing.Size(140, 35);
            this.buttonDownload.Text = "Download";
            this.buttonDownload.UseVisualStyleBackColor = false;
            this.buttonDownload.Click += new System.EventHandler(this.buttonDownload_Click);

            // buttonSearch
            this.buttonSearch.BackColor = System.Drawing.Color.RoyalBlue;
            this.buttonSearch.ForeColor = System.Drawing.Color.White;
            this.buttonSearch.Location = new System.Drawing.Point(660, 80);
            this.buttonSearch.Size = new System.Drawing.Size(90, 30);
            this.buttonSearch.Text = "Search";
            this.buttonSearch.UseVisualStyleBackColor = false;
            this.buttonSearch.Click += new System.EventHandler(this.buttonSearch_Click);

            // buttonRefresh
            this.buttonRefresh.BackColor = System.Drawing.Color.Gray;
            this.buttonRefresh.ForeColor = System.Drawing.Color.White;
            this.buttonRefresh.Location = new System.Drawing.Point(760, 80);
            this.buttonRefresh.Size = new System.Drawing.Size(90, 30);
            this.buttonRefresh.Text = "Refresh";
            this.buttonRefresh.UseVisualStyleBackColor = false;
            this.buttonRefresh.Click += new System.EventHandler(this.buttonRefresh_Click);

            // dataGridView1
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(260, 120);
            this.dataGridView1.Size = new System.Drawing.Size(680, 300);
            this.dataGridView1.ReadOnly = true;

            // dateTimeFrom
            this.dateTimeFrom.Location = new System.Drawing.Point(760, 10);
            this.dateTimeFrom.Size = new System.Drawing.Size(180, 22);

            // dateTimeTo
            this.dateTimeTo.Location = new System.Drawing.Point(760, 40);
            this.dateTimeTo.Size = new System.Drawing.Size(180, 22);

            // Form setup
            this.ClientSize = new System.Drawing.Size(970, 440);
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.labelCategory);
            this.Controls.Add(this.labelUploadedBy);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.textBoxCategory);
            this.Controls.Add(this.textBoxUploadedBy);
            this.Controls.Add(this.labelSearchTitle);
            this.Controls.Add(this.labelSearchCategory);
            this.Controls.Add(this.labelSearchUploader);
            this.Controls.Add(this.textBoxTitleSearch);
            this.Controls.Add(this.textBoxCategorySearch);
            this.Controls.Add(this.textBoxUploaderSearch);
            this.Controls.Add(this.labelSearchFrom);
            this.Controls.Add(this.labelSearchTo);
            this.Controls.Add(this.dateTimeFrom);
            this.Controls.Add(this.dateTimeTo);
            this.Controls.Add(this.buttonUpload);
            this.Controls.Add(this.buttonDownload);
            this.Controls.Add(this.buttonSearch);
            this.Controls.Add(this.buttonRefresh);
            this.Controls.Add(this.dataGridView1);

            this.Text = "Documents - Manipal DMS";

            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}




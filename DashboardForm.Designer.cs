namespace ManipalDMS
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.Button buttonDocs;
        private System.Windows.Forms.Button buttonAds;
        private System.Windows.Forms.Button buttonPublish;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelRole = new System.Windows.Forms.Label();
            this.buttonDocs = new System.Windows.Forms.Button();
            this.buttonAds = new System.Windows.Forms.Button();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.SteelBlue;
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.labelRole);
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(850, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(30, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(448, 46);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Manipal Document System";
            // 
            // labelRole
            // 
            this.labelRole.AutoSize = true;
            this.labelRole.Font = new System.Drawing.Font("Segoe UI", 11.5F);
            this.labelRole.ForeColor = System.Drawing.Color.White;
            this.labelRole.Location = new System.Drawing.Point(650, 20);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(132, 28);
            this.labelRole.TabIndex = 1;
            this.labelRole.Text = "Logged in as: ";
            // 
            // buttonDocs
            // 
            this.buttonDocs.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonDocs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDocs.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.buttonDocs.ForeColor = System.Drawing.Color.White;
            this.buttonDocs.Location = new System.Drawing.Point(26, 170);
            this.buttonDocs.Name = "buttonDocs";
            this.buttonDocs.Size = new System.Drawing.Size(180, 56);
            this.buttonDocs.TabIndex = 1;
            this.buttonDocs.Text = "Documents";
            this.buttonDocs.UseVisualStyleBackColor = false;
            this.buttonDocs.Click += new System.EventHandler(this.buttonDocs_Click);
            // 
            // buttonAds
            // 
            this.buttonAds.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonAds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAds.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.buttonAds.ForeColor = System.Drawing.Color.White;
            this.buttonAds.Location = new System.Drawing.Point(298, 170);
            this.buttonAds.Name = "buttonAds";
            this.buttonAds.Size = new System.Drawing.Size(213, 56);
            this.buttonAds.TabIndex = 2;
            this.buttonAds.Text = "Advertisements";
            this.buttonAds.UseVisualStyleBackColor = false;
            this.buttonAds.Click += new System.EventHandler(this.buttonAds_Click);
            // 
            // buttonPublish
            // 
            this.buttonPublish.BackColor = System.Drawing.Color.SteelBlue;
            this.buttonPublish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPublish.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.buttonPublish.ForeColor = System.Drawing.Color.White;
            this.buttonPublish.Location = new System.Drawing.Point(590, 170);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(180, 56);
            this.buttonPublish.TabIndex = 3;
            this.buttonPublish.Text = "Publishing";
            this.buttonPublish.UseVisualStyleBackColor = false;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);
            // 
            // DashboardForm
            // 
            this.ClientSize = new System.Drawing.Size(852, 343);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.buttonDocs);
            this.Controls.Add(this.buttonAds);
            this.Controls.Add(this.buttonPublish);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "DashboardForm";
            this.Text = "Dashboard - Manipal DMS";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}



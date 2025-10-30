namespace ManipalDMS
{
    partial class AdsForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelClient;
        private System.Windows.Forms.Label labelType;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.TextBox textBoxClient;
        private System.Windows.Forms.TextBox textBoxType;
        private System.Windows.Forms.TextBox textBoxCost;
        private System.Windows.Forms.Button buttonAddAd;
        private System.Windows.Forms.DataGridView dataGridViewAds;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelClient = new System.Windows.Forms.Label();
            this.labelType = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.textBoxClient = new System.Windows.Forms.TextBox();
            this.textBoxType = new System.Windows.Forms.TextBox();
            this.textBoxCost = new System.Windows.Forms.TextBox();
            this.buttonAddAd = new System.Windows.Forms.Button();
            this.dataGridViewAds = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAds)).BeginInit();
            this.SuspendLayout();
            //
            // labelClient
            //
            this.labelClient.AutoSize = true;
            this.labelClient.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelClient.ForeColor = System.Drawing.Color.Gray;
            this.labelClient.Location = new System.Drawing.Point(60, 60);
            this.labelClient.Name = "labelClient";
            this.labelClient.Size = new System.Drawing.Size(107, 25);
            this.labelClient.Text = "Client Name:";
            //
            // textBoxClient
            //
            this.textBoxClient.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxClient.Location = new System.Drawing.Point(200, 57);
            this.textBoxClient.Size = new System.Drawing.Size(230, 31);
            //
            // labelType
            //
            this.labelType.AutoSize = true;
            this.labelType.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelType.ForeColor = System.Drawing.Color.Gray;
            this.labelType.Location = new System.Drawing.Point(60, 110);
            this.labelType.Name = "labelType";
            this.labelType.Size = new System.Drawing.Size(70, 25);
            this.labelType.Text = "Ad Type:";
            //
            // textBoxType
            //
            this.textBoxType.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxType.Location = new System.Drawing.Point(200, 107);
            this.textBoxType.Size = new System.Drawing.Size(230, 31);
            //
            // labelCost
            //
            this.labelCost.AutoSize = true;
            this.labelCost.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelCost.ForeColor = System.Drawing.Color.Gray;
            this.labelCost.Location = new System.Drawing.Point(60, 160);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(51, 25);
            this.labelCost.Text = "Cost:";
            //
            // textBoxCost
            //
            this.textBoxCost.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxCost.Location = new System.Drawing.Point(200, 157);
            this.textBoxCost.Size = new System.Drawing.Size(230, 31);
            //
            // buttonAddAd
            //
            this.buttonAddAd.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.buttonAddAd.BackColor = System.Drawing.Color.RoyalBlue;
            this.buttonAddAd.ForeColor = System.Drawing.Color.White;
            this.buttonAddAd.Location = new System.Drawing.Point(150, 210);
            this.buttonAddAd.Size = new System.Drawing.Size(190, 40);
            this.buttonAddAd.Text = "Add Advertisement";
            this.buttonAddAd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAddAd.Click += new System.EventHandler(this.buttonAddAd_Click);
            //
            // dataGridViewAds
            //
            this.dataGridViewAds.Location = new System.Drawing.Point(470, 30);
            this.dataGridViewAds.Size = new System.Drawing.Size(350, 260);
            this.dataGridViewAds.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dataGridViewAds.ReadOnly = true;
            this.dataGridViewAds.AllowUserToAddRows = false;
            this.dataGridViewAds.AllowUserToDeleteRows = false;
            this.dataGridViewAds.RowHeadersVisible = false;
            this.dataGridViewAds.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //
            // AdsForm
            //
            this.ClientSize = new System.Drawing.Size(850, 330);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelClient);
            this.Controls.Add(this.textBoxClient);
            this.Controls.Add(this.labelType);
            this.Controls.Add(this.textBoxType);
            this.Controls.Add(this.labelCost);
            this.Controls.Add(this.textBoxCost);
            this.Controls.Add(this.buttonAddAd);
            this.Controls.Add(this.dataGridViewAds);
            this.Text = "Manage Advertisements";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAds)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}



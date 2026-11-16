using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelRole;
        private System.Windows.Forms.Label labelWelcome;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel cardDocuments;
        private System.Windows.Forms.Panel cardAds;
        private System.Windows.Forms.Panel cardPublishing;
        private System.Windows.Forms.Label labelDocsTitle;
        private System.Windows.Forms.Label labelDocsCount;
        private System.Windows.Forms.Label labelAdsTitle;
        private System.Windows.Forms.Label labelAdsCount;
        private System.Windows.Forms.Label labelPubTitle;
        private System.Windows.Forms.Label labelPubCount;
        private System.Windows.Forms.Button buttonDocs;
        private System.Windows.Forms.Button buttonAds;
        private System.Windows.Forms.Button buttonPublish;
        private System.Windows.Forms.Button buttonUsers;

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
            this.labelWelcome = new System.Windows.Forms.Label();
            this.panelContent = new System.Windows.Forms.Panel();
            this.cardDocuments = new System.Windows.Forms.Panel();
            this.cardAds = new System.Windows.Forms.Panel();
            this.cardPublishing = new System.Windows.Forms.Panel();
            this.labelDocsTitle = new System.Windows.Forms.Label();
            this.labelDocsCount = new System.Windows.Forms.Label();
            this.labelAdsTitle = new System.Windows.Forms.Label();
            this.labelAdsCount = new System.Windows.Forms.Label();
            this.labelPubTitle = new System.Windows.Forms.Label();
            this.labelPubCount = new System.Windows.Forms.Label();
            this.buttonDocs = new System.Windows.Forms.Button();
            this.buttonAds = new System.Windows.Forms.Button();
            this.buttonPublish = new System.Windows.Forms.Button();
            this.buttonUsers = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelContent.SuspendLayout();
            this.cardDocuments.SuspendLayout();
            this.cardAds.SuspendLayout();
            this.cardPublishing.SuspendLayout();
            this.SuspendLayout();

            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(19)))), ((int)(((byte)(69)))), ((int)(((byte)(120)))));
            this.panelHeader.Controls.Add(this.labelTitle);
            this.panelHeader.Controls.Add(this.labelRole);
            this.panelHeader.Controls.Add(this.labelWelcome);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1000, 110);
            this.panelHeader.TabIndex = 0;

            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTitle.ForeColor = System.Drawing.Color.White;
            this.labelTitle.Location = new System.Drawing.Point(25, 25);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(414, 45);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Manipal Document System";

            this.labelWelcome.AutoSize = true;
            this.labelWelcome.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelWelcome.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(230)))), ((int)(((byte)(255)))));
            this.labelWelcome.Location = new System.Drawing.Point(28, 75);
            this.labelWelcome.Name = "labelWelcome";
            this.labelWelcome.Size = new System.Drawing.Size(127, 19);
            this.labelWelcome.TabIndex = 1;
            this.labelWelcome.Text = "Operations dashboard";

            this.labelRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.labelRole.AutoSize = true;
            this.labelRole.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelRole.ForeColor = System.Drawing.Color.White;
            this.labelRole.Location = new System.Drawing.Point(690, 42);
            this.labelRole.Name = "labelRole";
            this.labelRole.Size = new System.Drawing.Size(129, 25);
            this.labelRole.TabIndex = 2;
            this.labelRole.Text = "Signed in: User";

            this.panelContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(244)))), ((int)(((byte)(249)))));
            this.panelContent.Controls.Add(this.cardDocuments);
            this.panelContent.Controls.Add(this.cardAds);
            this.panelContent.Controls.Add(this.cardPublishing);
            this.panelContent.Controls.Add(this.buttonDocs);
            this.panelContent.Controls.Add(this.buttonAds);
            this.panelContent.Controls.Add(this.buttonPublish);
            this.panelContent.Controls.Add(this.buttonUsers);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 110);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1000, 450);
            this.panelContent.TabIndex = 1;

            this.cardDocuments.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(239)))), ((int)(((byte)(255)))));
            this.cardDocuments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardDocuments.Controls.Add(this.labelDocsTitle);
            this.cardDocuments.Controls.Add(this.labelDocsCount);
            this.cardDocuments.Location = new System.Drawing.Point(55, 50);
            this.cardDocuments.Name = "cardDocuments";
            this.cardDocuments.Size = new System.Drawing.Size(240, 130);
            this.cardDocuments.TabIndex = 3;

            this.labelDocsTitle.AutoSize = true;
            this.labelDocsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDocsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(91)))), ((int)(((byte)(157)))));
            this.labelDocsTitle.Location = new System.Drawing.Point(20, 18);
            this.labelDocsTitle.Name = "labelDocsTitle";
            this.labelDocsTitle.Size = new System.Drawing.Size(120, 32);
            this.labelDocsTitle.TabIndex = 0;
            this.labelDocsTitle.Text = "Documents";

            this.labelDocsCount.AutoSize = true;
            this.labelDocsCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDocsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(82)))), ((int)(((byte)(150)))));
            this.labelDocsCount.Location = new System.Drawing.Point(18, 60);
            this.labelDocsCount.Name = "labelDocsCount";
            this.labelDocsCount.Size = new System.Drawing.Size(72, 62);
            this.labelDocsCount.TabIndex = 1;
            this.labelDocsCount.Text = "0";

            this.cardAds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(228)))), ((int)(((byte)(245)))), ((int)(((byte)(233)))));
            this.cardAds.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardAds.Controls.Add(this.labelAdsTitle);
            this.cardAds.Controls.Add(this.labelAdsCount);
            this.cardAds.Location = new System.Drawing.Point(380, 50);
            this.cardAds.Name = "cardAds";
            this.cardAds.Size = new System.Drawing.Size(240, 130);
            this.cardAds.TabIndex = 4;

            this.labelAdsTitle.AutoSize = true;
            this.labelAdsTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdsTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(32)))), ((int)(((byte)(120)))), ((int)(((byte)(83)))));
            this.labelAdsTitle.Location = new System.Drawing.Point(20, 18);
            this.labelAdsTitle.Name = "labelAdsTitle";
            this.labelAdsTitle.Size = new System.Drawing.Size(144, 32);
            this.labelAdsTitle.TabIndex = 0;
            this.labelAdsTitle.Text = "Advertisements";

            this.labelAdsCount.AutoSize = true;
            this.labelAdsCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAdsCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(27)))), ((int)(((byte)(102)))), ((int)(((byte)(69)))));
            this.labelAdsCount.Location = new System.Drawing.Point(18, 60);
            this.labelAdsCount.Name = "labelAdsCount";
            this.labelAdsCount.Size = new System.Drawing.Size(72, 62);
            this.labelAdsCount.TabIndex = 1;
            this.labelAdsCount.Text = "0";

            this.cardPublishing.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(245)))), ((int)(((byte)(224)))));
            this.cardPublishing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.cardPublishing.Controls.Add(this.labelPubTitle);
            this.cardPublishing.Controls.Add(this.labelPubCount);
            this.cardPublishing.Location = new System.Drawing.Point(705, 50);
            this.cardPublishing.Name = "cardPublishing";
            this.cardPublishing.Size = new System.Drawing.Size(240, 130);
            this.cardPublishing.TabIndex = 5;

            this.labelPubTitle.AutoSize = true;
            this.labelPubTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPubTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(146)))), ((int)(((byte)(91)))), ((int)(((byte)(20)))));
            this.labelPubTitle.Location = new System.Drawing.Point(20, 18);
            this.labelPubTitle.Name = "labelPubTitle";
            this.labelPubTitle.Size = new System.Drawing.Size(132, 32);
            this.labelPubTitle.TabIndex = 0;
            this.labelPubTitle.Text = "Publishing";

            this.labelPubCount.AutoSize = true;
            this.labelPubCount.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPubCount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(101)))), ((int)(((byte)(15)))));
            this.labelPubCount.Location = new System.Drawing.Point(18, 60);
            this.labelPubCount.Name = "labelPubCount";
            this.labelPubCount.Size = new System.Drawing.Size(72, 62);
            this.labelPubCount.TabIndex = 1;
            this.labelPubCount.Text = "0";

            this.buttonDocs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(123)))), ((int)(((byte)(255)))));
            this.buttonDocs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDocs.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonDocs.ForeColor = System.Drawing.Color.White;
            this.buttonDocs.Location = new System.Drawing.Point(55, 250);
            this.buttonDocs.Name = "buttonDocs";
            this.buttonDocs.Size = new System.Drawing.Size(230, 72);
            this.buttonDocs.TabIndex = 6;
            this.buttonDocs.Text = "Documents";
            this.buttonDocs.UseVisualStyleBackColor = false;
            this.buttonDocs.Click += new System.EventHandler(this.buttonDocs_Click);

            this.buttonAds.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(167)))), ((int)(((byte)(69)))));
            this.buttonAds.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAds.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonAds.ForeColor = System.Drawing.Color.White;
            this.buttonAds.Location = new System.Drawing.Point(385, 250);
            this.buttonAds.Name = "buttonAds";
            this.buttonAds.Size = new System.Drawing.Size(230, 72);
            this.buttonAds.TabIndex = 7;
            this.buttonAds.Text = "Advertisements";
            this.buttonAds.UseVisualStyleBackColor = false;
            this.buttonAds.Click += new System.EventHandler(this.buttonAds_Click);

            this.buttonPublish.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(153)))), ((int)(((byte)(0)))));
            this.buttonPublish.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonPublish.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPublish.ForeColor = System.Drawing.Color.White;
            this.buttonPublish.Location = new System.Drawing.Point(715, 250);
            this.buttonPublish.Name = "buttonPublish";
            this.buttonPublish.Size = new System.Drawing.Size(230, 72);
            this.buttonPublish.TabIndex = 8;
            this.buttonPublish.Text = "Publishing";
            this.buttonPublish.UseVisualStyleBackColor = false;
            this.buttonPublish.Click += new System.EventHandler(this.buttonPublish_Click);

            this.buttonUsers.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(108)))), ((int)(((byte)(117)))), ((int)(((byte)(125)))));
            this.buttonUsers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonUsers.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonUsers.ForeColor = System.Drawing.Color.White;
            this.buttonUsers.Location = new System.Drawing.Point(385, 340);
            this.buttonUsers.Name = "buttonUsers";
            this.buttonUsers.Size = new System.Drawing.Size(230, 72);
            this.buttonUsers.TabIndex = 9;
            this.buttonUsers.Text = "Users";
            this.buttonUsers.UseVisualStyleBackColor = false;
            this.buttonUsers.Click += new System.EventHandler(this.buttonUsers_Click);

            this.ClientSize = new System.Drawing.Size(1000, 560);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelContent);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "DashboardForm";
            this.Text = "Dashboard - Manipal DMS";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelContent.ResumeLayout(false);
            this.cardDocuments.ResumeLayout(false);
            this.cardDocuments.PerformLayout();
            this.cardAds.ResumeLayout(false);
            this.cardAds.PerformLayout();
            this.cardPublishing.ResumeLayout(false);
            this.cardPublishing.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}

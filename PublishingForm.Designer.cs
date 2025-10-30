namespace ManipalDMS
{
    partial class PublishingForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label labelCost;
        private System.Windows.Forms.Label labelRevenue;
        private System.Windows.Forms.TextBox textBoxTitle;
        private System.Windows.Forms.TextBox textBoxCost;
        private System.Windows.Forms.TextBox textBoxRevenue;
        private System.Windows.Forms.Button buttonAdd;
        private System.Windows.Forms.DataGridView dataGridViewPub;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.labelTitle = new System.Windows.Forms.Label();
            this.labelCost = new System.Windows.Forms.Label();
            this.labelRevenue = new System.Windows.Forms.Label();
            this.textBoxTitle = new System.Windows.Forms.TextBox();
            this.textBoxCost = new System.Windows.Forms.TextBox();
            this.textBoxRevenue = new System.Windows.Forms.TextBox();
            this.buttonAdd = new System.Windows.Forms.Button();
            this.dataGridViewPub = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPub)).BeginInit();
            this.SuspendLayout();
            //
            // labelTitle
            //
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelTitle.ForeColor = System.Drawing.Color.Gray;
            this.labelTitle.Location = new System.Drawing.Point(60, 60);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(94, 25);
            this.labelTitle.Text = "Article Title:";
            //
            // textBoxTitle
            //
            this.textBoxTitle.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxTitle.Location = new System.Drawing.Point(200, 57);
            this.textBoxTitle.Size = new System.Drawing.Size(230, 31);
            //
            // labelCost
            //
            this.labelCost.AutoSize = true;
            this.labelCost.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelCost.ForeColor = System.Drawing.Color.Gray;
            this.labelCost.Location = new System.Drawing.Point(60, 110);
            this.labelCost.Name = "labelCost";
            this.labelCost.Size = new System.Drawing.Size(51, 25);
            this.labelCost.Text = "Cost:";
            //
            // textBoxCost
            //
            this.textBoxCost.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxCost.Location = new System.Drawing.Point(200, 107);
            this.textBoxCost.Size = new System.Drawing.Size(230, 31);
            //
            // labelRevenue
            //
            this.labelRevenue.AutoSize = true;
            this.labelRevenue.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.labelRevenue.ForeColor = System.Drawing.Color.Gray;
            this.labelRevenue.Location = new System.Drawing.Point(60, 160);
            this.labelRevenue.Name = "labelRevenue";
            this.labelRevenue.Size = new System.Drawing.Size(83, 25);
            this.labelRevenue.Text = "Revenue:";
            //
            // textBoxRevenue
            //
            this.textBoxRevenue.Font = new System.Drawing.Font("Segoe UI", 13F);
            this.textBoxRevenue.Location = new System.Drawing.Point(200, 157);
            this.textBoxRevenue.Size = new System.Drawing.Size(230, 31);
            //
            // buttonAdd
            //
            this.buttonAdd.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Bold);
            this.buttonAdd.BackColor = System.Drawing.Color.RoyalBlue;
            this.buttonAdd.ForeColor = System.Drawing.Color.White;
            this.buttonAdd.Location = new System.Drawing.Point(150, 210);
            this.buttonAdd.Size = new System.Drawing.Size(190, 40);
            this.buttonAdd.Text = "Add Publishing";
            this.buttonAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonAdd.Click += new System.EventHandler(this.buttonAdd_Click);
            //
            // dataGridViewPub
            //
            this.dataGridViewPub.Location = new System.Drawing.Point(470, 30);
            this.dataGridViewPub.Size = new System.Drawing.Size(350, 260);
            this.dataGridViewPub.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.dataGridViewPub.ReadOnly = true;
            this.dataGridViewPub.AllowUserToAddRows = false;
            this.dataGridViewPub.AllowUserToDeleteRows = false;
            this.dataGridViewPub.RowHeadersVisible = false;
            this.dataGridViewPub.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            //
            // PublishingForm
            //
            this.ClientSize = new System.Drawing.Size(850, 330);
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.labelTitle);
            this.Controls.Add(this.textBoxTitle);
            this.Controls.Add(this.labelCost);
            this.Controls.Add(this.textBoxCost);
            this.Controls.Add(this.labelRevenue);
            this.Controls.Add(this.textBoxRevenue);
            this.Controls.Add(this.buttonAdd);
            this.Controls.Add(this.dataGridViewPub);
            this.Text = "Manage Publishing";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPub)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}


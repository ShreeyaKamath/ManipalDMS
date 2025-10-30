using System;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            textBoxPassword.UseSystemPasswordChar = true;
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string user = textBoxUsername.Text;
            string pass = textBoxPassword.Text;
            string query = $"SELECT * FROM Users WHERE Username='{user}' AND Password='{pass}'";
            var dt = DatabaseHelper.ExecuteQuery(query);
            if (dt.Rows.Count == 1)
            {
                string role = dt.Rows[0]["Role"].ToString();
                DashboardForm dash = new DashboardForm(role);
                dash.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Invalid username or password.");
            }
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }
    }
}



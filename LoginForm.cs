using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            textBoxPassword.UseSystemPasswordChar = true;
            AcceptButton = buttonLogin;
            textBoxUsername.Focus();
        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            string username = textBoxUsername.Text.Trim();
            string password = textBoxPassword.Text;
            if (username.Length == 0 || password.Length == 0)
            {
                MessageBox.Show("Enter both your username and password.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            buttonLogin.Enabled = false;
            try
            {
                DataTable users = DatabaseHelper.ExecuteQuery(
                    "SELECT Role FROM Users WHERE Username = @Username AND Password = @Password",
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                    new SqlParameter("@Password", SqlDbType.NVarChar, 50) { Value = password });

                if (users.Rows.Count != 1)
                {
                    MessageBox.Show("The username or password is incorrect.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    textBoxPassword.Clear();
                    textBoxPassword.Focus();
                    return;
                }

                DashboardForm dashboard = new DashboardForm(username, users.Rows[0]["Role"].ToString());
                dashboard.FormClosed += (s, args) => Close();
                Hide();
                dashboard.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to sign in. Check your database connection and try again.\n\n" + ex.Message,
                    "Database connection", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && Visible) buttonLogin.Enabled = true;
            }
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }
    }
}

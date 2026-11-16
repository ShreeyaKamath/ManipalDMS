using System;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
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

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Enter both your username and password.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            buttonLogin.Enabled = false;
            try
            {
                DataTable users = DatabaseHelper.ExecuteQuery(
                    @"SELECT UserID, Role, PasswordHash, PasswordSalt, IsActive
                      FROM Users
                      WHERE Username = @Username",
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });

                if (users.Rows.Count != 1)
                {
                    ShowInvalidLogin();
                    return;
                }

                DataRow row = users.Rows[0];
                object isActiveValue = row["IsActive"];
                if (isActiveValue != DBNull.Value && !(bool)isActiveValue)
                {
                    ShowInvalidLogin();
                    return;
                }

                byte[] storedHash = row["PasswordHash"] as byte[];
                byte[] storedSalt = row["PasswordSalt"] as byte[];

                if (storedHash == null || storedSalt == null || storedHash.Length == 0 || storedSalt.Length == 0)
                {
                    ShowInvalidLogin();
                    return;
                }

                bool isValid = PasswordHashing.VerifyPassword(password, storedHash, storedSalt);
                if (!isValid)
                {
                    ShowInvalidLogin();
                    return;
                }

                string role = row["Role"] == DBNull.Value ? string.Empty : row["Role"].ToString();
                DashboardForm dashboard = new DashboardForm(username, role);
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

        private static void ShowInvalidLogin()
        {
            MessageBox.Show("The username or password is incorrect.", "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
        {
            textBoxPassword.UseSystemPasswordChar = !checkBoxShowPassword.Checked;
        }
    }
}

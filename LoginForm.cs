using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class LoginForm : Form
    {
        private const int MaxFailedAttempts = 5;
        private const int LockoutMinutes = 15;

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
                    @"SELECT UserID, Role, PasswordHash, PasswordSalt, IsActive,
                             FailedLoginAttempts, LockoutUntilUtc
                      FROM dbo.Users WHERE Username = @Username",
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });

                if (users.Rows.Count != 1)
                {
                    BurnUnknownUserPassword(password);
                    ShowInvalidLogin();
                    return;
                }

                DataRow row = users.Rows[0];
                DateTime? lockoutUntil = row["LockoutUntilUtc"] == DBNull.Value
                    ? (DateTime?)null : Convert.ToDateTime(row["LockoutUntilUtc"]);
                bool active = row["IsActive"] != DBNull.Value && Convert.ToBoolean(row["IsActive"]);
                if (!active || (lockoutUntil.HasValue && lockoutUntil.Value > DateTime.UtcNow))
                {
                    BurnUnknownUserPassword(password);
                    ShowInvalidLogin();
                    return;
                }

                byte[] hash = row["PasswordHash"] as byte[];
                byte[] salt = row["PasswordSalt"] as byte[];
                bool valid = PasswordHashing.VerifyPassword(password, hash, salt);
                if (!valid)
                {
                    RecordFailedLogin(username);
                    ShowInvalidLogin();
                    return;
                }

                DatabaseHelper.ExecuteNonQuery(
                    "UPDATE dbo.Users SET FailedLoginAttempts = 0, LockoutUntilUtc = NULL, UpdatedUtc = SYSUTCDATETIME() WHERE Username = @Username",
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });

                string role = Convert.ToString(row["Role"]);
                DashboardForm dashboard = new DashboardForm(username, role);
                dashboard.FormClosed += (s, args) => Close();
                Hide();
                dashboard.Show();
            }
            catch (Exception ex)
            {
                Trace.TraceError("Sign-in failed due to a system error: {0}", ex);
                MessageBox.Show("Unable to sign in. Contact your system administrator if the problem persists.",
                    "Sign in", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (!IsDisposed && Visible) buttonLogin.Enabled = true;
                textBoxPassword.Clear();
            }
        }

        private static void RecordFailedLogin(string username)
        {
            const string sql = @"UPDATE dbo.Users
                SET FailedLoginAttempts = CASE
                        WHEN LockoutUntilUtc IS NOT NULL AND LockoutUntilUtc <= SYSUTCDATETIME() THEN 1
                        ELSE FailedLoginAttempts + 1 END,
                    LockoutUntilUtc = CASE
                        WHEN (CASE WHEN LockoutUntilUtc IS NOT NULL AND LockoutUntilUtc <= SYSUTCDATETIME()
                                   THEN 1 ELSE FailedLoginAttempts + 1 END) >= @MaxAttempts
                        THEN DATEADD(MINUTE, @LockoutMinutes, SYSUTCDATETIME())
                        WHEN LockoutUntilUtc IS NOT NULL AND LockoutUntilUtc <= SYSUTCDATETIME() THEN NULL
                        ELSE LockoutUntilUtc END,
                    UpdatedUtc = SYSUTCDATETIME()
                WHERE Username = @Username";
            DatabaseHelper.ExecuteNonQuery(sql,
                new SqlParameter("@MaxAttempts", SqlDbType.Int) { Value = MaxFailedAttempts },
                new SqlParameter("@LockoutMinutes", SqlDbType.Int) { Value = LockoutMinutes },
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username });
        }

        private static void BurnUnknownUserPassword(string password)
        {
            byte[] salt = new byte[PasswordHashing.SaltSize];
            byte[] hash = new byte[PasswordHashing.HashSize];
            PasswordHashing.VerifyPassword(password, hash, salt);
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

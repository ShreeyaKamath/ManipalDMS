using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    public partial class UserManagementForm : Form
    {
        private readonly string currentUser;
        private readonly string currentRole;
        private readonly DataGridView userGrid;
        private readonly ComboBox roleComboBox;
        private readonly TextBox newUsernameBox;
        private readonly TextBox newPasswordBox;
        private readonly TextBox confirmPasswordBox;
        private readonly ComboBox newRoleComboBox;
        private readonly Button saveRoleButton;
        private readonly Button toggleActiveButton;
        private readonly Button createUserButton;

        public UserManagementForm(string username, string role)
        {
            RequireUserAdmin(role);
            currentUser = username;
            currentRole = role;
            Text = "User Management";
            Width = 900;
            Height = 680;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label title = new Label { Text = "User Administration", AutoSize = true, Font = new Font("Segoe UI", 16F, FontStyle.Bold), Location = new Point(20, 15) };
            userGrid = new DataGridView
            {
                Location = new Point(20, 60), Width = 840, Height = 260, ReadOnly = true,
                AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect, MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            userGrid.SelectionChanged += UserGrid_SelectionChanged;

            Label existingLabel = new Label { Text = "Selected account role:", AutoSize = true, Location = new Point(20, 340) };
            roleComboBox = CreateRoleCombo(new Point(165, 335));
            saveRoleButton = new Button { Text = "Save Role", Location = new Point(360, 332), Size = new Size(110, 34) };
            saveRoleButton.Click += SaveRoleButton_Click;
            toggleActiveButton = new Button { Text = "Toggle Active", Location = new Point(485, 332), Size = new Size(125, 34) };
            toggleActiveButton.Click += ToggleActiveButton_Click;
            Button refreshButton = new Button { Text = "Refresh", Location = new Point(625, 332), Size = new Size(100, 34) };
            refreshButton.Click += (sender, args) => LoadUsers();

            Label createTitle = new Label { Text = "Create account", AutoSize = true, Font = new Font("Segoe UI", 12F, FontStyle.Bold), Location = new Point(20, 395) };
            Label usernameLabel = new Label { Text = "Username", AutoSize = true, Location = new Point(20, 440) };
            newUsernameBox = new TextBox { Location = new Point(120, 435), Width = 210, MaxLength = 50 };
            Label passwordLabel = new Label { Text = "Password", AutoSize = true, Location = new Point(350, 440) };
            newPasswordBox = new TextBox { Location = new Point(435, 435), Width = 190, UseSystemPasswordChar = true, MaxLength = 256 };
            Label confirmLabel = new Label { Text = "Confirm", AutoSize = true, Location = new Point(645, 440) };
            confirmPasswordBox = new TextBox { Location = new Point(710, 435), Width = 150, UseSystemPasswordChar = true, MaxLength = 256 };
            Label newRoleLabel = new Label { Text = "Role", AutoSize = true, Location = new Point(20, 490) };
            newRoleComboBox = CreateRoleCombo(new Point(120, 485));
            createUserButton = new Button { Text = "Create User", Location = new Point(330, 480), Size = new Size(130, 36) };
            createUserButton.Click += CreateUserButton_Click;
            Label passwordHelp = new Label { Text = "Use at least 12 characters. Passwords are hashed before they are stored.", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(20, 545) };

            Controls.AddRange(new Control[] { title, userGrid, existingLabel, roleComboBox, saveRoleButton, toggleActiveButton, refreshButton,
                createTitle, usernameLabel, newUsernameBox, passwordLabel, newPasswordBox, confirmLabel, confirmPasswordBox,
                newRoleLabel, newRoleComboBox, createUserButton, passwordHelp });
            LoadUsers();
        }

        private static ComboBox CreateRoleCombo(Point location)
        {
            ComboBox combo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 180, Location = location };
            combo.Items.AddRange(new object[] { "Admin", "Editor", "Accountant" });
            return combo;
        }

        private void RequireUserAdmin(string role)
        {
            if (!SecurityPolicy.CanManageUsers(role))
                throw new UnauthorizedAccessException("User administration permission is required.");
        }

        private void LoadUsers()
        {
            RequireUserAdmin(currentRole);
            try
            {
                userGrid.DataSource = DatabaseHelper.ExecuteQuery(
                    "SELECT UserID, Username, Role, IsActive, CreatedUtc FROM dbo.Users ORDER BY Username");
                if (userGrid.Rows.Count > 0)
                {
                    userGrid.Rows[0].Selected = true;
                    ApplySelectedUser();
                }
                else
                {
                    roleComboBox.SelectedIndex = -1;
                    saveRoleButton.Enabled = toggleActiveButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("User list query failed: {0}", ex);
                MessageBox.Show("Unable to load users. Check the database connection.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserGrid_SelectionChanged(object sender, EventArgs e) { ApplySelectedUser(); }

        private void ApplySelectedUser()
        {
            if (userGrid.SelectedRows.Count == 0 && userGrid.CurrentRow != null) userGrid.CurrentRow.Selected = true;
            if (userGrid.SelectedRows.Count == 0)
            {
                roleComboBox.SelectedIndex = -1;
                saveRoleButton.Enabled = toggleActiveButton.Enabled = false;
                return;
            }
            roleComboBox.SelectedItem = Convert.ToString(userGrid.SelectedRows[0].Cells["Role"].Value ?? string.Empty);
            saveRoleButton.Enabled = toggleActiveButton.Enabled = true;
        }

        private void CreateUserButton_Click(object sender, EventArgs e)
        {
            RequireUserAdmin(currentRole);
            string username = newUsernameBox.Text.Trim();
            string password = newPasswordBox.Text;
            if (username.Length == 0 || username.Length > 50 || password.Length < 12 || newRoleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Enter a username, choose a role, and use a password of at least 12 characters.", "Create user", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (!string.Equals(password, confirmPasswordBox.Text, StringComparison.Ordinal))
            {
                MessageBox.Show("The password and confirmation do not match.", "Create user", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            byte[] hash;
            byte[] salt;
            PasswordHashing.HashPassword(password, out hash, out salt);
            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    "EXEC dbo.CreateApplicationUser @Username, @PasswordHash, @PasswordSalt, @Role",
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                    new SqlParameter("@PasswordHash", SqlDbType.VarBinary, PasswordHashing.HashSize) { Value = hash },
                    new SqlParameter("@PasswordSalt", SqlDbType.VarBinary, PasswordHashing.SaltSize) { Value = salt },
                    new SqlParameter("@Role", SqlDbType.NVarChar, 30) { Value = newRoleComboBox.SelectedItem.ToString() });
                DatabaseHelper.LogActivity(currentUser, "UserCreated", "Users", "Username: " + username + "; Role: " + newRoleComboBox.SelectedItem);
                newUsernameBox.Clear(); newPasswordBox.Clear(); confirmPasswordBox.Clear(); newRoleComboBox.SelectedIndex = -1;
                LoadUsers();
                MessageBox.Show("User account created.", "Create user", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("User creation failed: {0}", ex);
                MessageBox.Show("Could not create the account. Confirm the username is unique and the provisioning migration has been applied.", "Create user", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SaveRoleButton_Click(object sender, EventArgs e)
        {
            RequireUserAdmin(currentRole);
            if (userGrid.SelectedRows.Count == 0 || roleComboBox.SelectedItem == null) return;
            int userId = Convert.ToInt32(userGrid.SelectedRows[0].Cells["UserID"].Value);
            string username = Convert.ToString(userGrid.SelectedRows[0].Cells["Username"].Value ?? string.Empty);
            string newRole = roleComboBox.SelectedItem.ToString();
            if (string.Equals(username, currentUser, StringComparison.OrdinalIgnoreCase) && !string.Equals(newRole, currentRole, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You cannot change your own role during this session.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DatabaseHelper.ExecuteNonQuery("UPDATE dbo.Users SET Role = @Role, UpdatedUtc = SYSUTCDATETIME() WHERE UserID = @UserID",
                    new SqlParameter("@Role", SqlDbType.NVarChar, 30) { Value = newRole },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });
                DatabaseHelper.LogActivity(currentUser, "RoleUpdated", "Users", "Username: " + username + "; Role: " + newRole);
                LoadUsers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Role update failed: {0}", ex);
                MessageBox.Show("Failed to update role.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleActiveButton_Click(object sender, EventArgs e)
        {
            RequireUserAdmin(currentRole);
            if (userGrid.SelectedRows.Count == 0) return;
            int userId = Convert.ToInt32(userGrid.SelectedRows[0].Cells["UserID"].Value);
            string username = Convert.ToString(userGrid.SelectedRows[0].Cells["Username"].Value ?? string.Empty);
            bool active = Convert.ToBoolean(userGrid.SelectedRows[0].Cells["IsActive"].Value);
            if (string.Equals(username, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You cannot deactivate your own account while signed in.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                DatabaseHelper.ExecuteNonQuery("UPDATE dbo.Users SET IsActive = @Active, UpdatedUtc = SYSUTCDATETIME() WHERE UserID = @UserID",
                    new SqlParameter("@Active", SqlDbType.Bit) { Value = !active },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });
                DatabaseHelper.LogActivity(currentUser, "UserStatusChanged", "Users", "Username: " + username + "; IsActive: " + (!active));
                LoadUsers();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("User status update failed: {0}", ex);
                MessageBox.Show("Failed to update user status.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

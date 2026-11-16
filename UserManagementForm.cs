using System;
using System.ComponentModel;
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
        private readonly Button saveButton;
        private readonly Button toggleActiveButton;
        private readonly Button refreshButton;

        public UserManagementForm(string username, string role)
        {
            if (!SecurityPolicy.CanManageUsers(role))
                throw new UnauthorizedAccessException("Role is not authorized to manage users.");

            currentUser = username;
            currentRole = role;
            Text = "User Management";
            Width = 820;
            Height = 540;
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;

            Label title = new Label
            {
                Text = "User Role Administration",
                AutoSize = true,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                Location = new Point(20, 18)
            };

            userGrid = new DataGridView
            {
                Location = new Point(20, 70),
                Width = 760,
                Height = 280,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
            };
            userGrid.SelectionChanged += UserGrid_SelectionChanged;

            Label roleLabel = new Label
            {
                Text = "Role:",
                AutoSize = true,
                Location = new Point(20, 380)
            };

            roleComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Width = 180,
                Location = new Point(80, 375)
            };
            roleComboBox.Items.Add("Admin");
            roleComboBox.Items.Add("Editor");
            roleComboBox.Items.Add("Accountant");

            saveButton = new Button
            {
                Text = "Save Role",
                Width = 120,
                Height = 36,
                Location = new Point(285, 370)
            };
            saveButton.Click += SaveButton_Click;

            toggleActiveButton = new Button
            {
                Text = "Toggle Active",
                Width = 120,
                Height = 36,
                Location = new Point(425, 370)
            };
            toggleActiveButton.Click += ToggleActiveButton_Click;

            refreshButton = new Button
            {
                Text = "Refresh",
                Width = 100,
                Height = 36,
                Location = new Point(565, 370)
            };
            refreshButton.Click += RefreshButton_Click;

            Controls.Add(title);
            Controls.Add(userGrid);
            Controls.Add(roleLabel);
            Controls.Add(roleComboBox);
            Controls.Add(saveButton);
            Controls.Add(toggleActiveButton);
            Controls.Add(refreshButton);

            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                DataTable users = DatabaseHelper.ExecuteQuery(
                    @"SELECT UserID, Username, Role, IsActive, CreatedUtc
                      FROM dbo.Users
                      ORDER BY Username");

                userGrid.DataSource = users;
                if (userGrid.Rows.Count > 0)
                {
                    userGrid.Rows[0].Selected = true;
                    ApplySelectedUser();
                }
                else
                {
                    roleComboBox.SelectedIndex = -1;
                    saveButton.Enabled = false;
                    toggleActiveButton.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to load user list: " + ex.Message, "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UserGrid_SelectionChanged(object sender, EventArgs e)
        {
            ApplySelectedUser();
        }

        private void ApplySelectedUser()
        {
            if (userGrid.SelectedRows.Count == 0 && userGrid.CurrentRow != null)
            {
                userGrid.CurrentRow.Selected = true;
            }

            if (userGrid.SelectedRows.Count == 0)
            {
                roleComboBox.SelectedIndex = -1;
                saveButton.Enabled = false;
                toggleActiveButton.Enabled = false;
                return;
            }

            DataGridViewRow row = userGrid.SelectedRows[0];
            string role = Convert.ToString(row.Cells["Role"].Value ?? string.Empty);
            roleComboBox.SelectedItem = role;
            saveButton.Enabled = true;
            toggleActiveButton.Enabled = true;
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user first.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (roleComboBox.SelectedItem == null)
            {
                MessageBox.Show("Select a valid role before saving.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(userGrid.SelectedRows[0].Cells["UserID"].Value);
            string selectedRole = roleComboBox.SelectedItem.ToString();

            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    @"UPDATE dbo.Users
                      SET Role = @Role,
                          UpdatedUtc = SYSUTCDATETIME()
                      WHERE UserID = @UserID",
                    new SqlParameter("@Role", SqlDbType.NVarChar, 30) { Value = selectedRole },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });

                DatabaseHelper.LogActivity(currentUser, "RoleUpdated", "Users", "UserID: " + userId + "; Role: " + selectedRole);
                MessageBox.Show("User role updated successfully.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update role: " + ex.Message, "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToggleActiveButton_Click(object sender, EventArgs e)
        {
            if (userGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a user first.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int userId = Convert.ToInt32(userGrid.SelectedRows[0].Cells["UserID"].Value);
            string username = Convert.ToString(userGrid.SelectedRows[0].Cells["Username"].Value ?? string.Empty);
            bool isActive = Convert.ToBoolean(userGrid.SelectedRows[0].Cells["IsActive"].Value);

            if (string.Equals(username, currentUser, StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("You cannot deactivate your own account while signed in.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                DatabaseHelper.ExecuteNonQuery(
                    @"UPDATE dbo.Users
                      SET IsActive = @IsActive,
                          UpdatedUtc = SYSUTCDATETIME()
                      WHERE UserID = @UserID",
                    new SqlParameter("@IsActive", SqlDbType.Bit) { Value = !isActive },
                    new SqlParameter("@UserID", SqlDbType.Int) { Value = userId });

                DatabaseHelper.LogActivity(currentUser, "UserStatusChanged", "Users", "UserID: " + userId + "; IsActive: " + (!isActive));
                MessageBox.Show("User status updated.", "User management", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update user status: " + ex.Message, "User management", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            LoadUsers();
        }
    }
}

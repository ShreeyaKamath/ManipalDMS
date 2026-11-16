using System;
using System.Data;
using System.Windows.Forms;

namespace ManipalDMS
{
    public sealed class AuditLogForm : Form
    {
        private readonly string currentRole;
        private readonly DataGridView grid;

        public AuditLogForm(string role)
        {
            if (!SecurityPolicy.CanViewAuditLog(role))
                throw new UnauthorizedAccessException("Role is not authorized to view the audit log.");

            currentRole = role;
            Text = "Audit Log";
            Width = 1000;
            Height = 600;
            StartPosition = FormStartPosition.CenterParent;
            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Button refresh = new Button { Text = "Refresh", Dock = DockStyle.Top, Height = 38 };
            refresh.Click += (sender, args) => LoadEntries();
            Controls.Add(grid);
            Controls.Add(refresh);
            LoadEntries();
        }

        private void LoadEntries()
        {
            if (!SecurityPolicy.CanViewAuditLog(currentRole))
                throw new UnauthorizedAccessException("Audit log permission is required.");

            try
            {
                grid.DataSource = DatabaseHelper.ExecuteQuery(
                    @"SELECT TOP (500) LogID, Username, Action, TableAffected, [Timestamp], Details
                      FROM dbo.AuditLog
                      ORDER BY [Timestamp] DESC, LogID DESC");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Audit log query failed: {0}", ex);
                MessageBox.Show("Unable to load audit records. Check the database connection and permissions.", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

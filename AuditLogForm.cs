using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    public sealed class AuditLogForm : Form
    {
        private readonly string currentRole;
        private readonly DataGridView grid;
        private readonly TextBox usernameFilter;
        private readonly TextBox actionFilter;
        private readonly DateTimePicker fromDate;
        private readonly DateTimePicker toDate;

        public AuditLogForm(string role)
        {
            if (!SecurityPolicy.CanViewAuditLog(role)) throw new UnauthorizedAccessException("Role is not authorized to view the audit log.");
            currentRole = role;
            Text = "Audit Log";
            Width = 1100; Height = 650; StartPosition = FormStartPosition.CenterParent;

            Panel filters = new Panel { Dock = DockStyle.Top, Height = 82 };
            Label usernameLabel = new Label { Text = "Username", AutoSize = true, Location = new Point(12, 12) };
            usernameFilter = new TextBox { Location = new Point(80, 8), Width = 150, MaxLength = 50 };
            Label actionLabel = new Label { Text = "Action", AutoSize = true, Location = new Point(245, 12) };
            actionFilter = new TextBox { Location = new Point(295, 8), Width = 130, MaxLength = 50 };
            Label fromLabel = new Label { Text = "From", AutoSize = true, Location = new Point(440, 12) };
            fromDate = new DateTimePicker { Location = new Point(485, 8), Width = 145, Format = DateTimePickerFormat.Short };
            fromDate.Value = DateTime.Today.AddDays(-30);
            Label toLabel = new Label { Text = "To", AutoSize = true, Location = new Point(645, 12) };
            toDate = new DateTimePicker { Location = new Point(670, 8), Width = 145, Format = DateTimePickerFormat.Short };
            Button search = new Button { Text = "Search", Location = new Point(830, 7), Size = new Size(95, 30) };
            search.Click += (sender, args) => LoadEntries();
            Button refresh = new Button { Text = "Clear / Refresh", Location = new Point(935, 7), Size = new Size(130, 30) };
            refresh.Click += (sender, args) => { usernameFilter.Clear(); actionFilter.Clear(); fromDate.Value = DateTime.Today.AddDays(-30); toDate.Value = DateTime.Today; LoadEntries(); };
            Label hint = new Label { Text = "Search returns at most 500 matching entries.", AutoSize = true, ForeColor = Color.DimGray, Location = new Point(12, 48) };
            filters.Controls.AddRange(new Control[] { usernameLabel, usernameFilter, actionLabel, actionFilter, fromLabel, fromDate, toLabel, toDate, search, refresh, hint });

            grid = new DataGridView
            {
                Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                MultiSelect = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            Controls.Add(grid);
            Controls.Add(filters);
            LoadEntries();
        }

        private void LoadEntries()
        {
            if (!SecurityPolicy.CanViewAuditLog(currentRole)) throw new UnauthorizedAccessException("Audit log permission is required.");
            DateTime from = fromDate.Value.Date;
            DateTime to = toDate.Value.Date;
            if (from > to)
            {
                MessageBox.Show("The start date must be on or before the end date.", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                grid.DataSource = DatabaseHelper.ExecuteQuery(
                    @"SELECT TOP (500) LogID, Username, Action, TableAffected, [Timestamp], Details
                      FROM dbo.AuditLog
                      WHERE [Timestamp] >= @From AND [Timestamp] < @To
                        AND (@Username = N'' OR Username LIKE @UsernamePattern)
                        AND (@Action = N'' OR Action LIKE @ActionPattern)
                      ORDER BY [Timestamp] DESC, LogID DESC",
                    new SqlParameter("@From", SqlDbType.DateTime2) { Value = from },
                    new SqlParameter("@To", SqlDbType.DateTime2) { Value = to.AddDays(1) },
                    new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = usernameFilter.Text.Trim() },
                    new SqlParameter("@UsernamePattern", SqlDbType.NVarChar, 102) { Value = "%" + usernameFilter.Text.Trim() + "%" },
                    new SqlParameter("@Action", SqlDbType.NVarChar, 50) { Value = actionFilter.Text.Trim() },
                    new SqlParameter("@ActionPattern", SqlDbType.NVarChar, 102) { Value = "%" + actionFilter.Text.Trim() + "%" });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError("Audit log query failed: {0}", ex);
                MessageBox.Show("Unable to load audit records. Check the database connection and permissions.", "Audit Log", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

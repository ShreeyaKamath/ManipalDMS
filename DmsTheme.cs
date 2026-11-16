using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ManipalDMS
{
    /// <summary>Applies a consistent visual theme to every WinForms screen as it is opened.</summary>
    internal static class DmsTheme
    {
        private static readonly Color Canvas = Color.FromArgb(241, 245, 249);
        private static readonly Color Ink = Color.FromArgb(31, 41, 55);
        private static readonly Color Muted = Color.FromArgb(100, 116, 139);
        private static readonly Color Blue = Color.FromArgb(29, 100, 177);
        private static readonly Color BlueHover = Color.FromArgb(21, 78, 145);
        private static readonly Color Header = Color.FromArgb(19, 69, 120);
        private static readonly HashSet<Control> StyledControls = new HashSet<Control>();

        public static void Initialize()
        {
            Application.Idle += StyleOpenForms;
        }

        private static void StyleOpenForms(object sender, EventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                form.BackColor = Canvas;
                form.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                StyleChildren(form);
            }
        }

        private static void StyleChildren(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (StyledControls.Add(control))
                    StyleControl(control);
                if (control.HasChildren)
                    StyleChildren(control);
            }
        }

        private static void StyleControl(Control control)
        {
            Panel panel = control as Panel;
            if (panel != null)
            {
                if (panel.Name == "panelHeader" || panel.Name == "panelBlue")
                {
                    panel.BackColor = Header;
                    foreach (Control child in panel.Controls)
                        child.ForeColor = Color.White;
                }
                else if (panel.Name == "panelContent")
                    panel.BackColor = Canvas;
                return;
            }

            Label label = control as Label;
            if (label != null)
            {
                label.ForeColor = label.Parent != null &&
                    (label.Parent.Name == "panelHeader" || label.Parent.Name == "panelBlue") ? Color.White : Ink;
                if (label.Name == "labelTitle" || label.Name == "labelWelcome")
                {
                    label.Font = new Font("Segoe UI", label.Name == "labelTitle" ? 20F : 12F, FontStyle.Bold);
                    if (label.Name == "labelTitle" && label.Parent != null && label.Parent.Name != "panelHeader" && label.Parent.Name != "panelBlue")
                        label.ForeColor = Header;
                }
                else if (label.Name == "labelRole")
                    label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                else
                    label.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                return;
            }

            Button button = control as Button;
            if (button != null)
            {
                button.FlatStyle = FlatStyle.Flat;
                button.FlatAppearance.BorderSize = 0;
                button.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                button.Cursor = Cursors.Hand;
                if (button.Enabled)
                {
                    button.BackColor = button.Name == "buttonRefresh" ? Color.FromArgb(100, 116, 139) : Blue;
                    button.ForeColor = Color.White;
                    button.FlatAppearance.MouseOverBackColor = button.Name == "buttonRefresh" ?
                        Color.FromArgb(71, 85, 105) : BlueHover;
                }
                else
                {
                    button.BackColor = Color.FromArgb(203, 213, 225);
                    button.ForeColor = Color.FromArgb(100, 116, 139);
                }
                return;
            }

            TextBox textBox = control as TextBox;
            if (textBox != null)
            {
                textBox.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                textBox.BackColor = Color.White;
                textBox.ForeColor = Ink;
                textBox.BorderStyle = BorderStyle.FixedSingle;
                return;
            }

            DateTimePicker datePicker = control as DateTimePicker;
            if (datePicker != null)
            {
                datePicker.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                return;
            }

            CheckBox checkBox = control as CheckBox;
            if (checkBox != null)
            {
                checkBox.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                checkBox.ForeColor = Ink;
                return;
            }

            DataGridView grid = control as DataGridView;
            if (grid != null)
            {
                grid.BackgroundColor = Color.White;
                grid.BorderStyle = BorderStyle.None;
                grid.GridColor = Color.FromArgb(226, 232, 240);
                grid.EnableHeadersVisualStyles = false;
                grid.ColumnHeadersDefaultCellStyle.BackColor = Header;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
                grid.DefaultCellStyle.ForeColor = Ink;
                grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
                grid.DefaultCellStyle.SelectionForeColor = Ink;
                grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 250, 252);
                grid.RowTemplate.Height = 30;
                grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                grid.MultiSelect = false;
            }
        }
    }
}

using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryDesk
{
    internal static class Theme
    {
        public static readonly Color Background = Color.FromArgb(244, 247, 252);
        public static readonly Color Ink = Color.FromArgb(21, 35, 59);
        public static readonly Color Muted = Color.FromArgb(106, 122, 145);
        public static readonly Color Primary = Color.FromArgb(40, 91, 226);
        public static readonly Color LightBlue = Color.FromArgb(230, 238, 255);
        public static readonly Color Border = Color.FromArgb(224, 231, 241);
        public static readonly Color Success = Color.FromArgb(19, 128, 95);
        public static readonly Color Danger = Color.FromArgb(186, 57, 63);

        public static void Setup(Form form, string title, int width, int height)
        {
            form.Text = title + "  |  LibraryDesk";
            form.StartPosition = FormStartPosition.CenterScreen;
            form.ClientSize = new Size(width, height);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.BackColor = Background;
            form.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            form.AutoScaleMode = AutoScaleMode.Font;
        }

        public static Label Label(string text, int x, int y, int width, int height,
                                  float size = 10F, bool bold = false, Color? color = null)
        {
            return new Label {
                Text = text, Location = new Point(x, y), Size = new Size(width, height),
                ForeColor = color ?? Ink,
                Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft
            };
        }

        public static Panel Card(int x, int y, int width, int height)
        {
            return new Panel { Location = new Point(x, y), Size = new Size(width, height),
                BackColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
        }

        public static Button Button(string text, int x, int y, int width, int height,
                                    bool primary = true)
        {
            var button = new Button {
                Text = text, Location = new Point(x, y), Size = new Size(width, height),
                FlatStyle = FlatStyle.Flat, BackColor = primary ? Primary : LightBlue,
                ForeColor = primary ? Color.White : Primary, Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold), UseVisualStyleBackColor = false
            };
            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseOverBackColor = primary
                ? Color.FromArgb(28, 72, 188) : Color.FromArgb(215, 228, 253);
            return button;
        }

        public static TextBox Input(int x, int y, int width)
        {
            return new TextBox {
                Location = new Point(x, y), Size = new Size(width, 32),
                BorderStyle = BorderStyle.FixedSingle, Font = new Font("Segoe UI", 11F)
            };
        }

        public static DataGridView Grid(int x, int y, int width, int height)
        {
            var grid = new DataGridView {
                Location = new Point(x, y), Size = new Size(width, height),
                AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AllowUserToResizeRows = false, ReadOnly = true, MultiSelect = false,
                RowHeadersVisible = false, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                BorderStyle = BorderStyle.None, BackgroundColor = Color.White,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                ColumnHeadersHeight = 40, RowTemplate = { Height = 39 },
                EnableHeadersVisualStyles = false,
                GridColor = Border, CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal
            };
            grid.ColumnHeadersDefaultCellStyle.BackColor = LightBlue;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Ink;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            grid.DefaultCellStyle.ForeColor = Ink;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(220, 233, 255);
            grid.DefaultCellStyle.SelectionForeColor = Ink;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(249, 251, 255);
            grid.Columns.Add("Id", "ID");
            grid.Columns.Add("Title", "BOOK TITLE");
            grid.Columns.Add("Author", "AUTHOR");
            grid.Columns.Add("Status", "STATUS");
            grid.Columns.Add("Borrower", "BORROWER");
            grid.Columns.Add("DueDate", "DUE DATE");
            grid.Columns["Id"].FillWeight = 10;
            grid.Columns["Title"].FillWeight = 33;
            grid.Columns["Author"].FillWeight = 29;
            grid.Columns["Status"].FillWeight = 17;
            grid.Columns["Borrower"].FillWeight = 23;
            grid.Columns["DueDate"].FillWeight = 18;
            grid.Columns["Id"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            return grid;
        }

        public static void FillGrid(DataGridView grid, System.Collections.Generic.IEnumerable<Book> books)
        {
            grid.Rows.Clear();
            foreach (var book in books)
            {
                string status = !book.Issued ? "Available" :
                    book.DueDate.HasValue && book.DueDate.Value.Date < DateTime.Today ? "Overdue" : "Issued";
                int index = grid.Rows.Add(book.Id, book.Title, book.Author, status,
                    book.Borrower ?? "—", book.DueDate.HasValue ? book.DueDate.Value.ToString("dd MMM yyyy") : "—");
                if (status == "Overdue") grid.Rows[index].Cells["Status"].Style.ForeColor = Danger;
                else if (status == "Available") grid.Rows[index].Cells["Status"].Style.ForeColor = Success;
                else grid.Rows[index].Cells["Status"].Style.ForeColor = Primary;
            }
            grid.ClearSelection();
        }

        public static int SelectedBookId(DataGridView grid)
        {
            if (grid.SelectedRows.Count == 0)
                throw new InvalidOperationException("Select a book in the table first.");
            return Convert.ToInt32(grid.SelectedRows[0].Cells["Id"].Value);
        }

        public static void Error(Exception ex)
        {
            MessageBox.Show(ex.Message, "LibraryDesk", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

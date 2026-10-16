using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryDesk
{
    public sealed class UserPanelForm : Form
    {
        private readonly Library library;
        private readonly TextBox searchInput;
        private readonly TextBox borrowerInput;
        private readonly DataGridView grid;
        private readonly Label selectedLabel;
        private readonly Label metrics;

        public UserPanelForm(Library lib)
        {
            library = lib;
            Theme.Setup(this, "Library desk", 1120, 695);
            Controls.Add(Theme.Label("Browse & borrow", 35, 24, 780, 43, 23F, true));
            Controls.Add(Theme.Label("Choose a title to issue or return. Loan period: 14 days.", 37, 69,
                820, 27, 10F, false, Theme.Muted));
            var back = Theme.Button("Close", 980, 31, 102, 35, false);
            back.Click += (s, e) => Close();
            Controls.Add(back);

            var catalog = Theme.Card(35, 117, 738, 488);
            catalog.Controls.Add(Theme.Label("Library collection", 20, 18, 380, 34, 17F, true));
            metrics = Theme.Label("", 21, 54, 680, 28, 10F, false, Theme.Muted);
            catalog.Controls.Add(metrics);
            searchInput = Theme.Input(20, 95, 420);
            searchInput.TextChanged += (s, e) => RefreshBooks();
            catalog.Controls.Add(searchInput);
            catalog.Controls.Add(Theme.Label("Search title or author", 448, 96, 250, 38,
                9F, false, Theme.Muted));
            grid = Theme.Grid(20, 150, 697, 306);
            grid.SelectionChanged += (s, e) => UpdateSelection();
            catalog.Controls.Add(grid);
            Controls.Add(catalog);

            var actions = Theme.Card(793, 117, 292, 488);
            actions.Controls.Add(Theme.Label("Borrowing", 20, 19, 254, 36, 17F, true));
            actions.Controls.Add(Theme.Label("SELECTED BOOK", 20, 75, 250, 25, 9F, true, Theme.Muted));
            selectedLabel = Theme.Label("None selected", 20, 102, 249, 63, 10F, false);
            actions.Controls.Add(selectedLabel);
            actions.Controls.Add(Theme.Label("BORROWER NAME", 20, 187, 247, 25, 9F, true, Theme.Muted));
            borrowerInput = Theme.Input(20, 218, 249);
            actions.Controls.Add(borrowerInput);
            var issue = Theme.Button("Issue book", 20, 277, 249, 44);
            issue.Click += (s, e) => Issue();
            actions.Controls.Add(issue);
            var bringBack = Theme.Button("Return selected book", 20, 332, 249, 43, false);
            bringBack.Click += (s, e) => Return();
            actions.Controls.Add(bringBack);
            actions.Controls.Add(Theme.Label("Overdue titles are flagged in the list.", 20, 405,
                245, 50, 9F, false, Theme.Muted));
            Controls.Add(actions);

            Controls.Add(Theme.Label("Select a row, then use the borrowing panel on the right.", 37,
                621, 920, 32, 10F, false, Theme.Muted));
            RefreshBooks();
        }

        private void UpdateSelection()
        {
            if (grid.SelectedRows.Count == 0)
            {
                selectedLabel.Text = "None selected";
                return;
            }
            selectedLabel.Text = Convert.ToString(grid.SelectedRows[0].Cells["Title"].Value);
        }

        private void Issue()
        {
            try
            {
                library.IssueBook(Theme.SelectedBookId(grid), borrowerInput.Text);
                borrowerInput.Clear();
                RefreshBooks();
                MessageBox.Show("Book issued. Please return it within 14 days.", "LibraryDesk",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { Theme.Error(ex); }
        }

        private void Return()
        {
            try
            {
                library.ReturnBook(Theme.SelectedBookId(grid));
                borrowerInput.Clear();
                RefreshBooks();
                MessageBox.Show("Book returned successfully.", "LibraryDesk",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex) { Theme.Error(ex); }
        }

        private void RefreshBooks()
        {
            var books = library.GetBooks();
            string search = searchInput.Text.Trim();
            Theme.FillGrid(grid, books.Where(b => search.Length == 0 ||
                b.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.Author.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0));
            metrics.Text = string.Format("{0} books in the collection  ·  {1} available to borrow",
                books.Count, books.Count(b => !b.Issued));
            UpdateSelection();
        }
    }
}

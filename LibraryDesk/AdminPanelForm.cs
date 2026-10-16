using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryDesk
{
    public sealed class AdminPanelForm : Form
    {
        private readonly Library library;
        private readonly TextBox titleInput;
        private readonly TextBox authorInput;
        private readonly TextBox searchInput;
        private readonly DataGridView grid;
        private readonly Label metrics;

        public AdminPanelForm(Library lib)
        {
            library = lib;
            Theme.Setup(this, "Admin workspace", 1120, 695);
            Controls.Add(Theme.Label("Admin workspace", 35, 24, 720, 43, 23F, true));
            Controls.Add(Theme.Label("Keep your catalog clean, searchable and up to date.", 37, 69,
                800, 27, 10F, false, Theme.Muted));
            var back = Theme.Button("Close", 980, 31, 102, 35, false);
            back.Click += (s, e) => Close();
            Controls.Add(back);

            var add = Theme.Card(35, 117, 305, 488);
            add.Controls.Add(Theme.Label("Add a book", 22, 23, 250, 35, 17F, true));
            add.Controls.Add(Theme.Label("Create a new catalog entry.", 22, 60, 263, 25,
                10F, false, Theme.Muted));
            add.Controls.Add(Theme.Label("BOOK TITLE", 22, 113, 260, 25, 9F, true, Theme.Muted));
            titleInput = Theme.Input(22, 146, 259);
            add.Controls.Add(titleInput);
            add.Controls.Add(Theme.Label("AUTHOR", 22, 208, 260, 25, 9F, true, Theme.Muted));
            authorInput = Theme.Input(22, 241, 259);
            add.Controls.Add(authorInput);
            var addButton = Theme.Button("+  Add to library", 22, 305, 259, 43);
            addButton.Click += (s, e) => AddBook();
            add.Controls.Add(addButton);
            add.Controls.Add(Theme.Label("Changes are saved automatically on this PC.", 22, 378,
                265, 57, 9F, false, Theme.Muted));
            Controls.Add(add);

            var catalog = Theme.Card(359, 117, 726, 488);
            catalog.Controls.Add(Theme.Label("Book catalog", 20, 18, 380, 34, 17F, true));
            metrics = Theme.Label("", 21, 54, 650, 28, 10F, false, Theme.Muted);
            catalog.Controls.Add(metrics);
            searchInput = Theme.Input(20, 95, 429);
            searchInput.TextChanged += (s, e) => RefreshBooks();
            catalog.Controls.Add(searchInput);
            catalog.Controls.Add(Theme.Label("Search by title, author or borrower", 459, 96,
                243, 38, 9F, false, Theme.Muted));
            grid = Theme.Grid(20, 150, 684, 255);
            catalog.Controls.Add(grid);
            var delete = Theme.Button("Remove selected", 20, 427, 178, 37, false);
            delete.Click += (s, e) => DeleteBook();
            catalog.Controls.Add(delete);
            var refresh = Theme.Button("Refresh", 568, 427, 136, 37, false);
            refresh.Click += (s, e) => RefreshBooks();
            catalog.Controls.Add(refresh);
            Controls.Add(catalog);

            Controls.Add(Theme.Label("Tip: issued books must be returned before removal.", 37, 621,
                830, 32, 10F, false, Theme.Muted));
            RefreshBooks();
        }

        private void AddBook()
        {
            try
            {
                library.AddBook(titleInput.Text, authorInput.Text);
                titleInput.Clear();
                authorInput.Clear();
                searchInput.Clear();
                RefreshBooks();
                titleInput.Focus();
            }
            catch (Exception ex) { Theme.Error(ex); }
        }

        private void DeleteBook()
        {
            try
            {
                int id = Theme.SelectedBookId(grid);
                if (MessageBox.Show("Remove the selected book from your catalog?", "Confirm removal",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
                library.DeleteBook(id);
                RefreshBooks();
            }
            catch (Exception ex) { Theme.Error(ex); }
        }

        private void RefreshBooks()
        {
            var books = library.GetBooks();
            string search = searchInput.Text.Trim();
            var filtered = books.Where(b => search.Length == 0 ||
                b.Title.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                b.Author.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 ||
                (b.Borrower ?? "").IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0);
            Theme.FillGrid(grid, filtered);
            metrics.Text = string.Format("{0} titles  ·  {1} available  ·  {2} issued",
                books.Count, books.Count(b => !b.Issued), books.Count(b => b.Issued));
        }
    }
}

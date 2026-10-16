using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LibraryDesk
{
    public sealed class MainForm : Form
    {
        private readonly Library library;
        private readonly Label totalValue;
        private readonly Label availableValue;
        private readonly Label issuedValue;

        public MainForm(Library lib)
        {
            library = lib;
            Theme.Setup(this, "Welcome", 940, 580);

            var top = new Panel { Location = new Point(0, 0), Size = new Size(940, 104), BackColor = Theme.Ink };
            top.Controls.Add(Theme.Label("LIBRARYDESK", 54, 29, 420, 40, 21F, true, Color.White));
            top.Controls.Add(Theme.Label("Desktop library manager", 711, 39, 200, 25, 10F, false,
                Color.FromArgb(198, 210, 233)));
            Controls.Add(top);

            Controls.Add(Theme.Label("Your library, organized.", 54, 133, 760, 44, 25F, true));
            Controls.Add(Theme.Label("Manage books and borrowing in one simple place.", 56, 183, 680, 30,
                11F, false, Theme.Muted));

            var stats = Theme.Card(54, 240, 832, 102);
            stats.Controls.Add(Theme.Label("TOTAL BOOKS", 26, 13, 215, 24, 9F, true, Theme.Muted));
            stats.Controls.Add(Theme.Label("AVAILABLE", 313, 13, 215, 24, 9F, true, Theme.Muted));
            stats.Controls.Add(Theme.Label("ISSUED", 588, 13, 170, 24, 9F, true, Theme.Muted));
            totalValue = Theme.Label("0", 26, 40, 185, 46, 25F, true);
            availableValue = Theme.Label("0", 313, 40, 170, 46, 25F, true, Theme.Success);
            issuedValue = Theme.Label("0", 588, 40, 170, 46, 25F, true, Theme.Primary);
            stats.Controls.Add(totalValue);
            stats.Controls.Add(availableValue);
            stats.Controls.Add(issuedValue);
            Controls.Add(stats);

            var admin = Theme.Card(54, 364, 403, 136);
            admin.Controls.Add(Theme.Label("Admin workspace", 22, 15, 300, 30, 15F, true));
            admin.Controls.Add(Theme.Label("Add, search and maintain your catalog.", 22, 47, 350, 28,
                10F, false, Theme.Muted));
            var adminButton = Theme.Button("Open admin  →", 22, 84, 350, 38);
            adminButton.Click += (s, e) => { using (var form = new AdminLoginForm(library)) form.ShowDialog(this); UpdateStats(); };
            admin.Controls.Add(adminButton);
            Controls.Add(admin);

            var browse = Theme.Card(481, 364, 405, 136);
            browse.Controls.Add(Theme.Label("Library desk", 22, 15, 300, 30, 15F, true));
            browse.Controls.Add(Theme.Label("Browse titles, issue and return books.", 22, 47, 350, 28,
                10F, false, Theme.Muted));
            var browseButton = Theme.Button("Browse library  →", 22, 84, 351, 38, false);
            browseButton.Click += (s, e) => { using (var form = new UserPanelForm(library)) form.ShowDialog(this); UpdateStats(); };
            browse.Controls.Add(browseButton);
            Controls.Add(browse);

            Controls.Add(Theme.Label("Local desktop app  •  Your books are saved automatically", 54, 522,
                800, 28, 9F, false, Theme.Muted));
            Activated += (s, e) => UpdateStats();
            UpdateStats();
        }

        private void UpdateStats()
        {
            var books = library.GetBooks();
            totalValue.Text = books.Count.ToString();
            availableValue.Text = books.Count(b => !b.Issued).ToString();
            issuedValue.Text = books.Count(b => b.Issued).ToString();
        }
    }
}

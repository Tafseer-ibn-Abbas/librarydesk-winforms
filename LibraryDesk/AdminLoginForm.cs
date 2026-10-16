using System;
using System.Drawing;
using System.Windows.Forms;

namespace LibraryDesk
{
    public sealed class AdminLoginForm : Form
    {
        private readonly Library library;
        private readonly TextBox password;

        public AdminLoginForm(Library lib)
        {
            library = lib;
            Theme.Setup(this, "Admin access", 450, 352);
            var card = Theme.Card(30, 29, 390, 291);
            card.Controls.Add(Theme.Label("Admin sign in", 24, 24, 338, 40, 20F, true));
            card.Controls.Add(Theme.Label("Enter your administrator password to continue.", 24, 68,
                340, 44, 9.5F, false, Theme.Muted));
            card.Controls.Add(Theme.Label("PASSWORD", 24, 126, 340, 24, 9F, true, Theme.Muted));
            password = Theme.Input(24, 154, 339);
            password.UseSystemPasswordChar = true;
            card.Controls.Add(password);
            var signIn = Theme.Button("Sign in", 24, 204, 164, 43);
            signIn.Click += (s, e) => SignIn();
            var cancel = Theme.Button("Cancel", 199, 204, 164, 43, false);
            cancel.Click += (s, e) => Close();
            card.Controls.Add(signIn);
            card.Controls.Add(cancel);
            Controls.Add(card);
            AcceptButton = signIn;
            CancelButton = cancel;
            Shown += (s, e) => password.Focus();
        }

        private void SignIn()
        {
            // A clearly documented offline demo password; set env var to override locally.
            string expected = Environment.GetEnvironmentVariable("LIBRARYDESK_ADMIN_PASSWORD");
            if (string.IsNullOrEmpty(expected)) expected = "admin123";
            if (password.Text != expected)
            {
                MessageBox.Show("Incorrect password. Please try again.", "Admin access",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                password.Clear();
                password.Focus();
                return;
            }
            Hide();
            try { using (var form = new AdminPanelForm(library)) form.ShowDialog(Owner); }
            finally { Close(); }
        }
    }
}

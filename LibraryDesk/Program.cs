using System;
using System.Windows.Forms;

namespace LibraryDesk
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var library = new Library();
                Application.Run(new MainForm(library));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Unable to open the library data. Your existing data was not reset.\n\n" +
                    ex.Message, "LibraryDesk startup error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

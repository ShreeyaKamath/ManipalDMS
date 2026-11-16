using System;
using System.Windows.Forms;

namespace ManipalDMS
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            DmsTheme.Initialize();
            Application.Run(new LoginForm());
        }
    }
}

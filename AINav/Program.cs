using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace AINav
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new View());
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Unhandled Exception was hit: \r\n\r\n\r\n" + e.ExceptionObject.ToString());
            Environment.Exit(-1);
        }
    }
}

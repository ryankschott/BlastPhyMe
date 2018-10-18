using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Pilgrimage.Activities;

namespace Pilgrimage
{
    static class Program
    {
        internal static DateTime StartTime { get; private set; }
        internal static string ProductName { get { return _productName; } } private static string _productName;

        internal static AppSettings Settings { get; set; }
        internal static DatabaseSettings DatabaseSettings { get; set; }
        internal static RecordSetSettings RecordSetSettings { get; set; }

        internal static Activities.InProgress InProgressActivities { get; private set; }
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Program.StartTime = DateTime.Now;
            Program.InProgressActivities = new InProgress();
            _productName = Utility.GetEntryAssemblyProductName();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            Application.Run(new frmMain());

            Settings.Save();
        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            try
            {
                Utility.ShowErrorMessage(null, e.Exception);
            }
            catch { }
        }

        internal static void DebugStartupTime(string Message = "")
        {
            System.Diagnostics.Debug.WriteLine(DateTime.Now.Subtract(StartTime).TotalMilliseconds.ToString() + ": " + (string.IsNullOrWhiteSpace(Message) ? " since startup" : Message));
        }
    }
}

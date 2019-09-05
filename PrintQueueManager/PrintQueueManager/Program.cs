using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace PrintQueueManager
{
    static class Program
    {
        public static MainForm mainForm { get; set; }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //bool mutexCreated = true;
            //using (Mutex mutex = new Mutex(true, "PrintQueueManager###", out mutexCreated))
            //{
            //    if (mutexCreated)
            //    {

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    Program.mainForm = new MainForm();
                    Application.Run(Program.mainForm);
            //    }
            //    else
            //    {
            //        Process current = Process.GetCurrentProcess();
            //        foreach (Process process in Process.GetProcessesByName(current.ProcessName))
            //        {
            //            if (process.Id != current.Id)
            //            {
            //                //Win32.SetForegroundWindow(process.MainWindowHandle);
            //                break;
            //            }
            //        }
            //    }
            //}

            //Program.mainForm = new MainForm();
            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(Program.mainForm); // new MainForm());
        }
    }
}

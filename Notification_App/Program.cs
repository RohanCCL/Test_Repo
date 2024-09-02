using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Notification_App
{
    internal static class Program
    {

       
        /// ///////////////// SHEDULER - INSERT TO WINDOWS SERVICE   //
        
        [STAThread]
        static void Main()
        {
			// Create a mutex with a unique name
			bool isNewInstance;
			using (Mutex mutex = new Mutex(true, "MyApp.SingleInstance", out isNewInstance))
			{
				if (!isNewInstance)
				{
					// If another instance is already running, exit the current one
					//MessageBox.Show("Another instance of the application is already running.");
					return;
				}

				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);

				// Register the application to start on system boot
				RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				reg.SetValue("CCLNotification", Application.ExecutablePath.ToString());

				// Run the main form
				Application.Run(new CustomizeView());
			}

		}

        /// //  DISABLE CURRENT SHEDULED SERVICE // /
        public static void DisableScheduledTask()
        {
            string taskName = "CCLNotification";

            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                if (task != null)
                {

					ts.RootFolder.DeleteTask(taskName);
					//task.Enabled = false;
                   
                }
                else
                {
                    MessageBox.Show($"Task '{taskName}' does not exist.");
                }
            }
        }

        // DISABLED SHEDULE ENABLE ///
        public static void EnableScheduledTask()
        {
            string taskName = "CCLNotification";

            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                if (task != null)
                {
                    task.Enabled = true;
                   
                }
                else
                {
                    MessageBox.Show($"Task '{taskName}' does not exist.");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;
using Microsoft.Win32.TaskScheduler;

namespace Notification_App
{
    internal static class Program
    {
   
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run",true);
            reg.SetValue("CCLNotification", Application.ExecutablePath.ToString());
            Application.Run(new CustomizeView());
           



        }

        public static void DisableScheduledTask()
        {
            string taskName = "CCLNotification";

            using (TaskService ts = new TaskService())
            {
                var task = ts.GetTask(taskName);
                if (task != null)
                {
                    task.Enabled = false;
                   
                }
                else
                {
                    MessageBox.Show($"Task '{taskName}' does not exist.");
                }
            }
        }

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

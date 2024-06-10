using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification.Task
{
    public class AppTask
    {
        public static int minute { get; set; } = 1;
        public static void RegisterScheduledTask()
        {
            string taskName = "CCLNotification";
            string executablePath = Application.ExecutablePath;
            string arguments = "-Notification";

            using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                Microsoft.Win32.TaskScheduler.TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Run the notification app every minute.";

                Microsoft.Win32.TaskScheduler.TimeTrigger timeTrigger = new Microsoft.Win32.TaskScheduler.TimeTrigger
                {
                    Repetition = { Interval = TimeSpan.FromMinutes(minute) },
                    StartBoundary = DateTime.Now
                };
                td.Triggers.Add(timeTrigger);

                td.Actions.Add(new Microsoft.Win32.TaskScheduler.ExecAction(executablePath, arguments, null));

                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }


        }

    }
}

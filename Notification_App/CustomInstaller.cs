using Microsoft.Win32.TaskScheduler;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCL_Notification
{
	[RunInstaller(true)]
	public class CustomInstaller : Installer
	{
		public override void Uninstall(System.Collections.IDictionary savedState)
		{
			base.Uninstall(savedState);

			// Remove the registry entry
			try
			{
				RegistryKey reg = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
				if (reg != null)
				{
					reg.DeleteValue("CCLNotification", false);
				}
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log it)
				Debug.WriteLine($"Error removing registry key: {ex.Message}");
			}

			// Remove the scheduled task
			try
			{
				using (TaskService ts = new TaskService())
				{
					ts.RootFolder.DeleteTask("CCLNotification", false);
				}
			}
			catch (Exception ex)
			{
				// Handle the exception (e.g., log it)
				Debug.WriteLine($"Error removing scheduled task: {ex.Message}");
			}
		}
	}
}

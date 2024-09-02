using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace CCL_Notification.Task
{
    public class AppTask
    {
        public static decimal minute { get; set; } = 10;


        public static void GetAppID()
        {
			// string installFolder = AppDomain.CurrentDomain.BaseDirectory;
			// string filePath = Path.Combine(installFolder, "NotificationBotConfig.xml");

	     string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		 string filePath = Path.Combine(documentsFolder, "NotificationBotConfig.xml");

		// string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		// string filePath = Path.Combine(desktopFolder, "NotificationBotConfig.xml");

		List<ConfigModel> P1 = new List<ConfigModel>();
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<ConfigModel>));

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    P1 = xmlSerialize.Deserialize(fs) as List<ConfigModel>;
                }

                if (P1 != null && P1.Count > 0)
                {
                    string AppID = Convert.ToString(P1[0].ID);

                    _ = FetchMinuteFromDatabase(Convert.ToInt32(AppID));
                  
                }
                
            }
            catch (Exception ex)
            {
              
            }
        }

        public static async Task<decimal> FetchMinuteFromDatabase(int APPID)
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getUserDetails";
            decimal executeTime = 1;
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new
                    {
                        AppId = APPID,
                        key = "GetAll@1API"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(responseContent);

                        foreach (JObject obj in jsonArray.Children<JObject>())
                        {
                            foreach (JProperty property in obj.Properties())
                            {
                                if (property.Name == "executionTime")
                                {
                                    executeTime = (decimal)property.Value;

                                    minute = Convert.ToDecimal(property.Value);
                                }
                            }
                        }
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine("Connection lost. Please check your internet connection and try again.");
                Console.WriteLine("Exception Message: " + ex.Message);
            }
            catch (Exception ex)
            {
                FetchMinuteFromDatabaseLocal(APPID);
			}

            return executeTime;
        }


		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>


		public static async Task<decimal> FetchMinuteFromDatabaseLocal(int APPID)
		{
			string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getUserDetails";
			decimal executeTime = 1;
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var parameters = new
					{
						AppId = APPID,
						key = "GetAll@1API"
					};

					HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

					if (response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						JArray jsonArray = JArray.Parse(responseContent);

						foreach (JObject obj in jsonArray.Children<JObject>())
						{
							foreach (JProperty property in obj.Properties())
							{
								if (property.Name == "executionTime")
								{
									executeTime = (decimal)property.Value;

									minute = Convert.ToDecimal(property.Value);
								}
							}
						}
					}
				}
			}
			catch (HttpRequestException ex)
			{
				Console.WriteLine("Connection lost. Please check your internet connection and try again.");
				Console.WriteLine("Exception Message: " + ex.Message);
			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}

			return executeTime;
		}

		/// /////////////////////////////////  MAIN SHEDULER  ///

		public static void RegisterScheduledTask()
        {
            GetAppID();

            string taskName = "CCLNotification";
            string executablePath = Application.ExecutablePath;
            string arguments = "-Notification";

            using (Microsoft.Win32.TaskScheduler.TaskService ts = new Microsoft.Win32.TaskScheduler.TaskService())
            {
                Microsoft.Win32.TaskScheduler.TaskDefinition td = ts.NewTask();
                td.RegistrationInfo.Description = "Run the notification app every minute.";

                Microsoft.Win32.TaskScheduler.TimeTrigger timeTrigger = new Microsoft.Win32.TaskScheduler.TimeTrigger
                {
                    Repetition = { Interval = TimeSpan.FromMinutes((double)minute) },
                    StartBoundary = DateTime.Now
                };
                td.Triggers.Add(timeTrigger);

                td.Actions.Add(new Microsoft.Win32.TaskScheduler.ExecAction(executablePath, arguments, null));

                ts.RootFolder.RegisterTaskDefinition(taskName, td);
            }
        }





    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CCL_Notification
{
    public class DatabaseHelper
    {

        public static readonly HttpClient client = new HttpClient();

        
        public static async Task<List<Plant>> GetPlantsFromDatabase()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getAllPlants";
            List<Plant> plants = new List<Plant>();
            var postData = new { Key = "GetAll@1API" };
            string json = JsonConvert.SerializeObject(postData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    plants = JsonConvert.DeserializeObject<List<Plant>>(responseData);
                }
                else
                {
                    // Handle non-success status codes here if needed
                    Console.WriteLine("Error: Unable to retrieve plants. Status Code: " + response.StatusCode);
                }
            }
            catch (HttpRequestException ex)
            {
				GetPlantsFromDatabaseLocal();

			}
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
            }

            return plants;
        }

		/// <summary>
		/// ///////////////// LOCAL SERVER API CHECK ///////////////////////////////////
		/// </summary>
		/// <returns></returns>
		public static async Task<List<Plant>> GetPlantsFromDatabaseLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getAllPlants";
			List<Plant> plants = new List<Plant>();
			var postData = new { Key = "GetAll@1API" };
			string json = JsonConvert.SerializeObject(postData);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync(apiUrl, content);
				if (response.IsSuccessStatusCode)
				{
					string responseData = await response.Content.ReadAsStringAsync();
					plants = JsonConvert.DeserializeObject<List<Plant>>(responseData);
				}
				else
				{
					// Handle non-success status codes here if needed
					Console.WriteLine("Error: Unable to retrieve plants. Status Code: " + response.StatusCode);
				}
			}
			catch (HttpRequestException ex)
			{

			}
			catch (Exception ex)
			{
				Console.WriteLine("An error occurred: " + ex.Message);
			}

			return plants;
		}


	}
}

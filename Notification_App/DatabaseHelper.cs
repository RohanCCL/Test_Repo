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

            HttpResponseMessage response = await client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseData = await response.Content.ReadAsStringAsync();
                plants = JsonConvert.DeserializeObject<List<Plant>>(responseData);
            }
            else
            {

                throw new Exception("Error fetching data from API: " + response.ReasonPhrase);
            }

            return plants;
        }

    }
}

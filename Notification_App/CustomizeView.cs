using CCL_Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks.Sources;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using System.Security.Authentication;
using CCL_Notification.Task;
using System.Diagnostics.Eventing.Reader;
using System.Text.RegularExpressions;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using System.Diagnostics;


namespace Notification_App
{

   
    public partial class CustomizeView : Form
    {
        static SqlConnection conn;

        int NetworkAvilable = 0;

        int CCL = 0;
        int CCW = 0;
        int CCR = 0;
        int CCD = 0;
        int CCK = 0;
        int IF = 0;

        private System.Windows.Forms.ComboBox comboBox;
        private DataSet ds;
        private SqlDataAdapter adapter;
		// string installFolder = AppDomain.CurrentDomain.BaseDirectory;
		// string filePath = Path.Combine(installFolder, "NotificationBotConfig.xml");

		public static string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		public static string filePath = Path.Combine(documentsFolder, "NotificationBotConfig.xml");

		// public static string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		// public static string filePath = Path.Combine(desktopFolder, "NotificationBotConfig.xml");

		public CustomizeView()
        {
            InitializeComponent();


            BindComboBox();

            btnApply.Enabled= false;
            // Add ComboBox to the form
            Controls.Add(comboBox);
        }

       

        private void CustomizeView_Load(object sender, EventArgs e)
        {
            bindPlant();

			BindComboBox();

			btnApply.Enabled=false;
            checkUseNewID.Checked = true;

            Random rand1 = new Random();
            int num1 = rand1.Next(1, 1000);
            int num2 = rand1.Next(1, 1000);
            int answer = num1 + num2;


            Location = new Point(50, 10);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, 0);


            /////////   BACKGROUND COLOR  CHANGE
            this.BackColor = Color.FromArgb(56, 59, 57);
            //this.BackColor = Color.DarkSlateGray;
            this.TransparencyKey = Color.Teal;
            this.Opacity = 0.80;

            List<ConfigModel> P1 = new List<ConfigModel>();
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<ConfigModel>));

           
            if (File.Exists(filePath))
            {

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    ///////////////////////   GET XML FILE DATA ////////////////////////////////////////

                    P1 = xmlSerialize.Deserialize(fs) as List<ConfigModel>;
                    labAutoID.Text = Convert.ToString(P1[0].ID);
                    labUname.Text = Convert.ToString(P1[0].UserName);

                     /////////////////////////// CHECK XML FILE ASSING PLANT AVILABILITY //////////////////////////////

                    if (P1[0].CCL <= 0 && P1[0].CCD <= 0 && P1[0].CCK <= 0 && P1[0].CCR <= 0 && P1[0].CCW <= 0 && P1[0].IF <= 0)
                    {
                        
                        panel1.Visible = false;

                    }
                    else
                    {

           

                            this.Hide();

						    AppTask.RegisterScheduledTask();  ////////////////////// SHEDULE TASK  /////////////////////////////
						    Notification destinationFormObj = new Notification();
                            destinationFormObj.ShowDialog();


                            this.Close();

                    }

                }
    

            }
            else
            {
                string userName = Environment.UserName;

                labUname.Text = userName;



                flowLayoutPanel1.Visible=false;
                labelGroup.Visible=false;
                cmGroup.Visible=false;
                labAutoID.Visible = false;
                label3.Visible = false;
                btnApply.Visible= false;

                labelExcicution.Visible = false;
                textExcicution.Visible = false;

                panel1.Visible=true;

            }
        }


		private async void BindComboBox()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri("http://cclwebadmin-001-site7.atempurl.com/");
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					// Create an empty POST request body
					var postData = new { Key = "GetAll@1API" };
					string json = JsonConvert.SerializeObject(postData);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync("getAllGroups", content);

					if (response.IsSuccessStatusCode)
					{
						string jsonData = await response.Content.ReadAsStringAsync();


						var groups = JsonConvert.DeserializeObject<List<PlantAccess>>(jsonData);


						DataTable dataTable = new DataTable();
						dataTable.Columns.Add("GroupID", typeof(int));
						dataTable.Columns.Add("group", typeof(string));

						foreach (var group in groups)
						{
							var row = dataTable.NewRow();
							row["GroupID"] = group.GroupID;
							row["group"] = group.group;
							dataTable.Rows.Add(row);
						}

						//DataRow pleaseSelectRow = dataTable.NewRow();
						//pleaseSelectRow["GroupID"] = 0;
						//pleaseSelectRow["group"] = "Please Select";
						//dataTable.Rows.InsertAt(pleaseSelectRow, 0);

						cmGroup.DataSource = dataTable;
						cmGroup.DisplayMember = "group";
						cmGroup.ValueMember = "GroupID";
					}
					else
					{
						BindComboBoxLoacl();
						string errorContent = await response.Content.ReadAsStringAsync();
						

					}
				}
			}
			catch (Exception ex)
			{
                BindComboBoxLoacl();
			}
		}



		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>
		private async void BindComboBoxLoacl()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					client.BaseAddress = new Uri("http://10.40.47.30:99/");
					client.DefaultRequestHeaders.Accept.Clear();
					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

					// Create an empty POST request body
					var postData = new { Key = "GetAll@1API" };
					string json = JsonConvert.SerializeObject(postData);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync("getAllGroups", content);

					if (response.IsSuccessStatusCode)
					{
						string jsonData = await response.Content.ReadAsStringAsync();


						var groups = JsonConvert.DeserializeObject<List<PlantAccess>>(jsonData);


						DataTable dataTable = new DataTable();
						dataTable.Columns.Add("GroupID", typeof(int));
						dataTable.Columns.Add("group", typeof(string));

						foreach (var group in groups)
						{
							var row = dataTable.NewRow();
							row["GroupID"] = group.GroupID;
							row["group"] = group.group;
							dataTable.Rows.Add(row);
						}

			
						cmGroup.DataSource = dataTable;
						cmGroup.DisplayMember = "group";
						cmGroup.ValueMember = "GroupID";
					}
					else
					{
						string errorContent = await response.Content.ReadAsStringAsync();

					}
				}
			}
			catch (Exception ex)
			{

			}
		}





		private async void bindPlant()
        {
            var plants = await DatabaseHelper.GetPlantsFromDatabase();

            CheckBox selectAllCheckBox = new CheckBox();
            selectAllCheckBox.Width = 90;
            selectAllCheckBox.Text = "All";
            selectAllCheckBox.CheckedChanged += new EventHandler(selectAllCheckBox_CheckedChanged);
            flowLayoutPanel1.Controls.Add(selectAllCheckBox);

            foreach (var item in plants)
            {
                CheckBox checkBox = new CheckBox();
                checkBox.Width = 90;
                checkBox.Text = item.PlantName;
                checkBox.Tag = item.PlantID;
                checkBox.CheckedChanged += new EventHandler(changeCheck);
                flowLayoutPanel1.Controls.Add(checkBox);
            }
        }


        private void selectAllCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox selectAllCheckBox = sender as CheckBox;

            foreach (Control control in flowLayoutPanel1.Controls)
            {
                if (control is CheckBox && control != selectAllCheckBox)
                {
                    ((CheckBox)control).CheckedChanged -= changeCheck;
                    ((CheckBox)control).Checked = selectAllCheckBox.Checked;
                    ((CheckBox)control).CheckedChanged += changeCheck;
                }
            }
        }

        private void changeCheck(object sender, EventArgs e)
        {

            CheckBox ch = sender as CheckBox;

            if (!ch.Checked)
            {
                CheckBox selectAllCheckBox = (CheckBox)flowLayoutPanel1.Controls[0];
                selectAllCheckBox.CheckedChanged -= selectAllCheckBox_CheckedChanged;
                selectAllCheckBox.Checked = false;
                selectAllCheckBox.CheckedChanged += selectAllCheckBox_CheckedChanged;
            }
            else
            {
                bool allChecked = true;
                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    if (control is CheckBox && control != flowLayoutPanel1.Controls[0])
                    {
                        if (!((CheckBox)control).Checked)
                        {
                            allChecked = false;
                            break;
                        }
                    }
                }

                if (allChecked)
                {
                    CheckBox selectAllCheckBox = (CheckBox)flowLayoutPanel1.Controls[0];
                    selectAllCheckBox.CheckedChanged -= selectAllCheckBox_CheckedChanged;
                    selectAllCheckBox.Checked = true;
                    selectAllCheckBox.CheckedChanged += selectAllCheckBox_CheckedChanged;
                }
            }

        }


       
        public async Task getID()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getAppId";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new
                    {
                        UserName = labUname.Text.ToString(),
                        key = "GetAll@1API"
                    };

                    string json = JsonConvert.SerializeObject(request);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseContent);
                        int appId = (int)jsonResponse["appId"];
                        decimal exe = (decimal)jsonResponse["executionTime"];
                        textExcicution.Text =exe.ToString();
                        labAutoID.Text = appId.ToString();

                        string userName = Environment.UserName;

                        List<ConfigModel> configModels = new List<ConfigModel>();
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConfigModel>));
                        configModels.Add(new ConfigModel() { ID = Convert.ToInt32(labAutoID.Text), UserName = userName });
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            xmlSerializer.Serialize(fs, configModels);
                        }
                        NetworkAvilable = 1;

					
						AppTask.RegisterScheduledTask();

                      
						flowLayoutPanel1.Visible = true;
						labelGroup.Visible = true;
						cmGroup.Visible = true;
						labAutoID.Visible = true;
						label3.Visible = true;
						btnApply.Visible = true;
						panel1.Visible = false;

						labelExcicution.Visible = true;
						textExcicution.Visible = true;
						
					}
                    else
                    {
						getIDLocal();
						// MessageBox.Show("APPID Generate Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
                }
            }
            catch (Exception ex)
            {
				NetworkAvilable = 0;
                getIDLocal();
			}
        }


		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>
        /// 


		public async Task getIDLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getAppId";
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var request = new
					{
						UserName = labUname.Text.ToString(),
						key = "GetAll@1API"
					};

					string json = JsonConvert.SerializeObject(request);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync(apiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						var responseContent = await response.Content.ReadAsStringAsync();
						JObject jsonResponse = JObject.Parse(responseContent);
						int appId = (int)jsonResponse["appId"];
						decimal exe = (decimal)jsonResponse["executionTime"];
						textExcicution.Text = exe.ToString();
						labAutoID.Text = appId.ToString();

						string userName = Environment.UserName;

						List<ConfigModel> configModels = new List<ConfigModel>();
						XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConfigModel>));
						configModels.Add(new ConfigModel() { ID = Convert.ToInt32(labAutoID.Text), UserName = userName });
						using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
						{
							xmlSerializer.Serialize(fs, configModels);
						}
						NetworkAvilable = 1;


						AppTask.RegisterScheduledTask();


						flowLayoutPanel1.Visible = true;
						labelGroup.Visible = true;
						cmGroup.Visible = true;
						labAutoID.Visible = true;
						label3.Visible = true;
						btnApply.Visible = true;
						panel1.Visible = false;

						labelExcicution.Visible = true;
						textExcicution.Visible = true;

					}
					else
					{
						// MessageBox.Show("APPID Generate Failed....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}
			catch (Exception ex)
			{
				NetworkAvilable = 0;
				MessageBox.Show("Network Not Avilable....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
        {
            
                List<int> selectedPlantIds = GetSelectedPlantIds();


                var A = SendSelectedPlantIdsToDatabase(selectedPlantIds, labAutoID.Text, cmGroup.SelectedValue.ToString(),Convert.ToDecimal(textExcicution.Text));


                UpdateXml();

                this.Hide();
                Notification destinationformobj = new Notification();
                destinationformobj.ShowDialog();

                this.Close();
            
        }


        public void UpdateXml()
        {
            /////////////////  COMPUTER USERNAME  //////////////////
            
            string userName = Environment.UserName;


            List<ConfigModel> configModels = new List<ConfigModel>();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConfigModel>));

            if (File.Exists(filePath))
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    configModels = (List<ConfigModel>)xmlSerializer.Deserialize(fs);
                }
            }

            ConfigModel existingConfig = configModels.FirstOrDefault(c => c.ID == Convert.ToInt32(labAutoID.Text));
            if (existingConfig != null)
            {
                // Update existing xml   As sample data update xml file, To identify plant access added or not
                existingConfig.UserName = userName;
                existingConfig.CCL = CCL;
                existingConfig.CCW = CCW;
                existingConfig.CCR = CCR;
                existingConfig.CCD = CCD;
                existingConfig.CCK = CCK;
                existingConfig.IF = IF;
            }

            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                xmlSerializer.Serialize(fs, configModels);
            }
        }
            private List<int> GetSelectedPlantIds()
            {
                List<int> selectedPlantIds = new List<int>();

                foreach (Control control in flowLayoutPanel1.Controls)
                {
                    if (control is CheckBox checkBox && checkBox.Checked)
                    {

                    if (checkBox.Text != "All")
                    {
                        selectedPlantIds.Add((int)checkBox.Tag);
                    }
                    }
                }

                return selectedPlantIds;
            }


        public async Task SendSelectedPlantIdsToDatabase(List<int> plantIds, string appId, string GroupIDs,decimal Excicution)
        {

            int groupID = Convert.ToInt32(GroupIDs);

            string joinedPlantIds = string.Join(",", plantIds);

            CCL = plantIds.Count > 0 ? plantIds[0] : 0;
            CCW = plantIds.Count > 1 ? plantIds[1] : 0;
            CCR = plantIds.Count > 2 ? plantIds[2] : 0;
            CCD = plantIds.Count > 3 ? plantIds[3] : 0;
            CCK = plantIds.Count > 4 ? plantIds[4] : 0;
            IF = plantIds.Count > 5 ? plantIds[5] : 0;

            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/insertPlantAccess";
            var apiData = new { appId, plantIds = joinedPlantIds, groupID, Key = "GetAll@1API" };

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(apiData);

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    string contentString = await content.ReadAsStringAsync();

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                        bool exchange = await changeExecutionTime(appId, Excicution);
                        Console.WriteLine("Data sent successfully.");
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                        Console.WriteLine($"Response: {responseContent}");
                    }
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"HttpRequestException: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
					var A = SendSelectedPlantIdsToDatabaselOCAL(plantIds, labAutoID.Text, cmGroup.SelectedValue.ToString(), Convert.ToDecimal(textExcicution.Text));
				}
                catch (Exception ex)
                {
					var A = SendSelectedPlantIdsToDatabaselOCAL(plantIds, labAutoID.Text, cmGroup.SelectedValue.ToString(), Convert.ToDecimal(textExcicution.Text));
				}
            }
        }


		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>
		public async Task SendSelectedPlantIdsToDatabaselOCAL(List<int> plantIds, string appId, string GroupIDs, decimal Execution)
		{
			int groupID = Convert.ToInt32(GroupIDs);

			string joinedPlantIds = string.Join(",", plantIds);

			CCL = plantIds.Count > 0 ? plantIds[0] : 0;
			CCW = plantIds.Count > 1 ? plantIds[1] : 0;
			CCR = plantIds.Count > 2 ? plantIds[2] : 0;
			CCD = plantIds.Count > 3 ? plantIds[3] : 0;
			CCK = plantIds.Count > 4 ? plantIds[4] : 0;
			IF = plantIds.Count > 5 ? plantIds[5] : 0;

			string apiUrl = "http://10.40.47.30:99/insertPlantAccess";
			var apiData = new { appId, plantIds = joinedPlantIds, groupID, Key = "GetAll@1API" };

			using (var client = new HttpClient())
			{
				try
				{
					var jsonData = JsonConvert.SerializeObject(apiData);

					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

					string contentString = await content.ReadAsStringAsync();

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = await client.PostAsync(apiUrl, content);

					string responseContent = await response.Content.ReadAsStringAsync();

					if (response.IsSuccessStatusCode)
					{
						bool exchange = await changeExecutionTimelOCAL(appId, Execution);
						Console.WriteLine("Data sent successfully.");
					}
					else
					{
						Console.WriteLine($"Error: {response.StatusCode}");
						Console.WriteLine($"Response: {responseContent}");
					}
				}
				catch (HttpRequestException ex)
				{
					Console.WriteLine($"HttpRequestException: {ex.Message}");
					if (ex.InnerException != null)
					{
						Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
					}
					
				}
				catch (Exception ex)
				{
					
				}
			}

		}




		private async Task<bool> changeExecutionTime(string appId, decimal executionTime)
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/changeExecutionTime";
            var apiData = new { appId, ExecutionTime = executionTime, Key = "GetAll@1API" };

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(apiData);
                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);
                    string responseContent = await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode)
                    {
                       
                        return true;
                    }
                    else
                    {
                     
                        return false;
                    }
                }
                catch (HttpRequestException ex)
                {
                    
					bool exchange = await changeExecutionTimelOCAL(appId, executionTime);
					return false;
                }
                catch (Exception ex)
                {
					bool exchange = await changeExecutionTimelOCAL(appId, executionTime);
					return false;
                }
            }
        }


		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>
        /// 
		private async Task<bool> changeExecutionTimelOCAL(string appId, decimal executionTime)
		{
			string apiUrl = "http://10.40.47.30:99/changeExecutionTime";
			var apiData = new { appId, ExecutionTime = executionTime, Key = "GetAll@1API" };

			using (var client = new HttpClient())
			{
				try
				{
					var jsonData = JsonConvert.SerializeObject(apiData);
					var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

					client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
					HttpResponseMessage response = await client.PostAsync(apiUrl, content);
					string responseContent = await response.Content.ReadAsStringAsync();

					if (response.IsSuccessStatusCode)
					{

						return true;
					}
					else
					{

						return false;
					}
				}
				catch (HttpRequestException ex)
				{
					
					return false;
				}
				catch (Exception ex)
				{
					
					return false;
				}
			}
		}





		private void label2_Click(object sender, EventArgs e)
        {

        }

        private void checkCurrentID_CheckedChanged(object sender, EventArgs e)
        {
            if (checkUseNewID.Checked == true)
            {
                checkUseNewID.Checked = false;

                textCurrentID.Visible=true;
            }
            else
            {
                checkCurrentID.Checked = true;
                textCurrentID.Visible = true;
            }
        }

        private void checkUseNewID_CheckedChanged(object sender, EventArgs e)
        {
            if (checkCurrentID.Checked == true)
            {
                checkCurrentID.Checked = false;
                textCurrentID.Visible = false;
            }
            else
            {
                checkUseNewID.Checked = true;
            }
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void checkBoxAlradyEx_CheckedChanged(object sender, EventArgs e)
        {
            panel1.Visible=false;
            panel2.Visible= true;
         
        }


        private async Task<string> GetAlradyexUserID(string APPID)
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getActiveStatus";
            string AppId = "0";
            if (string.IsNullOrEmpty(APPID))
            {
                APPID = "0";
            }
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new
                    {
                        appId = APPID,
                        userName = labUname.Text,
                        key = "GetAll@1API"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseContent = await response.Content.ReadAsStringAsync();
                        JArray jsonArray = JArray.Parse(responseContent);
                        bool appIdFound = false;

                        foreach (JObject obj in jsonArray.Children<JObject>())
                        {
                            foreach (JProperty property in obj.Properties())
                            {
                                if (property.Name == "appId")
                                {
                                    int appActive = (int)property.Value;
                                    AppId = appActive.ToString();

                                    // Example assignments, adjust as needed
                                    CCL = 1;
                                    CCD = 3;
                                    CCW = 5;

                                    labAutoID.Text = Convert.ToString(appActive);
                                    appIdFound = true;
                                    NetworkAvilable = 1;
                                    break; // Exit inner foreach loop once appID is found
                                }
                            }

                            if (appIdFound)
                            {
                                break; // Exit outer foreach loop once appID is found
                            }
                        }

                        if (!appIdFound)
                        {
                            panel2.Visible = true;
                            NetworkAvilable = 1;
						    AppId = "0";

							MessageBox.Show("AppID not Available....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);

							return AppId;
						}
                    }
                    else
                    {
						GetAlradyexUserIDlOACAL(APPID);
						AppId = "0";

                    }
                }
            }
            catch (Exception ex)
            {
                NetworkAvilable = 0;
				AppId = "0";

                GetAlradyexUserIDlOACAL(APPID);

			}

            return AppId;
        }


        /// <summary>
        /// /////////////////  LOCAL 
        /// </summary>
        /// <param name="APPID"></param>
        /// <returns></returns>
		private async Task<string> GetAlradyexUserIDlOACAL(string APPID)
		{
			string apiUrl = "http://10.40.47.30:99/getActiveStatus";
			string AppId = "0";
			if (string.IsNullOrEmpty(APPID))
			{
				APPID = "0";
			}
			try
			{
				using (HttpClient client = new HttpClient())
				{
					var parameters = new
					{
						appId = APPID,
						userName = labUname.Text,
						key = "GetAll@1API"
					};

					HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

					if (response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						JArray jsonArray = JArray.Parse(responseContent);
						bool appIdFound = false;

						foreach (JObject obj in jsonArray.Children<JObject>())
						{
							foreach (JProperty property in obj.Properties())
							{
								if (property.Name == "appId")
								{
									int appActive = (int)property.Value;
									AppId = appActive.ToString();

									// Example assignments, adjust as needed
									CCL = 1;
									CCD = 3;
									CCW = 5;

									labAutoID.Text = Convert.ToString(appActive);
									appIdFound = true;
									NetworkAvilable = 1;
									break; // Exit inner foreach loop once appID is found
								}
							}

							if (appIdFound)
							{
								break; // Exit outer foreach loop once appID is found
							}
						}

						if (!appIdFound)
						{
							panel2.Visible = true;
							NetworkAvilable = 1;
							AppId = "0";

							MessageBox.Show("AppID not Available....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);

                            return AppId;
						}
					}
					else
					{
						AppId = "0";
					}
				}
			}
			catch (Exception ex)
			{
				NetworkAvilable = 0;
				AppId = "0";
			}

			return AppId;
		}







		private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
            getID();

            //if (NetworkAvilable == )
            //{
            //    MessageBox.Show("Network Not Avilable....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
            //}


        }

        private async Task<string> GetAppID()
        {
            string AppID = "0";

            if (!string.IsNullOrEmpty(textAppID.Text))
            {
                AppID = await GetAlradyexUserID(textAppID.Text);
            }
            else
            {
                AppID = await GetAlradyexUserID("");
            }

            if(NetworkAvilable == 0)
            {
				panel2.Visible=true;
				MessageBox.Show("Network Not Avilable....", "Error", MessageBoxButtons.OK, MessageBoxIcon.Question);
			}

            return AppID;
        }

        private async void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible = false;

            string AppID = await GetAppID();

            //////// SHEDULE TASK ///////////////////
            AppTask.RegisterScheduledTask();

            if (labAutoID.Text != "0")
            {
                List<ConfigModel> configModels = new List<ConfigModel>();
                XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConfigModel>));
                configModels.Add(new ConfigModel() { ID = Convert.ToInt32(labAutoID.Text), UserName = labUname.Text });

                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                {
                    xmlSerializer.Serialize(fs, configModels);
                }

                UpdateXml();

                flowLayoutPanel1.Visible = true;
                labelGroup.Visible = true;
                cmGroup.Visible = true;
                labAutoID.Visible = true;
                label3.Visible = true;
                btnApply.Visible = true;
                panel1.Visible = false;

                labelExcicution.Visible = true;
                textExcicution.Visible = true;

                this.Hide();
                Notification destinationformobj = new Notification();
                destinationformobj.ShowDialog();
                this.Close();
            }
        }


        private void linkBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            panel2.Visible = false;
            flowLayoutPanel1.Visible = false;
            labelGroup.Visible = false;
            cmGroup.Visible = false;
            panel1.Visible = true;
            checkBoxAlradyEx.Checked = false;

            labelExcicution.Visible = false;
            textExcicution.Visible = false;
        }

        private void cmGroup_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmGroup.SelectedValue is DataRowView rowView)
            {
                // Extract the GroupID from the DataRowView
                var groupID = rowView["GroupID"];

                // Check if the extracted value is not 0
                if (groupID != null && groupID.ToString() != "0")
                {
                    btnApply.Enabled = true;
                }
                else
                {
                    btnApply.Enabled = false;
                }
            }
            else if (cmGroup.SelectedValue != null && cmGroup.SelectedValue.ToString() != "0")
            {
                btnApply.Enabled = true;
            }
            else
            {
                btnApply.Enabled = false;
            }
        }

        private void cmGroup_SelectedValueChanged(object sender, EventArgs e)
        {
          
        }
    }
}

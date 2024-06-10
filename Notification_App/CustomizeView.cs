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


namespace Notification_App
{
    public partial class CustomizeView : Form
    {
        static SqlConnection conn;

        int CCL = 0;
        int CCW = 0;
        int CCR = 0;
        int CCD = 0;
        int CCK = 0;
        int IF = 0;

        // string installFolder = AppDomain.CurrentDomain.BaseDirectory;
        // string filePath = Path.Combine(installFolder, "NotificationBotConfig.xml");

        public static string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static string filePath = Path.Combine(desktopFolder, "NotificationBotConfig.xml");

        public CustomizeView()
        {
            InitializeComponent();
        }

        private void CustomizeView_Load(object sender, EventArgs e)
        {
            bindPlant();

            checkUseNewID.Checked = true;

            Random rand1 = new Random();
            int num1 = rand1.Next(1, 1000);
            int num2 = rand1.Next(1, 1000);
            int answer = num1 + num2;


            Location = new Point(50, 10);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, 0);


            /////////   BACKGROUND COLOR  CHANGE
            this.BackColor = Color.FromArgb(17, 17, 19);
            //this.BackColor = Color.DarkSlateGray;
            this.TransparencyKey = Color.Teal;
            this.Opacity = 0.80;

            List<ConfigModel> P1 = new List<ConfigModel>();
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<ConfigModel>));

           
            if (File.Exists(filePath))
            {

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    P1 = xmlSerialize.Deserialize(fs) as List<ConfigModel>;
                    labAutoID.Text = Convert.ToString(P1[0].ID);
                    labUname.Text = Convert.ToString(P1[0].UserName);

                    if (P1[0].CCL <= 0 && P1[0].CCD <= 0 && P1[0].CCK <= 0 && P1[0].CCR <= 0 && P1[0].CCW <= 0 && P1[0].IF <= 0)
                    {
                        int AA = 4;
                        panel1.Visible = false;

                    }
                    else
                    {
                        this.Hide();


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
                labAutoID.Visible = false;
                label3.Visible = false;
                btnApply.Visible= false;

                panel1.Visible=true;

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


       
        //public void getID()
        //{
        //    SqlConnection conn = new SqlConnection(SQLTask.GetConnection());

        //    try
        //    {
        //        if (conn.State == System.Data.ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }

        //        using (SqlCommand cmd = new SqlCommand("InsertAppID_NotificationBot", conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;

        //            cmd.Parameters.Add(new SqlParameter("@UserName", labUname.Text.ToString()));
        //            SqlParameter outputIdParam = new SqlParameter("@ID", SqlDbType.Int)
        //            {
        //                Direction = ParameterDirection.Output
        //            };
        //            cmd.Parameters.Add(outputIdParam);


        //            cmd.ExecuteNonQuery();
        //            int newId = (int)cmd.Parameters["@ID"].Value;
        //            labAutoID.Text = Convert.ToString(newId);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to insert data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {
        //        if (conn.State == System.Data.ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //}



        public async Task getID()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getAppId/"+ labUname.Text.ToString() + "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var request = new
                    {
                        UserName = labUname.Text.ToString()
                    };

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        JObject jsonResponse = JObject.Parse(responseContent);
                        int appId = (int)jsonResponse["appId"];
                        labAutoID.Text = appId.ToString();

                        string userName = Environment.UserName;

                        List<ConfigModel> configModels = new List<ConfigModel>();
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ConfigModel>));
                        configModels.Add(new ConfigModel() { ID = Convert.ToInt32(labAutoID.Text), UserName = userName });
                        using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
                        {
                            xmlSerializer.Serialize(fs, configModels);

                        }
                    }
                    else
                    {

                        MessageBox.Show("Failed to insert data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to insert data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            // SaveConfigDetails();

            List<int> selectedPlantIds = GetSelectedPlantIds();

            
            SendSelectedPlantIdsToDatabase(selectedPlantIds, labAutoID.Text);
            

            UpdateXml();

            this.Hide();
            Notification destinationformobj = new Notification();
            destinationformobj.ShowDialog();

            this.Close();

        }


        public void UpdateXml()
        {

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
                // Update existing config
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


        //public void SendSelectedPlantIdsToDatabase(List<int> plantIds, string AppID)
        //{
        //    CCL = plantIds.Count > 0 ? plantIds[0] : 0;
        //    CCW = plantIds.Count > 1 ? plantIds[1] : 0;
        //    CCR = plantIds.Count > 2 ? plantIds[2] : 0;
        //    CCD = plantIds.Count > 3 ? plantIds[3] : 0;
        //    CCK = plantIds.Count > 4 ? plantIds[4] : 0;
        //    IF = plantIds.Count > 5 ? plantIds[5] : 0;

        //    string plantIdsCsv = string.Join(",", plantIds);

        //    using (SqlConnection connection = new SqlConnection(SQLTask.GetConnection()))
        //    {
        //        connection.Open();

        //        using (SqlCommand command = new SqlCommand("NotificationBot_InsertPlantAccess", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.Parameters.AddWithValue("@AppID", AppID);
        //            command.Parameters.AddWithValue("@PlantIDs", plantIdsCsv);
        //            command.ExecuteNonQuery();
        //        }
        //    }
        //}


        public async Task SendSelectedPlantIdsToDatabase(List<int> plantIds, string appId)
        {

            string joinedPlantIds = string.Join(",", plantIds);
            string apiUrl = "https://cclwebadmin-001-site7.atempurl.com/insertPlantAccess";
            var apiData = new { appId, plantIds = joinedPlantIds };

            using (var client = new HttpClient())
            {
                try
                {
                    var jsonData = JsonConvert.SerializeObject(apiData);
                    Console.WriteLine($"Request Data: {jsonData}");

                    var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                    // Log the content object
                    string contentString = await content.ReadAsStringAsync();
                    Console.WriteLine($"StringContent: {contentString}");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    string responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Response Content: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
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
                    Console.WriteLine($"Exception: {ex.Message}");
                }
            }
        }



        //public void SaveConfigDetails()
        //{


        //    SqlConnection conn = new SqlConnection(SQLTask.GetConnection());

        //    try
        //    {

        //        if (conn.State == System.Data.ConnectionState.Closed)
        //        {
        //            conn.Open();
        //        }



        //        // Create a SqlCommand to call the stored procedure
        //        using (SqlCommand cmd = new SqlCommand("UpdatePlantAccessDetails", conn))
        //        {
        //            cmd.CommandType = System.Data.CommandType.StoredProcedure;
        //            cmd.Parameters.Add(new SqlParameter("@UserName", labUname.Text.ToString()));
        //            cmd.Parameters.Add(new SqlParameter("@ID", Convert.ToInt32(labAutoID.Text)));
        //            cmd.Parameters.Add(new SqlParameter("@CCL", Convert.ToInt32(CCL)));
        //            cmd.Parameters.Add(new SqlParameter("@CCW", Convert.ToInt32(CCW)));
        //            cmd.Parameters.Add(new SqlParameter("@CCR", Convert.ToInt32(CCR)));
        //            cmd.Parameters.Add(new SqlParameter("@CCD", Convert.ToInt32(CCD)));
        //            cmd.Parameters.Add(new SqlParameter("@CCK", Convert.ToInt32(CCK)));
        //            cmd.Parameters.Add(new SqlParameter("@IF", Convert.ToInt32(IF)));
        //            cmd.ExecuteNonQuery();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Failed to insert data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    finally
        //    {

        //        if (conn.State == System.Data.ConnectionState.Open)
        //        {
        //            conn.Close();
        //        }
        //    }
        //}


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
            GetAlradyexUserID();
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
                labAutoID.Visible = true;
                label3.Visible = true;
                btnApply.Visible = true;
                panel1.Visible= false;

                this.Hide();
                Notification destinationformobj = new Notification();
                destinationformobj.ShowDialog();

                this.Close();
            }
        }

        public string GetAlradyexUserID()
        {
            try
            {

                conn = new SqlConnection(SQLTask.GetConnection());
                SqlCommand command = new SqlCommand("EXEC [GetAppID_NotificationBot] '" + labUname.Text + "'", conn);

                if (conn.State.ToString() == "Closed") { conn.Open(); }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int ID = (int)reader["AppID"];
                         CCL =1;
                         CCD =3;
                         CCW =5;
           
                        labAutoID.Text =Convert.ToString(ID);
                       
                    }
                }
                else
                {
                    MessageBox.Show($"Application ID not Found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                   
                }

               
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to insert data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {

                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                }

            }
            return "0";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            
            getID();

            AppTask.RegisterScheduledTask();

            flowLayoutPanel1.Visible = true;
            labAutoID.Visible = true;
            label3.Visible = true;
            btnApply.Visible = true;
            panel1.Visible = false;
        }
    }
}

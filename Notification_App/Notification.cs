
using CCL_Notification;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;


namespace Notification_App
{
    public partial class Notification : Form
    {
        static SqlConnection conn;

        private const int MF_REMOVE = 0x1000;
        private const int SC_CLOSE = 0xF060;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;

        private const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        private Timer minimizeTimer;
        private Timer restoreTimer;

        private readonly HttpClient _httpClient;

        int minute = 1;

        public Notification()
        {
            InitializeComponent();

            // Initialize the timer


            checkStateTimer = new Timer();
            checkStateTimer.Interval = minute * 60000; // 60,000 milliseconds = 1 minute  // 300000  - 5 min
            checkStateTimer.Tick += new EventHandler(CheckStateTimer_Tick);

            dataGridView1.Height = this.ClientSize.Height - 20;

            this.Resize += new System.EventHandler(this.Form1_Resize);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
          
            dataGridView1.Height = this.ClientSize.Height - 20; // Adjust 20 as needed
        }

        private async void CheckStateTimer_Tick(object sender, EventArgs e)
        {
             BindValue();


            string status = await checActivateStatus();

            if (status != "True")
            {
                Program.DisableScheduledTask();
                this.Close();
            }

            if (this.WindowState == FormWindowState.Minimized)
            {
                this.WindowState = FormWindowState.Normal;
            }
        }

        private async void Form1_Load(object sender, EventArgs e)
        {

            DataGridViewColumn column1 = dataGridView1.Columns[0];
            DataGridViewColumn column2 = dataGridView1.Columns[1];
            DataGridViewColumn column3 = dataGridView1.Columns[2];
            DataGridViewColumn column4 = dataGridView1.Columns[3];
          
            column1.Width = 50;
            column2.Width = 110;
            column3.Width = 100;
            column4.Width = 90;
          

            string pcName = System.Environment.MachineName;

            //label2.Text= pcName;

            //MaximizeBox = false;
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);

            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Location = new Point(50, 10);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, 0);


            // Attach the mouse event handlers to the form
            this.MouseDown += new MouseEventHandler(Notification_MouseDown);
            this.MouseMove += new MouseEventHandler(Notification_MouseMove);
            this.MouseUp += new MouseEventHandler(Notification_MouseUp);


            this.MouseDown += new MouseEventHandler(panel1_MouseDown);
            this.MouseMove += new MouseEventHandler(panel1_MouseMove);
            this.MouseUp += new MouseEventHandler(panel1_MouseUp);
            //this.TopMost = true;


            /////////   BACKGROUND COLOR  CHANGE
            ///
         
            this.BackColor = Color.FromArgb(17, 17, 19);
            // this.BackColor = Color.DarkKhaki;
            this.TransparencyKey = Color.DarkSlateGray;
            this.Opacity = 0.65;

            checkStateTimer.Start();

            getUserData();

            //  checActivateStatus();


            string status = await checActivateStatus();

            if (status != "True")
            {

                Program.DisableScheduledTask();

                this.Close();

            }
            else
            {
                // Program.EnableScheduledTask();

                RegisterScheduledTask();
                InitializeDataGridView();
                labelAppID.Text = labAutoID.Text;
                BindValue();
                
            }

          //  RegisterScheduledTask();
        }





        private void InitializeDataGridView()
        {

            dataGridView1.BackgroundColor = Color.FromArgb(17, 17, 19);

            // Set the default cell style background color
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);

            // Set the background color for alternating rows
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);

            // Set the column header background color
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
           

            // Set the row header background color
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);

            // Disable visual styles for headers to apply custom styles
            dataGridView1.EnableHeadersVisualStyles = false;
        }



        private async System.Threading.Tasks.Task BindValue()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getHomePageData"; // Replace with your API endpoint
            string appId = labAutoID.Text;
            string requestUrl = $"{apiUrl}/{appId}"; // Assuming the API expects appId as a query parameter

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

                    // Convert the list to a DataTable
                    DataTable dataTable = new DataTable();
                    dataTable.Columns.Add("plantName", typeof(string));
                    dataTable.Columns.Add("status", typeof(string));
                    dataTable.Columns.Add("frNow", typeof(string));
                    dataTable.Columns.Add("factoryPlan", typeof(string));
                    dataTable.Columns.Add("plantID", typeof(string));

                    foreach (var item in plantAccessList)
                    {
                        dataTable.Rows.Add(item.plantName, item.status, item.frNow, item.factoryPlan, item.plantID);
                    }

                    dataGridView1.DataSource = dataTable;
                }
                else
                {
                    // Handle API call failure
                    MessageBox.Show("Failed to retrieve data from API");
                }
            }
        }

        //private void BindValue()
        //{

        //    string connectionString = SQLTask.GetConnection();

        //    string query = "Select PlantM.PlantName as Plant ,'1200' as frhours,'1220' as planthours,'1200' as Actualhours FROM NotificationBot_PlantAccess INNER JOIN"
        //     + " PlantM ON NotificationBot_PlantAccess.PlantID = PlantM.PlantId where NotificationBot_PlantAccess.AppID='" + labAutoID.Text + "'";

        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        connection.Open();
        //        SqlDataAdapter adapter = new SqlDataAdapter(command);
        //        DataTable dataTable = new DataTable();
        //        adapter.Fill(dataTable);

        //        dataGridView1.DataSource = dataTable;
        //    }

        //}


        private async Task<DataTable> GetPlantData(string appId)
        {
            var response = await _httpClient.GetAsync($"api/PlantData/{appId}");
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DataTable>(json);
            return data;
        }



        public void getUserData()
        {

            List<ConfigModel> P1 = new List<ConfigModel>();
            XmlSerializer xmlSerialize = new XmlSerializer(typeof(List<ConfigModel>));

           // string installFolder = AppDomain.CurrentDomain.BaseDirectory;
           // string filePath = Path.Combine(installFolder, "NotificationBotConfig.xml");

           string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
           string filePath = Path.Combine(desktopFolder, "NotificationBotConfig.xml");

            if (File.Exists(filePath))
            {

                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    P1 = xmlSerialize.Deserialize(fs) as List<ConfigModel>;
                    labAutoID.Text = Convert.ToString(P1[0].ID);
                    labUname.Text = Convert.ToString(P1[0].UserName);
                }
            }
        }




        public async Task<string> checActivateStatus()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getActiveStatus/";
            string activeStatus = "0";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new Dictionary<string, string>
            {

                { "appID", labAutoID.Text },
                { "userName", labUname.Text }
            };

                    var content = new FormUrlEncodedContent(parameters);
                    var response = await client.GetAsync($"{apiUrl}/{labAutoID.Text}/{labUname.Text}");

                    if (response.IsSuccessStatusCode)
                    {
                        activeStatus = await response.Content.ReadAsStringAsync();

                        JArray jsonArray = JArray.Parse(activeStatus);
                        foreach (JObject obj in jsonArray.Children<JObject>())
                        {
                            foreach (JProperty property in obj.Properties())
                            {
                                if (property.Name == "appActive")
                                {
                                    bool appActive = (bool)property.Value;
                                    activeStatus = appActive.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        activeStatus = "false";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to get data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                activeStatus = "0";
            }

            return activeStatus;
        }



        //public string checActivateStatus()
        //{
        //    try
        //    {

        //        conn = new SqlConnection(SQLTask.GetConnection());
        //        SqlCommand command = new SqlCommand("EXEC [GetActiveStatus_NotificationBot] '" + labUname.Text + "'," + labAutoID.Text + "", conn);

        //        if (conn.State.ToString() == "Closed") { conn.Open(); }
        //        SqlDataReader reader = command.ExecuteReader();
        //        if (reader.HasRows)
        //        {
        //            while (reader.Read())
        //            {
        //                string Active = reader["AppActive"].ToString();

        //                return Active;
        //            }
        //        }
        //        else
        //        {
        //            return "False";
        //        }

        //        return "False";


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
        //    return "0";
        //}



        private void RegisterScheduledTask()
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



        private void MinimizeTimer_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            minimizeTimer.Stop();
            restoreTimer.Start();
        }

        private void RestoreTimer_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            restoreTimer.Stop();
            minimizeTimer.Start();
        }

     
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            CustomizeView destinationformobj = new CustomizeView();
            destinationformobj.Show();
        }

        private void Notification_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void Notification_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void Notification_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ccllbLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void ccdlbLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
        }

        private void ccklbLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
       
        }

        private void ccwlbLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
         
        }

        private void ccrlbLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
          
        }

        private void ifLab_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
            {
                string plant = dataGridView1.Rows[e.RowIndex].Cells["plantID"].Value.ToString();
                string plantName = dataGridView1.Rows[e.RowIndex].Cells["Plant"].Value.ToString();

                string UserID = labAutoID.Text;
                string AppID = labAutoID.Text;

                this.Hide();
                PlantView plantView = new PlantView(plant, UserID, plantName, AppID);
                plantView.ShowDialog();
                

            }
        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }
    }
}

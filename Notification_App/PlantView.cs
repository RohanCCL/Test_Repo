using Newtonsoft.Json;
using Notification_App;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification
{
    public partial class PlantView : Form
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

        private string plant;
        private string userID;

        int minute = 1;
        public PlantView(string plant, string userID,string PlantName,string AppID)
        {
            InitializeComponent();


            labelPlant.Text = PlantName;
            labUserID.Text= userID;
            labAppID.Text= AppID;
            labPlantID.Text = plant;


            CheckStateTimer = new Timer();
            CheckStateTimer.Interval = minute * 60000; 
            CheckStateTimer.Tick += new EventHandler(CheckStateTimer_Tick);

            CheckStateTimer.Start();
        }

        private void CheckStateTimer_Tick(object sender, EventArgs e)
        {
            BindValue();
      
        }

        private void PlantView_Load(object sender, EventArgs e)
        {
            DataGridViewColumn column1 = dataGridView1.Columns[0];
            DataGridViewColumn column2 = dataGridView1.Columns[1];
            DataGridViewColumn column3 = dataGridView1.Columns[2];
           // DataGridViewColumn column4 = dataGridView1.Columns[3];

            column1.Width = 80;
            column2.Width = 115;
            column3.Width = 110;


            DataGridViewColumn column4 = dataGridView2.Columns[0];
            DataGridViewColumn column5 = dataGridView2.Columns[1];
            DataGridViewColumn column6 = dataGridView2.Columns[2];
            // DataGridViewColumn column4 = dataGridView1.Columns[3];

            column4.Width = 80;
            column5.Width = 115;
            column6.Width = 110;
            // column4.Width = 110;

            string pcName = System.Environment.MachineName;
            //label2.Text= pcName;

            //MaximizeBox = false;
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);

            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Location = new Point(50, 10);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, 0);
          
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

            BindValue();
            InitializeDataGridView();
        }


        private void InitializeDataGridView()
        {

            dataGridView1.BackgroundColor = Color.FromArgb(17, 17, 19);
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView2.BackgroundColor = Color.FromArgb(17, 17, 19);
            dataGridView2.DefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(17, 17, 19);
            dataGridView2.EnableHeadersVisualStyles = false;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            dragCursorPoint = Cursor.Position;
            dragFormPoint = this.Location;
        }

        private void linkLabelBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();


            Notification Noti = new Notification();
            Noti.ShowDialog();
        }

        //public void BindValue()
        //{

        //    string connectionString = SQLTask.GetConnection();

        //    string query = "Select '120' as frhours,'120' as planthours,'122' as Actualhours FROM NotificationBot_PlantAccess INNER JOIN"
        //     + " PlantM ON NotificationBot_PlantAccess.PlantID = PlantM.PlantId where PlantM.PlantName='" + labelPlant.Text + "' AND NotificationBot_PlantAccess.AppID=" + labUserID.Text + "";

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


        private async System.Threading.Tasks.Task BindValue()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getPlantData"; 
            string appId = labAppID.Text;
            string PlantID = labPlantID.Text;
            string requestUrl = $"{apiUrl}/{appId}/{PlantID}"; 

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(requestUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

                    // Convert the list to a DataTable
                    DataTable dataTable = new DataTable();
                 
                    dataTable.Columns.Add("status", typeof(string));
                    dataTable.Columns.Add("frNow", typeof(string));
                    dataTable.Columns.Add("factoryPlan", typeof(string));

                    foreach (var item in plantAccessList)
                    {
                        dataTable.Rows.Add(item.status, item.frNow, item.factoryPlan);
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

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}

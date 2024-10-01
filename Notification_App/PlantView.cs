using CCL_Notification.Task;
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
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification
{
    public partial class PlantView : Form
    {

        static SqlConnection conn;

		public event EventHandler BackButtonClicked;

		private bool isExpanded = false;
		private Timer resizeTimer;
		private int targetWidth;
		private const int PanelCollapsedWidth = 35; // Width when collapsed
		private const int PanelExpandedWidth = 400; // Width when expanded
		private const int AnimationStep = 5; // Amount of width change per step
		private const int FormExpandedWidth = 400;
		private const int FormCollapsedWidth = 430;

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

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(


          int nLeftRect,
          int nTopRect,
          int nRightRect,
          int nBottmRect,
          int nWidthEllipse,
          int nHeightEllipse


      );

        private Timer minimizeTimer;
        private Timer restoreTimer;

        private string plant;
        private string userID;

        int minute = 1;

     


        public PlantView(string plant, string userID,string PlantName,string AppID)
        {
            InitializeComponent();

			/////////////////////////////////////////////////////////   

			this.Width = FormCollapsedWidth; // Start with collapsed width

			// Initialize timer for animation
			resizeTimer = new Timer();
			resizeTimer.Interval = 5; // Animation interval
			resizeTimer.Tick += ResizeTimer_Tick;

			// Set up the Button or PictureBox click event
			//button1.Click += TogglePanel_Click;


			//////////////////////////////////////////////////////////

			this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));

            labelPlant.Text = PlantName;
            labUserID.Text= userID;
            labAppID.Text= AppID;
            labPlantID.Text = plant;


            CheckStateTimer = new Timer();
            CheckStateTimer.Interval = 60000; // minute * 60000; 
			CheckStateTimer.Tick += new EventHandler(CheckStateTimer_Tick);

            CheckStateTimer.Start();
        }

		private void ResizeTimer_Tick(object sender, EventArgs e)
		{
			int widthChange;

			// Disable the user interaction during the animation
			this.Enabled = false;

			if (this.Width < targetWidth)
			{
				// Calculate how much the form should expand
				widthChange = Math.Min(AnimationStep, targetWidth - this.Width);

				// Ensure that moving left does not push the form off-screen
				if (this.Left - widthChange >= 0)
				{
					this.Left -= widthChange; // Move form left to expand right
				}

				this.Width += widthChange;
			}
			else if (this.Width > targetWidth)
			{
				// Calculate how much the form should shrink
				widthChange = Math.Min(AnimationStep, this.Width - targetWidth);

				// Ensure that the form doesn't shrink beyond a visible size
				if (this.Width - widthChange >= 0)
				{
					this.Left += widthChange; // Move form right to collapse left
					this.Width -= widthChange;
				}
			}

			// Stop the timer when the target width is reached
			if (this.Width == targetWidth)
			{
				resizeTimer.Stop();

				// Re-enable user interaction after the animation completes
				this.Enabled = true;
			}

			// Only initialize DataGridView if necessary
			InitializeDataGridView();
		}

		private void CheckStateTimer_Tick(object sender, EventArgs e)
        {

            

			BindValue();
            

        }

        private void PlantView_Load(object sender, EventArgs e)
        {

			pictureBack.SizeMode = PictureBoxSizeMode.Zoom;
			this.Size = pictureBack.Size;

			DataGridViewColumn column1 = dataGridView1.Columns[0];
            DataGridViewColumn column2 = dataGridView1.Columns[1];
            DataGridViewColumn column3 = dataGridView1.Columns[2];
            DataGridViewColumn column7 = dataGridView1.Columns[3];
			//DataGridViewColumn column8 = dataGridView1.Columns[4];

			column1.Width = 70;
            column2.Width = 120;
            column3.Width = 65;
            column7.Width = 70;
			//column8.Width = 90;


			DataGridViewColumn column4 = dataGridView2.Columns[0];
            DataGridViewColumn column5 = dataGridView2.Columns[1];
            DataGridViewColumn column6 = dataGridView2.Columns[2];
            DataGridViewColumn column8 = dataGridView2.Columns[2];
            // DataGridViewColumn column4 = dataGridView1.Columns[3];

            column4.Width = 70;
            column5.Width = 110;
            column6.Width = 50;
            column8.Width = 50;
            // column4.Width = 110;

            string pcName = System.Environment.MachineName;
			//label2.Text= pcName;
			this.TopMost = true;

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
            this.BackColor = Color.FromArgb(56, 59, 57);
            // this.BackColor = Color.DarkKhaki;
            this.TransparencyKey = Color.DarkSlateGray;
           // this.Opacity = 0.65;
            this.Opacity = 0.90;

            BindValue();
            InitializeDataGridView();
		}


        private void InitializeDataGridView()
        {

            dataGridView1.BackgroundColor = Color.FromArgb(56, 59, 57);
			dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView1.EnableHeadersVisualStyles = false;

            dataGridView2.BackgroundColor = Color.FromArgb(56, 59, 57);
			dataGridView2.DefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
			dataGridView2.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);
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
                //Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                //this.Location = Point.Add(dragFormPoint, new Size(diff));
            }
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //dragging = true;
            //dragCursorPoint = Cursor.Position;
            //dragFormPoint = this.Location;
        }

        
      
        private async System.Threading.Tasks.Task BindValue()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getPlantData";
            string appId = labAppID.Text;
            string plantID = labPlantID.Text;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new
                    {
                        appId = appId,
                        plantId = plantID,
                        key = "GetAll@1API"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

						DataTable dataTable = new DataTable();
						dataTable.Columns.Add("frPlan", typeof(string));
						dataTable.Columns.Add("utnfrHrs", typeof(string));
						dataTable.Columns.Add("teamOutHrs", typeof(string));
						dataTable.Columns.Add("fgHrs", typeof(string));
						//dataTable.Columns.Add("TeamOutEfficiency", typeof(string));

						foreach (var item in plantAccessList)
						{
							dataTable.Rows.Add(item.frPlan, item.utnfrHrs, item.TeamOutHrs, item.fgHrs);  // , item.TeamOutEfficiency

							labNetworkWaiting.Visible = false;

						}
						dataGridView1.DataSource = dataTable;
						dataGridView1.ClearSelection();
						BindValueMonth();
                    }
                    else
                    {
						BindValueLocal();
						labNetworkWaiting.Visible = true;
                       
					}
                }
            }
            catch (Exception ex)
            {
				labNetworkWaiting.Visible = true;
				BindValueLocal();
				
            }
        }


		/// <summary>
		/// //////////////////////  LOCAL NETWORK CONNECTION CHECK /////////////////////////// 
		/// </summary>
		/// <returns></returns>
		private async System.Threading.Tasks.Task BindValueLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getPlantData";
			string appId = labAppID.Text;
			string plantID = labPlantID.Text;

			try
			{
				using (HttpClient client = new HttpClient())
				{
					var parameters = new
					{
						appId = appId,
						plantId = plantID,
						key = "GetAll@1API"
					};

					HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

					if (response.IsSuccessStatusCode)
					{
						string responseData = await response.Content.ReadAsStringAsync();
						List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

						DataTable dataTable = new DataTable();
						dataTable.Columns.Add("frPlan", typeof(string));
						dataTable.Columns.Add("utnfrHrs", typeof(string));
						dataTable.Columns.Add("teamOutHrs", typeof(string));
						dataTable.Columns.Add("fgHrs", typeof(string));
						//dataTable.Columns.Add("TeamOutEfficiency", typeof(string));

						foreach (var item in plantAccessList)
						{
							dataTable.Rows.Add(item.frPlan, item.utnfrHrs,item.TeamOutHrs ,item.fgHrs);   // , item.TeamOutEfficiency

							labNetworkWaiting.Visible = false;

						}

						dataGridView1.DataSource = dataTable;
						dataGridView1.ClearSelection();
						BindValueMonthLocal();
					}
					else
					{
						labNetworkWaiting.Visible = true;
					}
				}
			}
			catch (Exception ex)
			{
				labNetworkWaiting.Visible = true;
				
			}
		}




		private async System.Threading.Tasks.Task BindValueMonth()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getPlantData";
            string appId = labAppID.Text;
            string plantID = labPlantID.Text;

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new
                    {
                        appId = appId,
                        plantId = plantID,
                        key = "GetAll@1API"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

                        DataTable dataTable = new DataTable();
                       // dataTable.Columns.Add("TargetHrsMonth", typeof(string));
                        //dataTable.Columns.Add("frHrsMontH", typeof(string));
                        //dataTable.Columns.Add("fgHrsMonth", typeof(string));

                        dataTable.Columns.Add("freezeFRPlanHrs", typeof(string));
                        dataTable.Columns.Add("frHrsMontH", typeof(string));
                        dataTable.Columns.Add("teamOutHrsMonth", typeof(string));                    
                        dataTable.Columns.Add("teamScanHrsMonth", typeof(string));


                        foreach (var item in plantAccessList)
                        {
                            dataTable.Rows.Add(item.freezeFRPlanHrs, item.frHrsMontH,item.teamOutHrsMonth, item.teamScanHrsMonth);
                        }

                        dataGridView2.DataSource = dataTable;

						dataGridView2.ClearSelection();
					}
                    else
                    {
						BindValueMonthLocal();
					}
                }
            }
            catch (Exception ex)
            {
                BindValueMonthLocal();

			}
        }



		/// <summary>
		/// //////////////////////  LOCAL NETWORK CONNECTION CHECK /////////////////////////// 
		/// </summary>
		/// <returns></returns>
		private async System.Threading.Tasks.Task BindValueMonthLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getPlantData";
			string appId = labAppID.Text;
			string plantID = labPlantID.Text;

			try
			{
				using (HttpClient client = new HttpClient())
				{
					var parameters = new
					{
						appId = appId,
						plantId = plantID,
						key = "GetAll@1API"
					};

					HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

					if (response.IsSuccessStatusCode)
					{
						string responseData = await response.Content.ReadAsStringAsync();
						List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

						DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("freezeFRPlanHrs", typeof(string));
                        dataTable.Columns.Add("frHrsMontH", typeof(string));
                        dataTable.Columns.Add("teamOutHrsMonth", typeof(string));
                        dataTable.Columns.Add("teamScanHrsMonth", typeof(string));


                        foreach (var item in plantAccessList)
                        {
                            dataTable.Rows.Add(item.freezeFRPlanHrs, item.frHrsMontH, item.teamOutHrsMonth, item.teamScanHrsMonth);
                        }

                        dataGridView2.DataSource = dataTable;

						dataGridView2.ClearSelection();
					}
					else
					{

					}
				}
			}
			catch (Exception ex)
			{
				// Handle the exception if needed
				Console.WriteLine($"An error occurred: {ex.Message}");
			}
		}


		private void label10_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                if (!row.IsNewRow)
                {

                  
                    //if (row.Cells[4].Value != null && double.TryParse(row.Cells[4].Value.ToString(), out double amount))
                    //{
                    //    row.Cells[4].Value = $"{amount}%";
                    //}
                    
                }

                row.Cells[0].Style.ForeColor = Color.White;
            }
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView2_DataSourceChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView2.Rows)
            {

               // row.Cells[0].Style.ForeColor = Color.FromArgb(245, 66, 239);
            }
        }

		private void pictureBack_Click(object sender, EventArgs e)
		{

			//this.Hide();

			//Notification mainForm = Application.OpenForms.OfType<Notification>().FirstOrDefault();
			//if (mainForm != null)
			//{
			//	mainForm.Show();
			//}
			//else
			//{
			//	mainForm = new Notification();
			//	mainForm.Show();
			//}

			//this.Close();


			//////////////////////////////   NEWLY ADDED   ///////////////////////////////////

			// Raise the event to notify the main form
			BackButtonClicked?.Invoke(this, EventArgs.Empty);

			// Optionally hide this form if needed
			this.Hide();
		}

		private void pictureBack_MouseEnter(object sender, EventArgs e)
		{
			
		}
	}
}

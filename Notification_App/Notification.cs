
using CCL_Notification;
using CCL_Notification.Task;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32.TaskScheduler;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace Notification_App
{
    public partial class Notification : Form
    {
        static SqlConnection conn;

		private bool isExpanded = false;
		private Timer resizeTimer;
		private int targetWidth;
		private const int PanelCollapsedWidth = 32; // Width when collapsed
		private const int PanelExpandedWidth = 413; // Width when expanded
		private const int AnimationStep = 6; // Amount of width change per step
		private const int FormExpandedWidth = 400;
		private const int FormCollapsedWidth = 434;


		/// <summary>
		/// /
		/// </summary>
		/// <returns></returns>

	

		[DllImport("user32.dll")]
		private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		private static readonly string[] SensitiveApps =
		{
		"MICROSOFT WORD",
		"OUTLOOK",
		"GMAIL",
		"NOTEPAD",
		"WORDPAD",
		"MICROSOFT VISUAL STUDIO",
		"MICROSOFT POWERPOINT"
		};

		private const int MaxTitleLength = 255;
		private const int SW_SHOWNOACTIVATE = 4;



		/// <summary>
		/// /////////////////////////
		/// </summary>


		public decimal ExecutedTime=1;

        //private Timer checkStateTime;
        //private bool isMinimized = false;

        //private Timer animationTimer;
        //private int targetColumnIndex = 5; // Column index to animate
        //private Color startColor = Color.White;
        //private Color endColor = Color.FromArgb(225, 3, 62);
        //private int animationStep = 0;
        //private int totalSteps = 100;

		// string installFolder = AppDomain.CurrentDomain.BaseDirectory;
		// string filePath = Path.Combine(installFolder, "NotificationBotConfig.xml");

		public static string documentsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
		public static string filePath = Path.Combine(documentsFolder, "NotificationBotConfig.xml");

		//public static string desktopFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
		// public static string filePath = Path.Combine(desktopFolder, "NotificationBotConfig.xml");

		private System.Windows.Forms.ProgressBar progressBar1;

        private const int MF_REMOVE = 0x1000;
        private const int SC_CLOSE = 0xF060;
        private bool dragging = false;
        private Point dragCursorPoint;
        private Point dragFormPoint;


        private int Isopen = 0;
		

		private const int MF_BYPOSITION = 0x400;
        [DllImport("User32")]
        private static extern int RemoveMenu(IntPtr hMenu, int nPosition, int wFlags);
        [DllImport("User32")]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
        [DllImport("User32")]
        private static extern int GetMenuItemCount(IntPtr hWnd);

        private Timer minimizeTimer;
        private Timer restoreTimer;

        private Timer minimizeTime;

        private readonly HttpClient _httpClient;

        /// <summary>
        /// 
        /// </summary>
       




        ///////////////////////////////////////////////////////////////////
        
        //  int minute = 1;

        [DllImport("Gdi32.dll",EntryPoint ="CreateRoundRectRgn") ]

        private static extern IntPtr CreateRoundRectRgn(
            
            
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottmRect,
            int nWidthEllipse,
            int nHeightEllipse
            
            
        );

        public Notification()
        {
           InitializeComponent();


			/////////////////////////////////////////////////////////   EXPAND OPTION ///////////////////////////////////////

			this.Width = FormCollapsedWidth; // Start with collapsed width

			// Initialize timer for animation
			resizeTimer = new Timer();
			resizeTimer.Interval = 5; // Animation interval
			resizeTimer.Tick += ResizeTimer_Tick;

			// Set up the Button or PictureBox click event
			//button1.Click += TogglePanel_Click;


			///////// Replace panel2 with TransparentPanel ///////////////////////////////////////////

			//TransparentPanel transparentPanel = new TransparentPanel();
			//transparentPanel.Location = panel3.Location;
			//transparentPanel.Size = panel3.Size;

			//this.Controls.Add(transparentPanel);
			//this.Controls.Remove(panel3);

			//////////////////////////////////////////////////////////

			// InitializeAnimationTimer();
			//StartAnimation(5, Color.Red, Color.Red, 100, 500);

			progressBar1 = new System.Windows.Forms.ProgressBar();

            // ProgressBar
            progressBar1.Location = new System.Drawing.Point(50, 50);
            progressBar1.Size = new System.Drawing.Size(200, 30);
            progressBar1.Visible = false;

            Controls.Add(progressBar1);

            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
            // Initialize the timer

            dataGridView1.DataSourceChanged += new EventHandler(dataGridView1_DataSourceChanged);

            getUserData();

			//decimal executeTime  = AppTask.FetchMinuteFromDatabase(Convert.ToInt32(labAutoID.Text));
			// Call the async method to set up timers and other logic

			// Save the currently active window (before this form opens)
			//previousForegroundWindow = GetForegroundWindow();


			_ = ExampleUsage();

			////////////////////////   AUTO REFRESH TIME  ///////////////////////////

			checkStateTimer = new Timer();
            checkStateTimer.Interval = 60000; // 60,000 milliseconds = 1 minute  // 300000  - 5 min AppTask.minute * 
            checkStateTimer.Tick += new EventHandler(CheckStateTimer_Tick);
            checkStateTimer.Start();
   

            dataGridView1.Height = this.ClientSize.Height - 20;

            this.Resize += new System.EventHandler(this.Form1_Resize);

            //this.Close();
            
        }

	



		private void TogglePanel_Click(object sender, EventArgs e)
		{
			if (!isExpanded)
			{
				// Set the target width for expanding
				//this.Opacity = 0.65;

				targetWidth = PanelExpandedWidth;
				resizeTimer.Start();
				isExpanded = true;
			}
			else
			{
				// Set the target width for collapsing
				targetWidth = PanelCollapsedWidth;
				resizeTimer.Start();
				isExpanded = false;
			}
		}

		
		/// <summary>
		/// /////////////////  EXPAND OPTION   //////////////////////////////////////
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
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




		public async Task<decimal> ExampleUsage()
		{
			int appId = Convert.ToInt32(labAutoID.Text);
			decimal executeTime = await AppTask.FetchMinuteFromDatabase(appId);
			ExecutedTime = executeTime;


			/////////////////////   APPLY AUTO POPUP THME & SHEDULE  ////////////////////////////////////////////////////////

			minimizeTime = new Timer();
			minimizeTime.Interval = (int)(decimal)(executeTime * 60000); // 1 minute  executeTime *
			minimizeTime.Tick += new EventHandler(MinTimer_Tick);
			minimizeTime.Start();

			/////////////////// Initialize restore timer (8 milliseconds interval) /////////////////////////

			restoreTimer = new Timer();
			restoreTimer.Interval = 60000;
			restoreTimer.Tick += new EventHandler(RestoreTimer_Tick);

			return executeTime;
		}



		private void Form1_Resize(object sender, EventArgs e)
        {
          
            dataGridView1.Height = this.ClientSize.Height - 20; // Adjust 20 as needed
        }

		private bool IsPlantViewOpen()
		{
			// Check if any open forms are of type PlantView
			foreach (Form openForm in Application.OpenForms)
			{
				if (openForm is PlantView)
				{
					return true;
				}
			}
			return false;
		}

		private void OpenMainForm()
		{
			if (!IsPlantViewOpen())
			{

				Isopen = 0;
				BindValue();

               
			}
		
		}
		private async void CheckStateTimer_Tick(object sender, EventArgs e)
        {
                OpenMainForm();
        
                string status = await checActivateStatus();



                if (status != "True" && status != "[]")
                {
                    Program.DisableScheduledTask();
                    this.Close();
                }

		
			BindValue();
			

		}
        private void RestoreTimer_Tick(object sender, EventArgs e)
        {
			BindValue();
			//RestoreForm();
           // restoreTimer.Stop(); // Stop the restore timer after restoring the form
        }
       

        private void MinTimer_Tick(object sender, EventArgs e)
        {
			
			if (!IsSensitiveAppInFocus())
			{
				//MinimizeForm();
				ShowWindow(this.Handle, SW_SHOWNOACTIVATE);
				targetWidth = PanelExpandedWidth;
				resizeTimer.Start();
				isExpanded = true;

				dataGridView1.ClearSelection();
			}
		}

        private void MinimizeForm()
        {
            this.WindowState = FormWindowState.Minimized;
            restoreTimer.Start();
        }

        private void RestoreForm()
        {
			//this.WindowState = FormWindowState.Normal;
			//this.BringToFront();
		
		}


		public static bool IsSensitiveAppInFocus()
		{
			IntPtr hWnd = GetForegroundWindow();
			StringBuilder windowTitle = new StringBuilder(MaxTitleLength);

			if (GetWindowText(hWnd, windowTitle, MaxTitleLength) > 0)
			{
				string title = windowTitle.ToString().ToUpper();

				foreach (var app in SensitiveApps)
				{
					if (title.Contains(app))
					{
						return true; // A sensitive app is in focus
					}
				}
			}

			return false;
		}




		private async void Form1_Load(object sender, EventArgs e)
        {

			label4.Visible = true;
			panel1.Paint += new PaintEventHandler(panel1_Paint);

			DataGridViewColumn column1 = dataGridView1.Columns[0];
            DataGridViewColumn column2 = dataGridView1.Columns[1];
            DataGridViewColumn column3 = dataGridView1.Columns[2];
            DataGridViewColumn column4 = dataGridView1.Columns[3];
            DataGridViewColumn column5 = dataGridView1.Columns[4];

            column1.Width = 57; // 50
            column2.Width = 95;
            column3.Width = 55;
            column4.Width = 110;
            column5.Width = 90;


            string pcName = System.Environment.MachineName;

			//label2.Text= pcName;
			this.TopMost = true;

			//MaximizeBox = false;
			IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);

            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Location = new Point(50, 10);
			int offset = 30; // Amount to move down
			Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, offset);



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
         
            this.BackColor = Color.FromArgb(56, 59, 57);

			//this.BackColor = Color.FromArgb(17, 17, 19);

			// this.BackColor = Color.DarkKhaki;
			//this.TransparencyKey = Color.FromArgb(56, 59, 57);
			//this.Opacity = 0.65;
			this.Opacity = 0.90;      ////////////////////////////////   Tansparancy  

            checkStateTimer.Start();

            getUserData();

            string status = await checActivateStatus();



            if (status != "True" && status != "[]")
            {

                Program.DisableScheduledTask();

                this.Close();

            }
            else
            {


                AppTask.RegisterScheduledTask();
                InitializeDataGridView();
                labelAppID.Text = labAutoID.Text;


                BindValue();

            }
        }


		
		private void InitializeDataGridView()
        {

            dataGridView1.BackgroundColor = Color.FromArgb(56, 59, 57);

            // Set the default cell style background color
            dataGridView1.DefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);

            // Set the background color for alternating rows
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);

            // Set the column header background color
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);

            // Set the row header background color
            dataGridView1.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(56, 59, 57);

            // Disable visual styles for headers to apply custom styles
            dataGridView1.EnableHeadersVisualStyles = false;
        }



        private async System.Threading.Tasks.Task BindValue()
        {   
		     string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getHomePageData"; 
		    
			string appId = labAutoID.Text;

            try
            {
                progressBar1.Visible = true; // Show progress bar
                using (HttpClient client = new HttpClient())
                {
                    var postData = new { appId = appId, Key = "GetAll@1API" };
                    string json = JsonConvert.SerializeObject(postData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(apiUrl, content);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

                        DataTable dataTable = new DataTable();
                        dataTable.Columns.Add("plantName", typeof(string));
                        dataTable.Columns.Add("frPlan", typeof(string));
                        dataTable.Columns.Add("utnfrHrs", typeof(string));
                        dataTable.Columns.Add("TeamOutHrs", typeof(string));  
                        dataTable.Columns.Add("plantID", typeof(string));
                        dataTable.Columns.Add("TeamOutEfficiency", typeof(decimal));

                        foreach (var item in plantAccessList)
                        {

                            //this.Hide();
                            //MessageNotification plantView = new MessageNotification();
                            //plantView.ShowDialog();

                            dataTable.Rows.Add(item.plantName, item.frPlan, item.utnfrHrs, item.TeamOutHrs, item.plantID, item.TeamOutEfficiency);

							labNetworkWaiting.Visible = false;

							dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
							dataGridView1.GridColor = Color.WhiteSmoke;  // You can set any color you like

							
							//dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
							//dataGridView1.GridColor = Color.WhiteSmoke;

						}

                        dataGridView1.DataSource = dataTable;

                        if (dataTable.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = dataTable;
                        }
                        else
                        {
                            this.Hide();
                        }
                    }
                    else
                    {
						BindValueLocal();
						// MessageBox.Show("Failed to retrieve data from API");
					}
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("An error occurred: " + ex.Message);
                labNetworkWaiting.Visible = true;
                BindValueLocal();

			}
            finally
            {
                progressBar1.Visible = false; // Hide progress bar
            }
        }



		/// <summary>
		/// ////////////////////// LOCAL SERVER API CHECK ///////////////////////////
		/// </summary>
		/// <returns></returns>

		private async System.Threading.Tasks.Task BindValueLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getHomePageData";
			string appId = labAutoID.Text;

			try
			{
				progressBar1.Visible = true; // Show progress bar
				using (HttpClient client = new HttpClient())
				{
					var postData = new { appId = appId, Key = "GetAll@1API" };
					string json = JsonConvert.SerializeObject(postData);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					HttpResponseMessage response = await client.PostAsync(apiUrl, content);

					if (response.IsSuccessStatusCode)
					{
						string responseData = await response.Content.ReadAsStringAsync();
						List<PlantAccess> plantAccessList = JsonConvert.DeserializeObject<List<PlantAccess>>(responseData);

						DataTable dataTable = new DataTable();
						dataTable.Columns.Add("plantName", typeof(string));
						dataTable.Columns.Add("frPlan", typeof(string));
						dataTable.Columns.Add("utnfrHrs", typeof(string));
						dataTable.Columns.Add("TeamOutHrs", typeof(string));
						dataTable.Columns.Add("plantID", typeof(string));
						dataTable.Columns.Add("TeamOutEfficiency", typeof(decimal));


						foreach (var item in plantAccessList)
						{

							//this.Hide();
							//MessageNotification plantView = new MessageNotification();
							//plantView.ShowDialog();

							dataTable.Rows.Add(item.plantName, item.frPlan, item.utnfrHrs, item.TeamOutHrs, item.plantID, item.TeamOutEfficiency);

							labNetworkWaiting.Visible = false;

							dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
							dataGridView1.GridColor = Color.WhiteSmoke;  


							//dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
							//dataGridView1.GridColor = Color.WhiteSmoke;

						}

						dataGridView1.DataSource = dataTable;

						if (dataTable.Rows.Count > 0)
						{
							dataGridView1.DataSource = dataTable;
						}
						else
						{
							this.Hide();
						}
					}
					else
					{
						// MessageBox.Show("Failed to retrieve data from API");
					}
				}
			}
			catch (Exception ex)
			{
				// MessageBox.Show("An error occurred: " + ex.Message);
				labNetworkWaiting.Visible = true;

			}
			finally
			{
				progressBar1.Visible = false; // Hide progress bar
			}
		}





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



        private async Task<string> checActivateStatus()
        {
            string apiUrl = "http://cclwebadmin-001-site7.atempurl.com/getActiveStatus";
            string activeStatus = "True";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var parameters = new
                    {
                        appId = labAutoID.Text,
                        userName = labUname.Text,
                        key = "GetAll@1API"
                    };

                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);

                 
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
                            activeStatus = "True";
                        }
                    
                }
            }
            catch (Exception ex)
            {
                checActivateStatusLocal();

			}

            return activeStatus;
        }


		/// <summary>
		/// //////////////////////  LOCAL SERVER API CHECK /////////////////////////// 
		/// </summary>
		/// <returns></returns>

		private async Task<string> checActivateStatusLocal()
		{
			string apiUrl = "http://10.40.47.30:99/getActiveStatus";
			string activeStatus = "True";

			try
			{
				using (HttpClient client = new HttpClient())
				{
					var parameters = new
					{
						appId = labAutoID.Text,
						userName = labUname.Text,
						key = "GetAll@1API"
					};

					HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, parameters);


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
						activeStatus = "True";
					}

				}
			}
			catch (Exception ex)
			{
				// Handle the exception if needed
				Console.WriteLine($"An error occurred: {ex.Message}");
			}

			return activeStatus;
		}


		private void MinimizeTimer_Tick(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            minimizeTimer.Stop();
            restoreTimer.Start();
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
            //dragging = true;
            //dragCursorPoint = Cursor.Position;
            //dragFormPoint = this.Location;
        }

        private void Notification_MouseMove(object sender, MouseEventArgs e)
        {
            //if (dragging)
            //{
            //    Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
            //    this.Location = Point.Add(dragFormPoint, new Size(diff));
            //}
        }

        private void Notification_MouseUp(object sender, MouseEventArgs e)
        {
          //  dragging = false;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            //dragging = true;
            //dragCursorPoint = Cursor.Position;
            //dragFormPoint = this.Location;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
			//if (dragging)
			//{
			//	// Calculate the difference in Y between the current cursor position and the starting cursor position
			//	int diffY = Cursor.Position.Y - dragCursorPoint.Y;

			//	// Calculate the new Y-coordinate for the form
			//	int newY = dragFormPoint.Y + diffY;

			//	// Check if the mouse is on the left side of the form (e.g., within 100 pixels from the left edge)
			//	if (Cursor.Position.X <= this.Location.X + 100)
			//	{
			//		// Set the form's location to the new position, keeping the X coordinate the same
			//		this.Location = new Point(this.Location.X, newY);
			//	}
			//}
		}

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
          //  dragging = false;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
			int borderRadius = 30; // Adjust the radius as needed

			// Set smoothing mode to make the border smooth
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

			// Create a path for the rounded rectangle
			System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
			path.AddArc(new Rectangle(0, 0, borderRadius, borderRadius), 190, 100); // Top-left corner
			path.AddArc(new Rectangle(panel1.Width - borderRadius, 0, borderRadius, borderRadius), -90, 90); // Top-right corner
			path.AddArc(new Rectangle(panel1.Width - borderRadius, panel1.Height - borderRadius, borderRadius, borderRadius), 0, 90); // Bottom-right corner
			path.AddArc(new Rectangle(0, panel1.Height - borderRadius, borderRadius, borderRadius), 90, 90); // Bottom-left corner
			path.CloseFigure();

			// Fill the panel with its background color
			using (Brush brush = new SolidBrush(panel1.BackColor))
			{
				e.Graphics.FillPath(brush, path);
			}

			// Optionally, draw a border around the panel
			using (Pen pen = new Pen(Color.Black, 2)) // Adjust color and thickness as needed
			{
				e.Graphics.DrawPath(pen, path);
			}

			// Set the panel's region to the rounded rectangle to clip its content
			panel1.Region = new Region(path);
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
				

				//////////////////////////////   NEWLY ADDED   ///////////////////////////////////
				
				

				//panelContainer.Visible = true;
				string plant = dataGridView1.Rows[e.RowIndex].Cells["plantID"].Value.ToString();
				string plantName = dataGridView1.Rows[e.RowIndex].Cells["Plant"].Value.ToString();

				if (plantName != "GROUP")
				{
					string UserID = labAutoID.Text;
					string AppID = labAutoID.Text;

					PlantView plantView = new PlantView(plant, UserID, plantName, AppID);
					plantView.TopLevel = false;
					plantView.FormBorderStyle = FormBorderStyle.None;
					plantView.Dock = DockStyle.Fill;

					plantView.BackButtonClicked += (s, args) =>
					{
						// Hide the panelContainer
						this.panelContainer.Visible = false;
						label4.Visible = true;
						this.Show();
					};

					this.panelContainer.Controls.Clear(); // Clear any existing controls
					this.panelContainer.Controls.Add(plantView);
					plantView.Show();
					label4.Visible = false;
					panelContainer.Visible = true;
				}


			}
        }

        private void label8_Click_1(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            
        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {

			
			if (dataGridView1.Rows.Count > 0 && dataGridView1.Rows[0].Cells[0].Value?.ToString() == "CCL2")
			{
				dataGridView1.Rows[0].Cells[0].Value = "CCL";
			}

			foreach (DataGridViewRow row in dataGridView1.Rows)
            {

				if (row.Cells[0].Value?.ToString() == "GROUP")
				{
					// Access the first cell in the row
					var cell = row.Cells[0];

					// Create a new DataGridViewTextBoxCell to replace the link cell
					var textCell = new DataGridViewTextBoxCell
					{
						Value = cell.Value,
						Style = cell.Style
					};

					// Set the font to white and bold
					textCell.Style.ForeColor = Color.White;
					textCell.Style.Font = new Font(dataGridView1.Font, FontStyle.Bold);

					// Replace the link cell with the regular text cell
					row.Cells[0] = textCell;
				}
			

			if (!row.IsNewRow)
                {

					row.Cells[1].Style.ForeColor = Color.White;

					if (row.Cells[5].Value != null && decimal.TryParse(row.Cells[5].Value.ToString(), out decimal percentage))
                    {
                        row.Cells[5].Value =percentage;

                        row.Cells[5].Style.ForeColor = Color.FromArgb(255, 3, 62);

						decimal B;

						if (decimal.TryParse(row.Cells[2].Value.ToString(), out decimal firstValue) &&
							 decimal.TryParse(row.Cells[3].Value.ToString(), out decimal secondValue))
						{
							if(secondValue == 0 && firstValue == 0)
							{
								B = 0;
							}
							else
							{

								if (firstValue == 0)
								{
									firstValue = 1;
								}

								B = (secondValue / firstValue);
							}
							//double A = firstValue - secondValue;
						   

							decimal C = B * 100;

							decimal percentage1 = C;

							if (percentage1 >= 100)                            /////////////////  Green Color
							{
								// If firstValue is equal to or exceeds secondValue (i.e., 100% or more)
								row.Cells[3].Style.ForeColor = Color.FromArgb(65, 252, 3);

								if (percentage == 0)
								{
									row.Cells[5].Style.ForeColor = Color.FromArgb(254, 0, 0);
								}
								else
								{
									row.Cells[5].Style.ForeColor = Color.FromArgb(65, 252, 3);
								}
							}
							//else if (percentage1 >= 50 && percentage1 <= 74)    /////////////////////// Ember Color
							//{
							//	row.Cells[3].Style.ForeColor = Color.FromArgb(249, 168, 81);								
							//    row.Cells[5].Style.ForeColor = Color.FromArgb(249, 168, 81);
							

							//}
							else if (percentage1 >= 75 && percentage1 <= 99)    /////////////////  Orange Color
							{
								// If firstValue has reached between 30% and 49% of secondValue
								//row.Cells[3].Style.ForeColor = Color.FromArgb(254, 254, 3);     /////////////////  Orange Color
								//row.Cells[5].Style.ForeColor = Color.FromArgb(254, 254, 3);

								row.Cells[3].Style.ForeColor = Color.FromArgb(249, 168, 81); /////////////////////// Ember Color
								row.Cells[5].Style.ForeColor = Color.FromArgb(249, 168, 81);
							}
							else
							{

								// row.Cells[3].Style.ForeColor = Color.FromArgb(254, 254, 3);
								row.Cells[3].Style.ForeColor = Color.FromArgb(254, 0, 0);    
								row.Cells[5].Style.ForeColor = Color.FromArgb(254, 0, 0);

							}


						}



					}

		
				}
            }

        }

		private void panel2_MouseHover(object sender, EventArgs e)
		{
			panel2.Click += TogglePanel_Click;
		}

		private void panel2_MouseUp(object sender, MouseEventArgs e)
		{
			

			if (e.Button == MouseButtons.Left)
			{
				dragging = false;
				panel2.Click += TogglePanel_Click;
			}
			
		}

		private void dataGridView1_MouseMove(object sender, MouseEventArgs e)
		{
			//if (dragging)
			//{
			//	// Calculate the difference in Y between the current cursor position and the starting cursor position
			//	int diffY = Cursor.Position.Y - dragCursorPoint.Y;

			//	// Calculate the new Y-coordinate for the form
			//	int newY = dragFormPoint.Y + diffY;

			//	// Check if the mouse is on the left side of the form (e.g., within 100 pixels from the left edge)
			//	if (Cursor.Position.X <= this.Location.X + 100)
			//	{
			//		// Set the form's location to the new position, keeping the X coordinate the same
			//		this.Location = new Point(this.Location.X, newY);
			//	}
			//}
		}

		private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
		{
			dragging = true;
			dragCursorPoint = Cursor.Position;
			dragFormPoint = this.Location;
		}

		private void panel2_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			//if (dragging)
			//{
			//	// Calculate the difference in Y between the current cursor position and the starting cursor position
			//	int diffY = Cursor.Position.Y - dragCursorPoint.Y;

			//	// Calculate the new Y-coordinate for the form
			//	int newY = dragFormPoint.Y + diffY;

			//	// Check if the mouse is on the left side of the form (e.g., within 100 pixels from the left edge)
			//	if (Cursor.Position.X <= this.Location.X + 100)
			//	{
			//		// Set the form's location to the new position, keeping the X coordinate the same
			//		this.Location = new Point(this.Location.X, newY);
			//	}
			//}
		}

		private void panel2_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left && e.X <= 100)
			{
				dragging = true;
				dragCursorPoint = Cursor.Position;
				dragFormPoint = this.Location;
			}
		}

		private void panel2_MouseMove(object sender, MouseEventArgs e)
		{
			// Only drag if dragging is active
			if (dragging)
			{
				// Calculate the difference in Y between the current cursor position and the starting cursor position
				int diffY = Cursor.Position.Y - dragCursorPoint.Y;

				// Calculate the new Y-coordinate for the form
				int newY = dragFormPoint.Y + diffY;

				// Get the screen's working area (excluding taskbar, etc.)
				Rectangle screenBounds = Screen.GetWorkingArea(this);

				// Ensure the form doesn't move above the top of the screen
				if (newY < screenBounds.Top)
				{
					newY = screenBounds.Top;
				}

				// Ensure the form doesn't move below the bottom of the screen
				if (newY + this.Height > screenBounds.Bottom)
				{
					newY = screenBounds.Bottom - this.Height;
				}

				// Set the form's location to the new position, keeping the X coordinate the same
				this.Location = new Point(this.Location.X, newY);
			}
			dataGridView1.ClearSelection();
		}

		private void panelContainer_Paint(object sender, PaintEventArgs e)
		{

		}
	}
}

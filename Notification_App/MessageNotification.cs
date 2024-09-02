using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification
{
    public partial class MessageNotification : Form
    {

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

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn(


            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottmRect,
            int nWidthEllipse,
            int nHeightEllipse


        );
        public MessageNotification()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.None;
            this.Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30));
        }

        private void Message_Load(object sender, EventArgs e)
        {
            string pcName = System.Environment.MachineName;

            //label2.Text= pcName;

            //MaximizeBox = false;
            IntPtr hMenu = GetSystemMenu(this.Handle, false);
            int menuItemCount = GetMenuItemCount(hMenu);

            RemoveMenu(hMenu, menuItemCount - 1, MF_BYPOSITION);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            Location = new Point(50, 10);
            Location = new Point(Screen.PrimaryScreen.Bounds.Width - Width, 0);


            /////////   BACKGROUND COLOR  CHANGE
            ///
            this.BackColor = Color.FromArgb(17, 17, 19);
            // this.BackColor = Color.DarkKhaki;
            this.TransparencyKey = Color.DarkSlateGray;
            // this.Opacity = 0.65;
            this.Opacity = 0.98;
        }
    }
}

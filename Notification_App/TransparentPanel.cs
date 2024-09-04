using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification
{
	public class TransparentPanel : Panel
	{
		private const int WS_EX_TRANSPARENT = 0x20;
		private const int WS_EX_LAYERED = 0x80000;
		private const int GWL_EXSTYLE = (-20);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		protected override void OnHandleCreated(EventArgs e)
		{
			base.OnHandleCreated(e);

			int exStyle = GetWindowLong(this.Handle, GWL_EXSTYLE);
			exStyle |= WS_EX_TRANSPARENT | WS_EX_LAYERED;
			SetWindowLong(this.Handle, GWL_EXSTYLE, exStyle);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Do not paint the background to keep it fully transparent
		}
	}
}


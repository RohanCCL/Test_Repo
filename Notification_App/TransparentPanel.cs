using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CCL_Notification
{
	public class TransparentPanel : Panel
	{
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// Do not paint the background to make the panel fully transparent
		}
	}
}

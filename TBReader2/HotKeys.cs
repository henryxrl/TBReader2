using System;
using System.Drawing;
using System.Globalization;
using System.Resources;

namespace TBReader2
{
	public partial class HotKeys : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public HotKeys(ResourceManager rm, CultureInfo ci, Color themeColor)
		{
			InitializeComponent();

			hotkey1.Text = rm.GetString("hotkey1", ci);
			hotkey2.Text = rm.GetString("hotkey2", ci);
			hotkey3.Text = rm.GetString("hotkey3", ci);
			hotkey4.Text = rm.GetString("hotkey4", ci);
			hotkey5.Text = rm.GetString("hotkey5", ci);
			hotkey6.Text = rm.GetString("hotkey6", ci);
			hotkey7.Text = rm.GetString("hotkey7", ci);
			hotkey8.Text = rm.GetString("hotkey8", ci);
			hotkeys_ok_button.Text = rm.GetString("ok_button", ci);

			hotkey1_label.ForeColor = themeColor;
			hotkey2_label.ForeColor = themeColor;
			hotkey3_label.ForeColor = themeColor;
			hotkey4_label.ForeColor = themeColor;
			hotkey5_label.ForeColor = themeColor;
			hotkey6_label.ForeColor = themeColor;
			hotkey7_label.ForeColor = themeColor;
			hotkey8_label.ForeColor = themeColor;
		}

		private void hotkeys_ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
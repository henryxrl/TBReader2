using System;
using System.Drawing;

namespace TBReader2
{
	public partial class HotKeys : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public HotKeys(Tools tools)
		{
			InitializeComponent();

			hotkey1.Text = tools.getString("hotkey1");
			hotkey2.Text = tools.getString("hotkey2");
			hotkey3.Text = tools.getString("hotkey3");
			hotkey4.Text = tools.getString("hotkey4");
			hotkey5.Text = tools.getString("hotkey5");
			hotkey6.Text = tools.getString("hotkey6");
			hotkey7.Text = tools.getString("hotkey7");
			hotkey8.Text = tools.getString("hotkey8");
			hotkey9.Text = tools.getString("hotkey9");
			hotkeys_ok_button.Text = tools.getString("button_ok");

			hotkey1_label.ForeColor = tools.color;
			hotkey2_label.ForeColor = tools.color;
			hotkey3_label.ForeColor = tools.color;
			hotkey4_label.ForeColor = tools.color;
			hotkey5_label.ForeColor = tools.color;
			hotkey6_label.ForeColor = tools.color;
			hotkey7_label.ForeColor = tools.color;
			hotkey8_label.ForeColor = tools.color;
			hotkey9_label.ForeColor = tools.color;
		}

		private void hotkeys_ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
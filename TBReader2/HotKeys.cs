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
			hotkey10.Text = tools.getString("hotkey10");
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
			hotkey10_label.ForeColor = tools.color;

			hotkey1_label.Text = tools.getString("hotkey1_label");
			hotkey2_label.Text = tools.getString("hotkey2_label");
			hotkey3_label.Text = tools.getString("hotkey3_label");
			hotkey4_label.Text = tools.getString("hotkey4_label");
			hotkey5_label.Text = tools.getString("hotkey5_label");
			hotkey6_label.Text = tools.getString("hotkey6_label");
			hotkey7_label.Text = tools.getString("hotkey7_label");
			hotkey8_label.Text = tools.getString("hotkey8_label");
			hotkey9_label.Text = tools.getString("hotkey9_label");
			hotkey10_label.Text = tools.getString("hotkey10_label");
		}

		private void hotkeys_ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
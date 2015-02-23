using System;
using System.Drawing;

namespace TBReader2
{
	public partial class HotKeys : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public HotKeys(String langCode, Color themeColor)
		{
			InitializeComponent();

			hotkey1.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey1");
			hotkey2.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey2");
			hotkey3.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey3");
			hotkey4.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey4");
			hotkey5.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey5");
			hotkey6.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey6");
			hotkey7.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey7");
			hotkey8.Text = Properties.Resources.ResourceManager.GetString(langCode + "hotkey8");
			hotkeys_ok_button.Text = Properties.Resources.ResourceManager.GetString(langCode + "ok_button");

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
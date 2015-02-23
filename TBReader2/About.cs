using System;
using System.Drawing;

namespace TBReader2
{
	public partial class About : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public About(String langCode, Color themeColor)
		{
			InitializeComponent();

			about_version_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "about_version_label");
			about_author_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "about_author_label");
			about_email_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "about_email_label");
			about_intro_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "about_intro_label");

			about_name.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			about_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			about_author.Text = "Henry Xu";
			about_email.Text = "HenryXrl@Gmail.com";
			about_intro.Text = Properties.Resources.ResourceManager.GetString(langCode + "about_intro");

			about_version_label.ForeColor = themeColor;
			about_author_label.ForeColor = themeColor;
			about_email_label.ForeColor = themeColor;
			about_intro_label.ForeColor = themeColor;

			about_ok_button.Text = Properties.Resources.ResourceManager.GetString(langCode + "ok_button");
		}

		private void ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
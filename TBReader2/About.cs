using System;
using System.Drawing;
using System.Globalization;
using System.Resources;

namespace TBReader2
{
	public partial class About : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public About(ResourceManager rm, CultureInfo ci, Color themeColor)
		{
			InitializeComponent();

			about_version_label.Text = rm.GetString("about_version_label", ci);
			about_author_label.Text = rm.GetString("about_author_label", ci);
			about_email_label.Text = rm.GetString("about_email_label", ci);
			about_intro_label.Text = rm.GetString("about_intro_label", ci);

			about_name.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			about_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			about_author.Text = "Henry Xu";
			about_email.Text = "HenryXrl@Gmail.com";
			about_intro.Text = rm.GetString("about_intro", ci);

			about_version_label.ForeColor = themeColor;
			about_author_label.ForeColor = themeColor;
			about_email_label.ForeColor = themeColor;
			about_intro_label.ForeColor = themeColor;

			about_ok_button.Text = rm.GetString("ok_button", ci);
		}

		private void ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
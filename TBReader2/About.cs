using System;
using System.Drawing;

namespace TBReader2
{
	public partial class About : DevComponents.DotNetBar.Controls.SlidePanel
	{
		public About(Tools tools)
		{
			InitializeComponent();

			about_version_label.Text = tools.getString("about_version_label");
			about_author_label.Text = tools.getString("about_author_label");
			about_email_label.Text = tools.getString("about_email_label");
			about_intro_label.Text = tools.getString("about_intro_label");

			about_name.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
			about_version.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			about_author.Text = "Henry Xu";
			about_email.Text = "HenryXrl@Gmail.com";
			about_intro.Text = tools.getString("about_intro");

			about_version_label.ForeColor = tools.color;
			about_author_label.ForeColor = tools.color;
			about_email_label.ForeColor = tools.color;
			about_intro_label.ForeColor = tools.color;

			about_ok_button.Text = tools.getString("button_ok");

			about_pictureBox.Image = tools.img;
			about_pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
		}

		private void ok_button_Click(object sender, EventArgs e)
		{
			this.IsOpen = false;
		}
	}
}
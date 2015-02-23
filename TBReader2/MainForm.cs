using DevComponents.DotNetBar;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace TBReader2
{
	public partial class MainForm : DevComponents.DotNetBar.Metro.MetroForm
	{
		String langCode;

		Color themeColor = System.Drawing.Color.FromArgb(255, 62, 120, 143);

		private About abt = null;
		private HotKeys hky = null;

		private Int32 aptTime = 0;
		private Int32 timerCount;
		private Int32 timerFlag = -1;	// 0: auto read forward; 1: auto read backward; 2: go back to current; -1: default

		public MainForm()
		{
			InitializeComponent();

			setUILanguageCode();

			TitleText = "<div align=\"left\">  " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "</div>";
			SettingsButtonText = Properties.Resources.ResourceManager.GetString(langCode + "hotkey_button");
			SettingsButtonVisible = true;
			SettingsButtonClick += FormHotKeysButtonClick;
			HelpButtonText = Properties.Resources.ResourceManager.GetString(langCode + "about_button");
			HelpButtonVisible = true;
			HelpButtonClick += FormAboutButtonClick;
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			apt_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "apt_label");
			apt_start_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "apt_start_label");
			apt_end_label.Text = Properties.Resources.ResourceManager.GetString(langCode + "apt_end_label");

			((Control)txt_pictureBox).AllowDrop = true;
			txt_pictureBox.DragEnter += cover_pictureBox_DragEnter;
			txt_pictureBox.DragLeave += cover_pictureBox_DragLeave;
			txt_pictureBox.BackgroundImage = drawBackGroundImage();

			overlay_cover.BackColor = Color.FromArgb(150, Color.Black);
			overlay_cover.Parent = txt_pictureBox;
			overlay_cover.Location = new Point(0, 0);
			overlay_cover.Size = new Size(txt_pictureBox.Width, txt_pictureBox.Height);

			openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
			openFileDialog.Multiselect = false;
			openFileDialog.FilterIndex = 1;

			setHotKeys(false);
			setAbout(false);

		}

		private void setUILanguageCode()
		{
			String curLang = System.Globalization.CultureInfo.CurrentCulture.ToString();
			Boolean isChinese = curLang.Contains("zh") ? true : false;
			if (isChinese)	//in chinese
			{
				langCode = "zh_";
			}
			else	//in english
			{
				langCode = "en_";
			}
		}

		private void FormHotKeysButtonClick(object sender, EventArgs e)
		{
			if (hky == null)
				setHotKeys(false);
			hky.BringToFront();
			hky.IsOpen = true;
		}

		private void setHotKeys(Boolean show)
		{
			SuspendLayout();
			hky = new HotKeys(langCode, themeColor);
			hky.IsOpen = true;
			hky.SetBounds(0, 0, this.Width, this.Height);
			if (!show)
				hky.IsOpen = false;
			Controls.Add(hky);
			if (!show)
				hky.SendToBack();
			else
				hky.BringToFront();
			hky.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Top;
			hky.Parent = this;
			ResumeLayout(false);
		}

		private void FormAboutButtonClick(object sender, EventArgs e)
		{
			if (abt == null)
				setAbout(false);
			abt.BringToFront();
			abt.IsOpen = true;
		}

		private void setAbout(Boolean show)
		{
			SuspendLayout();
			abt = new About(langCode, themeColor);
			abt.IsOpen = true;
			abt.SetBounds(0, 0, this.Width, this.Height);
			if (!show)
				abt.IsOpen = false;
			Controls.Add(abt);
			if (!show)
				abt.SendToBack();
			else
				abt.BringToFront();
			abt.SlideSide = DevComponents.DotNetBar.Controls.eSlideSide.Top;
			abt.Parent = this;
			ResumeLayout(false);
		}

		private void apt_trackBar_Scroll(object sender, EventArgs e)
		{
			aptTime = apt_trackBar.Value;

			if (aptTime > 0)
			{
				timer.Enabled = true;
				timerCount = aptTime;
				timerFlag = 0;	// auto read forward
			}

			String tooltip = aptTime.ToString() + " " + Properties.Resources.ResourceManager.GetString(langCode + "apt_tooltip");
			toolTip.SetToolTip(apt_trackBar, tooltip);
		}

		private void cover_pictureBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
				overlay_cover.Show();
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void cover_pictureBox_DragLeave(object sender, EventArgs e)
		{
			overlay_cover.Hide();
		}

		private Image drawBackGroundImage()
		{
			Image img = new Bitmap(txt_pictureBox.Width, txt_pictureBox.Height);

			using (Graphics g = Graphics.FromImage(img))
			{
				using (Pen pen = new Pen(themeColor, 5))
				{
					pen.DashStyle = DashStyle.Dash;
					pen.DashPattern = new Single[] { 2f, 1.96f, 2f, 1.96f };

					g.DrawLine(pen, 0, 0, txt_pictureBox.Width, 0);
					g.DrawLine(pen, 0, 0, 0, txt_pictureBox.Height);
					g.DrawLine(pen, txt_pictureBox.Width, txt_pictureBox.Height - 1, 0, txt_pictureBox.Height - 1);
					g.DrawLine(pen, txt_pictureBox.Width - 1, txt_pictureBox.Height, txt_pictureBox.Width - 1, 0);
				}

				using (SolidBrush b = new SolidBrush(themeColor))
				{
					String s = Properties.Resources.ResourceManager.GetString(langCode + "pictureBox_string_1");
					Font f = new Font("Microsoft YaHei UI", 25, FontStyle.Bold);
					SizeF size = g.MeasureString(s, f);
					Single px = txt_pictureBox.Width / 2 - size.Width / 2;
					Single py = txt_pictureBox.Height / 2 - size.Height / 2 - 50;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					g.DrawString(s, f, b, px, py);

					s = Properties.Resources.ResourceManager.GetString(langCode + "pictureBox_string_2");
					f = new Font("Microsoft YaHei UI", 25, FontStyle.Bold);
					size = g.MeasureString(s, f);
					px = txt_pictureBox.Width / 2 - size.Width / 2;
					py = txt_pictureBox.Height / 2 - size.Height / 2 + 10;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					g.DrawString(s, f, b, px, py);
				}
			}

			return img;
		}

		private void txt_pictureBox_DoubleClick(object sender, EventArgs e)
		{
			overlay_cover.Show();

			openFileDialog.Title = Properties.Resources.ResourceManager.GetString(langCode + "openFileDialog_title");
			openFileDialog.Filter = Properties.Resources.ResourceManager.GetString(langCode + "openFileDialog_filter");
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				MessageBoxEx.Show(openFileDialog.FileName);
			}

			overlay_cover.Hide();
		}

	}
}

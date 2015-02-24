using DevComponents.DotNetBar;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using AutoUpdate;

namespace TBReader2
{
	public partial class MainForm : DevComponents.DotNetBar.Metro.MetroForm, AutoUpdatable
	{
		#region AutoUpdate
		private AutoUpdater updater;
		
		public string ApplicationName
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; }
		}

		public string ApplicationID
		{
			get { return System.Reflection.Assembly.GetExecutingAssembly().GetName().Name; }
		}

		public Tools Tools
		{
			get { return tools; }
		}

		public Assembly ApplicationAssembly
		{
			get { return Assembly.GetExecutingAssembly(); }
		}

		public Icon ApplicationIcon
		{
			get { return this.Icon; }
		}

		public Uri UpdateXmlLocation
		{
			get { return new Uri("https://raw.githubusercontent.com/henryxrl/TBReader2/master/TBReader2_Update.xml"); }
		}

		public Form Context
		{
			get { return this; }
		}
		#endregion

		Tools tools = null;

		private About abt = null;
		private HotKeys hky = null;

		private Int32 aptTime = 0;
		private Int32 timerCount;
		private Int32 timerFlag = -1;	// 0: auto read forward; 1: auto read backward; 2: go back to current; -1: default

		public MainForm()
		{
			InitializeComponent();
			
			setTools();

			TitleText = "<div align=\"left\">  " + System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + "</div>";
			SettingsButtonText = tools.getString("hotkey_button");
			SettingsButtonVisible = true;
			SettingsButtonClick += FormHotKeysButtonClick;
			HelpButtonText = tools.getString("about_button");
			HelpButtonVisible = true;
			HelpButtonClick += FormAboutButtonClick;

			updater = new AutoUpdater(this);
		}

		private void MainForm_Load(object sender, EventArgs e)
		{
			apt_label.Text = tools.getString("apt_label");
			apt_start_label.Text = tools.getString("apt_start_label");
			apt_end_label.Text = tools.getString("apt_end_label");

			((Control)txt_pictureBox).AllowDrop = true;
			txt_pictureBox.DragEnter += txt_pictureBox_DragEnter;
			txt_pictureBox.DragLeave += txt_pictureBox_DragLeave;
			txt_pictureBox.DragDrop += txt_pictureBox_DragDrop;
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

			updater.DoUpdate(true);
		}

		private void setTools()
		{
			// themeColor
			Color themeColor = styleManager.MetroColorParameters.BaseColor;

			// langCode
			String langCode;
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

			// set tools
			tools = new Tools(themeColor, langCode);
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
			hky = new HotKeys(tools);
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
			abt = new About(tools);
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

			String tooltip = aptTime.ToString() + " " + tools.getString("apt_tooltip");
			toolTip.SetToolTip(apt_trackBar, tooltip);
		}

		private void txt_pictureBox_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				e.Effect = DragDropEffects.All;
				overlay_cover.Show();
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void txt_pictureBox_DragLeave(object sender, EventArgs e)
		{
			overlay_cover.Hide();
		}

		private void txt_pictureBox_DragDrop(object sender, DragEventArgs e)
		{
			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length != 1)
			{
				MessageBoxEx.Show(this, tools.getString("only_single_file"));
			}
			else if (!files[0].ToLower().EndsWith(".txt"))
			{
				MessageBoxEx.Show(this, tools.getString("only_txt_file"));
			}
			else
			{
				MessageBoxEx.Show(files[0]);
			}
			overlay_cover.Hide();
		}
		
		private Image drawBackGroundImage()
		{
			Image img = new Bitmap(txt_pictureBox.Width, txt_pictureBox.Height);

			using (Graphics g = Graphics.FromImage(img))
			{
				using (Pen pen = new Pen(tools.color, 5))
				{
					pen.DashStyle = DashStyle.Dash;
					pen.DashPattern = new Single[] { 2f, 1.96f, 2f, 1.96f };

					g.DrawLine(pen, 0, 0, txt_pictureBox.Width, 0);
					g.DrawLine(pen, 0, 0, 0, txt_pictureBox.Height);
					g.DrawLine(pen, txt_pictureBox.Width, txt_pictureBox.Height - 1, 0, txt_pictureBox.Height - 1);
					g.DrawLine(pen, txt_pictureBox.Width - 1, txt_pictureBox.Height, txt_pictureBox.Width - 1, 0);
				}

				using (SolidBrush b = new SolidBrush(tools.color))
				{
					String s = tools.getString("pictureBox_string_1");
					Font f = new Font("Microsoft YaHei UI", 25, FontStyle.Bold);
					SizeF size = g.MeasureString(s, f);
					Single px = txt_pictureBox.Width / 2 - size.Width / 2;
					Single py = txt_pictureBox.Height / 2 - size.Height / 2 - 50;
					g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
					g.DrawString(s, f, b, px, py);

					s = tools.getString("pictureBox_string_2");
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

			openFileDialog.Title = tools.getString("openFileDialog_title");
			openFileDialog.Filter = tools.getString("openFileDialog_filter");
			
			
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				MessageBoxEx.Show(openFileDialog.FileName);
			}
			
			
			overlay_cover.Hide();
		}

	}
}

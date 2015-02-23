namespace TBReader2
{
	partial class MainForm
	{
		/// <summary>
		/// 必需的设计器变量。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 清理所有正在使用的资源。
		/// </summary>
		/// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows 窗体设计器生成的代码

		/// <summary>
		/// 设计器支持所需的方法 - 不要
		/// 使用代码编辑器修改此方法的内容。
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.txt_pictureBox = new System.Windows.Forms.PictureBox();
			this.apt_start_label = new DevComponents.DotNetBar.LabelX();
			this.apt_end_label = new DevComponents.DotNetBar.LabelX();
			this.apt_label = new DevComponents.DotNetBar.LabelX();
			this.toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.overlay_cover = new System.Windows.Forms.PictureBox();
			this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.styleManager = new DevComponents.DotNetBar.StyleManager(this.components);
			this.apt_trackBar = new TBReader2.NoFocusTrackBar();
			((System.ComponentModel.ISupportInitialize)(this.txt_pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.overlay_cover)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.apt_trackBar)).BeginInit();
			this.SuspendLayout();
			// 
			// txt_pictureBox
			// 
			this.txt_pictureBox.BackColor = System.Drawing.Color.White;
			this.txt_pictureBox.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.txt_pictureBox, "txt_pictureBox");
			this.txt_pictureBox.Name = "txt_pictureBox";
			this.txt_pictureBox.TabStop = false;
			this.txt_pictureBox.DoubleClick += new System.EventHandler(this.txt_pictureBox_DoubleClick);
			// 
			// apt_start_label
			// 
			this.apt_start_label.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.apt_start_label.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.apt_start_label.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.apt_start_label, "apt_start_label");
			this.apt_start_label.Name = "apt_start_label";
			this.apt_start_label.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.apt_start_label.TextAlignment = System.Drawing.StringAlignment.Center;
			this.apt_start_label.WordWrap = true;
			// 
			// apt_end_label
			// 
			this.apt_end_label.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.apt_end_label.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			this.apt_end_label.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.apt_end_label, "apt_end_label");
			this.apt_end_label.Name = "apt_end_label";
			this.apt_end_label.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.apt_end_label.TextAlignment = System.Drawing.StringAlignment.Center;
			this.apt_end_label.WordWrap = true;
			// 
			// apt_label
			// 
			this.apt_label.BackColor = System.Drawing.Color.White;
			// 
			// 
			// 
			this.apt_label.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.apt_label, "apt_label");
			this.apt_label.ForeColor = System.Drawing.Color.Black;
			this.apt_label.Name = "apt_label";
			this.apt_label.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.apt_label.TextAlignment = System.Drawing.StringAlignment.Center;
			this.apt_label.WordWrap = true;
			// 
			// timer
			// 
			this.timer.Interval = 1000;
			// 
			// overlay_cover
			// 
			this.overlay_cover.BackColor = System.Drawing.Color.White;
			this.overlay_cover.ForeColor = System.Drawing.Color.Black;
			resources.ApplyResources(this.overlay_cover, "overlay_cover");
			this.overlay_cover.Name = "overlay_cover";
			this.overlay_cover.TabStop = false;
			// 
			// styleManager
			// 
			this.styleManager.ManagerColorTint = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(143)))));
			this.styleManager.ManagerStyle = DevComponents.DotNetBar.eStyle.Metro;
			this.styleManager.MetroColorParameters = new DevComponents.DotNetBar.Metro.ColorTables.MetroColorGeneratorParameters(System.Drawing.Color.White, System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(120)))), ((int)(((byte)(143))))));
			// 
			// apt_trackBar
			// 
			this.apt_trackBar.BackColor = System.Drawing.Color.White;
			this.apt_trackBar.Cursor = System.Windows.Forms.Cursors.NoMoveHoriz;
			resources.ApplyResources(this.apt_trackBar, "apt_trackBar");
			this.apt_trackBar.Maximum = 30;
			this.apt_trackBar.Name = "apt_trackBar";
			this.apt_trackBar.Scroll += new System.EventHandler(this.apt_trackBar_Scroll);
			// 
			// MainForm
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Controls.Add(this.apt_end_label);
			this.Controls.Add(this.overlay_cover);
			this.Controls.Add(this.apt_label);
			this.Controls.Add(this.apt_start_label);
			this.Controls.Add(this.apt_trackBar);
			this.Controls.Add(this.txt_pictureBox);
			this.DoubleBuffered = true;
			this.ForeColor = System.Drawing.Color.Black;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.HelpButtonText = "Help";
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.apt_trackBar_Scroll);
			((System.ComponentModel.ISupportInitialize)(this.txt_pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.overlay_cover)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.apt_trackBar)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		protected internal System.Windows.Forms.PictureBox txt_pictureBox;
		private NoFocusTrackBar apt_trackBar;
		protected internal System.Windows.Forms.PictureBox overlay_cover;
		protected internal System.Windows.Forms.OpenFileDialog openFileDialog;
		protected internal DevComponents.DotNetBar.LabelX apt_start_label;
		protected internal DevComponents.DotNetBar.LabelX apt_end_label;
		protected internal DevComponents.DotNetBar.LabelX apt_label;
		protected internal System.Windows.Forms.ToolTip toolTip;
		protected internal System.Windows.Forms.Timer timer;
		private DevComponents.DotNetBar.StyleManager styleManager;
	}
}


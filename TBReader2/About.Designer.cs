namespace TBReader2
{
	partial class About
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
			this.about_pictureBox = new System.Windows.Forms.PictureBox();
			this.about_intro_label = new System.Windows.Forms.Label();
			this.about_email_label = new System.Windows.Forms.Label();
			this.about_author_label = new System.Windows.Forms.Label();
			this.about_version_label = new System.Windows.Forms.Label();
			this.about_name = new System.Windows.Forms.Label();
			this.about_version = new DevComponents.DotNetBar.LabelX();
			this.about_author = new DevComponents.DotNetBar.LabelX();
			this.about_email = new DevComponents.DotNetBar.LabelX();
			this.about_intro = new DevComponents.DotNetBar.LabelX();
			this.about_ok_button = new DevComponents.DotNetBar.ButtonX();
			((System.ComponentModel.ISupportInitialize)(this.about_pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// about_pictureBox
			// 
			resources.ApplyResources(this.about_pictureBox, "about_pictureBox");
			this.about_pictureBox.Name = "about_pictureBox";
			this.about_pictureBox.TabStop = false;
			// 
			// about_intro_label
			// 
			resources.ApplyResources(this.about_intro_label, "about_intro_label");
			this.about_intro_label.Name = "about_intro_label";
			// 
			// about_email_label
			// 
			resources.ApplyResources(this.about_email_label, "about_email_label");
			this.about_email_label.Name = "about_email_label";
			// 
			// about_author_label
			// 
			resources.ApplyResources(this.about_author_label, "about_author_label");
			this.about_author_label.Name = "about_author_label";
			// 
			// about_version_label
			// 
			resources.ApplyResources(this.about_version_label, "about_version_label");
			this.about_version_label.Name = "about_version_label";
			// 
			// about_name
			// 
			resources.ApplyResources(this.about_name, "about_name");
			this.about_name.Name = "about_name";
			// 
			// about_version
			// 
			// 
			// 
			// 
			this.about_version.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.about_version, "about_version");
			this.about_version.Name = "about_version";
			// 
			// about_author
			// 
			// 
			// 
			// 
			this.about_author.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.about_author, "about_author");
			this.about_author.Name = "about_author";
			// 
			// about_email
			// 
			// 
			// 
			// 
			this.about_email.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.about_email, "about_email");
			this.about_email.Name = "about_email";
			// 
			// about_intro
			// 
			// 
			// 
			// 
			this.about_intro.BackgroundStyle.CornerType = DevComponents.DotNetBar.eCornerType.Square;
			resources.ApplyResources(this.about_intro, "about_intro");
			this.about_intro.Name = "about_intro";
			// 
			// about_ok_button
			// 
			this.about_ok_button.AccessibleRole = System.Windows.Forms.AccessibleRole.PushButton;
			this.about_ok_button.ColorTable = DevComponents.DotNetBar.eButtonColor.OrangeWithBackground;
			resources.ApplyResources(this.about_ok_button, "about_ok_button");
			this.about_ok_button.Name = "about_ok_button";
			this.about_ok_button.Style = DevComponents.DotNetBar.eDotNetBarStyle.StyleManagerControlled;
			this.about_ok_button.Click += new System.EventHandler(this.ok_button_Click);
			// 
			// About
			// 
			resources.ApplyResources(this, "$this");
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.about_ok_button);
			this.Controls.Add(this.about_intro);
			this.Controls.Add(this.about_email);
			this.Controls.Add(this.about_author);
			this.Controls.Add(this.about_version);
			this.Controls.Add(this.about_intro_label);
			this.Controls.Add(this.about_email_label);
			this.Controls.Add(this.about_author_label);
			this.Controls.Add(this.about_version_label);
			this.Controls.Add(this.about_name);
			this.Controls.Add(this.about_pictureBox);
			this.DoubleBuffered = true;
			this.Name = "About";
			this.SlideOutButtonVisible = false;
			((System.ComponentModel.ISupportInitialize)(this.about_pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox about_pictureBox;
		private System.Windows.Forms.Label about_intro_label;
		private System.Windows.Forms.Label about_email_label;
		private System.Windows.Forms.Label about_author_label;
		private System.Windows.Forms.Label about_version_label;
		private System.Windows.Forms.Label about_name;
		private DevComponents.DotNetBar.LabelX about_version;
		private DevComponents.DotNetBar.LabelX about_author;
		private DevComponents.DotNetBar.LabelX about_email;
		private DevComponents.DotNetBar.LabelX about_intro;
		private DevComponents.DotNetBar.ButtonX about_ok_button;
	}
}
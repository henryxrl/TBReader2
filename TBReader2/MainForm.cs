using DevComponents.DotNetBar;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;
using AutoUpdate;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms.VisualStyles;
using System.IO;
using System.Linq;

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

		#region Window-related variables

		// Global hot key
		private KeyboardHook hook = new KeyboardHook();
		[DllImport("user32.dll")]
		private static extern Boolean UnregisterHotKey(IntPtr hWnd, Int32 id);

		// Get foreground window and size
		[DllImport("user32.dll")]
		private static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern Boolean GetWindowRect(IntPtr hWnd, out RECT lpRect);
		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public Int32 Left;        // x position of upper-left corner  
			public Int32 Top;         // y position of upper-left corner  
			public Int32 Right;       // x position of lower-right corner  
			public Int32 Bottom;      // y position of lower-right corner  
		}

		// Get/set window title text
		[DllImport("user32.dll")]
		private static extern Int32 GetWindowText(IntPtr hWnd, StringBuilder text, Int32 count);
		[DllImport("user32.dll")]
		private static extern Int32 SetWindowText(Int32 hWnd, StringBuilder text);

		// Other window event
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetWinEventHook(UInt32 eventMin, UInt32 eventMax, IntPtr hmodWinEventProc, WinEventProc lpfnWinEventProc, Int32 idProcess, Int32 idThread, UInt32 dwflags);
		[DllImport("user32.dll")]
		private static extern Int32 UnhookWinEvent(IntPtr hWinEventHook);
		private delegate void WinEventProc(IntPtr hWinEventHook, UInt32 iEvent, IntPtr hWnd, Int32 idObject, Int32 idChild, Int32 dwEventThread, Int32 dwmsEventTime);
		private const UInt32 WINEVENT_OUTOFCONTEXT = 0;

		// Other window resize
		private const UInt32 EVENT_SYSTEM_MOVESIZEEND = 0x000B;
		private WinEventProc resize_listener;
		private IntPtr resize_winHook;

		// Other window switch event
		private const UInt32 EVENT_SYSTEM_FOREGROUND = 0x0003;
		private const UInt32 EVENT_SYSTEM_SWITCHEND = 0x0015;
		private WinEventProc switch_listener;
		private IntPtr switch_winHook;

		#endregion

		private Int32 window = 0;
		private Boolean isOriginalTitle = true;
		private Int32 dim_w = 0;		// Remember the width of the active window
		private Int32 displayWidth = 0;		// Actual length for text display

		private Tools tools = null;
		private About abt = null;
		private HotKeys hky = null;

		// Auto page turn
		private Int32 aptTime = 0;
		private Int32 timerCount;
		private Int32 timerFlag = -1;	// 0: auto read forward; 1: auto read backward; 2: go back to current; -1: default

		private String txt_URL;
		private String[] txt_book;

		private String curTitle;		// Current window's original title
		private String curLineText;		// Current text shown in windows' title bar

		private Int32 totalLineNum;		// 1-based. Total line number
		private Int32 curLineNum;		// 1-based. Need to minus 1 to get current line index
		private Int32 lineOffset;		// 0-based. character index of a line








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

			contextMenuStrip.Items.Add(tools.getString("exit"), null, item_Click);

			updater = new AutoUpdater(this);

			#region Register HotKeys
			// register the event that is fired after the key press.
			hook.KeyPressed += new EventHandler<KeyPressedEventArgs>(hook_KeyPressed);

			try
			{
				// ctrl+shift+Q = Quit
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Q);

				// ctrl+shift+right = next line
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Right);

				// ctrl+shift+left = prev line
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Left);

				// ctrl+shift+up = bookmark cur loc
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Up);

				// ctrl+shift+down = jump to bookmark
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Down);

				// ctrl+shift+delete = delete current bookmark
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Delete);

				// ctrl+shift+r = toggle to default title text/cur condition
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.R);

				// ctrl+shift+space = toggle hide/show
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.Space);

				// ctrl+shift+p = jump to previous location (only under bookmark view)
				hook.RegisterHotKey(global::ModifierKeys.Control | global::ModifierKeys.Shift, Keys.P);
			}
			catch
			{
				MessageBoxEx.Show(tools.getString("hotkey_registration_failed"));
			}
			#endregion
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

			StartListeningForWindowChanges();
		}

		#region UI Setup
		
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
				bookName_label.SendToBack();
				overlay_cover.Show();
			}
			else
				e.Effect = DragDropEffects.None;
		}

		private void txt_pictureBox_DragLeave(object sender, EventArgs e)
		{
			overlay_cover.Hide();
			bookName_label.BringToFront();
		}

		private void txt_pictureBox_DragDrop(object sender, DragEventArgs e)
		{
			String[] files = (String[])e.Data.GetData(DataFormats.FileDrop);
			if (files.Length != 1)
			{
				e.Effect = DragDropEffects.None;
				MessageBoxEx.Show(this, tools.getString("only_single_file"));
				overlay_cover.Hide();
				bookName_label.BringToFront();
				return;
			}
			else if (!files[0].ToLower().EndsWith(".txt"))
			{
				e.Effect = DragDropEffects.None;
				MessageBoxEx.Show(this, tools.getString("only_txt_file"));
				overlay_cover.Hide();
				bookName_label.BringToFront();
				return;
			}
			else
			{
				e.Effect = DragDropEffects.All;
				txt_URL = files[0];
				//MessageBoxEx.Show(txt_URL);
				processBook();
				overlay_cover.Hide();
				bookName_label.BringToFront();
				txt_pictureBox.BackgroundImage = drawBackGroundImage();
				return;
			}
		}

		private void txt_pictureBox_DoubleClick(object sender, EventArgs e)
		{
			bookName_label.SendToBack();
			overlay_cover.Show();

			openFileDialog.Title = tools.getString("openFileDialog_title");
			openFileDialog.Filter = tools.getString("openFileDialog_filter");
			
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				txt_URL = openFileDialog.FileName;
				//MessageBoxEx.Show(txt_URL);
				processBook();
				overlay_cover.Hide();
				bookName_label.BringToFront();
				txt_pictureBox.BackgroundImage = drawBackGroundImage();
				return;
			}
			
			overlay_cover.Hide();
			bookName_label.BringToFront();
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

			if (txt_URL != null)
			{
				bookName_label.Text = tools.getString("pictureBox_string_3")
										+ System.IO.Path.GetFileNameWithoutExtension(txt_URL);
			}

			return img;
		}

		private void item_Click(object sender, EventArgs e)
		{
			quitTBReader();
		}

		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			hideShow();
		}

		#endregion

		private void hook_KeyPressed(object sender, KeyPressedEventArgs e)
		{
			// show the keys pressed in a label.
			String key = e.Key.ToString();
			//String keyComb = e.Modifier.ToString() + " + " + key;
			//MessageBoxEx.Show(keyComb);

			switch (key)
			{
				case "Up":
					//StartListeningForWindowChanges();
					//StartListeningForWindowSwitch();
					//MessageBoxEx.Show("title: " + GetActiveWindowTitle());
					//checkOpenTXT();
					//addBookmark();
					break;
				case "Down":
					//MessageBoxEx.Show(TruncateAtWord("hey dude, how are you? I'm fine thank you!", 10));
					//MessageBoxEx.Show("font size: " + SystemFonts.CaptionFont.Size + "\nfont size in point: " + SystemFonts.CaptionFont.SizeInPoints);
					MessageBoxEx.Show(TruncatePixelLength("hey dudeee, how are you? I'm fine thank you!", 0, 100));
					MessageBoxEx.Show(TruncatePixelLength("hey dudeee, 比比比比? I'm fine 你妹啊比 you!", 0, 100));
					MessageBoxEx.Show(TruncatePixelLength("我草拟大爷的比比比比比吧！啊啊啊你妹啊比！阿比阿鼻！！！！", 0, 100));
					//checkOpenTXT();
					//jumpToBookmarks();
					break;
				case "Left":
					//checkOpenTXT();
					//readBackward();
					break;
				case "Right":
					//checkOpenTXT();
					//readForward();
					if (txt_URL != null)
					{
						readForward();
					}
					break;
				case "Q":
					quitTBReader();
					break;
				case "R":
					toggleTitleText();
					break;
				case "Space":
					hideShow();
					break;
				case "P":
					//jumpFromBookmarkToCurLoc();
					break;
				case "Delete":
					//deleteCurBookmakr();
					break;
				default:
					MessageBoxEx.Show(tools.getString("hotkey_invalid_pressed"));
					break;
			}
		}






		private String GetActiveWindowTitle()
		{
			const Int32 nChars = 256;
			StringBuilder Buff = new StringBuilder(nChars);
			IntPtr handle = GetForegroundWindow();

			if (GetWindowText(handle, Buff, nChars) > 0)
			{
				return Buff.ToString();
			}

			return null;
		}





		#region Other Window Event

		private void StartListeningForWindowChanges()
		{
			// Resize event
			resize_listener = new WinEventProc(resize_EventCallback);
			//setting the window hook
			resize_winHook = SetWinEventHook(EVENT_SYSTEM_MOVESIZEEND, EVENT_SYSTEM_MOVESIZEEND, IntPtr.Zero, resize_listener, 0, 0, WINEVENT_OUTOFCONTEXT);

			// Switch event
			switch_listener = new WinEventProc(switch_EventCallback);
			//setting the window hook
			switch_winHook = SetWinEventHook(EVENT_SYSTEM_FOREGROUND, EVENT_SYSTEM_FOREGROUND, IntPtr.Zero, switch_listener, 0, 0, WINEVENT_OUTOFCONTEXT);
		}

		private void StopListeningForWindowChanges()
		{
			UnhookWinEvent(resize_winHook);
			UnhookWinEvent(switch_winHook);
		}

		private void resize_EventCallback(IntPtr hWinEventHook, UInt32 iEvent, IntPtr hWnd, Int32 idObject, Int32 idChild, Int32 dwEventThread, Int32 dwmsEventTime)
		{
			getActiveWindowDisplayWidth();
			MessageBoxEx.Show("width: " + displayWidth);
		}

		private void switch_EventCallback(IntPtr hWinEventHook, UInt32 iEvent, IntPtr hWnd, Int32 idObject, Int32 idChild, Int32 dwEventThread, Int32 dwmsEventTime)
		{
			restorePrevTitle();
			curTitle = GetActiveWindowTitle();
			window = GetForegroundWindow().ToInt32();
			if (!isOriginalTitle && curLineText != null)
			{
				setCurTitleText(curLineText);
				isOriginalTitle = false;
			}
		}

		private void getActiveWindowDisplayWidth()
		{
			try
			{
				RECT dim = new RECT();
				GetWindowRect(GetForegroundWindow(), out dim);
				if (Width != dim.Right - dim.Left || Height != dim.Bottom - dim.Top)
				{
					dim_w = dim.Right - dim.Left;

					/*
					//MessageBoxEx.Show("Width: " + dim_w);
					Int32 AllButtonsAndPadding = GetWindowsMiscElementsSize();
					//MessageBoxEx.Show("windows misc elements size: " + AllButtonsAndPadding);
					Int32 realSize = dim_w - AllButtonsAndPadding;
					//MessageBoxEx.Show("real size for display text: " + realSize);
					Single titleFontSize = SystemFonts.CaptionFont.Size;
					//MessageBoxEx.Show("title font size: " + titleFontSize);
					Single pixelPerChar;
					using (Graphics g = this.CreateGraphics())
					{
						pixelPerChar = titleFontSize * g.DpiX / 72;
					}
					//MessageBoxEx.Show("title font width in px: " + pixelPerChar);
					Int32 numCharsInCurTitle = (Int32)Math.Floor((Double)realSize / (Double)pixelPerChar);
					//MessageBoxEx.Show("cur title can fit: " + numCharsInCurTitle + " words");

					result = (dim_w - 200) / 2;
					if (result < 0) result = 0;
					*/

					displayWidth = dim_w - GetWindowsMiscElementsSize();
				}
			}
			catch
			{
				MessageBoxEx.Show(tools.getString("window_get_dim_failed"));
				displayWidth = -1;
			}
		}

		VisualStyleRenderer renderer = null;
		//This gets the size of the X and the border of the form
		private Int32 GetWindowsMiscElementsSize()
		{
			Int32 result = 0;
			
			using (Graphics g = this.CreateGraphics())
			{
				// Get the size of the close button.
				Int32 closeSize = 0;
				if (SetRenderer(VisualStyleElement.Window.CloseButton.Normal))
				{
					closeSize = renderer.GetPartSize(g, ThemeSizeType.True).Width;
					result += closeSize;
				}
				
				// Get the size of the minimize button.
				if (SetRenderer(VisualStyleElement.Window.MinButton.Normal))
				{
					//Int32 minSize = renderer.GetPartSize(g, ThemeSizeType.True).Width;		// use close button size
					result += closeSize;
				}

				// Get the size of the maximize button.
				if (SetRenderer(VisualStyleElement.Window.MaxButton.Normal))
				{
					//Int32 maxSize = renderer.GetPartSize(g, ThemeSizeType.True).Width;		// use close button size
					result += closeSize;
				}

				// Get the size of the icon.
				if (this.ShowIcon)
				{
					Int32 iconSize = this.Icon.Width;
					result += iconSize;
				}

				// Get the thickness of the left, bottom, 
				// and right window frame.
				if (SetRenderer(VisualStyleElement.Window.FrameLeft.Active))
				{
					Int32 frameSize = (renderer.GetPartSize(g, ThemeSizeType.True).Width) * 2; //Borders on both side
					result += frameSize * 4;	// Just to be safe...
				}
			}

			return result;
		}

		// Set the VisualStyleRenderer to a new element.
		private bool SetRenderer(VisualStyleElement element)
		{
			if (!VisualStyleRenderer.IsElementDefined(element))
			{
				return false;
			}

			if (renderer == null)
			{
				renderer = new VisualStyleRenderer(element);
			}
			else
			{
				renderer.SetParameters(element);
			}

			return true;
		}

		#endregion






		private void toggleTitleText()
		{
			String tempTitle = GetActiveWindowTitle();
			if (curTitle != null && curLineText != null)
			{
				if (tempTitle.CompareTo(curTitle) != 0)
				{
					setCurTitleText(curTitle);
					isOriginalTitle = true;
				}
				else
				{
					setCurTitleText(curLineText);
					isOriginalTitle = false;
				}
			}
		}

		private void readForward()
		{
			if (lineOffset != 0)
				lineOffset++;
			else curLineNum++;
			jumpToLine();

		}

		private void restorePrevTitle()
		{
			if (curTitle != null)
			{
				setCurTitleText(curTitle);
			}
		}

		private void jumpToLine()
		{
			window = GetForegroundWindow().ToInt32();
			
			Double progress = (Double)curLineNum / (Double)totalLineNum * 100;
			String pre = String.Format("({0:0.0}%) ", progress);
			String line = txt_book[curLineNum-1].Trim();
			curLineText = pre + line;
			setCurTitleText(curLineText);
			isOriginalTitle = false;
		}

		private void setCurTitleText(String s)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(s);
			SetWindowText(window, sb);
			sb.Clear();
		}








		private void processBook()
		{
			txt_book = File.ReadAllLines(txt_URL, System.Text.Encoding.Default).Where(arg => !String.IsNullOrWhiteSpace(arg)).ToArray();
			totalLineNum = txt_book.Length;
			curLineNum = 0;
			lineOffset = 0;
		}

		private String TruncateAtWord(String input, Int32 charNum)
		{
			if (input == null || input.Length < charNum)
				return input;

			Int32 iNextSpace = input.LastIndexOf(" ", charNum);
			return String.Format("{0}...", input.Substring(0, (iNextSpace > 0) ? iNextSpace : charNum).Trim());
		}

		private String TruncatePixelLength(String input, Int32 startIdx, Int32 length)
		{
			
			//return TextTruncator.TruncateText(input, length, SystemFonts.CaptionFont);

			Int32 newLength = length - TextRenderer.MeasureText("...", SystemFonts.CaptionFont).Width;
			Int32 tempLength = 0;
			Int32 idx = 0;
			for (; idx < input.Length && tempLength < newLength; idx++)
			{
				tempLength += TextRenderer.MeasureText(input[idx].ToString(), SystemFonts.CaptionFont).Width;
			}
			return input.Substring(0, idx) + "...";
		}

		private void hideShow()
		{
			if (this.Visible == true) this.Hide();
			else this.Show();
		}

		private void quitTBReader()
		{
			/*
			saveCurrentLoc();

			if (defaultTitleText.ContainsKey(window))
			{
				restorePrevWindowTitle();
			}
			*/
			restorePrevTitle();
			StopListeningForWindowChanges();
			Application.Exit();
		}

		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			//if (e.CloseReason == CloseReason.WindowsShutDown) return;

			quitTBReader();
		}

	}
}

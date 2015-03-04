using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace TBReader2
{
	public static class TextTruncator
	{
		// Private Properties
		//

		private static Dictionary<String, Int32[]> _fontWidthDic;
		private static Dictionary<String, Int32[]> FontWidthDic
		{
			get
			{
				if (_fontWidthDic == null)
				{
					_fontWidthDic = new Dictionary<String, Int32[]>();
				}
				return _fontWidthDic;
			}
		}

		//
		// Public Methods
		//

		public static String TruncateText(String text, Int32 textMaxWidth, Font font)
		{
			return TruncateText(text, textMaxWidth, font.Name, (Int32)font.Size, false);
		}

		public static String TruncateText(String text, Int32 textMaxWidth, String fontName, Int32 fontSizeInPixels)
		{
			return TruncateText(text, textMaxWidth, fontName, fontSizeInPixels, false);
		}

		public static String TruncateText(String text, Int32 textMaxWidth, String fontName, Int32 fontSizeInPixels, Boolean isFontBold)
		{
			if (String.IsNullOrEmpty(text))
				return text;

			// Check
			//
			if (textMaxWidth < 1 || String.IsNullOrEmpty(fontName) || fontSizeInPixels < 1)
			{
				throw new ArgumentException();
			}

			Int32[] fontWidthArray = GetFontWidthArray(fontName, fontSizeInPixels, isFontBold);
			Int32 ellipsisWidth = fontWidthArray['.'] * 3;
			Int32 totalCharCount = text.Length;
			Int32 textWidth = 0;
			Int32 charIndex = 0;
			for (Int32 i = 0; i < totalCharCount; i++)
			{
				//MessageBox.Show("i: " + i);
				//MessageBox.Show("text[i]: " + text[i]);
				//MessageBox.Show("fontWidthArray[text[i]]: " + fontWidthArray[text[i]]);
				try
				{
					MessageBox.Show(text[i] + "'s width: " + fontWidthArray[text[i]]);
					Int32 test = TextRenderer.MeasureText(text[i].ToString(), SystemFonts.CaptionFont).Width;
					MessageBox.Show(text[i] + "'s width: " + test);
					textWidth += test;
				}
				catch
				{
					Int32 test = TextRenderer.MeasureText(text[i].ToString(), SystemFonts.CaptionFont).Width;
					MessageBox.Show(text[i] + "'s width: " + test);
					textWidth += test;
				}
				if (textWidth > textMaxWidth)
				{
					return text.Substring(0, charIndex) + "...";
				}
				else if (textWidth + ellipsisWidth <= textMaxWidth)
				{
					charIndex = i;
				}
			}
			return text;
		}

		//
		// Private Methods
		//

		private static Int32[] GetFontWidthArray(String fontName, Int32 fontSizeInPixels, Boolean isFontBold)
		{
			String fontEntryName = fontName.ToLower() + "_" + fontSizeInPixels.ToString() + "px" + (isFontBold ? "_bold" : "");
			Int32[] fontWidthArray;
			if (!FontWidthDic.TryGetValue(fontEntryName, out fontWidthArray))
			{
				fontWidthArray = CreateFontWidthArray(new Font(fontName,
					fontSizeInPixels, isFontBold ? FontStyle.Bold :
					FontStyle.Regular, GraphicsUnit.Pixel));
				FontWidthDic[fontEntryName] = fontWidthArray;
			}

			return fontWidthArray;
		}

		private static Int32[] CreateFontWidthArray(Font font)
		{
			Int32[] fontWidthArray = new Int32[256];
			for (Int32 i = 32; i < 256; i++)
			{
				Char c = (Char)i;
				fontWidthArray[i] =
					IsIllegalCharacter(c, false) ? 0 : GetCharWidth(c, font);
			}
			return fontWidthArray;
		}

		private static Int32 GetCharWidth(Char c, Font font)
		{
			// Note1: For typography related reasons,
			// TextRenderer.MeasureText() doesn't return the correct
			// width of the character in pixels, hence the need
			// to use this hack (with the '<' & '>'
			// characters and the subtraction). Note that <'
			// and '>' were chosen randomly, other characters 
			// can be used.
			//

			// Note2: As the TextRenderer class is intended
			// to be used with Windows Forms Applications, it has a 
			// special use for the ampersand character (used for Mnemonics).
			// Therefore, we need to check for the 
			// ampersand character and replace it with '&&'
			// to escape it (TextRenderer.MeasureText() will treat 
			// it as one ampersand character)
			//

			return
				TextRenderer.MeasureText("<" + (c == '&' ? "&&" :
											c.ToString()) + ">", font).Width -
				TextRenderer.MeasureText("<>", font).Width;
		}

		private static Boolean ContainsIllegalCharacters(String text,
							Boolean excludeLineBreaks)
		{
			if (!String.IsNullOrEmpty(text))
			{
				foreach (Char c in text)
				{
					if (IsIllegalCharacter(c, excludeLineBreaks))
						return true;
				}
			}

			return false;
		}

		private static Boolean IsIllegalCharacter(Char c, Boolean excludeLineBreaks)
		{
			// See the Windows-1252 encoding
			// (we use ISO-8859-1, but all browsers, or at least
			// IE, FF, Opera, Chrome and Safari,
			// interpret ISO-8859-1 as Windows-1252).
			// For more information,
			// see http://en.wikipedia.org/wiki/ISO/
			//        IEC_8859-1#ISO-8859-1_and_Windows-1252_confusion
			//

			return
				(c < 32 && (!excludeLineBreaks || c != '\n')) ||
				c > 255 ||
				c == 127 ||
				c == 129 ||
				c == 141 ||
				c == 143 ||
				c == 144 ||
				c == 157;
		}
	}
}

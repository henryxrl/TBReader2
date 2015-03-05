using Ini;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TBReader2
{
	public class Tools
	{
		Color themeColor;
		String langCode;

		public Tools(Color c, String s)
		{
			themeColor = c;
			langCode = s;
		}

		public String lang
		{
			get { return langCode; }
		}

		public Color color
		{
			get { return themeColor; }
		}

		public Image img
		{
			get { return TBReader2.Properties.Resources.clock; }
		}

		public String getString(String input)
		{
			return TBReader2.Properties.Resources.ResourceManager.GetString(langCode + input);
		}

		public Boolean writeCurLoc(IniFile ini, String fileName, Int32 lineNum)
		{
			try
			{
				ini.IniWriteValue(fileName, "Cur", lineNum.ToString());
				return true;
			}
			catch
			{
				return false;
			}
		}

		public Int32 loadCurLoc(IniFile ini, String fileName)
		{
			try
			{
				return Convert.ToInt32(ini.IniReadValue(fileName, "Cur"));
			}
			catch
			{
				return -1;
			}
		}
	}
}

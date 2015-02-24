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
	}
}

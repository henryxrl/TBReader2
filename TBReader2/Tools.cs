using Ini;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TBReader2
{
	public class Tools
	{
		private static String iniPath = Application.StartupPath + "\\bookmarks.ini";
		private IniFile ini = new IniFile(iniPath);

		Color themeColor;
		String langCode;
		String bookName;
		Int32 totalLineNum;

		public Tools(Color c, String s)
		{
			themeColor = c;
			langCode = s;
		}

		public String Name
		{
			set { bookName = value; }
		}

		public Int32 LineNum
		{
			set { totalLineNum = value; }
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

		public Boolean writeCurLoc(Int32 lineNum)
		{
			try
			{
				ini.IniWriteValue(bookName, "Cur", lineNum.ToString());
				return true;
			}
			catch
			{
				return false;
			}
		}

		public Int32 loadCurLoc()
		{
			try
			{
				Int32 prog = Convert.ToInt32(ini.IniReadValue(bookName, "Cur"));
				if (prog > -1 && prog <= totalLineNum)
				{
					return prog;
				}
				else
				{
					return 0;
				}
			}
			catch
			{
				return -1;
			}
		}

		public Boolean writeBookMark(Int32 lineNum, Int32 offset)
		{
			List<Tuple<Int32, Int32>> bookmarks = loadBookMarks();

			if (bookmarks == null)
				bookmarks = new List<Tuple<Int32, Int32>>();

			if (bookmarks.Contains(new Tuple<Int32, Int32>(lineNum, offset)))
				return false;
			else
			{
				Int32 bookmark_idx = bookmarks.Count;
				return writeBookMarkHelper(bookmark_idx, lineNum, offset);
			}
		}

		private Boolean writeBookMarkHelper(Int32 bookmark_idx, Int32 lineNum, Int32 offset)
		{
			try
			{
				ini.IniWriteValue(bookName, "Loc" + bookmark_idx, lineNum + "," + offset);
				return true;
			}
			catch
			{
				return false;
			}
		}

		public List<Tuple<Int32, Int32>> loadBookMarks()
		{
			try
			{
				ini.IniReadValue(bookName, "Loc0");
			}
			catch
			{
				return null;
			}
			
			List<Tuple<Int32, Int32>> result = new List<Tuple<Int32, Int32>>();
			Int32 i = 0;
			while (true)
			{
				try
				{
					String s = ini.IniReadValue(bookName, "Loc" + i.ToString());
					
					Int32 comma_idx_1 = s.IndexOf(',');
					Int32 lineNum = Convert.ToInt32(s.Substring(0, comma_idx_1));
					Int32 offset = Convert.ToInt32(s.Substring(comma_idx_1 + 1));

					if (lineNum > 0 && lineNum <= totalLineNum)
					{
						result.Add(new Tuple<Int32, Int32>(lineNum, offset));
						i++;
					}
					else
					{
						deleteBookMark(lineNum, offset, i);
					}
				}
				catch
				{
					break;
				}
			}
			return result;
		}

		public Boolean deleteBookMark(Int32 lineNum, Int32 offset)
		{
			try
			{
				deleteBookMarkHelper(ini.path, findLineToDelete(lineNum, offset));
				return true;
			}
			catch
			{
				return false;
			}
		}

		private Boolean deleteBookMark(Int32 lineNum, Int32 offset, Int32 idx)
		{
			try
			{
				deleteBookMarkHelper(ini.path, "Loc" + idx + "=" + lineNum + "," + offset);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void deleteBookMarkHelper(String path, String line_to_delete)
		{
			String[] lines = File.ReadAllLines(path, Encoding.Default);
			String[] newLines = new String[lines.Count() - 1];

			Int32 delete_idx = -1;
			Boolean stopModify = false;
			for (Int32 i = 0; i < lines.Count(); i++)
			{
				String curLine = lines[i];

				if (String.Compare(curLine, line_to_delete) == 0)
				{
					delete_idx = i;
				}
				else
				{
					if (delete_idx == -1)
					{
						newLines[i] = curLine;
					}
					else if (delete_idx > -1 && i > delete_idx)
					{
						if (curLine.StartsWith("Loc") && !stopModify)
						{
							Int32 numStart_idx = 3;
							Int32 numEnd_idx = curLine.IndexOf("=");
							Int32 curLocNum = Convert.ToInt32(curLine.Substring(numStart_idx, numEnd_idx - numStart_idx));
							Int32 newLocNum = curLocNum - 1;
							newLines[i - 1] = "Loc" + newLocNum + curLine.Substring(numEnd_idx);
						}
						else
						{
							if (curLine.StartsWith("["))		// Another book
							{
								stopModify = true;
							}
							newLines[i - 1] = curLine;
						}
					}
				}
			}

			File.WriteAllLines(path, newLines, Encoding.Default);
		}

		private String findLineToDelete(Int32 lineNum, Int32 offset)
		{
			List<Tuple<Int32, Int32>> bookmarks = loadBookMarks();
			Int32 index = bookmarks.IndexOf(new Tuple<Int32, Int32>(lineNum, offset));
			return "Loc" + index + "=" + lineNum + "," + offset;
		}

	}
}

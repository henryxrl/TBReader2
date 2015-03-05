using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ini
{
	/// <summary>
	/// Create a New INI file to store or load data
	/// </summary>
	public class IniFile
	{
		public String path;

		[DllImport("kernel32")]
		private static extern Int64 WritePrivateProfileString(String section, String key, String val, String filePath);
		[DllImport("kernel32")]
		private static extern Int32 GetPrivateProfileString(String section, String key, String def, StringBuilder retVal, Int32 size, String filePath);

		/// <summary>
		/// INIFile Constructor.
		/// </summary>
		/// <param name="INIPath"></param>
		public IniFile(String INIPath)
		{
			path = INIPath;
		}
		/// <summary>
		/// Write Data to the INI File
		/// </summary>
		/// <param name="Section"></param>
		/// Section name
		/// <param name="Key"></param>
		/// Key Name
		/// <param name="Value"></param>
		/// Value Name
		public void IniWriteValue(String Section, String Key, String Value)
		{
			WritePrivateProfileString(Section, Key, Value, this.path);
		}
		
		/// <summary>
		/// Read Data Value From the Ini File
		/// </summary>
		/// <param name="Section"></param>
		/// <param name="Key"></param>
		/// <param name="Path"></param>
		/// <returns></returns>
		public String IniReadValue(String Section, String Key)
		{
			StringBuilder temp = new StringBuilder(255);
			Int32 i = GetPrivateProfileString(Section, Key, "", temp, 255, this.path);
			return temp.ToString();

		}
	}
}

using System;
using System.IO;

namespace TicTacToe
{
	public class FileHelper
	{
		public static void CreateUserDataFile()
		{
			string dataDirectory = $"{Directory.GetCurrentDirectory()}/data";
			if (!Directory.Exists(dataDirectory))
			{
				Directory.CreateDirectory(dataDirectory);
			}

			string filepath = $"{dataDirectory}/users";
			if (!File.Exists(filepath))
			{
				FileStream stream = File.Create(filepath, 1);
				stream.Close();
			}
		}
	}
}

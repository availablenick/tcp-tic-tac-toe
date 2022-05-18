using System;
using System.IO;

namespace TicTacToe
{
	public class FileHelper
	{
		public static int CreateUserDataFile()
		{
			string dataDirectory = $"{Directory.GetCurrentDirectory()}/data";
			if (!Directory.Exists(dataDirectory))
			{
				try
				{
					Directory.CreateDirectory(dataDirectory);
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("Could not create data directory: insufficient permissions");
					return 1;
				}
			}

			if (!File.Exists($"{dataDirectory}/users"))
			{
				try
				{
					string filepath = $"{dataDirectory}/users";
					FileStream stream = File.Create(filepath, 1);
					stream.Close();
				}
				catch (UnauthorizedAccessException)
				{
					Console.WriteLine("Could not create user account file: insufficient permissions");
					return 1;
				}
			}

			return 0;
		}
	}
}

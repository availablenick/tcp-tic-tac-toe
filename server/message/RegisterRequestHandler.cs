using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class RegisterRequestHandler : IMessageHandler
	{
		private string _data;
		private Mutex _mutex;

		public RegisterRequestHandler(string data, Mutex mutex)
		{
			this._data = data;
			this._mutex = mutex;
		}

		public string HandleMessage()
		{
			string[] data = this._data.Split(';');
			if (data.Length != 2)
			{
				return "resregister 2\n";
			}

			string username = data[0];
			string password = data[1];
			string filepath = $"{Directory.GetCurrentDirectory()}/data/users";
			int statusCode = 0;
			this._mutex.WaitOne();

			if (UsernameAlreadyExists(filepath, username))
			{
				statusCode = 1;
			}
			else
			{
				AddUserEntry(filepath, username, password);
			}

			this._mutex.ReleaseMutex();
			return $"resregister {statusCode}\n";
		}

		private bool UsernameAlreadyExists(string filepath, string username)
		{
			foreach (string line in File.ReadLines(filepath))
			{
				string entryUsername = line.Split(" ")[0];
				if (username == entryUsername)
				{
					return true;
				}
			}

			return false;
		}

		private void AddUserEntry(string filepath, string username, string password)
		{
			using var stream = File.Open(filepath, FileMode.Open, FileAccess.Write);
			Byte[] buffer = new Byte[256];
			string newLine = $"{username} {password}\n";
			BufferHelper.WriteMessageToBuffer(buffer, newLine);
			stream.Seek(0, SeekOrigin.End);
			stream.Write(buffer, 0, newLine.Length);
		}
	}
}

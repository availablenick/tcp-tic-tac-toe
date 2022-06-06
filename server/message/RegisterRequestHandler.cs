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
			this._mutex.WaitOne();
			int statusCode = AddUser(username, password);
			this._mutex.ReleaseMutex();

			return $"resregister {statusCode}\n";
		}

		private int AddUser(string username, string password)
		{
			string filepath = $"{Directory.GetCurrentDirectory()}/data/users";
			using (var stream = File.Open(filepath, FileMode.Open, FileAccess.ReadWrite))
			{
				Byte[] line = new Byte[256];
				int i = 0;
				int nextByte;
				while ((nextByte = stream.ReadByte()) != -1)
				{
					byte character = (byte) nextByte;
					if (character == ' ')
					{
						string lineUsername = BufferHelper.GetBufferMessage(line, i);
						if (username == lineUsername)
						{
							return 1;
						}
					}

					line[i++] = character;
					if (TextHelper.IsLineBreak((char) character))
					{
						i = 0;
					}
				}

				Byte[] buffer = new Byte[256];
				string newLine = $"{username} {password}\n";
				BufferHelper.WriteMessageToBuffer(buffer, newLine);
				stream.Write(buffer, 0, newLine.Length);
				return 0;
			}
		}
	}
}

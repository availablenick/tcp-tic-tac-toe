using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class RegisterRequest : Request
	{
		public const int NumberOfParameters = 1;

		public RegisterRequest(params string[] parameters) : base(parameters) { }

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

		public override string Fulfill(Socket clientSocket, Mutex mutex,
			Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername)
		{
			string[] data = this.Data.Split(';', StringSplitOptions.RemoveEmptyEntries);
			if (data.Length != 2)
			{
				return "register 2";
			}

			string username = data[0];
			string password = data[1];
			mutex.WaitOne();
			int statusCode = AddUser(username, password);
			mutex.ReleaseMutex();

			return $"register {statusCode}";
		}
	}
}

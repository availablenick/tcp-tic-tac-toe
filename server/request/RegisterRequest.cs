using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class RegisterRequest : Request
	{
		public const int NumberOfParameters = 2;

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

		public override string Fulfill(Socket clientSocket, Mutex mutex)
		{
			mutex.WaitOne();
			int statusCode = AddUser(this.Parameters[0], this.Parameters[1]);
			mutex.ReleaseMutex();

			return $"register {statusCode}";
		}
	}
}

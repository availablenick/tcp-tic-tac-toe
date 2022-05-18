using System;
using System.IO;
using System.Net;

namespace TicTacToe.Server
{
	public class LoginRequest : Request
	{
		public const int NumberOfParameters = 2;
		public LoginRequest(params string[] parameters) : base(parameters) { }
		private int AuthenticateUser(string username, string password)
		{
			string filepath = $"{Directory.GetCurrentDirectory()}/data/users";
			using (var stream = File.Open(filepath, FileMode.Open, FileAccess.Read))
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
							i = 0;
							while (true)
							{
								nextByte = stream.ReadByte();
								character = (byte) nextByte;
								if (TextHelper.IsLineBreak((char) character))
								{
									string linePassword = BufferHelper.GetBufferMessage(line, i);
									if (password == linePassword)
									{
										return 0;
									}
									else
									{
										return 1;
									}
								}

								line[i++] = character;
							}
						}
					}

					line[i++] = character;
					if (TextHelper.IsLineBreak((char) character))
					{
						i = 0;
					}
				}
			
				return 1;
			}
		}

		public override string ExecuteAndCreateResponseMessage(RequestData data)
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			int statusCode;
			data.MutexLock.WaitOne();
			if (data.OnlineUsers.ContainsKey(username)) {
				statusCode = 2;
				data.MutexLock.ReleaseMutex();
			}
			else
			{
				data.MutexLock.ReleaseMutex();
				statusCode = AuthenticateUser(username, password);
				if (statusCode == 0)
				{
					string endpoint = data.ClientSocket.RemoteEndPoint.ToString();
					data.MutexLock.WaitOne();
					if (!data.OnlineUsers.ContainsKey(username)) {
						data.OnlineUsers.Add(username, endpoint);
					}
					data.MutexLock.ReleaseMutex();
				}
			}

			return $"login {statusCode}";
		}
	}
}

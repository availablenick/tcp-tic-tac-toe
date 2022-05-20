using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

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

		public override string Fulfill(Socket clientSocket, Mutex mutex)
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			int statusCode;
			mutex.WaitOne();
			if (Server.EndpointByUsername.ContainsKey(username)) {
				statusCode = 2;
			}
			else
			{
				statusCode = AuthenticateUser(username, password);
				if (statusCode == 0)
				{
					string remoteEndpoint = clientSocket.RemoteEndPoint.ToString();
					Server.AddOnlineUser(username, remoteEndpoint);
				}
			}

			mutex.ReleaseMutex();

			return $"login {statusCode}";
		}
	}
}

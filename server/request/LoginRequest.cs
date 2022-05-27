using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class LoginRequest : Request
	{
		public const int NumberOfParameters = 1;

		private Socket _clientSocket;
		private Mutex _mutex;
		private Dictionary<string, string> _usernameByEndpoint;
		private Dictionary<string, string> _endpointByUsername;

		public LoginRequest(string[] parameters, Socket clientSocket,
			Mutex mutex, Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername) : base(parameters)
		{
			this._clientSocket = clientSocket;
			this._mutex = mutex;
			this._usernameByEndpoint = usernameByEndpoint;
			this._endpointByUsername = endpointByUsername;
		}

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

		public override string Fulfill()
		{
			string[] data = this.Data.Split(';', StringSplitOptions.RemoveEmptyEntries);
			if (data.Length != 2)
			{
				return "login 3";
			}

			string username = data[0];
			string password = data[1];
			int statusCode;
			this._mutex.WaitOne();
			if (this._endpointByUsername.ContainsKey(username)) {
				statusCode = 2;
			}
			else
			{
				statusCode = AuthenticateUser(username, password);
				if (statusCode == 0)
				{
					string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
					UserHelper.AddOnlineUser(username, remoteEndpoint,
						this._usernameByEndpoint, this._endpointByUsername);
				}
			}

			this._mutex.ReleaseMutex();

			return $"login {statusCode}";
		}
	}
}

using System;
using System.IO;
using System.Net.Sockets;

namespace TicTacToe.ServerSide
{
	public class LoginRequestHandler : IMessageHandler
	{
		private string _data;
		private Server _server;
		private Socket _clientSocket;

		public LoginRequestHandler(string data, Server server, Socket clientSocket)
		{
			this._data = data;
			this._server = server;
			this._clientSocket = clientSocket;
		}

		public string HandleMessage()
		{
			string[] data = this._data.Split(';');
			if (data.Length != 2)
			{
				return "reslogin 3";
			}

			string username = data[0];
			string password = data[1];
			int statusCode = 0;
			if (this._server.IsUserOnline(username)) {
				statusCode = 2;
			}
			else
			{
				statusCode = AuthenticateUser(username, password);
				if (statusCode == 0)
				{
					string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
					this._server.AddOnlineUser(username, remoteEndpoint);
				}
			}

			return $"reslogin {statusCode}";
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
	}
}

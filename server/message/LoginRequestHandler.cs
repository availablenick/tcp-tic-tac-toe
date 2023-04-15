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
				return "reslogin 3\n";
			}

			string username = data[0];
			string password = data[1];
			int statusCode = 0;
			if (this._server.UserIsOnline(username)) {
				statusCode = 2;
			}
			else
			{
				if (CredentialsAreValid(username, password))
				{
					string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
					this._server.AddOnlineUser(username, remoteEndpoint);
				}
				else
				{
					statusCode = 1;
				}
			}

			return $"reslogin {statusCode}\n";
		}

		private bool CredentialsAreValid(string username, string password)
		{
			string filepath = $"{Directory.GetCurrentDirectory()}/data/users";
			foreach (string line in File.ReadLines(filepath))
			{
				string[] data = line.Split(" ");
				string entryUsername = data[0];
				string entryPassword = data[1];
				if (username == entryUsername)
				{
					if (password == entryPassword)
					{
						return true;
					}

					return false;
				}
			}

			return false;
		}
	}
}

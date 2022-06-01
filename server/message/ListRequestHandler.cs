using System;
using System.Net.Sockets;
using System.Text;

namespace TicTacToe.ServerSide
{
	public class ListRequestHandler : IMessageHandler
	{
		private Server _server;
		private Socket _clientSocket;

		public ListRequestHandler(Server server, Socket clientSocket)
		{
			this._server = server;
			this._clientSocket = clientSocket;
		}

		public string HandleMessage()
		{
			int statusCode = 0;
			StringBuilder usernames = new StringBuilder(1024);
			foreach (string username in this._server.EndpointByUsername.Keys)
			{
				usernames.Append($"{username};");
			}

			return $"reslist {usernames.ToString()} {statusCode}";
		}
	}
}

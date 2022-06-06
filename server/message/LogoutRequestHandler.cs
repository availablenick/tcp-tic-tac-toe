using System;
using System.Net.Sockets;

namespace TicTacToe.ServerSide
{
	public class LogoutRequestHandler : IMessageHandler
	{
		private Server _server;
		private Socket _clientSocket;

		public LogoutRequestHandler(Server server, Socket clientSocket)
		{
			this._server = server;
			this._clientSocket = clientSocket;
		}

		public string HandleMessage()
		{
			string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
			int statusCode = 1;
			if (this._server.UserIsOnline(remoteEndpoint))
			{
				statusCode = 0;
				this._server.RemoveOnlineUser(remoteEndpoint);
			}

			return $"reslogout {statusCode}\n";
		}
	}
}

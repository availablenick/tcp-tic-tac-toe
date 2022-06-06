using System;
using System.Text;

namespace TicTacToe.ServerSide
{
	public class ListRequestHandler : IMessageHandler
	{
		private Server _server;

		public ListRequestHandler(Server server)
		{
			this._server = server;
		}

		public string HandleMessage()
		{
			int statusCode = 0;
			StringBuilder usernames = new StringBuilder(1024);
			foreach (string username in this._server.EndpointByUsername.Keys)
			{
				usernames.Append($"{username};");
			}

			return $"reslist {usernames.ToString()} {statusCode}\n";
		}
	}
}

using System;
using System.Text;

namespace TicTacToe.ServerSide
{
	public class ExitRequestHandler : IMessageHandler
	{
		private ConnectionHandler _connectionHandler;

		public ExitRequestHandler(ConnectionHandler connectionHandler)
		{
			this._connectionHandler = connectionHandler;
		}

		public string HandleMessage()
		{
			this._connectionHandler.ClientIsConnected = false;
			return "";
		}
	}
}

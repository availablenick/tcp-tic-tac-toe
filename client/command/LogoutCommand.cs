using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class LogoutCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for logout command. Usage: logout";

		private Client _client;

		public LogoutCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override void Execute()
		{
			string requestMessage = "reqlogout";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.HandlerCreator.CreateHandlerFor(
				responseMessage);
			int statusCode = handler.HandleMessage();
			if (statusCode == 0)
			{
				this._client.UserState = new LoggedOut();
			}
		}
	}
}

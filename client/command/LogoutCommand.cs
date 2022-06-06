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

		public override int Execute()
		{
			string requestMessage = "reqlogout\n";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.MessageHandlerCreator.CreateHandlerFor(
				responseMessage);
			int result = handler.HandleMessage();
			if (result == 0)
			{
				this._client.UserState = new LoggedOut(this._client);
			}
			
			return result;
		}
	}
}

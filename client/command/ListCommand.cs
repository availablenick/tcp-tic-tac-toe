using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class ListCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for list command. Usage: list";

		private Client _client;		

		public ListCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override void Execute()
		{
			string requestMessage = "reqlist";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.HandlerCreator.CreateHandlerFor(
				responseMessage);
			int statusCode = handler.HandleMessage();
		}
	}
}

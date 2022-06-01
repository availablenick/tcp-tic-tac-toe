using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class InviteCommand : Command
	{
		public const int NumberOfParameters = 1;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for invite command. Usage: invite <username>";
		
		private Client _client;

		public InviteCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override int Execute()
		{
			string username = this.Parameters[0];
			string requestMessage = $"reqinvite {username}";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.HandlerCreator.CreateHandlerFor(
				responseMessage);
			int statusCode = handler.HandleMessage();

			return statusCode;
		}
	}
}

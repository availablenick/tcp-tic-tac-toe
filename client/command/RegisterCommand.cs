using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class RegisterCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for register command. Usage: " +
			"register <username> <password>";

		private Client _client;

		public RegisterCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override int Execute()
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			string requestMessage = $"reqregister {username};{password}";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.MessageHandlerCreator.CreateHandlerFor(
				responseMessage);
			int result = handler.HandleMessage();

			return result;
		}
	}
}

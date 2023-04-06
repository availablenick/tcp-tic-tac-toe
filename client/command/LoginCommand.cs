using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class LoginCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for login command. Usage: " +
			"login <username> <password>";

		private Client _client;

		public LoginCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override void Execute()
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			string requestMessage = $"reqlogin {username};{password}\n";
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, requestMessage);
			string responseMessage = SocketHelper.ReceiveMessage(
				this._client.ServerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.MessageHandlerCreator.CreateHandlerFor(
				responseMessage);
			handler.HandleMessage();
			this._client.UserState = new LoggedIn(this._client);
		}
	}
}

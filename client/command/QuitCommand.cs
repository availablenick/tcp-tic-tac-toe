using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class QuitCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for quit command. Usage: quit";

		private Client _client;

		public QuitCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override void Execute()
		{
			if (this._client.PeerSocket == null)
			{
				throw new CommandFailedException();
			}

			string requestMessage = "reqquit\n";
			SocketHelper.SendMessage(this._client.PeerSocket,
				this._client.SendBuffer, requestMessage);
			this._client.PeerSocket.Close();
			this._client.PeerSocket = null;
			if (this._client.ListeningSocket != null)
			{
				this._client.ListeningSocket.Close();
				this._client.ListeningSocket = null;
			}

			this._client.UserState = new LoggedIn(this._client);
		}
	}
}

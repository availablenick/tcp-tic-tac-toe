using System;

namespace TicTacToe.ClientSide
{
	public class QuitRequestHandler : IMessageHandler
	{
		private Client _client;

		public QuitRequestHandler(Client client)
		{
			this._client = client;
		}

		public int HandleMessage()
		{
			Console.WriteLine("Your opponent quit the match");
			if (this._client.PeerSocket != null)
			{
				this._client.PeerSocket.Close();
				this._client.PeerSocket = null;
			}

			if (this._client.ListeningSocket != null)
			{
				this._client.ListeningSocket.Close();
				this._client.ListeningSocket = null;
			}

			this._client.UserState = new LoggedIn(this._client);

			return 0;
		}
	}
}

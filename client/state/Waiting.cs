using System;

namespace TicTacToe.ClientSide
{
	public class Waiting : IUserState
	{
		private Client _client;

		public Waiting(Client client)
		{
			this._client = client;
		}

		public void HandleInput()
		{
      Console.WriteLine("Waiting for opponent's move...");
      while (this._client.PeerSocket.Available <= 0)
      {
        this._client.CheckForServerMessage();
      }

      string peerMessage = SocketHelper.ReceiveMessage(
        this._client.PeerSocket, this._client.ReceiveBuffer);
      IMessageHandler handler = this._client.MessageHandlerCreator
        .CreateHandlerFor(peerMessage);
      handler.HandleMessage();
		}
	}
}

using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class PingRequestHandler : IMessageHandler
	{
		private Client _client;

		public PingRequestHandler(Client client)
		{
			this._client = client;
		}

		public void HandleMessage()
		{
			try
			{
				SocketHelper.SendMessage(this._client.ServerSocket,
					this._client.SendBuffer, "resping\n");
			}
			catch (SocketException) {}
		}
	}
}

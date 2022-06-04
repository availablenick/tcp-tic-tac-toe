using System;

namespace TicTacToe.ClientSide
{
	public class SendRequestHandler : IMessageHandler
	{
		private string _data;
		private Client _client;

		public SendRequestHandler(string data, Client client)
		{
			this._data = data;
			this._client = client;
		}

		public int HandleMessage()
		{
			string[] data = this._data.Split(';');
			int row = Int32.Parse(data[0]);
			int column = Int32.Parse(data[1]);
			int result = this._client.Board.MarkPosition(this._client.Board.OpponentMark,
				row, column);
			if (result == 0)
			{
				this._client.Board.Print();
				if (this._client.Board.HasWinner())
				{
					char winnerMark = this._client.Board.GetWinnerMark();
					if (winnerMark == this._client.Board.MyMark)
					{
						Console.WriteLine("You win");
					}
					else
					{
						Console.WriteLine("You lose");
					}

					this._client.PeerSocket.Close();
					this._client.PeerSocket = null;
					if (this._client.ListeningSocket != null)
					{
						this._client.ListeningSocket.Close();
						this._client.ListeningSocket = null;
					}

					this._client.Board = null;
					this._client.UserState = new LoggedIn(this._client);
				}

				return 0;
			}

			return 1;
		}
	}
}

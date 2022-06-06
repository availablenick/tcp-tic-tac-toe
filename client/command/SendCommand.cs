using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class SendCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for send command. Usage: send <row> <column>";
		
		private Client _client;

		public SendCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override int Execute()
		{
			int row = -1;
			int column = -1;
			try
			{
				row = Int32.Parse(this.Parameters[0]);
				column = Int32.Parse(this.Parameters[1]);
			}
			catch (FormatException)
			{
				Console.WriteLine("Row/column is not a valid number");
				return 1;
			}

			int result = this._client.Board.MarkPosition(this._client.Board.MyMark,
				row, column);
			if (result != 0)
			{
				Console.WriteLine("Specified row/column is out of bounds");
				return 1;
			}

			this._client.Board.Print();
			string requestMessage = $"reqsend {row};{column}\n";
			SocketHelper.SendMessage(this._client.PeerSocket,
				this._client.SendBuffer, requestMessage);
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
			else
			{
				Console.WriteLine("Waiting for opponent's move...");
			}

			return 0;
		}
	}
}

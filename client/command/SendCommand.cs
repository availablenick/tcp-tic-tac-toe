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

		public override void Execute()
		{
			int row = Parse(this.Parameters[0]);
			int column = Parse(this.Parameters[1]);
			TryToMarkBoardPosition(row, column);
			this._client.Board.Print();
			string requestMessage = $"reqsend {row};{column}\n";
			SocketHelper.SendMessage(this._client.PeerSocket,
				this._client.SendBuffer, requestMessage);
			if (this._client.Board.HasWinner())
			{
				PrintMatchResult();
				CleanUp();
				this._client.UserState = new LoggedIn(this._client);
			}
			else
			{
				Console.WriteLine("Waiting for opponent's move...");
			}
		}

		private int Parse(string text)
		{
			int result = -1;
			try
			{
				result = Int32.Parse(text);
			}
			catch (FormatException)
			{
				throw new CommandFailedException("Row/column is not a valid number");
			}

			return result;
		}

		private void TryToMarkBoardPosition(int row, int column)
		{
			try
			{
				this._client.Board.MarkPosition(this._client.Board.MyMark,
					row, column);
			}
			catch (IndexOutOfRangeException)
			{
				throw new CommandFailedException("Specified row/column is out of bounds");
			}
		}

		private void PrintMatchResult()
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
		}

		private void CleanUp()
		{
			this._client.PeerSocket.Close();
			this._client.PeerSocket = null;
			if (this._client.ListeningSocket != null)
			{
				this._client.ListeningSocket.Close();
				this._client.ListeningSocket = null;
			}

			this._client.Board = null;
		}
	}
}

using System;

namespace TicTacToe.ClientSide
{
	public class Board
	{
		private char[] _board;
		public char MyMark { get; set; }
		public char OpponentMark { get; set; }

		public Board(char myMark)
		{
			this._board = new char[9];
			this.MyMark = myMark;
			this.OpponentMark = (char) 1;
			if (myMark == 1)
			{
				this.OpponentMark = (char) 2;
			}

			for (int i = 0; i < 9; i++)
			{
				this._board[i] = (char) 0;
			}
		}

		public void MarkPosition(char mark, int row, int column)
		{
			if (!IsWithinBounds(row, column))
			{
				throw new IndexOutOfRangeException("Position is out of bounds");
			}

			int index = 3 * row + column;
			this._board[index] = mark;
		}

		public void Print()
		{
			for (int i = 0; i < 9; i++)
			{
				if (i % 3 == 0)
				{
					Console.WriteLine();
				}

				if (this._board[i] == 0)
				{
					Console.Write("* | ");
				}
				else if (this._board[i] == 1)
				{
					Console.Write("X | ");
				}
				else if (this._board[i] == 2)
				{
					Console.Write("O | ");
				}
			}

			Console.WriteLine();
		}

		public char GetWinnerMark()
		{
			if (MainDiagonalIsComplete())
			{
				return this._board[0];
			}

			if (AntidiagonalIsComplete())
			{
				return this._board[2];
			}

			for (int i = 0; i < 3; i++)
			{
				if (RowIsComplete(i))
				{
					return this._board[3 * i];
				}

				if (ColumnIsComplete(i))
				{
					return this._board[i];
				}
			}

			return (char) 0;
		}

		public bool HasWinner()
		{
			return GetWinnerMark() != 0;
		}

		private bool IsWithinBounds(int row, int column)
		{
			if (row >= 0 && row < 3 && column >= 0 && column < 3)
			{
				return true;
			}

			return false;
		}

		private bool MainDiagonalIsComplete()
		{
			if (this._board[0] == 0)
			{
				return false;
			}

			if (this._board[0] == this._board[4] &&
				this._board[0] == this._board[8])
			{
				return true;
			}

			return false;
		}

		private bool AntidiagonalIsComplete()
		{
			if (this._board[2] == 0)
			{
				return false;
			}

			if (this._board[2] == this._board[4] &&
				this._board[2] == this._board[6])
			{
				return true;
			}

			return false;
		}

		private bool RowIsComplete(int row)
		{
			int index = 3 * row;
			if (this._board[index] == 0)
			{
				return false;
			}

			if (this._board[index] == this._board[index+1] &&
				this._board[index] == this._board[index+2])
			{
				return true;
			}

			return false;
		}

		private bool ColumnIsComplete(int column)
		{
			int index = column;
			if (this._board[index] == 0)
			{
				return false;
			}

			if (this._board[index] == this._board[index+3] &&
				this._board[index] == this._board[index+6])
			{
				return true;
			}

			return false;
		}
	}
}

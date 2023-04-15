using System;
using System.Text;

namespace TicTacToe.ClientSide
{
	public class InputReader
	{
		private static InputReader _instance = null;
		private StringBuilder _buffer;
		public bool IsReading { get; set; }

		private InputReader()
		{
			this._buffer = new StringBuilder();
			this.IsReading = true;
		}

		public static InputReader GetInstance()
		{
			if (_instance == null)
			{
				_instance = new InputReader();
			}

			return _instance;
		}

		public static InputReader Create()
		{
			return new InputReader();
		}

		public string ReadLine(Action somethingToDo)
		{
			DiscardInput();
			this.IsReading = true;
			int cursorInitialPosition = Console.CursorLeft;
			while (this.IsReading)
			{
				somethingToDo();
				if (Console.KeyAvailable)
				{
					HandleKeyInput(cursorInitialPosition);
				}
			}

			bool characterWasAdded = this._buffer.Length > 0;
			if (characterWasAdded)
			{
				Console.WriteLine();
			}

			return this._buffer.ToString();
		}

		public void DiscardInput()
		{
			this._buffer.Clear();
		}

		private void HandleKeyInput(int cursorInitialPosition)
		{
			ConsoleKeyInfo info = Console.ReadKey(true);
			if (info.Key == ConsoleKey.Enter)
			{
				this.IsReading = false;
			}
			else if (info.Key == ConsoleKey.Backspace)
			{
				HandleBackspaceKey(cursorInitialPosition);
			}
			else if (CharIsPrintableASCII(info.KeyChar))
			{
				AddCharacter(info.KeyChar);
			}
		}

		private void HandleBackspaceKey(int cursorInitialPosition)
		{
			if (Console.CursorLeft > cursorInitialPosition)
			{
				DeleteLastCharacter();
			}
		}

		private void DeleteLastCharacter()
		{
			Console.Write("\b");
			if (this._buffer.Length > 0)
			{
				this._buffer.Remove(this._buffer.Length - 1, 1);
			}
		}

		private bool CharIsPrintableASCII(char character)
		{
			return character >= 32 && character <= 126;
		}

		private void AddCharacter(char character)
		{
			this._buffer.Append(character);
			Console.Write(character);
		}
	}
}

using System;
using System.Text;

namespace TicTacToe.ClientSide
{
	public class InputReader
	{
		private StringBuilder _buffer;

		public InputReader()
		{
			this._buffer = new StringBuilder();
		}

		public string ReadLine()
		{
			Func<bool> func = () => false;
			return ReadLine(func);
		}

		public string ReadLine(Func<bool> somethingToDo)
		{
			this._buffer.Clear();
			int cursorInitialPosition = Console.CursorLeft;
			while (true)
			{
				bool shouldStopReadingInput = somethingToDo();
				if (shouldStopReadingInput)
				{
					return null;
				}
				
				if (Console.KeyAvailable)
				{
					ConsoleKeyInfo info = Console.ReadKey(true);
					if (info.Key == ConsoleKey.Enter)
					{
						Console.Write(info.KeyChar);
						break;
					}
					else if (info.Key == ConsoleKey.Backspace)
					{
						if (Console.CursorLeft > cursorInitialPosition)
						{
							Console.Write("\b");
							if (this._buffer.Length > 0)
							{
								this._buffer.Remove(this._buffer.Length - 1, 1);
							}
						}
					}
					else if (info.Key == ConsoleKey.LeftArrow ||
						info.Key == ConsoleKey.RightArrow ||
						info.Key == ConsoleKey.UpArrow ||
						info.Key == ConsoleKey.DownArrow)
					{
						Console.SetCursorPosition(Console.CursorLeft, Console.CursorTop);
					}
					else if (TextHelper.IsPrintableASCII(info.KeyChar))
					{
						this._buffer.Append(info.KeyChar);
						Console.Write(info.KeyChar);
					}
				}
			}

			return this._buffer.ToString();
		}
	}
}

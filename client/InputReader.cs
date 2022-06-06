using System;
using System.Text;

namespace TicTacToe.ClientSide
{
	public class InputReader
	{
		private StringBuilder _buffer;
		public bool ShouldRead { get; set; }

		public InputReader()
		{
			this._buffer = new StringBuilder();
			this.ShouldRead = true;
		}

		public void Reset()
		{
			this._buffer.Clear();
		}

		public string ReadLine(Action somethingToDo)
		{
			Reset();
			this.ShouldRead = true;
			int cursorInitialPosition = Console.CursorLeft;
			while (this.ShouldRead)
			{
				somethingToDo();
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

		public static string ReadLine(Func<bool> somethingToDo)
		{
			StringBuilder buffer = new StringBuilder();
			int cursorInitialPosition = Console.CursorLeft;
			while (true)
			{
				bool shouldStopReading = somethingToDo();
				if (shouldStopReading)
				{
					break;
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
							if (buffer.Length > 0)
							{
								buffer.Remove(buffer.Length - 1, 1);
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
						buffer.Append(info.KeyChar);
						Console.Write(info.KeyChar);
					}
				}
			}

			return buffer.ToString();
		}
	}
}

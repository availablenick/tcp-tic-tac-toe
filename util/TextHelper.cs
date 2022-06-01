namespace TicTacToe
{
	public class TextHelper
	{
		public static bool IsPrintableASCII(char character)
		{
			if (character >= 32 && character <= 126)
			{
				return true;
			}

			return false;
		}

		public static bool IsLineBreak(char character)
		{
			if (character == '\r' || character == '\n')
			{
				return true;
			}

			return false;
		}
	}
}

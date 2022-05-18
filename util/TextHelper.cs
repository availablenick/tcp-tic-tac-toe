namespace TicTacToe
{
	public class TextHelper
	{
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

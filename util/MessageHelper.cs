using System;

namespace TicTacToe
{
	public class MessageHelper
	{
		public static string[] GetQueuedMessagesFrom(string message)
		{
			string[] messages = message.Split('\n',
				StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < messages.Length; i++)
			{
				messages[i] = messages[i] + '\n';
			}

			return messages;
		}
	}
}

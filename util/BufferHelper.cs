using System;

namespace TicTacToe
{
	public class BufferHelper
	{
		public static void WriteMessageToBuffer(Byte[] buffer, string message)
		{
			for (int i = 0; i < message.Length && i < buffer.Length - 1; i++)
			{
				buffer[i] = (byte) message[i];
			}

			buffer[buffer.Length - 1] = (byte) '\0';
		}

		public static string GetBufferMessage(Byte[] buffer, int numberOfBytes)
		{
			string message = "";
			for (int i = 0; i < numberOfBytes && buffer[i] != '\0'; i++)
			{
				message += (char) buffer[i];
			}

			return message;
		}
	}
}

using System;

namespace TicTacToe.ClientSide
{
	public class InvalidResponseHandler : IMessageHandler
	{
		public int HandleMessage()
		{
			Console.WriteLine("Received an invalid response from server");
			return 1;
		}
	}
}

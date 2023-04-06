using System;

namespace TicTacToe.ClientSide
{
	public class InvalidResponseHandler : IMessageHandler
	{
		public void HandleMessage()
		{
			throw new CommandFailedException("Received an invalid response from server");
		}
	}
}

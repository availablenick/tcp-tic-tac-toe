using System;

namespace TicTacToe.ClientSide
{
	public class LogoutResponseHandler : IMessageHandler
	{
		private int _statusCode;

		public LogoutResponseHandler(string statusCode)
		{
			this._statusCode = Int32.Parse(statusCode);
		}

		public void HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("You are no longer logged in");
					break;
				case 1:
					throw new CommandFailedException("You are not logged in");
				default:
					throw new CommandFailedException("Application error");
			}
		}
	}
}

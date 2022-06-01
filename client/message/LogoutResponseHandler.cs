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

		public int HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("You are no longer logged in");
					return 0;
				case 1:
					Console.WriteLine("You are not logged in");
					return 1;
			}

			Console.WriteLine("Application error");
			return 1;
		}
	}
}

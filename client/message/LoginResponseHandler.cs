using System;

namespace TicTacToe.ClientSide
{
	public class LoginResponseHandler : IMessageHandler
	{
		private int _statusCode;

		public LoginResponseHandler(string statusCode)
		{
			this._statusCode = Int32.Parse(statusCode);
		}

		public int HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("Logged in successfully");
					return 0;
				case 1:
					Console.WriteLine("Incorrect username or password");
					return 1;
				case 2:
					Console.WriteLine("This user is already logged in");
					return 1;
				case 3:
					Console.WriteLine("Invalid login request format");
					return 1;
			}

			Console.WriteLine("Application error");
			return 1;
		}
	}
}

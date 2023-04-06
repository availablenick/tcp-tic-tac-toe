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

		public void HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("Logged in successfully");
					break;
				case 1:
					throw new CommandFailedException("Incorrect username or password");
				case 2:
					throw new CommandFailedException("This user is already logged in");
				case 3:
					throw new CommandFailedException("Invalid login request format");
				default:
					throw new CommandFailedException("Application error");
			}
		}
	}
}

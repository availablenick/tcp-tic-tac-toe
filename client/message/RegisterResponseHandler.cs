using System;

namespace TicTacToe.ClientSide
{
	public class RegisterResponseHandler : IMessageHandler
	{
		private int _statusCode;

		public RegisterResponseHandler(string statusCode)
		{
			this._statusCode = Int32.Parse(statusCode);
		}

		public void HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("Registered successfully");
					break;
				case 1:
					throw new CommandFailedException("Username already exists");
				case 2:
					throw new CommandFailedException("Invalid register request format");
				default:
					throw new CommandFailedException("Application error");
			}
		}
	}
}

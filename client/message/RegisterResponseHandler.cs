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

		public int HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					Console.WriteLine("Registered successfully");
					return 0;
				case 1:
					Console.WriteLine("Username already exists");
					return 1;
				case 2:
					Console.WriteLine("Invalid register request format");
					return 1;
			}

			Console.WriteLine("Application error");
			return 1;
		}
	}
}

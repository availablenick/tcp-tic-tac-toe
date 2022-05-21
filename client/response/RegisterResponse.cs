using System;

namespace TicTacToe.Client
{
	public class RegisterResponse : Response
	{
		public const int NumberOfParameters = 1;
		public RegisterResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			int statusCode = Int32.Parse(this.Parameters[NumberOfParameters - 1]);
			switch (statusCode)
			{
				case 0:
					return "Registered successfully";
				case 1:
					return "Username already exists";
			}

			return "Application error";
		}
	}
}

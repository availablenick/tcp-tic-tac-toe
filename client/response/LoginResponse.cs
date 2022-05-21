using System;

namespace TicTacToe.Client
{
	public class LoginResponse : Response
	{
		public const int NumberOfParameters = 1;
		public LoginResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			int statusCode = Int32.Parse(this.Parameters[NumberOfParameters - 1]);
			switch (statusCode)
			{
				case 0:
					return "Logged in successfully";
				case 1:
					return "Incorrect username or password";
				case 2:
					return "This user is already logged in";
			}

			return "Application error";
		}
	}
}

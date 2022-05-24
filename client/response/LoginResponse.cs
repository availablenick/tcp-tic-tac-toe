using System;

namespace TicTacToe.ClientSide
{
	public class LoginResponse : Response
	{
		public const int NumberOfParameters = 1;
		public LoginResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			switch (this.StatusCode)
			{
				case 0:
					return "Logged in successfully";
				case 1:
					return "Incorrect username or password";
				case 2:
					return "This user is already logged in";
				case 3:
					return "Invalid login request format";
			}

			return "Application error";
		}
	}
}

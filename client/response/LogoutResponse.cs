using System;

namespace TicTacToe.Client
{
	public class LogoutResponse : Response
	{
		public const int NumberOfParameters = 1;
		public LogoutResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			switch (this.StatusCode)
			{
				case 0:
					return "You are no longer logged in";
				case 1:
					return "You are not logged in";
			}

			return "Application error";
		}
	}
}

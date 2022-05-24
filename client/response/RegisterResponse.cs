using System;

namespace TicTacToe.ClientSide
{
	public class RegisterResponse : Response
	{
		public const int NumberOfParameters = 1;
		public RegisterResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			switch (this.StatusCode)
			{
				case 0:
					return "Registered successfully";
				case 1:
					return "Username already exists";
				case 2:
					return "Invalid register request format";
			}

			return "Application error";
		}
	}
}

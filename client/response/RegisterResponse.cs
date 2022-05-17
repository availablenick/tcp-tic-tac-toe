namespace TicTacToe.Client
{
	public class RegisterResponse : Response
	{
		public RegisterResponse(int statusCode) : base(statusCode) { }
		public override string ParseStatusCode()
		{
			switch (this.StatusCode)
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

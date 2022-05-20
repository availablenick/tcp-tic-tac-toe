namespace TicTacToe.Client
{
	public class LoginResponse : Response
	{
		public LoginResponse(int statusCode) : base(statusCode) { }
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
			}

			return "Application error";
		}
	}
}

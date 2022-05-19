namespace TicTacToe.Client
{
	public class LogoutResponse : Response
	{
		public LogoutResponse(int statusCode) : base(statusCode) { }
		public override string ParseStatusCode()
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

namespace TicTacToe.Server
{
	public class RegisterRequest : Request
	{
		public const int NumberOfParameters = 2;
		public RegisterRequest(params string[] parameters) : base(parameters) { }
		public override string ExecuteAndCreateResponseMessage()
		{
			System.Console.WriteLine($"received: register {this.Parameters[0]} {this.Parameters[1]}");
			System.Console.WriteLine("Register work done");

			return $"register 1";
		}
	}
}

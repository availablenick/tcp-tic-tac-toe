namespace TicTacToe.ServerSide
{
	public class InvalidRequestHandler : IMessageHandler
	{
		public string HandleMessage()
		{
			return "invalid 1";
		}
	}
}

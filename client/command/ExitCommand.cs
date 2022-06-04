using System;

namespace TicTacToe.ClientSide
{
	public class ExitCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for exit command. Usage: exit";

		private Client _client;		

		public ExitCommand(string[] parameters, Client client) : base(parameters)
		{
			this._client = client;
		}

		public override int Execute()
		{
			if (this._client.ServerSocket != null)
			{
				this._client.ServerSocket.Close();
			}

			Environment.Exit(0);

			return 0;
		}
	}
}

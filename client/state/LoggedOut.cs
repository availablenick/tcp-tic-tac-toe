using System;

namespace TicTacToe.ClientSide
{
	public class LoggedOut : IUserState
	{
		public Client _client { get; set; }

		public LoggedOut(Client client)
		{
			this._client = client;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is ListCommand)
			{
				Console.WriteLine("You must log in first");
			}
			else if (command is LoginCommand)
			{
				int statusCode = command.Execute();
				if (statusCode == 0)
				{
					this._client.UserState = new LoggedIn(this._client);
				}
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You are not logged in");
			}
			else if (command is RegisterCommand)
			{
				command.Execute();
			}
		}
	}
}

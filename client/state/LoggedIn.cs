using System;

namespace TicTacToe.ClientSide
{
	public class LoggedIn : IUserState
	{
		public Client _client { get; set; }

		public LoggedIn(Client client)
		{
			this._client = client;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is ListCommand)
			{
				command.Execute();
			}
			else if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				int statusCode = command.Execute();
				if (statusCode == 0)
				{
					this._client.UserState = new LoggedOut(this._client);
				}
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

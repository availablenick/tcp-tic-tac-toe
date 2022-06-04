using System;

namespace TicTacToe.ClientSide
{
	public class LoggedOut : IUserState
	{
		private Client _client;

		public LoggedOut(Client client)
		{
			this._client = client;
		}

		public bool HandleInput()
		{
			Console.Write("> ");
			string line = Console.ReadLine();
			if (line == null)
			{
				return true;
			}

			try
			{
				Command command = this._client.CommandParser.Parse(line);
				if (command != null)
				{
					ExecuteCommand(command);
				}
			}
			catch (InvalidCommandException exception)
			{
				Console.WriteLine(exception.Message);
			}

			return false;
		}

		private void ExecuteCommand(Command command)
		{
			if (command is InviteCommand)
			{
				Console.WriteLine("You must log in first");
			}
			else if (command is ListCommand)
			{
				Console.WriteLine("You must log in first");
			}
			else if (command is LoginCommand)
			{
				command.Execute();
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You are not logged in");
			}
			else if (command is QuitCommand)
			{
				Console.WriteLine("You are not in a match");
			}
			else if (command is RegisterCommand)
			{
				command.Execute();
			}
			else if (command is SendCommand)
			{
				Console.WriteLine("You are not in a match");
			}
		}
	}
}

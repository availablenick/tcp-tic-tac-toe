using System;

namespace TicTacToe.ClientSide
{
	public class LoggedIn : IUserState
	{
		private Client _client;

		public LoggedIn(Client client)
		{
			this._client = client;
		}

		public bool HandleInput()
		{
			Action checkForServerMessage = this._client.CheckForServerMessage;
			Console.Write("> ");
			string line = this._client.InputReader.ReadLine(checkForServerMessage);
			Command command = this._client.CommandParser.Parse(line);

			try
			{
				ExecuteCommand(command);
			}
			catch (CommandFailedException exception)
			{
				Console.WriteLine(exception.Message);
			}

			return false;
		}

		private void ExecuteCommand(Command command)
		{
			if (command is ExitCommand)
			{
				command.Execute();
			}
			else if (command is InvalidCommand)
			{
				command.Execute();
			}
			else if (command is InviteCommand)
			{
				command.Execute();
			}
			else if (command is ListCommand)
			{
				command.Execute();
			}
			else if (command is LoginCommand)
			{
				throw new CommandFailedException("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				command.Execute();
			}
			else if (command is QuitCommand)
			{
				throw new CommandFailedException("You are not in a match");
			}
			else if (command is RegisterCommand)
			{
				throw new CommandFailedException("You need to log out first");
			}
			else if (command is SendCommand)
			{
				throw new CommandFailedException("You are not in a match");
			}
		}
	}
}

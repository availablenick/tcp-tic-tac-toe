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
			ExecuteCommand(command);
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
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				command.Execute();
			}
			else if (command is QuitCommand)
			{
				Console.WriteLine("You are not in a match");
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
			else if (command is SendCommand)
			{
				Console.WriteLine("You are not in a match");
			}
		}
	}
}

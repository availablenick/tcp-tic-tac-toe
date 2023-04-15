using System;

namespace TicTacToe.ClientSide
{
	public class Playing : IUserState
	{
		private Client _client;

		public Playing(Client client)
		{
			this._client = client;
		}

		public void HandleInput()
		{
      Console.WriteLine("It is your turn");
			Action checkForServerMessage = this._client.CheckForServerMessage;
      while (true)
      {
        Console.Write("> ");
        string line = InputReader.GetInstance().ReadLine(checkForServerMessage);
        Command command = this._client.CommandParser.Parse(line);

        try
        {
          ExecuteCommand(command);
          break;
        }
        catch (CommandFailedException exception)
        {
          Console.WriteLine(exception.Message);
        }
      }
		}

		private void ExecuteCommand(Command command)
		{
			if (command is ExitCommand)
			{
				throw new CommandFailedException("You must either quit or finish your match first");
			}
			else if (command is InviteCommand)
			{
				throw new CommandFailedException("You cannot invite another player during a match");
			}
			else if (command is ListCommand)
			{
				throw new CommandFailedException("You cannot access the online user list during a match");
			}
			else if (command is LoginCommand)
			{
				throw new CommandFailedException("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				throw new CommandFailedException("You cannot log out during a match");
			}
			else if (command is RegisterCommand)
			{
				throw new CommandFailedException("You cannot register during a match");
			}

			command.Execute();
		}
	}
}

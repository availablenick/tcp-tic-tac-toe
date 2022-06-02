using System;

namespace TicTacToe.ClientSide
{
	public class LoggedIn : IUserState
	{
		public void ExecuteCommand(Command command)
		{
			if (command is InviteCommand)
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
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

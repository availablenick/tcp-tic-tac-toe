using System;

namespace TicTacToe.ClientSide
{
	public class LoggedOut : IUserState
	{
		public void ExecuteCommand(Command command)
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
			else if (command is RegisterCommand)
			{
				command.Execute();
			}
		}
	}
}

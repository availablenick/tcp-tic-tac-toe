using System;

namespace TicTacToe.ClientSide
{
	public class Playing : IUserState
	{
		public void ExecuteCommand(Command command)
		{
			if (command is InviteCommand)
			{
				Console.WriteLine("You cannot invite another player during a match");
			}
			else if (command is ListCommand)
			{
				Console.WriteLine("You cannot access the online user list during a match");
			}
			else if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You cannot log out during a match");
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You cannot register during a match");
			}
		}
	}
}

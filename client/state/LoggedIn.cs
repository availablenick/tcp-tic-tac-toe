using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class LoggedIn : IUserState
	{
		public UserInputReader Reader { get; set; }

		public LoggedIn(UserInputReader reader)
		{
			this.Reader = reader;
		}

		public void ExecuteCommand(Command command, CommandData data)
		{
			if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				int statusCode = command.TakeEffect(data);
				if (statusCode == 0)
				{
					this.Reader.UserState = new LoggedOut(this.Reader);
					data.CurrentUsername = "";
				}
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

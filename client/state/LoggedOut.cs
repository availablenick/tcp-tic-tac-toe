using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class LoggedOut : IUserState
	{
		public UserInputReader Reader { get; set; }

		public LoggedOut(UserInputReader reader)
		{
			this.Reader = reader;
		}

		public void ExecuteCommand(Command command, CommandData data)
		{
			if (command is LoginCommand)
			{
				int statusCode = command.TakeEffect(data);
				if (statusCode == 0)
				{
					this.Reader.UserState = new LoggedIn(this.Reader);
					data.CurrentUsername = command.Parameters[0];
				}
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You are not logged in");
			}
			else if (command is RegisterCommand)
			{
				command.TakeEffect(data);
			}
		}
	}
}

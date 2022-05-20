using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class LoggedIn : IUserState
	{
		public InputHandler Handler { get; set; }

		public LoggedIn(InputHandler handler)
		{
			this.Handler = handler;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				int statusCode = command.Execute(this.Handler.ServerSocket,
					this.Handler.ReceiveBuffer, this.Handler.SendBuffer);
				if (statusCode == 0)
				{
					this.Handler.UserState = new LoggedOut(this.Handler);
				}
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

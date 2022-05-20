using System;

namespace TicTacToe.Client
{
	public class LoggedOut : IUserState
	{
		public InputHandler Handler { get; set; }

		public LoggedOut(InputHandler handler)
		{
			this.Handler = handler;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is LoginCommand)
			{
				int statusCode = command.Execute(this.Handler.ServerSocket,
					this.Handler.ReceiveBuffer, this.Handler.SendBuffer);
				if (statusCode == 0)
				{
					this.Handler.UserState = new LoggedIn(this.Handler);
				}
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You are not logged in");
			}
			else if (command is RegisterCommand)
			{
				command.Execute(this.Handler.ServerSocket,
					this.Handler.ReceiveBuffer, this.Handler.SendBuffer);
			}
		}
	}
}

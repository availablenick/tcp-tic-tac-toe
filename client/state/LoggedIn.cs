using System;

namespace TicTacToe.ClientSide
{
	public class LoggedIn : IUserState
	{
		public Client ClientObject { get; set; }

		public LoggedIn(Client clientObject)
		{
			this.ClientObject = clientObject;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is ListCommand)
			{
				command.Execute(this.ClientObject.ServerSocket,
					this.ClientObject.ReceiveBuffer, this.ClientObject.SendBuffer);
			}
			else if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				int statusCode = command.Execute(this.ClientObject.ServerSocket,
					this.ClientObject.ReceiveBuffer, this.ClientObject.SendBuffer);
				if (statusCode == 0)
				{
					this.ClientObject.UserState = new LoggedOut(this.ClientObject);
				}
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

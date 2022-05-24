using System;

namespace TicTacToe.ClientSide
{
	public class LoggedOut : IUserState
	{
		public Client ClientObject { get; set; }

		public LoggedOut(Client clientObject)
		{
			this.ClientObject = clientObject;
		}

		public void ExecuteCommand(Command command)
		{
			if (command is ListCommand)
			{
				Console.WriteLine("You must log in first");
			}
			else if (command is LoginCommand)
			{
				int statusCode = command.Execute(this.ClientObject.ServerSocket,
					this.ClientObject.ReceiveBuffer, this.ClientObject.SendBuffer);
				if (statusCode == 0)
				{
					this.ClientObject.UserState = new LoggedIn(this.ClientObject);
				}
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You are not logged in");
			}
			else if (command is RegisterCommand)
			{
				command.Execute(this.ClientObject.ServerSocket,
					this.ClientObject.ReceiveBuffer, this.ClientObject.SendBuffer);
			}
		}
	}
}

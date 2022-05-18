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

		public void ExecuteCommand(Command command, Socket socket,
			Byte[] receiveBuffer, Byte[] sendBuffer)
		{
			if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

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

		public void ExecuteCommand(Command command, Socket socket,
			Byte[] receiveBuffer, Byte[] sendBuffer)
		{
			if (command is RegisterCommand)
			{
				command.TakeEffect(socket, receiveBuffer, sendBuffer);
			}
		}
	}
}

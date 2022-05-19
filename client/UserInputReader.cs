using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class UserInputReader
	{
		public IUserState UserState { get; set; }

		public void Read(Socket socket)
		{
			this.UserState = new LoggedOut(this);			
			Byte[] receiveBuffer = new Byte[128];
			Byte[] sendBuffer = new Byte[128];
			CommandData data = new CommandData(socket, receiveBuffer, sendBuffer, "");
			string line;
			while (true)
			{
				Console.Write("> ");
				line = Console.ReadLine();
				if (line == null)
				{
					break;
				}

				try
				{
					Command command = CommandParser.Parse(line);
					if (command != null)
					{
						this.UserState.ExecuteCommand(command, data);
					}
				}
				catch (InvalidCommandException exception)
				{
					Console.WriteLine(exception.Message);
				}
			}
		}
	}
}

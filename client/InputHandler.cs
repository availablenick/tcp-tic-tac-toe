using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class InputHandler
	{
		public IUserState UserState { get; set; }
		public Socket ServerSocket { get; }
		public Byte[] ReceiveBuffer;
		public Byte[] SendBuffer;

		public InputHandler(Socket serverSocket)
		{
			this.ServerSocket = serverSocket;
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.UserState = new LoggedOut(this);
		}

		public void HandleInput()
		{
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
						this.UserState.ExecuteCommand(command);
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

using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class Client
	{
		public Socket ServerSocket { get; }
		public IUserState UserState { get; set; }
		public Byte[] ReceiveBuffer;
		public Byte[] SendBuffer;

		public Client(string serverAddress, int serverPort)
		{
			Console.WriteLine($"Trying to connect to {serverAddress} on port {serverPort}...");

			this.ServerSocket = SocketHelper.CreateConnectionSocket(serverAddress,
				serverPort);
			if (this.ServerSocket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				return;
			}

			Console.WriteLine("Connection established");

			this.UserState = new LoggedOut(this);
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
		}

		public void HandleInput()
		{
			CommandParser parser = new CommandParser(this);
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
					Command command = parser.Parse(line);
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

			Console.WriteLine("End");
			this.ServerSocket.Close();
		}
	}
}

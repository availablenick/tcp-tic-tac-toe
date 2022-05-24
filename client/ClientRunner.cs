using System;

namespace TicTacToe.ClientSide
{
	public class ClientRunner
	{
		public static void Main(string[] args)
		{
			string serverAddress = "";
			int serverPort = 0;
			if (args.Length < 2)
			{
				Console.WriteLine("You need to specify the server's address and port to connect to");
				return;
			}
			else
			{
				serverAddress = args[0];
				try
				{
					serverPort = Int32.Parse(args[1]);
				}
				catch (FormatException)
				{
					Console.WriteLine("Specified port has invalid format");
					return;
				}
			}

			Client client = new Client(serverAddress, serverPort);
			client.HandleInput();
		}
	}
}

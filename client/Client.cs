using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class Client
	{
		public static void Main(string[] args)
		{
			string serverAddress = "";
			int port = 0;
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
					port = Int32.Parse(args[1]);
				}
				catch (FormatException)
				{
					Console.WriteLine("Specified port has invalid format");
					return;
				}
			}

			Console.WriteLine($"Trying to connect to {serverAddress} on port {port}...");

			Socket socket = SocketHelper.CreateConnectionSocket(serverAddress, port);
			if (socket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				return;
			}

			Console.WriteLine("Connection established");

			InputHandler handler = new InputHandler(socket);
			handler.HandleInput();

			Console.WriteLine("End");
			socket.Close();
		}
	}
}

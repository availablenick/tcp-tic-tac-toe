using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class Client
	{
		public static void Main(string[] args)
		{
			Socket socket = SocketHelper.CreateConnectionSocket("127.0.0.1", 3000);
			if (socket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				return;
			}

			Byte[] receiveBuffer = new Byte[128];
			Byte[] sendBuffer = new Byte[128];
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
						command.TakeEffect(socket, receiveBuffer, sendBuffer);
					}
				}
				catch (InvalidCommandException exception)
				{
					Console.WriteLine(exception.Message);
				}
			}

			Console.WriteLine("End");
			socket.Close();
		}
	}
}

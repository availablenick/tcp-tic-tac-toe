using System;

namespace TicTacToe.ServerSide
{
	public class ServerInitializer
	{
		public static void Main(string[] args)
		{
			int port = 3000;
			if (args.Length > 0)
			{
				try
				{
					port = Int32.Parse(args[0]);
				}
				catch (FormatException)
				{
					Console.WriteLine("Specified port does not have a valid format");
				}
			}

			Server server = new Server(port);
			server.WaitConnection();
		}
	}
}

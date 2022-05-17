using System;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class Server
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Initializing server...");

			Socket listeningSocket = SocketHelper.CreateListeningSocket(3000);
			if (listeningSocket == null)
			{
				Console.WriteLine("Could not create socket");
				return;
			}

			Console.WriteLine("Done");

			while (true)
			{
				Console.WriteLine("Waiting for incoming connections...");
				Socket connectionSocket = listeningSocket.Accept();
				Console.WriteLine("Connection accepted...");

				ThreadDataWrapper wrapper = new ThreadDataWrapper(connectionSocket);
				Thread thread = new Thread(new ThreadStart(wrapper.HandleConnection));
				thread.Start();

				Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
			}
		}
	}
}

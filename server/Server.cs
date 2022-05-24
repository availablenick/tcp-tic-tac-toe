using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class Server
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
					Console.WriteLine("Specified port has invalid format");
				}
			}

			Console.WriteLine($"Initializing server on port {port}...");

			Socket listeningSocket = SocketHelper.CreateListeningSocket(port);
			if (listeningSocket == null)
			{
				Console.WriteLine("Could not create socket");
				return;
			}

			int statusCode = FileHelper.CreateUserDataFile();
			if (statusCode != 0)
			{
				return;
			}

			Console.WriteLine("Done");

			Mutex mutex = new Mutex();
			Dictionary<string, string> usernameByEndpoint = new Dictionary<string, string>();
			Dictionary<string, string> endpointByUsername = new Dictionary<string, string>();
			while (true)
			{
				Console.WriteLine("Waiting for incoming connections...");
				Socket connectionSocket = listeningSocket.Accept();
				Console.WriteLine("Connection accepted...");

				ThreadDataWrapper wrapper = new ThreadDataWrapper(connectionSocket,
					mutex, usernameByEndpoint, endpointByUsername);
				Thread thread = new Thread(new ThreadStart(wrapper.HandleConnection));
				thread.Start();

				Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
			}
		}
	}
}

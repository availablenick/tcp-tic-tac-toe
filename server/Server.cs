using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class Server
	{
		public static readonly Dictionary<string, string> UsernameByEndpoint = new Dictionary<string, string>();
		public static readonly Dictionary<string, string> EndpointByUsername = new Dictionary<string, string>();

		public static void AddOnlineUser(string username, string remoteEndpoint)
		{
			UsernameByEndpoint.Add(remoteEndpoint, username);
			EndpointByUsername.Add(username, remoteEndpoint);
		}

		public static void RemoveOnlineUser(string remoteEndpoint)
		{
			string username = UsernameByEndpoint[remoteEndpoint];
			UsernameByEndpoint.Remove(remoteEndpoint);
			EndpointByUsername.Remove(username);
		}

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
			while (true)
			{
				Console.WriteLine("Waiting for incoming connections...");
				Socket connectionSocket = listeningSocket.Accept();
				Console.WriteLine("Connection accepted...");

				ThreadDataWrapper wrapper = new ThreadDataWrapper(connectionSocket,
					mutex);
				Thread thread = new Thread(new ThreadStart(wrapper.HandleConnection));
				thread.Start();

				Console.WriteLine($"Main thread: {Thread.CurrentThread.ManagedThreadId}");
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class Server
	{
		private Socket _listeningSocket;

		public Server() : this(3000) {}

		public Server(int port)
		{
			Console.WriteLine($"Initializing server on port {port}...");

			this._listeningSocket = SocketHelper.CreateListeningSocket(port);
			if (this._listeningSocket == null)
			{
				Console.WriteLine("Could not create listening socket");
				return;
			}

			int statusCode = FileHelper.CreateUserDataFile();
			if (statusCode != 0)
			{
				return;
			}

			Console.WriteLine("Done");
		}

		public void WaitConnection()
		{
			Mutex mutex = new Mutex();
			Dictionary<string, string> usernameByEndpoint = new Dictionary<string, string>();
			Dictionary<string, string> endpointByUsername = new Dictionary<string, string>();
			while (true)
			{
				Console.WriteLine("Waiting for incoming connections...");
				Socket connectionSocket = this._listeningSocket.Accept();
				Console.WriteLine("Connection accepted...");

				ThreadDataWrapper wrapper = new ThreadDataWrapper(connectionSocket,
					mutex, usernameByEndpoint, endpointByUsername);
				Thread thread = new Thread(new ThreadStart(wrapper.HandleConnection));
				thread.Start();
			}
		}
	}
}

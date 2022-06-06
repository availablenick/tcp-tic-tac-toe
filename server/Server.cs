using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class Server
	{
		private Socket _listeningSocket;
		private Dictionary<string, ConnectionHandler> _connectionHandlerByEndpoint;
		public Dictionary<string, Socket> SocketByEndpoint { get; }
		public Dictionary<string, string> UsernameByEndpoint { get; }
		public Dictionary<string, string> EndpointByUsername { get; }
		public Mutex Mutex { get; }

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

			this._connectionHandlerByEndpoint = new Dictionary<string, ConnectionHandler>();
			this.SocketByEndpoint = new Dictionary<string, Socket>();
			this.UsernameByEndpoint = new Dictionary<string, string>();
			this.EndpointByUsername = new Dictionary<string, string>();
			this.Mutex = new Mutex();

			int statusCode = FileHelper.CreateUserDataFile();
			if (statusCode != 0)
			{
				return;
			}

			Console.WriteLine("Done");
		}

		public void WaitConnection()
		{
			while (true)
			{
				Console.WriteLine("Waiting for incoming connections...");
				Socket clientSocket = this._listeningSocket.Accept();
				clientSocket.Blocking = false;
				string remoteEndpoint = clientSocket.RemoteEndPoint.ToString();
				this.SocketByEndpoint.Add(remoteEndpoint, clientSocket);
				Console.WriteLine("Connection accepted...");

				ConnectionHandler handler = new ConnectionHandler(this, clientSocket);
				this._connectionHandlerByEndpoint.Add(remoteEndpoint, handler);
				Thread thread = new Thread(new ThreadStart(handler.HandleConnection));
				thread.Start();
			}
		}

		public void AddOnlineUser(string username, string remoteEndpoint)
		{
			this.Mutex.WaitOne();
			this.UsernameByEndpoint.Add(remoteEndpoint, username);
			this.EndpointByUsername.Add(username, remoteEndpoint);
			this.Mutex.ReleaseMutex();
		}

		public void RemoveOnlineUser(string remoteEndpoint)
		{
			string username = this.UsernameByEndpoint[remoteEndpoint];
			this.Mutex.WaitOne();
			this.UsernameByEndpoint.Remove(remoteEndpoint);
			this.EndpointByUsername.Remove(username);
			this.Mutex.ReleaseMutex();
		}

		public bool UserIsOnline(string key)
		{
			return this.EndpointByUsername.ContainsKey(key) ||
				this.UsernameByEndpoint.ContainsKey(key);
		}

		public void RemoveConnectedEndpoint(string remoteEndpoint)
		{
			this.Mutex.WaitOne();
			this._connectionHandlerByEndpoint.Remove(remoteEndpoint);
			this.Mutex.ReleaseMutex();
		}

		public void NotifyThread(string endpoint, bool shouldReadData)
		{
			if (this._connectionHandlerByEndpoint.ContainsKey(endpoint))
			{
				this._connectionHandlerByEndpoint[endpoint].ShouldReadData = shouldReadData;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class Server
	{
		private Socket _listeningSocket;
		private Dictionary<string, ThreadDataWrapper> _threadDataWrapperByEndpoint;
		public Dictionary<string, Socket> SocketByEndpoint { get; }
		public Dictionary<string, string> UsernameByEndpoint { get; }
		public Dictionary<string, string> EndpointByUsername { get; }
		public Mutex MutexLock { get; }

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

			this._threadDataWrapperByEndpoint = new Dictionary<string, ThreadDataWrapper>();
			this.SocketByEndpoint = new Dictionary<string, Socket>();
			this.UsernameByEndpoint = new Dictionary<string, string>();
			this.EndpointByUsername = new Dictionary<string, string>();
			this.MutexLock = new Mutex();

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
				Socket connectionSocket = this._listeningSocket.Accept();
				connectionSocket.Blocking = false;
				string remoteEndpoint = connectionSocket.RemoteEndPoint.ToString();
				this.SocketByEndpoint.Add(remoteEndpoint, connectionSocket);
				Console.WriteLine("Connection accepted...");

				ThreadDataWrapper wrapper = new ThreadDataWrapper(this, connectionSocket);
				this._threadDataWrapperByEndpoint.Add(remoteEndpoint, wrapper);
				Thread thread = new Thread(new ThreadStart(wrapper.HandleConnection));
				thread.Start();
			}
		}

		public void AddOnlineUser(string username, string remoteEndpoint)
		{
			this.MutexLock.WaitOne();
			this.UsernameByEndpoint.Add(remoteEndpoint, username);
			this.EndpointByUsername.Add(username, remoteEndpoint);
			this.MutexLock.ReleaseMutex();
		}

		public void RemoveOnlineUser(string remoteEndpoint)
		{
			string username = this.UsernameByEndpoint[remoteEndpoint];
			this.MutexLock.WaitOne();
			this.UsernameByEndpoint.Remove(remoteEndpoint);
			this.EndpointByUsername.Remove(username);
			this.MutexLock.ReleaseMutex();
		}

		public bool UserIsOnline(string key)
		{
			return this.EndpointByUsername.ContainsKey(key) ||
				this.UsernameByEndpoint.ContainsKey(key);
		}

		public void NotifyThread(string endpoint, bool shouldReadData)
		{
			if (this._threadDataWrapperByEndpoint.ContainsKey(endpoint))
			{
				this._threadDataWrapperByEndpoint[endpoint].ShouldReadData = shouldReadData;
			}
		}
	}
}

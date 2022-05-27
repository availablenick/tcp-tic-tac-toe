using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class ListRequest : Request
	{
		public const int NumberOfParameters = 0;

		private Socket _clientSocket;
		private Mutex _mutex;
		private Dictionary<string, string> _endpointByUsername;

		public ListRequest(string[] parameters, Socket clientSocket,
			Mutex mutex, Dictionary<string, string> endpointByUsername) : base(parameters)
		{
			this._clientSocket = clientSocket;
			this._mutex = mutex;
			this._endpointByUsername = endpointByUsername;
		}

		public override string Fulfill()
		{
			int statusCode = 0;
			this._mutex.WaitOne();
			StringBuilder usernames = new StringBuilder(1024);
			foreach (string username in this._endpointByUsername.Keys)
			{
				usernames.Append($"{username};");
			}

			this._mutex.ReleaseMutex();

			return $"list {usernames.ToString()} {statusCode}";
		}
	}
}

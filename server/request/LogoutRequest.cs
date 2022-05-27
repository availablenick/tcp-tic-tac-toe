using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class LogoutRequest : Request
	{
		public const int NumberOfParameters = 0;

		private Socket _clientSocket;
		private Mutex _mutex;
		private Dictionary<string, string> _usernameByEndpoint;
		private Dictionary<string, string> _endpointByUsername;

		public LogoutRequest(string[] parameters, Socket clientSocket,
			Mutex mutex, Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername) : base(parameters)
		{
			this._clientSocket = clientSocket;
			this._mutex = mutex;
			this._usernameByEndpoint = usernameByEndpoint;
			this._endpointByUsername = endpointByUsername;
		}

		public override string Fulfill()
		{
			string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
			int statusCode;
			this._mutex.WaitOne();
			if (this._usernameByEndpoint.ContainsKey(remoteEndpoint))
			{
				statusCode = 0;
				UserHelper.RemoveOnlineUser(remoteEndpoint, this._usernameByEndpoint,
					this._endpointByUsername);
			}
			else
			{
				statusCode = 1;
			}

			this._mutex.ReleaseMutex();

			return $"logout {statusCode}";
		}
	}
}

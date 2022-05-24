using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class LogoutRequest : Request
	{
		public const int NumberOfParameters = 0;

		public LogoutRequest(params string[] parameters) : base(parameters) { }

		public override string Fulfill(Socket clientSocket, Mutex mutex,
			Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername)
		{
			string remoteEndpoint = clientSocket.RemoteEndPoint.ToString();
			int statusCode;
			mutex.WaitOne();
			if (usernameByEndpoint.ContainsKey(remoteEndpoint))
			{
				statusCode = 0;
				UserHelper.RemoveOnlineUser(remoteEndpoint, usernameByEndpoint,
					endpointByUsername);
			}
			else
			{
				statusCode = 1;
			}

			mutex.ReleaseMutex();

			return $"logout {statusCode}";
		}
	}
}

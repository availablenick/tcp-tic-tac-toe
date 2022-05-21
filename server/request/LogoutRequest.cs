using System;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class LogoutRequest : Request
	{
		public const int NumberOfParameters = 0;

		public LogoutRequest(params string[] parameters) : base(parameters) { }

		public override string Fulfill(Socket clientSocket, Mutex mutex)
		{
			string remoteEndpoint = clientSocket.RemoteEndPoint.ToString();
			int statusCode;
			mutex.WaitOne();
			if (Server.UsernameByEndpoint.ContainsKey(remoteEndpoint))
			{
				statusCode = 0;
				Server.RemoveOnlineUser(remoteEndpoint);
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

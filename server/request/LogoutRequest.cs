using System;
using System.IO;
using System.Net;

namespace TicTacToe.Server
{
	public class LogoutRequest : Request
	{
		public const int NumberOfParameters = 1;
		public LogoutRequest(params string[] parameters) : base(parameters) { }
		public override string Fulfill(RequestData data)
		{
			string remoteEndpoint = this.Parameters[0];
			int statusCode;
			data.MutexLock.WaitOne();
			if (data.OnlineUsers.ContainsKey(remoteEndpoint)) {
				data.OnlineUsers.Remove(remoteEndpoint);
				data.MutexLock.ReleaseMutex();
				statusCode = 0;
			}
			else
			{
				data.MutexLock.ReleaseMutex();
				statusCode = 1;
			}

			return $"logout {statusCode}";
		}
	}
}

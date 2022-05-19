using System;
using System.IO;
using System.Net;

namespace TicTacToe.Server
{
	public class LogoutRequest : Request
	{
		public const int NumberOfParameters = 1;
		public LogoutRequest(params string[] parameters) : base(parameters) { }
		public override string ExecuteAndCreateResponseMessage(RequestData data)
		{
			string username = this.Parameters[0];
			int statusCode;
			data.MutexLock.WaitOne();
			if (data.OnlineUsers.ContainsKey(username)) {
				data.OnlineUsers.Remove(username);
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

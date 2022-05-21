using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TicTacToe.Server
{
	public class ListRequest : Request
	{
		public const int NumberOfParameters = 0;

		public ListRequest(params string[] parameters) : base(parameters) { }

		public override string Fulfill(Socket clientSocket, Mutex mutex)
		{
			string remoteEndpoint = clientSocket.RemoteEndPoint.ToString();
			int statusCode = 0;
			mutex.WaitOne();
			StringBuilder usernames = new StringBuilder(1024);
			foreach (string username in Server.EndpointByUsername.Keys)
			{
				usernames.Append($"{username};");
			}

			mutex.ReleaseMutex();

			return $"list {usernames.ToString()} {statusCode}";
		}
	}
}

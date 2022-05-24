using System;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class InvalidRequest : Request
	{
		public const int NumberOfParameters = 0;

		public InvalidRequest(params string[] parameters) : base(parameters) { }

		public override string Fulfill(Socket clientSocket, Mutex mutex)
		{
			return "invalid 1";
		}
	}
}

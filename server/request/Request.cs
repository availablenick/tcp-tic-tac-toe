using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public abstract class Request
	{
		public string Data { get; }

		public Request(string[] messageParameters)
		{
			this.Data = null;
			if (messageParameters.Length == 1)
			{
				this.Data = messageParameters[0];
			}
		}

		public abstract string Fulfill();
	}
}

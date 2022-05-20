using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public abstract class Request
	{
		public List<string> Parameters;

		public Request(params string[] parameters)
		{
			this.Parameters = new List<string>();
			foreach (string parameter in parameters)
			{
				this.Parameters.Add(parameter);
			}
		}

		public abstract string Fulfill(Socket clientSocket, Mutex mutex);
	}
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public abstract class Command
	{
		public List<string> Parameters;

		public Command(params string[] parameters)
		{
			this.Parameters = new List<string>();
			foreach (string parameter in parameters)
			{
				this.Parameters.Add(parameter);
			}
		}

		public abstract int Execute(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer);
	}
}

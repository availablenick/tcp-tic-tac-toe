using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public abstract class Command
	{
		public List<string> Parameters;

		public Command(params string[] messageParameters)
		{
			this.Parameters = new List<string>();
			foreach (string parameter in messageParameters)
			{
				this.Parameters.Add(parameter);
			}
		}

		public abstract int Execute(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer);
	}
}

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public abstract class Command
	{
		protected List<string> parameters;

		public Command(params string[] parameters)
		{
			this.parameters = new List<string>();
			foreach (string parameter in parameters)
			{
				this.parameters.Add(parameter);
			}
		}

		public abstract void TakeEffect(Socket socket, Byte[] receiveBuffer,
			Byte[] sendBuffer);
	}
}

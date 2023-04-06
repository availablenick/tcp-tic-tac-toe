using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public abstract class Command
	{
		protected List<string> Parameters;

		public Command(string[] messageParameters)
		{
			this.Parameters = new List<string>();
			foreach (string parameter in messageParameters)
			{
				this.Parameters.Add(parameter);
			}
		}

		public abstract void Execute();
	}
}

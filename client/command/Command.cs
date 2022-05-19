using System;
using System.Collections.Generic;

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

		public abstract int TakeEffect(CommandData data);
	}
}

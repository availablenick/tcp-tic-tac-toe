using System.Collections.Generic;

namespace TicTacToe.Server
{
	public abstract class Request
	{
		public List<string> parameters;

		public Request(params string[] parameters)
		{
			this.parameters = new List<string>();
			foreach (string parameter in parameters)
			{
				this.parameters.Add(parameter);
			}
		}

		public abstract string ExecuteAndCreateResponseMessage();
	}
}

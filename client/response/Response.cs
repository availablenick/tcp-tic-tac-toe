using System.Collections.Generic;

namespace TicTacToe.Client
{
	public abstract class Response
	{
		public List<string> Parameters;

		public Response(params string[] parameters)
		{
			this.Parameters = new List<string>();
			foreach (string parameter in parameters)
			{
				this.Parameters.Add(parameter);
			}
		}

		public abstract override string ToString();
	}
}

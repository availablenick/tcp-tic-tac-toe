using System.Collections.Generic;

namespace TicTacToe.Client
{
	public abstract class Response
	{
		protected int StatusCode;

		public Response(int statusCode)
		{
			this.StatusCode = statusCode;
		}

		public abstract string ParseStatusCode();
	}
}

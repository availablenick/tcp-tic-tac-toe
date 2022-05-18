using System.Collections.Generic;

namespace TicTacToe.Client
{
	public abstract class Response
	{
		public int StatusCode { get; }

		public Response(int statusCode)
		{
			this.StatusCode = statusCode;
		}

		public abstract string ParseStatusCode();
	}
}

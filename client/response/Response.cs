using System;

namespace TicTacToe.ClientSide
{
	public abstract class Response
	{
		public string Data;
		public int StatusCode { get; }

		public Response(params string[] messageParameters)
		{
			this.Data = null;
			this.StatusCode = 1;
			if (messageParameters.Length == 1)
			{
				this.Data = null;
				this.StatusCode = Int32.Parse(messageParameters[0]);
			}
			else if (messageParameters.Length == 2)
			{
				this.Data = messageParameters[0];
				this.StatusCode = Int32.Parse(messageParameters[1]);
			}
		}

		public abstract override string ToString();
	}
}

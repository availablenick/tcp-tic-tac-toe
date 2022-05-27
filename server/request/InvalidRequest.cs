using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class InvalidRequest : Request
	{
		public const int NumberOfParameters = 0;

		public InvalidRequest(params string[] parameters) : base(parameters) { }

		public override string Fulfill()
		{
			return "invalid 1";
		}
	}
}

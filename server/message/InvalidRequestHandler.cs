using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class InvalidRequestHandler : IMessageHandler
	{
		public string HandleMessage()
		{
			return "invalid 1";
		}
	}
}

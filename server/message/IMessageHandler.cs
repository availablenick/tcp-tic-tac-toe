using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public interface IMessageHandler
	{
		public string HandleMessage();
	}
}

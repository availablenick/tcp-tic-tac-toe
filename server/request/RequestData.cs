using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class RequestData
	{
		public Socket ClientSocket { get; set; }
		public Mutex MutexLock { get; set; }
		public Dictionary<string, string> OnlineUsers { get; set; }

		public RequestData(Socket clientSocket, Mutex mutexLock,
			Dictionary<string, string> onlineUsers)
		{
			this.ClientSocket = clientSocket;
			this.MutexLock = mutexLock;
			this.OnlineUsers = onlineUsers;
		}
	}
}

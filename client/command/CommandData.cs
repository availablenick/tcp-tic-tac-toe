using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class CommandData
	{
		public Socket ServerSocket { get; set; }
		public Byte[] ReceiveBuffer { get; set; }
		public Byte[] SendBuffer { get; set; }
		public string CurrentUsername { get; set; }

		public CommandData(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer, string currentUsername)
		{
			this.ServerSocket = serverSocket;
			this.SendBuffer = sendBuffer;
			this.ReceiveBuffer = receiveBuffer;
			this.CurrentUsername = currentUsername;
		}
	}
}

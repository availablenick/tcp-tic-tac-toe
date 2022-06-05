using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class ConnectionHandler
	{
		public Server Server { get; }
		public Socket ClientSocket { get; }
		public Byte[] ReceiveBuffer { get; }
		public Byte[] SendBuffer { get; }
		public bool ShouldReadData { get; set; }

		public ConnectionHandler(Server server, Socket clientSocket)
		{
			this.Server = server;
			this.ClientSocket = clientSocket;
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.ShouldReadData = true;
		}

		public void HandleConnection()
		{
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] New client connected");

			MessageHandlerCreator handlerCreator = new MessageHandlerCreator(this);
			while (true)
			{
				if (this.ShouldReadData && this.ClientSocket.Available > 0)
				{
					string requestMessage = SocketHelper.ReceiveMessage(
						this.ClientSocket, this.ReceiveBuffer);
					IMessageHandler handler = handlerCreator.CreateHandlerFor(
						requestMessage);
					string responseMessage = handler.HandleMessage();
					SocketHelper.SendMessage(this.ClientSocket,
						this.SendBuffer, responseMessage);
				}
			}

			string remoteEndpoint = this.ClientSocket.RemoteEndPoint.ToString();
			if (this.Server.UserIsOnline(remoteEndpoint))
			{
				this.Server.RemoveOnlineUser(remoteEndpoint);
			}

			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Connection closed");
			this.ClientSocket.Close();
		}
	}
}

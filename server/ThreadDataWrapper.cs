using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class ThreadDataWrapper
	{
		private Server _server;
		private Socket _clientSocket;
		public bool ShouldReadData { get; set; }

		public ThreadDataWrapper(Server server, Socket clientSocket)
		{
			this._server = server;
			this._clientSocket = clientSocket;
			this.ShouldReadData = true;
		}

		public void HandleConnection()
		{
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] New client connected");

			Byte[] receiveBuffer = new Byte[1024];
			Byte[] sendBuffer = new Byte[1024];
			MessageHandlerCreator handlerCreator = new MessageHandlerCreator(
				this._server, this._clientSocket, receiveBuffer, sendBuffer);
			while (true)
			{
				if (this.ShouldReadData && this._clientSocket.Available > 0)
				{
					string requestMessage = SocketHelper.ReceiveMessage(this._clientSocket, receiveBuffer);
					IMessageHandler handler = handlerCreator.CreateHandlerFor(requestMessage);
					string responseMessage = handler.HandleMessage();
					SocketHelper.SendMessage(this._clientSocket, sendBuffer, responseMessage);
				}
			}

			string remoteEndpoint = this._clientSocket.RemoteEndPoint.ToString();
			if (this._server.UsernameByEndpoint.ContainsKey(remoteEndpoint))
			{
				this._server.RemoveOnlineUser(remoteEndpoint);
			}

			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Connection closed");
			this._clientSocket.Close();
		}
	}
}

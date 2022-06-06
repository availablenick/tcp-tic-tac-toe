using System;
using System.Collections.Generic;
using System.Diagnostics;
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
		public bool ClientIsConnected { get; set; }
		private string _remoteEndpoint;
		private Stopwatch _pingRequestStopwatch;
		private Stopwatch _pingResponseStopwatch;

		public ConnectionHandler(Server server, Socket clientSocket)
		{
			this.Server = server;
			this.ClientSocket = clientSocket;
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.ShouldReadData = true;
			this.ClientIsConnected = true;
			this._remoteEndpoint = this.ClientSocket.RemoteEndPoint.ToString();
			this._pingRequestStopwatch = new Stopwatch();
			this._pingResponseStopwatch = new Stopwatch();
		}

		public void HandleConnection()
		{
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] New client connected");
			MessageHandlerCreator handlerCreator = new MessageHandlerCreator(this);
			this._pingRequestStopwatch.Start();
			while (this.ClientIsConnected)
			{
				if (this.ShouldReadData)
				{
					if (this._pingResponseStopwatch.Elapsed.Seconds >= 5)
					{
						break;
					}

					try
					{
						if (this._pingRequestStopwatch.Elapsed.Seconds >= 5)
						{
							PingClient();
						}

						if (this.ClientSocket.Available > 0)
						{
							string message = SocketHelper.ReceiveMessage(
								this.ClientSocket, this.ReceiveBuffer);
							ProcessMessage(message, handlerCreator);
						}
					}
					catch (SocketException)
					{
						break;
					}
				}
			}

			CleanUp();
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Connection closed");
		}

		private void PingClient()
		{
			SocketHelper.SendMessage(this.ClientSocket, this.SendBuffer, "reqping");
			this._pingRequestStopwatch.Restart();
			this._pingResponseStopwatch.Start();
		}

		private void ProcessMessage(string message, MessageHandlerCreator handlerCreator)
		{
			this._pingResponseStopwatch.Stop();
			string[] queuedMessages = MessageHelper.GetQueuedMessagesFrom(message);
			foreach (string queuedMessage in queuedMessages)
			{
				if (String.Equals(queuedMessage, "resping\n"))
				{
					this._pingResponseStopwatch.Reset();
					continue;
				}

				IMessageHandler handler = handlerCreator.CreateHandlerFor(queuedMessage);
				string responseMessage = handler.HandleMessage();
				SocketHelper.SendMessage(this.ClientSocket, this.SendBuffer,
					responseMessage);
			}
		}

		private void CleanUp()
		{
			this._pingRequestStopwatch.Stop();
			this._pingResponseStopwatch.Stop();
			this.Server.RemoveConnectedEndpoint(this._remoteEndpoint);
			if (this.Server.UserIsOnline(this._remoteEndpoint))
			{
				this.Server.RemoveOnlineUser(this._remoteEndpoint);
			}

			try
			{
				this.ClientSocket.Shutdown(SocketShutdown.Both);
			}
			catch
			{
				this.ClientSocket.Close();
			}
		}
	}
}

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TicTacToe.ClientSide
{
	public class Client
	{
		public Socket ListeningSocket { get; set; }
		public Socket ServerSocket { get; }
		public Socket PeerSocket { get; set; }
		public Byte[] ReceiveBuffer { get; }
		public Byte[] SendBuffer { get; }
		public CommandParser CommandParser { get; }
		public MessageHandlerCreator MessageHandlerCreator { get; }
		public IUserState UserState { get; set; }
		public InputReader InputReader { get; }
		public Board Board { get; set; }
		public bool IsConnected { get; set; }

		public Client(string serverAddress, int serverPort)
		{
			Console.WriteLine($"Trying to connect to {serverAddress} on port {serverPort}...");

			this.ServerSocket = SocketHelper.CreateConnectionSocket(serverAddress,
				serverPort);
			if (this.ServerSocket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				Environment.Exit(0);
			}

			Console.WriteLine("Connection established");

			this.ListeningSocket = null;
			this.PeerSocket = null;
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.CommandParser = new CommandParser(this);
			this.MessageHandlerCreator = new MessageHandlerCreator(this);
			this.UserState = new LoggedOut(this);
			this.InputReader = new InputReader();
			this.Board = null;
			this.IsConnected = true;
		}

		public void HandleInput()
		{
			while (this.IsConnected)
			{
				this.UserState.HandleInput();
			}

			Console.WriteLine("End");
			this.ServerSocket.Close();
		}

		public void CheckForServerMessage()
		{
			if (this.ServerSocket.Available > 0)
			{
				string message = SocketHelper.ReceiveMessage(this.ServerSocket,
					this.ReceiveBuffer);
				ProcessMessage(message);
			}
		}

		private void ProcessMessage(string message)
		{
			string[] queuedMessages = MessageHelper.GetQueuedMessagesFrom(message);
			foreach (string queuedMessage in queuedMessages)
			{
				IMessageHandler handler = this.MessageHandlerCreator
					.CreateHandlerFor(queuedMessage);
				handler.HandleMessage();
			}
		}
	}
}

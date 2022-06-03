using System;
using System.Net.Sockets;
using System.Text;

namespace TicTacToe.ClientSide
{
	public class Client
	{
		public Socket ListeningSocket { get; set; }
		public Socket ServerSocket { get; }
		public Socket PeerSocket { get; set; }
		public Byte[] ReceiveBuffer { get; }
		public Byte[] SendBuffer { get; }
		public CommandParser Parser { get; }
		public MessageHandlerCreator HandlerCreator { get; }
		public IUserState UserState { get; set; }
		public InputReader Reader { get; }

		public Client(string serverAddress, int serverPort)
		{
			Console.WriteLine($"Trying to connect to {serverAddress} on port {serverPort}...");

			this.ServerSocket = SocketHelper.CreateConnectionSocket(serverAddress,
				serverPort);
			if (this.ServerSocket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				return;
			}

			Console.WriteLine("Connection established");

			this.ListeningSocket = null;
			this.PeerSocket = null;
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.Parser = new CommandParser(this);
			this.HandlerCreator = new MessageHandlerCreator(this);
			this.UserState = new LoggedOut(this);
			this.Reader = new InputReader();
		}

		public void HandleInput()
		{
			while (true)
			{
				bool shouldStopHandlingInput = this.UserState.HandleInput();
				if (shouldStopHandlingInput)
				{
					break;
				}
			}

			Console.WriteLine("End");
			this.ServerSocket.Close();
		}
	}
}

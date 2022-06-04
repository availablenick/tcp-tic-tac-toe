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
		public CommandParser CommandParser { get; }
		public MessageHandlerCreator MessageHandlerCreator { get; }
		public IUserState UserState { get; set; }
		public InputReader InputReader { get; }
		public Board Board { get; set; }

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
			this.CommandParser = new CommandParser(this);
			this.MessageHandlerCreator = new MessageHandlerCreator(this);
			this.UserState = new LoggedOut(this);
			this.InputReader = new InputReader();
			this.Board = null;
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

		public void UpdateBoard()
		{

		}
	}
}

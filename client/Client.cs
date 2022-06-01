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
		public MessageHandlerCreator HandlerCreator { get; }
		public IUserState UserState { get; set; }
		public Byte[] ReceiveBuffer { get; }
		public Byte[] SendBuffer { get; }
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
			this.HandlerCreator = new MessageHandlerCreator(this);
			this.UserState = new LoggedOut(this);
			this.ReceiveBuffer = new Byte[1024];
			this.SendBuffer = new Byte[1024];
			this.Reader = new InputReader();
		}

		public void HandleInput()
		{
			CommandParser parser = new CommandParser(this);
			Func<bool> didServerSendMessage = DidServerSendMessage;
			string line;
			while (true)
			{
				Console.Write("> ");
				line = this.Reader.ReadLine(didServerSendMessage);
				if (line == null)
				{
					continue;
				}

				try
				{
					Command command = parser.Parse(line.ToString());
					if (command != null)
					{
						this.UserState.ExecuteCommand(command);
					}
				}
				catch (InvalidCommandException exception)
				{
					Console.WriteLine(exception.Message);
				}
			}

			Console.WriteLine("End");
			this.ServerSocket.Close();
		}

		private bool DidServerSendMessage()
		{
			if (this.UserState is LoggedIn)
			{
				if (this.ServerSocket.Available > 0)
				{
					string serverMessage = SocketHelper.ReceiveMessage(
						this.ServerSocket, this.ReceiveBuffer);
					IMessageHandler handler = this.HandlerCreator
						.CreateHandlerFor(serverMessage);
					handler.HandleMessage();

					return true;	
				}
			}

			return false;
		}
	}
}

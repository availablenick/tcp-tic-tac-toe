using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class InviteResponseHandler : IMessageHandler
	{
		private string _data;
		private int _statusCode;
		private Client _client;

		public InviteResponseHandler(string data, string statusCode, Client client)
		{
			this._data = data;
			this._statusCode = Int32.Parse(statusCode);
			this._client = client;
		}

		public int HandleMessage()
		{
			string username;
			switch (this._statusCode)
			{
				case 0:
					string[] data = this._data.Split(';');
					username = data[0];
					string peerAddress = data[1];
					int peerPort = 0;
					try
					{
						peerPort = Int32.Parse(data[2]);
					}
					catch (FormatException)
					{
						return 1;
					}

					this._client.PeerSocket = SocketHelper.CreateConnectionSocket(
						peerAddress, peerPort);
					if (this._client.PeerSocket == null)
					{
						return 1;
					}

					Console.WriteLine($"You are now in a match with {username}. " +
						"Your mark is O");
					Console.WriteLine("Waiting for opponent's move...");

					return 0;
				case 1:
					username = this._data;
					Console.WriteLine($"User \"{username}\" is not online");
					return 1;
				case 2:
					username = this._data;
					Console.WriteLine($"User \"{username}\" declined your invitation");
					return 1;
				case 3:
					username = this._data;
					Console.WriteLine($"Error while inviting \"{username}\"");
					return 1;
				case 4:
					username = this._data;
					Console.WriteLine($"{username} took too long to reply");
					return 1;
				case 5:
					Console.WriteLine("You cannot invite yourself");
					return 1;
			}

			Console.WriteLine("Application error");
			return 1;
		}
	}
}

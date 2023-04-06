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

		public void HandleMessage()
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
						throw new CommandFailedException();
					}

					this._client.PeerSocket = SocketHelper.CreateConnectionSocket(
						peerAddress, peerPort);
					if (this._client.PeerSocket == null)
					{
						throw new CommandFailedException();
					}

					this._client.PeerSocket.Blocking = false;
					Console.WriteLine($"You are now in a match with {username}. " +
						"Your mark is O");
					Console.WriteLine("Waiting for opponent's move...");
					break;
				case 1:
					username = this._data;
					throw new CommandFailedException($"User \"{username}\" is not online");
				case 2:
					username = this._data;
					throw new CommandFailedException($"User \"{username}\" declined your invitation");
				case 3:
					username = this._data;
					throw new CommandFailedException($"Error while inviting \"{username}\"");
				case 4:
					username = this._data;
					throw new CommandFailedException($"{username} took too long to reply");
				case 5:
					throw new CommandFailedException("You cannot invite yourself");
				default:
					throw new CommandFailedException("Application error");
			}
		}
	}
}

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class InviteRequestHandler : IMessageHandler
	{
		private string _data;
		private Client _client;
		private Stopwatch _stopwatch;

		public InviteRequestHandler(string data, Client client)
		{
			this._data = data;
			this._client = client;
			this._stopwatch = new Stopwatch();
		}

		private bool IsTimeForReplyUp()
		{
			return this._stopwatch.Elapsed.Seconds >= 5;
		}

		public int HandleMessage()
		{
			string username = this._data;
			Console.Write($"\n{username} is inviting you for a match. Do you accept? [y/n] ");
			Func<bool> isTimeForReplyUp = IsTimeForReplyUp;
			this._stopwatch.Start();
			string answer = InputReader.ReadLine(isTimeForReplyUp);
			this._stopwatch.Stop();
			this._client.InputReader.Reset();
			this._client.InputReader.ShouldRead = false;

			if (answer == "")
			{
				Console.WriteLine("\nYou took too long to reply");
				return 1;
			}

			if (answer.ToLower() == "y")
			{
				this._client.ListeningSocket = SocketHelper.CreateListeningSocket(0);
				if (this._client.ListeningSocket == null)
				{
					SocketHelper.SendMessage(this._client.ServerSocket,
						this._client.SendBuffer, $"resinvite 0\n");
					return 1;
				}

				int assignedPort = ((IPEndPoint) this._client.ListeningSocket.LocalEndPoint).Port;
				SocketHelper.SendMessage(this._client.ServerSocket,
					this._client.SendBuffer, $"resinvite {assignedPort}\n");
				this._client.PeerSocket = this._client.ListeningSocket.Accept();
				this._client.PeerSocket.Blocking = false;
				this._client.UserState = new PlayingAsX(this._client);
				this._client.Board = new Board((char) 1);
				Console.WriteLine($"You are now in a match with {username}. " +
					"Your mark is X");
			}
			else
			{
				SocketHelper.SendMessage(this._client.ServerSocket,
					this._client.SendBuffer, "resinvite 0\n");
			}

			return 0;
		}
	}
}

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class InviteRequestHandler : IMessageHandler
	{
		private string _invitingUsername;
		private Client _client;
		private Stopwatch _stopwatch;

		public InviteRequestHandler(string invitingUsername, Client client)
		{
			this._invitingUsername = invitingUsername;
			this._client = client;
			this._stopwatch = new Stopwatch();
		}

		public void HandleMessage()
		{
			Console.Write($"\n{this._invitingUsername} is inviting you for a match. Do you accept? [y/n] ");
			this._stopwatch.Start();
			string answer = InputReader.GetInstance().ReadLine(ManageReplyTime);
			this._stopwatch.Stop();
			InputReader.GetInstance().DiscardInput();

			if (answer == "")
			{
				Console.WriteLine("\nYou took too long to reply");
			}
			else if (answer.ToLower() == "y")
			{
				SetUpP2PCommunication();
			}
			else
			{
				SocketHelper.SendMessage(this._client.ServerSocket,
					this._client.SendBuffer, "resinvite 0\n");
			}
		}

		private void ManageReplyTime()
		{
			bool timeForReplyIsUp = this._stopwatch.Elapsed.Seconds >= 5;
			if (timeForReplyIsUp)
			{
				InputReader.GetInstance().IsReading = false;
			}
		}

		private void SetUpP2PCommunication()
		{
			try
			{
				SetUpListeningSocket();
				SetUpPeerSocket();
				this._client.UserState = new PlayingAsX(this._client);
				this._client.Board = new Board((char) 1);
				Console.WriteLine(
					$"You are now in a match with {this._invitingUsername}. Your mark is X");
			}
			catch (SocketException)
			{
				SocketHelper.SendMessage(this._client.ServerSocket,
					this._client.SendBuffer, $"resinvite 0\n");
			}
		}

		private void SetUpListeningSocket()
		{
			this._client.ListeningSocket = SocketHelper.CreateListeningSocket(0);
			if (this._client.ListeningSocket == null)
			{
				throw new SocketException();
			}
		}

		private void SetUpPeerSocket()
		{
			int assignedPort = ((IPEndPoint) this._client.ListeningSocket.LocalEndPoint).Port;
			SocketHelper.SendMessage(this._client.ServerSocket,
				this._client.SendBuffer, $"resinvite {assignedPort}\n");
			this._client.PeerSocket = this._client.ListeningSocket.Accept();
			this._client.PeerSocket.Blocking = false;
		}
	}
}

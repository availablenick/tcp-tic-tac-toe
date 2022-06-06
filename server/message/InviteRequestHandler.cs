using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace TicTacToe.ServerSide
{
	public class InviteRequestHandler : IMessageHandler
	{
		private string _data;
		private ConnectionHandler _connectionHandler;

		public InviteRequestHandler(string data, ConnectionHandler connectionHandler)
		{
			this._data = data;
			this._connectionHandler = connectionHandler;
		}

		public string HandleMessage()
		{
			string invitedUserUsername = this._data;
			if (invitedUserUsername == GetInvitingUserUsername())
			{
				return $"resinvite {invitedUserUsername} 5\n";
			}

			string responseMessage = $"resinvite {invitedUserUsername} 1\n";
			if (this._connectionHandler.Server.UserIsOnline(invitedUserUsername))
			{
				Socket invitedUserSocket = GetInvitedUserSocket(invitedUserUsername);
				string invitedUserEndpoint = this._connectionHandler.Server.EndpointByUsername[invitedUserUsername];
				this._connectionHandler.Server.NotifyThread(invitedUserEndpoint, false);
				AskInvitedUser(invitedUserSocket);
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				while (true)
				{
					if (stopwatch.Elapsed.Seconds >= 5)
					{
						stopwatch.Stop();
						responseMessage = $"resinvite {invitedUserUsername} 4\n";
						break;
					}

					if (invitedUserSocket.Available > 0)
					{
						string invitationReply = SocketHelper.ReceiveMessage(
							invitedUserSocket, this._connectionHandler.ReceiveBuffer);
						responseMessage = CreateResponseMessage(invitationReply);
						break;
					}
				}

				this._connectionHandler.Server.NotifyThread(invitedUserEndpoint, true);
			}

			return responseMessage;
		}

		private void AskInvitedUser(Socket invitedUserSocket)
		{
			string invitingUserUsername = GetInvitingUserUsername();
			string invitationMessage = $"reqinvite {invitingUserUsername}\n";
			SocketHelper.SendMessage(invitedUserSocket,
				this._connectionHandler.SendBuffer, invitationMessage);
		}

		private string GetInvitingUserUsername()
		{
			string invitingUserEndpoint = this._connectionHandler.ClientSocket.RemoteEndPoint.ToString();
			return this._connectionHandler.Server.UsernameByEndpoint[invitingUserEndpoint];
		}

		private Socket GetInvitedUserSocket(string invitedUserUsername)
		{
			string invitedUserEndpoint = this._connectionHandler.Server.EndpointByUsername[invitedUserUsername];
			return this._connectionHandler.Server.SocketByEndpoint[invitedUserEndpoint];
		}

		private string CreateResponseMessage(string invitationReply)
		{
			string invitedUserUsername = this._data;
			string invitedUserEndpoint = this._connectionHandler.Server.EndpointByUsername[invitedUserUsername];
			string[] messageMembers = MessageHandlerCreator.ParseMessage(invitationReply);
			if (messageMembers != null)
			{
				string messageType = messageMembers[0];
				if (messageType == "resinvite")
				{
					string data = messageMembers[1];
					int invitedUserListeningPort = 0;
					try
					{
						invitedUserListeningPort = Int32.Parse(data);
					}
					catch (FormatException)
					{
						return "resinvite 0;0 3\n";
					}

					if (invitedUserListeningPort == 0)
					{
						return $"resinvite {invitedUserUsername} 2\n";
					}

					string invitedUserIPAddress = invitedUserEndpoint.Split(":")[0];
					return $"resinvite {invitedUserUsername};{invitedUserIPAddress};{invitedUserListeningPort} 0\n";
				}
			}

			return "resinvite 0;0 3\n";
		}
	}
}

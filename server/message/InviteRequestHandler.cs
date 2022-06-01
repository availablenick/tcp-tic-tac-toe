using System;
using System.Diagnostics;
using System.Net.Sockets;

namespace TicTacToe.ServerSide
{
	public class InviteRequestHandler : IMessageHandler
	{
		private string _data;
		private Server _server;
		private Socket _clientSocket;
		private Byte[] _receiveBuffer;
		private Byte[] _sendBuffer;

		public InviteRequestHandler(string data, Server server,
			Socket clientSocket, Byte[] receiveBuffer, Byte[] sendBuffer)
		{
			this._data = data;
			this._server = server;
			this._clientSocket = clientSocket;
			this._receiveBuffer = receiveBuffer;
			this._sendBuffer = sendBuffer;
		}

		public string HandleMessage()
		{
			string invitedUserUsername = this._data;
			if (invitedUserUsername == GetInvitingUserUsername())
			{
				return $"resinvite {invitedUserUsername} 5";
			}

			string responseMessage = $"resinvite {invitedUserUsername} 1";
			if (this._server.IsUserOnline(invitedUserUsername))
			{
				Socket invitedUserSocket = GetInvitedUserSocket(invitedUserUsername);
				string invitedUserEndpoint = this._server.EndpointByUsername[invitedUserUsername];
				this._server.NotifyThread(invitedUserEndpoint, false);
				AskInvitedUser(invitedUserSocket);
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				while (true)
				{
					if (stopwatch.Elapsed.Seconds >= 5)
					{
						stopwatch.Stop();
						responseMessage = $"resinvite {invitedUserUsername} 4";
						break;
					}

					if (invitedUserSocket.Available > 0)
					{
						string invitationReply = SocketHelper.ReceiveMessage(invitedUserSocket,
							this._receiveBuffer);
						responseMessage = CreateResponseMessage(invitationReply);
						break;
					}
				}

				this._server.NotifyThread(invitedUserEndpoint, true);
			}

			return responseMessage;
		}

		private void AskInvitedUser(Socket invitedUserSocket)
		{
			string invitingUserUsername = GetInvitingUserUsername();
			string invitationMessage = $"reqinvite {invitingUserUsername}";
			SocketHelper.SendMessage(invitedUserSocket, this._sendBuffer, invitationMessage);
		}

		private string GetInvitingUserUsername()
		{
			string invitingUserEndpoint = this._clientSocket.RemoteEndPoint.ToString();
			return this._server.UsernameByEndpoint[invitingUserEndpoint];
		}

		private Socket GetInvitedUserSocket(string invitedUserUsername)
		{
			string invitedUserEndpoint = this._server.EndpointByUsername[invitedUserUsername];
			return this._server.SocketByEndpoint[invitedUserEndpoint];
		}

		private string CreateResponseMessage(string invitationReply)
		{
			string invitedUserUsername = this._data;
			string invitedUserEndpoint = this._server.EndpointByUsername[invitedUserUsername];
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
						return "resinvite 0;0 3";
					}

					if (invitedUserListeningPort == 0)
					{
						return $"resinvite {invitedUserUsername} 2";
					}

					string invitedUserIPAddress = invitedUserEndpoint.Split(":")[0];
					return $"resinvite {invitedUserUsername};{invitedUserIPAddress};{invitedUserListeningPort} 0";
				}
			}

			return "resinvite 0;0 3";
		}
	}
}

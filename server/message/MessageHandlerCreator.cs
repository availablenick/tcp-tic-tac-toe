using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class MessageHandlerCreator
	{
		private Server _server;
		private Socket _clientSocket;
		private Byte[] _receiveBuffer { get; }
		private Byte[] _sendBuffer { get; }

		public MessageHandlerCreator(Server server, Socket clientSocket,
			Byte[] receiveBuffer, Byte[] sendBuffer)
		{
			this._server = server;
			this._clientSocket = clientSocket;
			this._receiveBuffer = receiveBuffer;
			this._sendBuffer = sendBuffer;
		}

		public static string[] ParseMessage(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?");
			MatchCollection matches = regex.Matches(message);
			if (matches.Count > 0) {
				string messageType = matches[0].Groups[1].Value.ToLower();
				string data = matches[0].Groups[3].Value;
				return new string[] { messageType, data };
			}

			return null;
		}

		public IMessageHandler CreateHandlerFor(string message)
		{
			string[] messageMembers = ParseMessage(message);
			if (messageMembers != null)
			{
				string messageType = messageMembers[0];
				string data = messageMembers[1];
				switch (messageType)
				{
					case "reqinvite":
						if (data != "")
						{
							return new InviteRequestHandler(data, this._server,
								this._clientSocket, this._receiveBuffer,
								this._sendBuffer);
						}

						break;
					case "reqlist":
						return new ListRequestHandler(this._server,
							this._clientSocket);

					case "reqlogin":
						if (data != "")
						{
							return new LoginRequestHandler(data, this._server,
								this._clientSocket);
						}

						break;
					case "reqlogout":
						return new LogoutRequestHandler(this._server,
							this._clientSocket);

					case "reqregister":
						if (data != "")
						{
							return new RegisterRequestHandler(data,
								this._server.MutexLock);
						}

						break;
				}
			}

			return new InvalidRequestHandler();
		}
	}
}

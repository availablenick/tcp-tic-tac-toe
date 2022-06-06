using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TicTacToe.ServerSide
{
	public class MessageHandlerCreator
	{
		private ConnectionHandler _connectionHandler;

		public MessageHandlerCreator(ConnectionHandler connectionHandler)
		{
			this._connectionHandler = connectionHandler;
		}

		public static string[] ParseMessage(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?\n$");
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
							return new InviteRequestHandler(data,
								this._connectionHandler);
						}

						break;
					case "reqlist":
						return new ListRequestHandler(this._connectionHandler.Server);

					case "reqlogin":
						if (data != "")
						{
							return new LoginRequestHandler(data,
								this._connectionHandler.Server,
								this._connectionHandler.ClientSocket);
						}

						break;
					case "reqlogout":
						return new LogoutRequestHandler(this._connectionHandler.Server,
							this._connectionHandler.ClientSocket);

					case "reqregister":
						if (data != "")
						{
							return new RegisterRequestHandler(data,
								this._connectionHandler.Server.Mutex);
						}

						break;
				}
			}

			return new InvalidRequestHandler();
		}
	}
}

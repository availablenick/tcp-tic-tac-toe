using System;
using System.Text.RegularExpressions;

namespace TicTacToe.ClientSide
{
	public class MessageHandlerCreator
	{
		private Client _client;

		public MessageHandlerCreator(Client client)
		{
			this._client = client;
		}

		public static string[] ParseMessage(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?(\s+([\x21-\x80]+))?\n$");
			MatchCollection matches = regex.Matches(message);
			if (matches.Count > 0) {
				string messageType = matches[0].Groups[1].Value.ToLower();
				string data = matches[0].Groups[3].Value;
				string statusCode = matches[0].Groups[5].Value;
				return new string[] { messageType, data, statusCode };
			}

			return null;
		}

		public IMessageHandler CreateHandlerFor(string message)
		{
			string[] messageMembers = ParseMessage(message);
			if (messageMembers != null)
			{
				string responseType = messageMembers[0];
				string data;
				string statusCode;
				switch (responseType)
				{
					case "reqinvite":
						data = messageMembers[1];
						if (data != "")
						{
							return new InviteRequestHandler(data, this._client);
						}

						break;
					case "reqping":
						return new PingRequestHandler(this._client);

					case "reqquit":
						return new QuitRequestHandler(this._client);

					case "reqsend":
						data = messageMembers[1];
						if (data != "")
						{
							return new SendRequestHandler(data, this._client);
						}

						break;

					case "resinvite":
						data = messageMembers[1];
						statusCode = messageMembers[2];
						if (data != "" && statusCode != "")
						{
							return new InviteResponseHandler(data, statusCode,
								this._client);
						}

						break;
					case "reslist":
						data = messageMembers[1];
						statusCode = messageMembers[2];
						if (data != "" && statusCode != "")
						{
							return new ListResponseHandler(data, statusCode);
						}

						break;
					case "reslogin":
						statusCode = messageMembers[1];
						if (statusCode != "")
						{
							return new LoginResponseHandler(statusCode);
						}

						break;
					case "reslogout":
						statusCode = messageMembers[1];
						if (statusCode != "")
						{
							return new LogoutResponseHandler(statusCode);
						}

						break;
					case "resregister":
						statusCode = messageMembers[1];
						if (statusCode != "")
						{
							return new RegisterResponseHandler(statusCode);
						}

						break;
				}
			}

			return new InvalidResponseHandler();
		}
	}
}

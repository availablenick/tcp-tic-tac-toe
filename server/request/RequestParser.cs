using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using System.Threading;

namespace TicTacToe.ServerSide
{
	public class RequestParser
	{
		private Socket _clientSocket;
		private Mutex _mutex;
		private Dictionary<string, string> _usernameByEndpoint;
		private Dictionary<string, string> _endpointByUsername;

		public RequestParser(Socket socket, Mutex mutex,
			Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername)
		{
			this._clientSocket = socket;
			this._mutex = mutex;
			this._usernameByEndpoint = usernameByEndpoint;
			this._endpointByUsername = endpointByUsername;
		}

		public Request Parse(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?");
			MatchCollection matches = regex.Matches(message);
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string requestType = groups[1].Value.ToLower();
				switch (requestType)
				{
					case "list":
						if (MessageHelper.HasCorrectNumberOfParameters(
								ListRequest.NumberOfParameters, groups))
						{
							return new ListRequest(MessageHelper.CreateParameterArray(
								ListRequest.NumberOfParameters, groups),
								this._clientSocket, this._mutex,
								this._endpointByUsername);
						}

						break;
					case "login":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LoginRequest.NumberOfParameters, groups))
						{
							return new LoginRequest(MessageHelper.CreateParameterArray(
								LoginRequest.NumberOfParameters, groups),
								this._clientSocket, this._mutex,
								this._usernameByEndpoint, this._endpointByUsername);
						}

						break;
					case "logout":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LogoutRequest.NumberOfParameters, groups))
						{
							return new LogoutRequest(MessageHelper.CreateParameterArray(
								LogoutRequest.NumberOfParameters, groups),
								this._clientSocket, this._mutex,
								this._usernameByEndpoint, this._endpointByUsername);
						}

						break;
					case "register":
						if (MessageHelper.HasCorrectNumberOfParameters(
								RegisterRequest.NumberOfParameters, groups))
						{
							return new RegisterRequest(MessageHelper.CreateParameterArray(
								RegisterRequest.NumberOfParameters, groups),
								this._mutex);
						}

						break;
				}
			}

			return new InvalidRequest();
		}
	}
}

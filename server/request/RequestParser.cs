using System;
using System.Text.RegularExpressions;

namespace TicTacToe.Server
{
	public class RequestParser
	{
		public static Request Parse(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?(\s+([\x21-\x80]+))?");
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
								ListRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					case "login":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LoginRequest.NumberOfParameters, groups))
						{
							return new LoginRequest(MessageHelper.CreateParameterArray(
								LoginRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					case "logout":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LogoutRequest.NumberOfParameters, groups))
						{
							return new LogoutRequest(MessageHelper.CreateParameterArray(
								LogoutRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					case "register":
						if (MessageHelper.HasCorrectNumberOfParameters(
								RegisterRequest.NumberOfParameters, groups))
						{
							return new RegisterRequest(MessageHelper.CreateParameterArray(
								RegisterRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					default:
						throw new InvalidRequestException();
				}
			}

			return null;
		}
	}
}

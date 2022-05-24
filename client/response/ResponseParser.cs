using System;
using System.Text.RegularExpressions;

namespace TicTacToe.Client
{
	public class ResponseParser
	{
		public static Response Parse(string message)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?(\s+([\x21-\x80]+))?");
			MatchCollection matches = regex.Matches(message);
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string responseType = groups[1].Value.ToLower();
				switch (responseType)
				{
					case "list":
						if (MessageHelper.HasCorrectNumberOfParameters(
								ListResponse.NumberOfParameters, groups))
						{
							return new ListResponse(MessageHelper.CreateParameterArray(
								ListResponse.NumberOfParameters, groups));
						}

						break;
					case "login":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LoginResponse.NumberOfParameters, groups))
						{
							return new LoginResponse(MessageHelper.CreateParameterArray(
								LoginResponse.NumberOfParameters, groups));
						}

						break;
					case "logout":
						if (MessageHelper.HasCorrectNumberOfParameters(
								RegisterResponse.NumberOfParameters, groups))
						{
							return new LogoutResponse(MessageHelper.CreateParameterArray(
								LogoutResponse.NumberOfParameters, groups));
						}

						break;
					case "register":
						if (MessageHelper.HasCorrectNumberOfParameters(
								RegisterResponse.NumberOfParameters, groups))
						{
							return new RegisterResponse(MessageHelper.CreateParameterArray(
								RegisterResponse.NumberOfParameters, groups));
						}

						break;
				}
			}

			return new InvalidResponse();
		}
	}
}

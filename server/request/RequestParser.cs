using System;
using System.Text.RegularExpressions;

namespace TicTacToe.Server
{
	public class RequestParser
	{
		private static bool HasCorrectNumberOfParameters(
			int numberOfParameters, GroupCollection groups)
		{
			for (int i = 1; i <= numberOfParameters; i++)
			{
				if (groups[2*i + 1].Value == "")
				{
					return false;
				}
			}

			return true;
		}

		private static string[] CreateParameterArray(
			int numberOfParameters, GroupCollection groups)
		{
			string[] parameters = new string[numberOfParameters];
			for (int i = 0; i < numberOfParameters; i++)
			{
				parameters[i] = groups[2*(i+1) + 1].Value;
			}

			return parameters;
		}

		public static Request Parse(string message)
		{
			Regex regex = new Regex(@"(\w+)(\s+(\w+))?(\s+(\w+))?");
			MatchCollection matches = regex.Matches(message);
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string requestType = groups[1].Value.ToLower();
				switch (requestType)
				{
					case "login":
						if (HasCorrectNumberOfParameters(
								LoginRequest.NumberOfParameters, groups))
						{
							return new LoginRequest(CreateParameterArray(
								LoginRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					case "logout":
						if (HasCorrectNumberOfParameters(
								LogoutRequest.NumberOfParameters, groups))
						{
							return new LogoutRequest(CreateParameterArray(
								LogoutRequest.NumberOfParameters, groups));
						}

						throw new InvalidRequestException();

					case "register":
						if (HasCorrectNumberOfParameters(
								RegisterRequest.NumberOfParameters, groups))
						{
							return new RegisterRequest(CreateParameterArray(
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

using System;
using System.Text.RegularExpressions;

namespace TicTacToe.Client
{
	public class ResponseParser
	{
		public static Response Parse(string message)
		{
			Regex regex = new Regex(@"(\w+)(\s+([0-9]+))?");
			MatchCollection matches = regex.Matches(message);
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string responseType = groups[1].Value.ToLower();
				switch (responseType)
				{
					case "login":
						return new LoginResponse(Int32.Parse(groups[3].Value));
					case "logout":
						return new LogoutResponse(Int32.Parse(groups[3].Value));
					case "register":
						return new RegisterResponse(Int32.Parse(groups[3].Value));
				}
			}

			return null;
		}
	}
}

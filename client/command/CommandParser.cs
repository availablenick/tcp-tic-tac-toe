using System;
using System.Text.RegularExpressions;

namespace TicTacToe.Client
{
	public class CommandParser
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

		public static Command Parse(string input)
		{
			Regex regex = new Regex(@"(\w+)(\s+(\w+))?(\s+(\w+))?");
			MatchCollection matches = regex.Matches(input);
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string command = groups[1].Value.ToLower();
				switch (command)
				{
					case "register":
						if (HasCorrectNumberOfParameters(
							RegisterCommand.NumberOfParameters, groups))
						{
							return new RegisterCommand(CreateParameterArray(
								RegisterCommand.NumberOfParameters, groups));
						}

						throw new InvalidCommandException(
							RegisterCommand.WrongNumberOfParametersMessage);
					default:
						throw new InvalidCommandException($"Command \"{command}\" not recognized");
				}
			}

			return null;
		}
	}
}

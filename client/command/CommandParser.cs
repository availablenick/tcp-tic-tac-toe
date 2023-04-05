using System;
using System.Text.RegularExpressions;

namespace TicTacToe.ClientSide
{
	public class CommandParser
	{
		private Client _client;

		public CommandParser(Client client)
		{
			this._client = client;
		}

		public Command Parse(string input)
		{
			Regex regex = new Regex(@"([\x21-\x80]+)(\s+([\x21-\x80]+))?(\s+([\x21-\x80]+))?");
			MatchCollection matches = regex.Matches(input);
			if (matches.Count > 0) {
				GroupCollection groups = matches[0].Groups;
				string command = groups[1].Value.ToLower();
				switch (command)
				{
					case "exit":
						if (HasCorrectNumberOfParameters(
								ExitCommand.NumberOfParameters, groups))
						{
							return new ExitCommand(CreateParameterArray(
								ExitCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(ExitCommand.WrongNumberOfParametersMessage);

					case "invite":
						if (HasCorrectNumberOfParameters(
								InviteCommand.NumberOfParameters, groups))
						{
							return new InviteCommand(CreateParameterArray(
								InviteCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(InviteCommand.WrongNumberOfParametersMessage);

					case "list":
						if (HasCorrectNumberOfParameters(
								ListCommand.NumberOfParameters, groups))
						{
							return new ListCommand(CreateParameterArray(
								ListCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(ListCommand.WrongNumberOfParametersMessage);

					case "login":
						if (HasCorrectNumberOfParameters(
								LoginCommand.NumberOfParameters, groups))
						{
							return new LoginCommand(CreateParameterArray(
								LoginCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(LoginCommand.WrongNumberOfParametersMessage);

					case "logout":
						if (HasCorrectNumberOfParameters(
								LogoutCommand.NumberOfParameters, groups))
						{
							return new LogoutCommand(CreateParameterArray(
								LogoutCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(LogoutCommand.WrongNumberOfParametersMessage);

					case "quit":
						if (HasCorrectNumberOfParameters(
								QuitCommand.NumberOfParameters, groups))
						{
							return new QuitCommand(CreateParameterArray(
								QuitCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(QuitCommand.WrongNumberOfParametersMessage);

					case "register":
						if (HasCorrectNumberOfParameters(
								RegisterCommand.NumberOfParameters, groups))
						{
							return new RegisterCommand(CreateParameterArray(
								RegisterCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(RegisterCommand.WrongNumberOfParametersMessage);

					case "send":
						if (HasCorrectNumberOfParameters(
								SendCommand.NumberOfParameters, groups))
						{
							return new SendCommand(CreateParameterArray(
								SendCommand.NumberOfParameters, groups),
								this._client);
						}

						return new InvalidCommand(SendCommand.WrongNumberOfParametersMessage);

					default:
						return new InvalidCommand($"Command \"{command}\" not recognized");
				}
			}

			return new InvalidCommand("");
		}

		private bool HasCorrectNumberOfParameters(
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

		private string[] CreateParameterArray(
			int numberOfParameters, GroupCollection groups)
		{
			string[] parameters = new string[numberOfParameters];
			for (int i = 0; i < numberOfParameters; i++)
			{
				parameters[i] = groups[2*(i+1) + 1].Value;
			}

			return parameters;
		}
	}
}

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
			foreach (Match match in matches)
			{
				GroupCollection groups = match.Groups;
				string command = groups[1].Value.ToLower();
				switch (command)
				{
					case "list":
						if (MessageHelper.HasCorrectNumberOfParameters(
								ListCommand.NumberOfParameters, groups))
						{
							return new ListCommand(MessageHelper.CreateParameterArray(
								ListCommand.NumberOfParameters, groups),
								this._client.ServerSocket, this._client.ReceiveBuffer,
								this._client.SendBuffer);
						}

						throw new InvalidCommandException(
							ListCommand.WrongNumberOfParametersMessage);

					case "login":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LoginCommand.NumberOfParameters, groups))
						{
							return new LoginCommand(MessageHelper.CreateParameterArray(
								LoginCommand.NumberOfParameters, groups),
								this._client.ServerSocket, this._client.ReceiveBuffer,
								this._client.SendBuffer);
						}

						throw new InvalidCommandException(
							LoginCommand.WrongNumberOfParametersMessage);

					case "logout":
						if (MessageHelper.HasCorrectNumberOfParameters(
								LogoutCommand.NumberOfParameters, groups))
						{
							return new LogoutCommand(MessageHelper.CreateParameterArray(
								LogoutCommand.NumberOfParameters, groups),
								this._client.ServerSocket, this._client.ReceiveBuffer,
								this._client.SendBuffer);
						}

						throw new InvalidCommandException(
							LogoutCommand.WrongNumberOfParametersMessage);

					case "register":
						if (MessageHelper.HasCorrectNumberOfParameters(
								RegisterCommand.NumberOfParameters, groups))
						{
							return new RegisterCommand(MessageHelper.CreateParameterArray(
								RegisterCommand.NumberOfParameters, groups),
								this._client.ServerSocket, this._client.ReceiveBuffer,
								this._client.SendBuffer);
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

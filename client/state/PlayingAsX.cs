using System;

namespace TicTacToe.ClientSide
{
	public class PlayingAsX : IUserState
	{
		private Client _client;

		public PlayingAsX(Client client)
		{
			this._client = client;
		}

		public void HandleInput()
		{
			Console.WriteLine("It is your turn");
			Action checkForServerMessage = this._client.CheckForServerMessage;
			while (true)
			{
				Console.Write("> ");
				string line = this._client.InputReader.ReadLine(checkForServerMessage);
				Command command = this._client.CommandParser.Parse(line);

				try
				{
					ExecuteCommand(command);
					break;
				}
				catch (CommandFailedException exception)
				{
					Console.WriteLine(exception.Message);
				}
			}

			if (this._client.PeerSocket == null)
			{
				return;
			}

			while (true)
			{
				this._client.CheckForServerMessage();
				if (this._client.PeerSocket.Available > 0)
				{
					string peerMessage = SocketHelper.ReceiveMessage(
						this._client.PeerSocket, this._client.ReceiveBuffer);
					IMessageHandler handler = this._client.MessageHandlerCreator
						.CreateHandlerFor(peerMessage);
					handler.HandleMessage();

					break;
				}
			}
		}

		private void ExecuteCommand(Command command)
		{
			if (command is ExitCommand)
			{
				throw new CommandFailedException("You must either quit or finish your match first");
			}
			else if (command is InviteCommand)
			{
				throw new CommandFailedException("You cannot invite another player during a match");
			}
			else if (command is ListCommand)
			{
				throw new CommandFailedException("You cannot access the online user list during a match");
			}
			else if (command is LoginCommand)
			{
				throw new CommandFailedException("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				throw new CommandFailedException("You cannot log out during a match");
			}
			else if (command is RegisterCommand)
			{
				throw new CommandFailedException("You cannot register during a match");
			}

			command.Execute();
		}
	}
}

using System;

namespace TicTacToe.ClientSide
{
	public class PlayingAsO : IUserState
	{
		private Client _client;

		public PlayingAsO(Client client)
		{
			this._client = client;
		}

		public bool HandleInput()
		{
			string peerMessage = SocketHelper.ReceiveMessage(
				this._client.PeerSocket, this._client.ReceiveBuffer);
			IMessageHandler handler = this._client.HandlerCreator
				.CreateHandlerFor(peerMessage);
			handler.HandleMessage();
			if (this._client.PeerSocket == null)
			{
				return false;
			}

			Console.Write("> ");
			string line = Console.ReadLine();
			if (line == null)
			{
				return true;
			}

			try
			{
				Command command = this._client.Parser.Parse(line);
				if (command != null)
				{
					ExecuteCommand(command);
				}
			}
			catch (InvalidCommandException exception)
			{
				Console.WriteLine(exception.Message);
			}

			return false;
		}

		private void ExecuteCommand(Command command)
		{
			if (command is InviteCommand)
			{
				Console.WriteLine("You cannot invite another player during a match");
			}
			else if (command is ListCommand)
			{
				Console.WriteLine("You cannot access the online user list during a match");
			}
			else if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				Console.WriteLine("You cannot log out during a match");
			}
			else if (command is QuitCommand)
			{
				command.Execute();
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You cannot register during a match");
			}
		}
	}
}

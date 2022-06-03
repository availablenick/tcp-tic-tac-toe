using System;

namespace TicTacToe.ClientSide
{
	public class LoggedIn : IUserState
	{
		private Client _client;

		public LoggedIn(Client client)
		{
			this._client = client;
		}

		public bool HandleInput()
		{
			Func<bool> didReceiveServerMessage = DidReceiveServerMessage;
			Console.Write("> ");
			string line = this._client.Reader.ReadLine(didReceiveServerMessage);			
			if (line == null)
			{
				return false;
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

		private bool DidReceiveServerMessage()
		{
			if (this._client.ServerSocket.Available > 0)
			{
				string serverMessage = SocketHelper.ReceiveMessage(
					this._client.ServerSocket, this._client.ReceiveBuffer);
				IMessageHandler handler = this._client.HandlerCreator
					.CreateHandlerFor(serverMessage);
				handler.HandleMessage();

				return true;
			}

			return false;
		}

		private void ExecuteCommand(Command command)
		{
			if (command is InviteCommand)
			{
				command.Execute();
			}
			else if (command is ListCommand)
			{
				command.Execute();
			}
			else if (command is LoginCommand)
			{
				Console.WriteLine("You are already logged in");
			}
			else if (command is LogoutCommand)
			{
				command.Execute();
			}
			else if (command is RegisterCommand)
			{
				Console.WriteLine("You need to log out first");
			}
		}
	}
}

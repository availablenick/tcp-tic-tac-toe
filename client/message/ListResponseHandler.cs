using System;

namespace TicTacToe.ClientSide
{
	public class ListResponseHandler : IMessageHandler
	{
		private string _data;
		private int _statusCode;

		public ListResponseHandler(string data, string statusCode)
		{
			this._data = data;
			this._statusCode = Int32.Parse(statusCode);
		}

		public void HandleMessage()
		{
			switch (this._statusCode)
			{
				case 0:
					string[] usernames = this._data.Split(';', StringSplitOptions.RemoveEmptyEntries);
					foreach (string username in usernames)
					{
						Console.WriteLine(username);
					}

					break;
				default:
					throw new CommandFailedException("Application error");
			}
		}
	}
}

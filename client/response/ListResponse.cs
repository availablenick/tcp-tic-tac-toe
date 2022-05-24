using System;
using System.Text;

namespace TicTacToe.Client
{
	public class ListResponse : Response
	{
		public const int NumberOfParameters = 2;
		public ListResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			switch (this.StatusCode)
			{
				case 0:
					string[] usernames = this.Data.Split(';', StringSplitOptions.RemoveEmptyEntries);
					StringBuilder message = new StringBuilder(1024);
					foreach (string username in usernames)
					{
						message.Append($"{username}\n");
					}

					message.Remove(message.Length - 1, 1);

					return message.ToString();
			}

			return "Application error";
		}
	}
}

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
			int statusCode = Int32.Parse(this.Parameters[NumberOfParameters - 1]);
			switch (statusCode)
			{
				case 0:
					string[] usernames = this.Parameters[0].Split(';', StringSplitOptions.RemoveEmptyEntries);
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

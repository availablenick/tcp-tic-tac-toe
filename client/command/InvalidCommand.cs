using System;

namespace TicTacToe.ClientSide
{
	public class InvalidCommand : Command
	{
		private string _message;

		public InvalidCommand(string message) : base(new string[] {}) {
			this._message = message;
		}

		public override int Execute()
		{
      Console.WriteLine(this._message);
			return 0;
		}
	}
}

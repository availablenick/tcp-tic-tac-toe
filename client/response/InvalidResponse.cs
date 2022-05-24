using System;
using System.Text;

namespace TicTacToe.Client
{
	public class InvalidResponse : Response
	{
		public const int NumberOfParameters = 0;
		public InvalidResponse(params string[] parameters) : base(parameters) { }
		public override string ToString()
		{
			return "Received an invalid response from server";
		}
	}
}

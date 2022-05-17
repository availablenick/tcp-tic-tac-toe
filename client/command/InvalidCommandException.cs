using System;
using System.Runtime.Serialization;

namespace TicTacToe.Client
{
	[Serializable]
	public class InvalidCommandException : Exception
	{
		public InvalidCommandException() : base() { }
		public InvalidCommandException(string message) : base(message) { }
    	public InvalidCommandException(string message, Exception inner) : base(message, inner) { }
		protected InvalidCommandException(SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}

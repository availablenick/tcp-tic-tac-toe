using System;
using System.Runtime.Serialization;

namespace TicTacToe.Server
{
	[Serializable]
	public class InvalidRequestException : Exception
	{
		public InvalidRequestException() : base() { }
		public InvalidRequestException(string message) : base(message) { }
    	public InvalidRequestException(string message, Exception inner) : base(message, inner) { }
		protected InvalidRequestException(SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}

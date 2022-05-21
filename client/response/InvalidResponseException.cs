using System;
using System.Runtime.Serialization;

namespace TicTacToe.Client
{
	[Serializable]
	public class InvalidResponseException : Exception
	{
		public InvalidResponseException() : base() { }
		public InvalidResponseException(string message) : base(message) { }
    	public InvalidResponseException(string message, Exception inner) : base(message, inner) { }
		protected InvalidResponseException(SerializationInfo info,
			StreamingContext context) : base(info, context) { }
	}
}

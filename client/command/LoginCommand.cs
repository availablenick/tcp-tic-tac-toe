using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class LoginCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for login command. Usage: " +
			"login <username> <password>";
		public LoginCommand(params string[] parameters) : base(parameters) { }
		public override int TakeEffect(Socket socket, Byte[] receiveBuffer,
			Byte[] sendBuffer)
		{
			string requestMessage = $"login {this.Parameters[0]} {this.Parameters[1]}";
			BufferHelper.WriteMessageToBuffer(sendBuffer, requestMessage);
			socket.Send(sendBuffer, requestMessage.Length, 0);
			Console.WriteLine($"Client message: {requestMessage}");
			int numberOfReceivedBytes = socket.Receive(receiveBuffer, receiveBuffer.Length, 0);
			string responseMessage = BufferHelper.GetBufferMessage(receiveBuffer, numberOfReceivedBytes);
			Console.WriteLine($"Server reply: {responseMessage}");
			Response response = ResponseParser.Parse(BufferHelper.GetBufferMessage(receiveBuffer, numberOfReceivedBytes));
			Console.WriteLine(response.ParseStatusCode());

			return response.StatusCode;
		}
	}
}

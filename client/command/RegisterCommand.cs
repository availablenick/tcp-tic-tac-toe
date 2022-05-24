using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class RegisterCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for register command. Usage: " +
			"register <username> <password>";
		public RegisterCommand(params string[] parameters) : base(parameters) { }
		public override int Execute(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer)
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			string requestMessage = $"register {username};{password}";
			BufferHelper.WriteMessageToBuffer(sendBuffer, requestMessage);
			serverSocket.Send(sendBuffer, requestMessage.Length, 0);

			int numberOfReceivedBytes = serverSocket.Receive(receiveBuffer,
				receiveBuffer.Length, 0);
			string responseMessage = BufferHelper.GetBufferMessage(receiveBuffer,
				numberOfReceivedBytes);

			Response response = ResponseParser.Parse(responseMessage);
			Console.WriteLine(response.ToString());

			return response.StatusCode;
		}
	}
}

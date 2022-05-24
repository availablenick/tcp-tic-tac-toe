using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class LogoutCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for logout command. Usage: logout";
		public LogoutCommand(params string[] parameters) : base(parameters) { }
		public override int Execute(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer)
		{
			string requestMessage = "logout";
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

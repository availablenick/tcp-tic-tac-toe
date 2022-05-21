using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class ListCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for list command. Usage: list";
		public ListCommand(params string[] parameters) : base(parameters) { }
		public override int Execute(Socket serverSocket, Byte[] receiveBuffer,
			Byte[] sendBuffer)
		{
			string requestMessage = "list";
			BufferHelper.WriteMessageToBuffer(sendBuffer, requestMessage);
			serverSocket.Send(sendBuffer, requestMessage.Length, 0);

			int numberOfReceivedBytes = serverSocket.Receive(receiveBuffer,
				receiveBuffer.Length, 0);
			string responseMessage = BufferHelper.GetBufferMessage(receiveBuffer,
				numberOfReceivedBytes);
			Response response = ResponseParser.Parse(responseMessage);
			Console.WriteLine(response.ToString());

			return Int32.Parse(response.Parameters[response.Parameters.Count - 1]);
		}
	}
}

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
		public override int TakeEffect(CommandData data)
		{
			string requestMessage = $"logout {data.CurrentUsername}";
			BufferHelper.WriteMessageToBuffer(data.SendBuffer, requestMessage);
			data.ServerSocket.Send(data.SendBuffer, requestMessage.Length, 0);

			int numberOfReceivedBytes = data.ServerSocket.Receive(data.ReceiveBuffer,
				data.ReceiveBuffer.Length, 0);
			string responseMessage = BufferHelper.GetBufferMessage(data.ReceiveBuffer,
				numberOfReceivedBytes);
			Response response = ResponseParser.Parse(responseMessage);
			Console.WriteLine(response.ParseStatusCode());

			return response.StatusCode;
		}
	}
}

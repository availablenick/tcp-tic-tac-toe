using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class ListCommand : Command
	{
		public const int NumberOfParameters = 0;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for list command. Usage: list";
		
		private Socket _serverSocket;
		private Byte[] _receiveBuffer;
		private Byte[] _sendBuffer;

		public ListCommand(string[] parameters, Socket serverSocket,
			Byte[] receiveBuffer, Byte[] sendBuffer) : base(parameters)
		{
			this._serverSocket = serverSocket;
			this._receiveBuffer = receiveBuffer;
			this._sendBuffer = sendBuffer;
		}

		public override int Execute()
		{
			string requestMessage = "list";
			BufferHelper.WriteMessageToBuffer(this._sendBuffer, requestMessage);
			this._serverSocket.Send(this._sendBuffer, requestMessage.Length, 0);

			int numberOfReceivedBytes = this._serverSocket.Receive(this._receiveBuffer,
				this._receiveBuffer.Length, 0);
			string responseMessage = BufferHelper.GetBufferMessage(this._receiveBuffer,
				numberOfReceivedBytes);

			Response response = ResponseParser.Parse(responseMessage);
			Console.WriteLine(response.ToString());

			return response.StatusCode;
		}
	}
}

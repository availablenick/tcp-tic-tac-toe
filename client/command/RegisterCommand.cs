using System;
using System.Net.Sockets;

namespace TicTacToe.ClientSide
{
	public class RegisterCommand : Command
	{
		public const int NumberOfParameters = 2;
		public const string WrongNumberOfParametersMessage =
			"Wrong number of parameters for register command. Usage: " +
			"register <username> <password>";

		private Socket _serverSocket;
		private Byte[] _receiveBuffer;
		private Byte[] _sendBuffer;

		public RegisterCommand(string[] parameters, Socket serverSocket,
			Byte[] receiveBuffer, Byte[] sendBuffer) : base(parameters)
		{
			this._serverSocket = serverSocket;
			this._receiveBuffer = receiveBuffer;
			this._sendBuffer = sendBuffer;
		}

		public override int Execute()
		{
			string username = this.Parameters[0];
			string password = this.Parameters[1];
			string requestMessage = $"register {username};{password}";
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

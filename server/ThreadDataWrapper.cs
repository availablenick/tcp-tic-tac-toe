using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class ThreadDataWrapper
	{
		private Socket _socket;
		private Mutex _mutex;
		private Dictionary<string, string> _onlineUsers;

		public ThreadDataWrapper(Socket socket, Mutex mutex,
			Dictionary<string, string> onlineUsers)
		{
			this._socket = socket;
			this._mutex = mutex;
			this._onlineUsers = onlineUsers;
		}

		public void HandleConnection()
		{
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] New client connected");

			RequestData requestData = new RequestData(this._socket, this._mutex,
				this._onlineUsers);
			Byte[] receiveBuffer = new Byte[128];
			Byte[] sendBuffer = new Byte[128];
			int numberOfReceivedBytes;
			while (true)
			{
				numberOfReceivedBytes = this._socket.Receive(receiveBuffer,
					receiveBuffer.Length, 0);
				if (numberOfReceivedBytes <= 0)
				{
					break;
				}

				Console.Write($"[{Thread.CurrentThread.ManagedThreadId}] ");
				string bufferMessage = BufferHelper.GetBufferMessage(receiveBuffer, numberOfReceivedBytes);
				Request request = RequestParser.Parse(bufferMessage);
				string responseMessage = request.Fulfill(requestData);
				BufferHelper.WriteMessageToBuffer(sendBuffer, responseMessage);
				this._socket.Send(sendBuffer, responseMessage.Length, 0);
			}

			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Connection closed");
			this._socket.Close();
		}
	}
}

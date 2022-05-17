using System;
using System.Net.Sockets;
using System.Threading;

namespace TicTacToe.Server
{
	public class ThreadDataWrapper
	{
		private Socket socket;

		public ThreadDataWrapper(Socket socket)
		{
			this.socket = socket;
		}

		public void HandleConnection()
		{
			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] New client connected");

			Byte[] receiveBuffer = new Byte[128];
			Byte[] sendBuffer = new Byte[128];
			int numberOfReceivedBytes;
			while (true)
			{
				numberOfReceivedBytes = socket.Receive(receiveBuffer, receiveBuffer.Length, 0);
				if (numberOfReceivedBytes <= 0)
				{
					break;
				}

				string bufferMessage = BufferHelper.GetBufferMessage(receiveBuffer, numberOfReceivedBytes);
				Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {bufferMessage}");
				string responseMessage = "reply";
				BufferHelper.WriteMessageToBuffer(sendBuffer, responseMessage);
				socket.Send(sendBuffer, responseMessage.Length, 0);
			}

			Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Connection closed");
			socket.Close();
		}
	}
}

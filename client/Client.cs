using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public class Client
	{
		public static void Main(string[] args)
		{
			Socket socket = SocketHelper.CreateConnectionSocket("127.0.0.1", 3000);
			if (socket == null)
			{
				Console.WriteLine("Could not connect to specified host");
				return;
			}

			Byte[] receiveBuffer = new Byte[128];
			Byte[] sendBuffer = new Byte[128];
			string line;
			while (true)
			{
				Console.Write("> ");
				line = Console.ReadLine();
				if (line == null)
				{
					break;
				}

				string requestMessage = "message";
				BufferHelper.WriteMessageToBuffer(sendBuffer, requestMessage);
				socket.Send(sendBuffer, requestMessage.Length, 0);
				Console.WriteLine($"Client message: {requestMessage}");
				int numberOfReceivedBytes = socket.Receive(receiveBuffer, receiveBuffer.Length, 0);
				string responseMessage = BufferHelper.GetBufferMessage(receiveBuffer, numberOfReceivedBytes);
				Console.WriteLine($"Server reply: {responseMessage}");
			}

			Console.WriteLine("End");
			socket.Close();
		}
	}
}

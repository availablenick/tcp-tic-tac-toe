using System;
using System.Net;
using System.Net.Sockets;

namespace TicTacToe
{
	public class SocketHelper
	{
		public static Socket CreateListeningSocket(int port)
		{
			Socket socket = new Socket(
				AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp
			);
			IPAddress hostAddress = (Dns.GetHostEntry(IPAddress.Any))
				.AddressList[0];
			IPEndPoint endpoint = new IPEndPoint(hostAddress, port);
			try
			{
				socket.Bind(endpoint);
				socket.Listen(2);
			}
			catch (SocketException)
			{
				return null;
			}

			return socket;
		}

		public static Socket CreateConnectionSocket(string address, int port)
		{
			Socket socket = new Socket(
				AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp
			);
			try
			{
				socket.Connect(address, port);
			}
			catch (SocketException)
			{
				return null;
			}

			return socket;
		}

		public static void SendMessage(Socket socket, Byte[] buffer, string message)
		{
			BufferHelper.WriteMessageToBuffer(buffer, message);
			socket.Send(buffer, message.Length, 0);
		}

		public static string ReceiveMessage(Socket socket, Byte[] buffer, int timeout)
		{
			socket.ReceiveTimeout = timeout;
			try {
				int numberOfReceivedBytes = socket.Receive(buffer, buffer.Length, 0);
				return BufferHelper.GetBufferMessage(buffer, numberOfReceivedBytes);
			}
			catch (SocketException) {
				socket.ReceiveTimeout = -1;
			}

			return null;
		}

		public static string ReceiveMessage(Socket socket, Byte[] buffer)
		{
			return ReceiveMessage(socket, buffer, -1);
		}
	}
}

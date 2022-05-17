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

		public static Socket CreateConnectionSocket(string serverAddress, int port)
		{
			Socket socket = new Socket(
				AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp
			);
			try
			{
				socket.Connect(serverAddress, port);
			}
			catch (SocketException)
			{
				return null;
			}

			return socket;
		}
	}
}

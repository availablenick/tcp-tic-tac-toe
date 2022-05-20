using System;
using System.Net.Sockets;

namespace TicTacToe.Client
{
	public interface IUserState
	{
		public void ExecuteCommand(Command command);
	}
}

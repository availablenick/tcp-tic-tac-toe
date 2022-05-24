using System;

namespace TicTacToe.ClientSide
{
	public interface IUserState
	{
		public void ExecuteCommand(Command command);
	}
}

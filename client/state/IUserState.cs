using System;

namespace TicTacToe.Client
{
	public interface IUserState
	{
		public void ExecuteCommand(Command command);
	}
}

using System.Collections.Generic;

namespace TicTacToe
{
	public class UserHelper
	{
		public static void AddOnlineUser(string username, string remoteEndpoint,
			Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername)
		{
			usernameByEndpoint.Add(remoteEndpoint, username);
			endpointByUsername.Add(username, remoteEndpoint);
		}

		public static void RemoveOnlineUser(string remoteEndpoint,
			Dictionary<string, string> usernameByEndpoint,
			Dictionary<string, string> endpointByUsername)
		{
			string username = usernameByEndpoint[remoteEndpoint];
			usernameByEndpoint.Remove(remoteEndpoint);
			endpointByUsername.Remove(username);
		}
	}
}

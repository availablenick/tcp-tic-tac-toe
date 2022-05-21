using System.Text.RegularExpressions;

namespace TicTacToe
{
	public class MessageHelper
	{
		public static bool HasCorrectNumberOfParameters(
			int numberOfParameters, GroupCollection groups)
		{
			for (int i = 1; i <= numberOfParameters; i++)
			{
				if (groups[2*i + 1].Value == "")
				{
					return false;
				}
			}

			return true;
		}

		public static string[] CreateParameterArray(
			int numberOfParameters, GroupCollection groups)
		{
			string[] parameters = new string[numberOfParameters];
			for (int i = 0; i < numberOfParameters; i++)
			{
				parameters[i] = groups[2*(i+1) + 1].Value;
			}

			return parameters;
		}
	}
}

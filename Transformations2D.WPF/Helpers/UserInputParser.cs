using System.Windows;

namespace Transformations2D.WPF.Helpers
{
	public class UserInputParser : IUserInputParser
	{
		public Point? StringToPoint(string input)
		{
			try
			{
				return Point.Parse(input);
			}
			catch
			{
				return null;
			}
		}
	}
}

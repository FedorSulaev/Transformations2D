using System.Windows;

namespace Transformations2D.WPF.Helpers
{
	public interface IUserInputParser
	{
		Point? StringToPoint(string input);
	}
}
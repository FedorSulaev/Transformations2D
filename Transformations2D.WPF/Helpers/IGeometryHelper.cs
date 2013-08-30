using System.Windows;

namespace Transformations2D.WPF.Helpers
{
	public interface IGeometryHelper
	{
		Point ConvertIntoCanvasCoordinates(Point point, int canvasSideLength);
	}
}
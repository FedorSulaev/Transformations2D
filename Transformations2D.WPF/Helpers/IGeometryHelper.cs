using System.Collections.Generic;
using System.Windows;
using System.Windows.Shapes;

namespace Transformations2D.WPF.Helpers
{
	public interface IGeometryHelper
	{
		Point ConvertIntoCanvasCoordinates(Point point, int canvasSideLength);
		List<Point> ConvertIntoCanvasCoordinates(List<Point> points, int canvasSideLength);
		Path MakePath(List<Point> points);
	}
}
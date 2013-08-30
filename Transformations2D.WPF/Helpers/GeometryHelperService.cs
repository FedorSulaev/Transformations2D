using System.Windows;

namespace Transformations2D.WPF.Helpers
{
	public class GeometryHelperService : IGeometryHelper
	{
		public Point ConvertIntoCanvasCoordinates(Point point, int canvasSideLength)
		{
			int oneAxisUnit = canvasSideLength/20;
			int canvasSideHalf = canvasSideLength/2;
			return new Point(point.X*oneAxisUnit + canvasSideHalf,
				point.Y*-oneAxisUnit + canvasSideHalf);
		}
	}
}

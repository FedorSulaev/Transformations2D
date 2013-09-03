using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Shapes;

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

		public List<Point> ConvertIntoCanvasCoordinates(List<Point> points, int canvasSideLength)
		{
			return points.Select(p => ConvertIntoCanvasCoordinates(p, canvasSideLength)).ToList();
		}

		public Path MakePath(List<Point> points)
		{
			Path path = new Path();
			GeometryGroup geometry = new GeometryGroup();
			AddPoints(geometry, points);
			AddLines(geometry, points);
			path.Data = geometry;
			path.Fill = Brushes.Black;
			path.StrokeThickness = 2;
			path.Stroke = Brushes.Black;
			return path;
		}

		private void AddLines(GeometryGroup geometry, List<Point> points)
		{
			for (int i = 1; i < points.Count; i++)
			{
				geometry.Children.Add(MakeLine(points[i-1], points[i]));
			}
			if (points.Count > 2)
			{
				geometry.Children.Add(MakeLine(points[points.Count-1], points[0]));
			}
		}

		private void AddPoints(GeometryGroup geometry, IEnumerable<Point> points)
		{
			foreach (Point point in points)
			{
				geometry.Children.Add(MakeCirсle(point));
			}
		}

		private EllipseGeometry MakeCirсle(Point point)
		{
			return new EllipseGeometry(point, 3, 3);
		}

		private LineGeometry MakeLine(Point startPoint, Point endPoint)
		{
			return new LineGeometry(startPoint, endPoint);
		}
	}
}

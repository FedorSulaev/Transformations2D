using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using NUnit.Framework;
using Transformations2D.WPF.Helpers;

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
    public class GeometryHelperServiceTests
    {
		private static CultureInfo GetDefaultCultureInfo()
		{
			return CultureInfo.GetCultureInfo("en");
		}

		private static GeometryHelperService MakeGeometryHelperService()
		{
			return new GeometryHelperService();
		}

		private static GeometryCollection GetGeometries(Path path)
		{
			return ((GeometryGroup)path.Data).Children;
		}

		private static List<Point> MakeListOfPoints(int pointsNum)
		{
			List<Point> points = new List<Point>(pointsNum);
			points.AddRange(Enumerable.Repeat(new Point(), pointsNum));
			return points;
		}

		private bool GeometryIsLineBetweenPoints(Geometry geometry, Point startPoint, Point endPoint)
		{
			return geometry is LineGeometry
				&& ((LineGeometry)geometry).StartPoint == startPoint
				&& ((LineGeometry)geometry).EndPoint == endPoint;
		}

		[TestCase(100, "50,50")]
		[TestCase(200, "100,100")]
		public void ConvertIntoCanvasCoordinates_OriginCoordinates_ReturnCanvasCenterCoordinates(int canvasSideLength, string expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			Point result = geometryHelper.ConvertIntoCanvasCoordinates(new Point(0, 0), canvasSideLength);

			Assert.AreEqual(expected, result.ToString(GetDefaultCultureInfo()));
		}

		[TestCase(100, "-10,10", "0,0")]
		[TestCase(100, "10,-10", "100,100")]
		[TestCase(100, "10,10", "100,0")]
		[TestCase(100, "-10,-10", "0,100")]
		[TestCase(200, "10,-10", "200,200")]
		public void ConvertIntoCanvasCoordinates_CornerCoordinates_ReturnCanvasCornerCoordinates(int canvasSideLength, string pointCoordinates, string expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			Point result = geometryHelper.ConvertIntoCanvasCoordinates(Point.Parse(pointCoordinates), canvasSideLength);

			Assert.AreEqual(expected, result.ToString(GetDefaultCultureInfo()));
		}

		[TestCase(100, "-10,10", "10,-10", "0,0", "100,100")]
		[TestCase(100, "10,10", "-10,-10", "100,0", "0,100")]
		public void ConvertIntoCanvasCoordinates_ListOf2Points_ReturnListOfCanvasCoordinates(int canvasSideLength, string point1, string point2,
			string expectedPoint1, string expectedPoint2)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{Point.Parse(point1), Point.Parse(point2)};

			List<Point> result = geometryHelper.ConvertIntoCanvasCoordinates(points, canvasSideLength);

			Assert.IsTrue(result.SequenceEqual(new List<Point>{Point.Parse(expectedPoint1), Point.Parse(expectedPoint2)}));
		}

		[Test, RequiresSTA]
		public void MakePath_ListOfPoints_ReturnPath()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			dynamic result = geometryHelper.MakePath(MakeListOfPoints(1));

			Assert.IsAssignableFrom<Path>(result);
		}

		[Test, RequiresSTA]
		public void MakePath_ListOfPoints_ReturnPathWithGeometryGroup()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			Path result = geometryHelper.MakePath(MakeListOfPoints(1));

			Assert.IsAssignableFrom<GeometryGroup>(result.Data);
		}

		[TestCase(1, 1), RequiresSTA]
		[TestCase(2, 2)]
		public void MakePath_ListOfPoints_ReturnPathWithSameNumberOfEllipseGeometry(int pointsNum, int expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			Path result = geometryHelper.MakePath(MakeListOfPoints(pointsNum));

			Assert.AreEqual(expected, GetGeometries(result).Count(g => g is EllipseGeometry));
		}

		[TestCase("175,175", "175,175"), RequiresSTA]
		[TestCase("50,50", "50,50")]
		public void MakePath_OnePoint_ReturnEllipseGeometryWithPointsCoordinates(string point, string expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{Point.Parse(point)};

			Path result = geometryHelper.MakePath(points);

			Assert.AreEqual(Point.Parse(expected), ((EllipseGeometry)GetGeometries(result)[0]).Center);
		}

		[TestCase("175,175", "175,175"), RequiresSTA]
		[TestCase("1,1", "1,1")]
		public void MakePath_TwoPoints_ReturnLineGeometryWithBeginningAtFirstPoint(string firstPoint, string expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{Point.Parse(firstPoint), new Point()};

			Path result = geometryHelper.MakePath(points);

			Assert.AreEqual(Point.Parse(expected), ((LineGeometry)GetGeometries(result).First(g => g is LineGeometry)).StartPoint);
		}

		[TestCase("175,175", "175,175"), RequiresSTA]
		[TestCase("1,1", "1,1")]
		public void MakePath_TwoPoints_ReturnLineGeometryWithEndAtSecondPoint(string secondPoint, string expected)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{new Point(), Point.Parse(secondPoint)};

			Path result = geometryHelper.MakePath(points);

			Assert.AreEqual(Point.Parse(expected), ((LineGeometry)GetGeometries(result).First(g => g is LineGeometry)).EndPoint);
		}

		[Test, RequiresSTA]
		public void MakePath_OnePoint_DoNotAddLineGeometry()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();

			Path result = geometryHelper.MakePath(MakeListOfPoints(1));

			Assert.IsFalse(GetGeometries(result).Any(g => g is LineGeometry));
		}

		[TestCase("2,2", "3,3", "2,2", "3,3"), RequiresSTA]
		[TestCase("4,4","5,5","4,4","5,5")]
		public void MakePath_ThreePoints_AddLineBetweenSecondAndThirdPoints(string point2, string point3, string expectedStart, string expectedEnd)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{new Point(1,1), Point.Parse(point2), Point.Parse(point3)};

			Path result = geometryHelper.MakePath(points);

			Assert.IsTrue(GetGeometries(result).Any(g => GeometryIsLineBetweenPoints(g, Point.Parse(expectedStart), Point.Parse(expectedEnd))));
		}

		[TestCase("1,1", "3,3", "3,3", "1,1"), RequiresSTA]
		[TestCase("4,4", "5,5", "5,5", "4,4")]
		public void MakePath_ThreePoints_AddLineBetweenThirdAndFirstPoints(string point1, string point3, string expectedStart, string expectedEnd)
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{Point.Parse(point1), new Point(2,2), Point.Parse(point3)};

			Path result = geometryHelper.MakePath(points);

			Assert.IsTrue(GetGeometries(result).Any(g => GeometryIsLineBetweenPoints(g, Point.Parse(expectedStart), Point.Parse(expectedEnd))));
		}

		[Test, RequiresSTA]
		public void MakePath_TwoPoints_AddOnlyOneLine()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{new Point(1,1), new Point(2,2)};

			Path result = geometryHelper.MakePath(points);

			Assert.AreEqual(1, GetGeometries(result).Count(g => g is LineGeometry));
		}

		[Test, RequiresSTA]
		public void MakePath_FourPoints_NoLineBetweenThirdAndFirstPoints()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point>{new Point(1,1), new Point(2,2), new Point(3,3), new Point(4,4)};

			Path result = geometryHelper.MakePath(points);

			Assert.IsFalse(GetGeometries(result).Any(g => GeometryIsLineBetweenPoints(g, new Point(3,3), new Point(1,1))));
		}

		[Test, RequiresSTA]
		public void MakePath_FourPoints_AddLineBetweenFourthAndFirstPoints()
		{
			GeometryHelperService geometryHelper = MakeGeometryHelperService();
			List<Point> points = new List<Point> { new Point(1, 1), new Point(2, 2), new Point(3, 3), new Point(4, 4) };

			Path result = geometryHelper.MakePath(points);

			Assert.IsTrue(GetGeometries(result).Any(g => GeometryIsLineBetweenPoints(g, new Point(4, 4), new Point(1, 1))));
		}
    }
}

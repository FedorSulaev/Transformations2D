using System.Globalization;
using System.Windows;
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

		private static GeometryHelperService GetGeometryHelperService()
		{
			return new GeometryHelperService();
		}

		[TestCase(100, "50,50")]
		[TestCase(200, "100,100")]
		public void ConvertIntoCanvasCoordinates_OriginCoordinates_ReturnCanvasCenterCoordinates(int canvasSideLength, string expected)
		{
			GeometryHelperService geometryHelper = GetGeometryHelperService();

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
			GeometryHelperService geometryHelper = GetGeometryHelperService();

			Point result = geometryHelper.ConvertIntoCanvasCoordinates(Point.Parse(pointCoordinates), canvasSideLength);

			Assert.AreEqual(expected, result.ToString(GetDefaultCultureInfo()));
		}
    }
}

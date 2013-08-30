using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using Moq;
using Transformations2D.WPF.Controls;
using Transformations2D.WPF.Helpers;
using System.Windows.Shapes;

namespace Transformations2D.TestsUtility
{
	public class Maker
	{
		public static InitialViewUserControlViewModel MakeInitialViewUserControlViewModel(IUserInputParser parser = null, IGeometryHelper geometryHelper = null)
		{
			InitialViewUserControlViewModel viewModel = new InitialViewUserControlViewModel();
			if (parser != null) viewModel.UserInputParser = parser;
			if (geometryHelper != null) viewModel.GeometryHelper = geometryHelper;
			return viewModel;
		}

		public static Mock<IUserInputParser> MakeFakeIUserInputParser(Point? parseTo)
		{
			Mock<IUserInputParser> parser = new Mock<IUserInputParser>();
			parser.Setup(p => p.StringToPoint(null)).Returns(parseTo);
			return parser;
		}

		public static TransformViewUserControlViewModel MakeTransformViewUserControlViewModel()
		{
			return new TransformViewUserControlViewModel();
		}

		public static Mock<IGeometryHelper> MakeFakeIGeometryHelper(Point convertTo)
		{
			Mock<IGeometryHelper> geometryHelper = new Mock<IGeometryHelper>();
			geometryHelper.Setup(g => g.ConvertIntoCanvasCoordinates(It.IsAny<Point>(), It.IsAny<int>())).Returns(convertTo);
			return geometryHelper;
		}

		public static Path MakePointPath(Point point)
		{
			return new Path
			{
				Data = new EllipseGeometry(point, 3, 3)
			};
		}

		public static Path MakeLinePath(Point start, Point end)
		{
			return new Path
			{
				Data = new LineGeometry(start, end)
			};
		}
	}
}

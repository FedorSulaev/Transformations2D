using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Moq;
using NUnit.Framework;
using Transformations2D.WPF.Controls;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Utility;

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
	public class TransformViewUserControlViewModelTests
	{
		[TestCase("0,0")]
		[TestCase("1,1")]
		public void TransformViewItems_PointsChangedEventRaisedWithOnePointInList_AddConvertedPointToTransformViewItems(string point)
		{
			TransformViewUserControlViewModel viewModel = new TransformViewUserControlViewModel();
			Mock<IGeometryHelper> fakeGeometryHelper = new Mock<IGeometryHelper>();
			Point transformedPoint = new Point(10,10);
			fakeGeometryHelper.Setup(gh => gh.ConvertIntoCanvasCoordinates(It.IsAny<List<Point>>(), It.IsAny<int>()))
				.Returns(new List<Point> {transformedPoint});
			fakeGeometryHelper.Setup(gh => gh.MakePath(It.IsAny<List<Point>>())).Returns(new Path
			{
				Data = new GeometryGroup
				{
					Children = new GeometryCollection { new EllipseGeometry(transformedPoint, 3, 3) }
				}
			});
			viewModel.GeometryHelper = fakeGeometryHelper.Object;
			List<Point> initialPoints = new List<Point>{Point.Parse(point)};

			ServicesFactory.EventService.GetEvent<GenericEvent<List<Point>>>()
				.Publish(new EventParameters<List<Point>>("PointsChangedEvent", initialPoints));

			Assert.AreEqual(transformedPoint, 
				((EllipseGeometry)((GeometryGroup)viewModel.TransformViewItems[0].Data).Children.First(g => g is EllipseGeometry)).Center);
		}
	}
}

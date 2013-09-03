using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Moq;
using NUnit.Framework;
using Transformations2D.TestsUtility;
using Transformations2D.WPF.Controls;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Utility;

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
	class InitialViewUserControlViewModelTests
	{
		private static InitialViewUserControlViewModel MakeInitialViewUserControlViewModel(IUserInputParser parser = null, IGeometryHelper geometryHelper = null)
		{
			InitialViewUserControlViewModel viewModel = new InitialViewUserControlViewModel();
			viewModel.UserInputParser = parser ?? new Mock<IUserInputParser>().Object;
			viewModel.GeometryHelper = geometryHelper ?? new Mock<IGeometryHelper>().Object;
			return viewModel;
		}

		private static Mock<IUserInputParser> MakeFakeIUserInputParser(Point? parseTo)
		{
			Mock<IUserInputParser> parser = new Mock<IUserInputParser>();
			parser.Setup(p => p.StringToPoint(null)).Returns(parseTo);
			return parser;
		}

		[Test]
		public void ListOfPoints_InitialViewUserControlViewModelCreated_ListIsEmpty()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();

			Assert.AreEqual(0, viewModel.ListOfPoints.Count);
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsValidString_AddPointToListOfPoints()
		{
			Mock<IUserInputParser> fakeUserInputParser = MakeFakeIUserInputParser(parseTo: new Point(1, 2));
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual("1,2", viewModel.ListOfPoints[0].ToString(CultureInfo.GetCultureInfo("en")));
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsValidString_ClearNewPointCoordinates()
		{

			Mock<IUserInputParser> fakeUserInputParser = MakeFakeIUserInputParser(parseTo: new Point(1, 2));
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(String.Empty, viewModel.NewPointCoordinates);
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsInvalid_DoNotAddAnythingToListOfPoints()
		{
			Mock<IUserInputParser> fakeUserInputParser = MakeFakeIUserInputParser(parseTo: null);
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(0, viewModel.ListOfPoints.Count);
		}
		[Test]
		public void NewPointCoordinates_CoordinatesCleared_NotifyPropertyChangedRaised()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();

			viewModel.NewPointCoordinates = "1,2";

			viewModel.ShouldNotifyOn(vm => vm.NewPointCoordinates).When(vm => vm.NewPointCoordinates = String.Empty);
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsContains2Points_DeleteAllPointsFromListOfPoints()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			viewModel.ListOfPoints.Add(new Point(1, 2));
			viewModel.ListOfPoints.Add(new Point(3, 4));

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsEmpty(viewModel.ListOfPoints);
		}

		[Test]
		public void DeleteAllPointsCommand_InitialViewItemsContains2Points_ClearInitialViewItems()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			viewModel.InitialViewItems.Add(new Path());

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsNull(viewModel.InitialViewItems[0]);
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsIsEmpty_CanNotExecuteCommand()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();

			Assert.IsFalse(viewModel.DeleteAllPointsCommand.CanExecute(null));
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsContainsPoint_CanExecuteCommand()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			viewModel.ListOfPoints.Add(new Point());

			Assert.IsTrue(viewModel.DeleteAllPointsCommand.CanExecute(null));
		}
		
		[Test]
		public void DeleteAllPointsCommand_AddPointExecuted_CanExecuteChangedRaised()
		{
			Mock<IUserInputParser> fakeParser = MakeFakeIUserInputParser(parseTo: new Point());
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeParser.Object);
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllPointsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;
			
			viewModel.AddPointCommand.Execute(null);

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void DeleteAllPointsCommand_DeleteAllPointsExecuted_CanExecuteChangedRaised()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllPointsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void AddPointCommand_AddPointExecutedPointWithTheSameCoordinatesExistInTheList_DoNotAddNewPoint()
		{
			Mock<IUserInputParser> fakeParser = MakeFakeIUserInputParser(parseTo: new Point(0, 0));
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeParser.Object);
			viewModel.ListOfPoints.Add(new Point(0, 0));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(1, viewModel.ListOfPoints.Count);
		}

		[TestCase("11,11")]
		[TestCase("1,11")]
		[TestCase("-11,-11")]
		public void AddPointCommand_PointWithCoordinatesOutOfBounds_DoNotAddPoint(string coordinates)
		{
			Mock<IUserInputParser> fakeParser = MakeFakeIUserInputParser(parseTo: Point.Parse(coordinates));
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel(parser: fakeParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.IsEmpty(viewModel.ListOfPoints);
		}

		[Test]
		public void AddPointCommand_ListOfPointsContains1Point_AddCorrectPathToInitialViewItems()
		{
			Mock<IUserInputParser> fakeParser = MakeFakeIUserInputParser(parseTo: new Point(0, 0));
			Mock<IGeometryHelper> fakeGeometryHelper = new Mock<IGeometryHelper>();
			fakeGeometryHelper.Setup(gh => gh.ConvertIntoCanvasCoordinates(It.IsAny<List<Point>>(), It.IsAny<int>()))
				.Returns(new List<Point> {new Point(175, 175)});
			fakeGeometryHelper.Setup(gh => gh.MakePath(It.IsAny<List<Point>>())).Returns(new Path
			{
				Data = new GeometryGroup
				{
					Children = new GeometryCollection {new EllipseGeometry(new Point(175, 175), 3, 3)}
				}
			});
			InitialViewUserControlViewModel viewModel = 
				MakeInitialViewUserControlViewModel(parser: fakeParser.Object, geometryHelper: fakeGeometryHelper.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(new Point(175, 175), 
				((EllipseGeometry)((GeometryGroup)viewModel.InitialViewItems[0].Data).Children.First(g => g is EllipseGeometry)).Center);
		}

		[TestCase("1,1")]
		[TestCase("3,3")]
		public void DeletePointCommand_OneOfTwoPointsSelected_DeleteSelectedPoint(string point)
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			Point selectedPoint = Point.Parse(point);
			viewModel.ListOfPoints.Add(selectedPoint);
			viewModel.ListOfPoints.Add(new Point(2,2));
			viewModel.SelectedPoint = selectedPoint;

			viewModel.DeletePointCommand.Execute(null);

			Assert.IsFalse(viewModel.ListOfPoints.Contains(selectedPoint));
		}

		[Test]
		public void DeletePointCommand_SelectedPointIsNull_CanNotExecute()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			viewModel.ListOfPoints.Add(new Point());
			viewModel.SelectedPoint = null;

			Assert.IsFalse(viewModel.DeletePointCommand.CanExecute(null));
		}

		[Test]
		public void DeletePointCommand_PointSelected_RaiseCanExecuteChanged()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeletePointCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;

			viewModel.SelectedPoint = new Point();

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void DeleteAllPointsCommand_PointDeleted_RaiseCanExecuteChanged()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllPointsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;
			viewModel.ListOfPoints.Add(new Point());
			viewModel.SelectedPoint = new Point();

			viewModel.DeletePointCommand.Execute(null);

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void ListOfPoints_CollectionChanged_RaisePointsChangedEventWithListOfPointsAsParameter()
		{
			InitialViewUserControlViewModel viewModel = MakeInitialViewUserControlViewModel();
			List<Point> points = new List<Point>();
			ServicesFactory.EventService.GetEvent<GenericEvent<List<Point>>>().Subscribe(
				s =>
				{
					points = s.Value;
				});

			viewModel.ListOfPoints.Add(new Point());

			Assert.AreEqual(viewModel.ListOfPoints, points);
		}
	}
}

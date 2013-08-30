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

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
	class InitialViewUserControlViewModelTests
	{
		[Test]
		public void ListOfPoints_InitialViewUserControlViewModelCreated_ListIsEmpty()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();

			Assert.AreEqual(0, viewModel.ListOfPoints.Count);
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsValidString_AddPointToListOfPoints()
		{
			Mock<IUserInputParser> fakeUserInputParser = Maker.MakeFakeIUserInputParser(parseTo: new Point(1, 2));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual("1,2", viewModel.ListOfPoints[0].ToString(CultureInfo.GetCultureInfo("en")));
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsValidString_ClearNewPointCoordinates()
		{

			Mock<IUserInputParser> fakeUserInputParser = Maker.MakeFakeIUserInputParser(parseTo: new Point(1, 2));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(string.Empty, viewModel.NewPointCoordinates);
		}

		[Test]
		public void AddPointCommand_NewPointCoordinatesIsInvalid_DoNotAddAnythingToListOfPoints()
		{
			Mock<IUserInputParser> fakeUserInputParser = Maker.MakeFakeIUserInputParser(parseTo: null);
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(parser: fakeUserInputParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(0, viewModel.ListOfPoints.Count);
		}
		[Test]
		public void NewPointCoordinates_CoordinatesCleared_NotifyPropertyChangedRaised()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();

			viewModel.NewPointCoordinates = "1,2";

			viewModel.ShouldNotifyOn(vm => vm.NewPointCoordinates).When(vm => vm.NewPointCoordinates = string.Empty);
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsContains2Points_DeleteAllPointsFromListOfPoints()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();
			viewModel.ListOfPoints.Add(new Point(1, 2));
			viewModel.ListOfPoints.Add(new Point(3, 4));

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsEmpty(viewModel.ListOfPoints);
		}

		[Test]
		public void DeleteAllPointsCommand_InitialViewItemsContains2Points_ClearInitialViewItems()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point()));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point(1,1)));

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsEmpty(viewModel.InitialViewItems);
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsIsEmpty_CanNotExecuteCommand()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();

			Assert.IsFalse(viewModel.DeleteAllPointsCommand.CanExecute(null));
		}

		[Test]
		public void DeleteAllPointsCommand_ListOfPointsContainsPoint_CanExecuteCommand()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();
			viewModel.ListOfPoints.Add(new Point());

			Assert.IsTrue(viewModel.DeleteAllPointsCommand.CanExecute(null));
		}
		
		[Test]
		public void DeleteAllPointsCommand_AddPointExecuted_CanExecuteChangedRaised()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(parser: fakeParser.Object);
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllPointsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;
			
			viewModel.AddPointCommand.Execute(null);

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void DeleteAllPointsCommand_DeleteAllPointsExecuted_CanExecuteChangedRaised()
		{
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllPointsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;

			viewModel.DeleteAllPointsCommand.Execute(null);

			Assert.IsTrue(canExecuteChangedWasRaised);
		}

		[Test]
		public void AddPointCommand_AddPointExecutedPointWithTheSameCoordinatesExistInTheList_DoNotAddNewPoint()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point(0, 0));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(parser: fakeParser.Object);
			viewModel.ListOfPoints.Add(new Point(0, 0));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(1, viewModel.ListOfPoints.Count);
		}

		[TestCase("175,175", "175,175")]
		[TestCase("50,50", "50,50")]
		public void AddPointCommand_NewPointAdded_AddNewEllipseGeometryWithConvertedCoordinatesToInitialViewItems(string convertedCoordinates, string expected)
		{
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: Point.Parse(convertedCoordinates));
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object, fakeGeometryHelper.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.IsTrue(((EllipseGeometry)viewModel.InitialViewItems[0].Data).Center == Point.Parse(expected));
		}

		[TestCase("11,11")]
		[TestCase("1,11")]
		[TestCase("-11,-11")]
		public void AddPointCommand_PointWithCoordinatesOutOfBounds_DoNotAddPoint(string coordinates)
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: Point.Parse(coordinates));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.IsEmpty(viewModel.ListOfPoints);
		}

		[TestCase("175,175", "175,175")]
		[TestCase("1,1", "1,1")]
		public void AddPointCommand_SecondPointAdded_AddLineWithBeginnigAtFirstPointIntoInitialViewItems(string firstPointCoordinates, string expected)
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: new Point());
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object, fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(Point.Parse(firstPointCoordinates)));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(Point.Parse(expected),
				((LineGeometry) viewModel.InitialViewItems.First(path => path.Data is LineGeometry).Data).StartPoint);
		}

		[Test]
		public void AddPointCommand_SecondPointAdded_AddLineWithEndAtSecondPointIntoInitialViewItems()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: new Point(100, 100));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object, fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point()));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(new Point(100, 100),
				((LineGeometry) viewModel.InitialViewItems.First(path => path.Data is LineGeometry).Data).EndPoint);
		}

		[Test]
		public void AddPointCommand_FirstPointAdded_DoNotAddLine()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: new Point());
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object, fakeGeometryHelper.Object);

			viewModel.AddPointCommand.Execute(null);

			Assert.IsFalse(viewModel.InitialViewItems.Any(path => path.Data is LineGeometry));
		}

		[Test]
		public void AddPointCommand_ThirdPointAdded_AddLineWithBeginningAtSecondPointIntoInitialViewItems()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: new Point());
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object, fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point()));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point(100,100)));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(new Point(100, 100),
				((LineGeometry) viewModel.InitialViewItems.First(path => path.Data is LineGeometry).Data).StartPoint);
		}

		[Test]
		public void AddPointCommand_ThirdPointAdded_AddLineBetweenFirstAndThirdPoints()
		{
			Point firstPoint = new Point(10, 10);
			Point secondPoint = new Point(20, 20);
			Point thirdPoint = new Point(30, 30);
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: thirdPoint);
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object,
				fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(firstPoint));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(secondPoint));

			viewModel.AddPointCommand.Execute(null);

			Assert.IsTrue(viewModel.InitialViewItems.Any(path => path.Data is LineGeometry && ((LineGeometry)path.Data).StartPoint == firstPoint &&
				((LineGeometry)path.Data).EndPoint == thirdPoint));
		}

		[Test]
		public void AddPointCommand_SecondPointAdded_OnlyOneLineAdded()
		{
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: new Point(10,10));
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object,
				fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(new Point()));

			viewModel.AddPointCommand.Execute(null);

			Assert.AreEqual(1, viewModel.InitialViewItems.Count(item => item.Data is LineGeometry));
		}

		[Test]
		public void AddPointCommand_FourthPointAdded_DeleteLineBetweenFirstAndThirdPoints()
		{
			Point firstPoint = new Point(10, 10);
			Point secondPoint = new Point(20, 20);
			Point thirdPoint = new Point(30, 30);
			Point fourthPoint = new Point(40, 40);
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: fourthPoint);
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object,
				fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(firstPoint));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(secondPoint));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(thirdPoint));
			viewModel.InitialViewItems.Add(Maker.MakeLinePath(firstPoint, secondPoint));
			viewModel.InitialViewItems.Add(Maker.MakeLinePath(secondPoint, thirdPoint));
			viewModel.InitialViewItems.Add(Maker.MakeLinePath(firstPoint, thirdPoint));

			viewModel.AddPointCommand.Execute(null);

			Assert.IsFalse(
				viewModel.InitialViewItems.Any(
					item => item.Data is LineGeometry && ((LineGeometry) item.Data).StartPoint == firstPoint &&
					        ((LineGeometry) item.Data).EndPoint == thirdPoint));
		}

		[Test]
		public void AddPointCommand_ThirdPointAdded_DoNotDeleteLineBetweenFirstAndSecondPoints()
		{
			Point firstPoint = new Point(10, 10);
			Point secondPoint = new Point(20, 20);
			Point thirdPoint = new Point(30, 30);
			Mock<IUserInputParser> fakeParser = Maker.MakeFakeIUserInputParser(parseTo: new Point());
			Mock<IGeometryHelper> fakeGeometryHelper = Maker.MakeFakeIGeometryHelper(convertTo: thirdPoint);
			InitialViewUserControlViewModel viewModel = Maker.MakeInitialViewUserControlViewModel(fakeParser.Object,
				fakeGeometryHelper.Object);
			viewModel.InitialViewItems.Add(Maker.MakePointPath(firstPoint));
			viewModel.InitialViewItems.Add(Maker.MakePointPath(secondPoint));
			viewModel.InitialViewItems.Add(Maker.MakeLinePath(firstPoint, secondPoint));

			viewModel.AddPointCommand.Execute(null);

			Assert.IsTrue(
				viewModel.InitialViewItems.Any(
					item => item.Data is LineGeometry && ((LineGeometry)item.Data).StartPoint == firstPoint &&
							((LineGeometry)item.Data).EndPoint == secondPoint));
		}
	}
}

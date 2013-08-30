using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Resourses.MVVM;

namespace Transformations2D.WPF.Controls
{
	public class InitialViewUserControlViewModel : PropertyChangedImplementation
	{
		public ICommand AddPointCommand
		{
			get
			{
				return _addPointCommand ?? (_addPointCommand = new DelegateCommand<object>(a => AddPoint()));
			}
		}

		public ICommand DeleteAllPointsCommand
		{
			get
			{
				return _deleteAllPointsCommand ?? (_deleteAllPointsCommand = new DelegateCommand<object>(a => DeleteAllPoints(), a => CanDeleteAllPoints()));
			}
		}

		public ObservableCollection<Point> ListOfPoints
		{
			get { return _listOfPoints; }
			set { _listOfPoints = value; }
		}

		public Point? SelectedPoint
		{
			get { return _selectedPoint; }
			set
			{
				_selectedPoint = value;
			}
		}

		public string NewPointCoordinates
		{
			get { return _newPointCoordinates; }
			set
			{
				_newPointCoordinates = value;
				OnPropertyChanged("NewPointCoordinates");
			}
		}

		public IUserInputParser UserInputParser
		{
			set { _userInputParser = value; }
		}

		public IGeometryHelper GeometryHelper
		{
			set { _geometryHelper = value; }
		}

		public ObservableCollection<Path> InitialViewItems
		{
			get { return _initialViewItems; }
		}

		private ICommand _addPointCommand;

		private ICommand _deleteAllPointsCommand;

		private ObservableCollection<Path> _initialViewItems;
		private ObservableCollection<Point> _listOfPoints;

		private Point? _selectedPoint;

		private string _newPointCoordinates;

		private IUserInputParser _userInputParser;
		private IGeometryHelper _geometryHelper;

		private const int CanvasSideLength = 350;

		public InitialViewUserControlViewModel()
		{
			_userInputParser = new UserInputParser();
			_geometryHelper = new GeometryHelperService();
			_listOfPoints = new ObservableCollection<Point>();
			_initialViewItems = new ObservableCollection<Path>();
		}

		private void AddPoint()
		{
			Point? point = _userInputParser.StringToPoint(_newPointCoordinates);
			if (!IsUserPointOk(point)) return;
			_listOfPoints.Add((Point)point);
			Point canvasCoordinates = _geometryHelper.ConvertIntoCanvasCoordinates((Point)point, CanvasSideLength);
			RemovePreviousClosingLine();
			AddLines(canvasCoordinates);
			AddPointToInitialView(canvasCoordinates);
			NewPointCoordinates = string.Empty;
			((DelegateCommand<object>)DeleteAllPointsCommand).RaiseCanExecuteChanged();
		}

		private void RemovePreviousClosingLine()
		{
			if(_initialViewItems.Count > 3)
				_initialViewItems.Remove(_initialViewItems.LastOrDefault(item => item.Data is LineGeometry));
		}

		private bool IsUserPointOk(Point? point)
		{
			if (point == null)
				return false;
			if (IsPointInTheList((Point) point) || IsPointOutOfBounds((Point) point))
				return false;
			return true;
		}

		private void AddLines(Point canvasCoordinates)
		{
			if (_initialViewItems.Count > 0)
				AddLineToInitialView(((EllipseGeometry) _initialViewItems.Last(path => path.Data is EllipseGeometry).Data).Center,
					canvasCoordinates);
			if(_initialViewItems.Count > 2)
				AddLineToInitialView(((EllipseGeometry)_initialViewItems.First(path => path.Data is EllipseGeometry).Data).Center, 
					canvasCoordinates);
		}

		private void AddLineToInitialView(Point startPoint, Point endPoint)
		{
			_initialViewItems.Add(new Path
			{
				Data = new LineGeometry(startPoint, endPoint),
				StrokeThickness = 2,
				Stroke = Brushes.Black
			});
		}

		private void AddPointToInitialView(Point point)
		{
			_initialViewItems.Add(new Path
			{
				Data = new EllipseGeometry(point, 3, 3),
				Fill = Brushes.Black,
			});
		}

		private bool IsPointOutOfBounds(Point point)
		{
			return Math.Abs(point.X) > 10 || Math.Abs(point.Y) > 10;
		}

		private bool IsPointInTheList(Point point)
		{
			return ListOfPoints.Any(p => p.ToString() == point.ToString());
		}

		private void DeleteAllPoints()
		{
			ListOfPoints.Clear();
			InitialViewItems.Clear();
			((DelegateCommand<object>)DeleteAllPointsCommand).RaiseCanExecuteChanged();
		}

		private bool CanDeleteAllPoints()
		{
			return ListOfPoints.Count > 0;
		}
	}
}

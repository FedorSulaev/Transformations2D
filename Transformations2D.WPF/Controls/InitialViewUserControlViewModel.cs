using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Practices.Prism.Commands;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Resourses.MVVM;
using Transformations2D.WPF.Utility;

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

		public ICommand DeletePointCommand
		{
			get
			{
				return _deletePointCommand ?? (_deletePointCommand = new DelegateCommand<object>(a => DeleteSelectedPoint(), a => CanDeleteSelectedPoint()));
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
				((DelegateCommand<object>)DeletePointCommand).RaiseCanExecuteChanged();
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
		private ICommand _deletePointCommand;

		private ObservableCollection<Path> _initialViewItems;
		private ObservableCollection<Point> _listOfPoints;

		private Point? _selectedPoint;

		private string _newPointCoordinates;

		private IUserInputParser _userInputParser;
		private IGeometryHelper _geometryHelper;

		private const int CanvasSideLength = 350;

		public InitialViewUserControlViewModel()
		{
			_userInputParser = DependencyFactory.Resolve<IUserInputParser>();
			_geometryHelper = DependencyFactory.Resolve<IGeometryHelper>();
			_listOfPoints = new ObservableCollection<Point>();
			_initialViewItems = new ObservableCollection<Path>();
			_listOfPoints.CollectionChanged += ListOfPoints_CollectionChanged;
		}

		void ListOfPoints_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			DrawFigure();
			((DelegateCommand<object>)DeleteAllPointsCommand).RaiseCanExecuteChanged();
			ServicesFactory.EventService.GetEvent<GenericEvent<List<Point>>>()
				.Publish(new EventParameters<List<Point>>("PointsChangedEvent", _listOfPoints.ToList()));
		}

		private void AddPoint()
		{
			Point? point = _userInputParser.StringToPoint(_newPointCoordinates);
			if (!IsUserPointOk(point)) return;
			_listOfPoints.Add((Point)point);
			NewPointCoordinates = string.Empty;
		}

		private void DrawFigure()
		{
			List<Point> convertedPoints = _geometryHelper.ConvertIntoCanvasCoordinates(ListOfPoints.ToList(), CanvasSideLength);
			InitialViewItems.Clear();
			InitialViewItems.Add(_geometryHelper.MakePath(convertedPoints));
		}

		private bool IsUserPointOk(Point? point)
		{
			if (point == null)
				return false;
			if (IsPointInTheList((Point) point) || IsPointOutOfBounds((Point) point))
				return false;
			return true;
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
		}

		private bool CanDeleteAllPoints()
		{
			return ListOfPoints.Count > 0;
		}

		private void DeleteSelectedPoint()
		{
			ListOfPoints.Remove((Point)_selectedPoint);
		}

		private bool CanDeleteSelectedPoint()
		{
			return SelectedPoint != null;
		}
	}
}

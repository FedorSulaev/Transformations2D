using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Utility;

namespace Transformations2D.WPF.Controls
{
	public class TransformViewUserControlViewModel
	{
		public IGeometryHelper GeometryHelper
		{
			get { return _geometryHelper; }
			set { _geometryHelper = value; }
		}

		public ObservableCollection<Path> TransformViewItems
		{
			get { return _transformViewItems; }
		}

		private List<Point> _initialPoints;
		private IGeometryHelper _geometryHelper;
		private readonly ObservableCollection<Path> _transformViewItems;
		private const int CanvasSideLength = 350;

		public TransformViewUserControlViewModel()
		{
			_geometryHelper = DependencyFactory.Resolve<IGeometryHelper>();
			_transformViewItems = new ObservableCollection<Path>();
			ServicesFactory.EventService.GetEvent<GenericEvent<List<Point>>>().Subscribe(s =>
			{
				if (s.Topic == "PointsChangedEvent")
				{
					_initialPoints = s.Value;
					DrawTransformation();
				}
			});
		}

		private void DrawTransformation()
		{
			List<Point> convertedPoints = _geometryHelper.ConvertIntoCanvasCoordinates(_initialPoints, CanvasSideLength);
			TransformViewItems.Clear();
			TransformViewItems.Add(_geometryHelper.MakePath(convertedPoints));
		}
	}
}

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Shapes;
using MathNet.Numerics.LinearAlgebra.Double;
using Microsoft.Practices.Prism.Commands;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;
using Transformations2D.WPF.Helpers;
using Transformations2D.WPF.Resourses.MVVM;
using Transformations2D.WPF.Utility;

namespace Transformations2D.WPF.Controls
{
	public class TransformViewUserControlViewModel : PropertyChangedImplementation
	{
		public ICommand AddTransformationCommand
		{
			get
			{
				return _addTransformationCommand ?? (_addTransformationCommand = new DelegateCommand<object>(a => AddTransformation()));
			}
		}

		public ICommand DeleteAllTransformationsCommand
		{
			get
			{
				return _deleteAllTransformationsCommand ?? (_deleteAllTransformationsCommand = new DelegateCommand<object>(
					a => DeleteAllTransformations(), a => CanDeleteAllTransformations));
			}
		}

		public ICommand DeleteTransformationCommand
		{
			get
			{
				return _deleteTransformationCommand ?? (_deleteTransformationCommand = new DelegateCommand<object>(
					a => DeleteTransformation(), a => CanDeleteTransformation));
			}
		}

		public bool CanDeleteAllTransformations
		{
			get { return Transformations.Any(); }
		}

		public bool CanDeleteTransformation
		{
			get { return SelectedTransformation != null; }
		}

		public ObservableCollection<Path> TransformViewItems
		{
			get { return _transformViewItems; }
		}

		public ObservableCollection<ITransformation2D> Transformations { get; private set; }

		public List<ITransformation2DType> TransformationTypes { get; set; }

		public ITransformation2DType SelectedTransformationType
		{
			get { return _selectedTransformationType; }
			set
			{
				_selectedTransformationType = value;
				ParameterNames = _selectedTransformationType.ParameterNames;
			}
		}

		public string ParameterNames
		{
			get { return _parameterNames; }
			set
			{
				_parameterNames = value;
				OnPropertyChanged("ParameterNames");
			}
		}

		public string Parameters
		{
			get { return _parameters; }
			set
			{
				_parameters = value;
				OnPropertyChanged("Parameters");
			}
		}

		public ITransformation2D SelectedTransformation
		{
			get { return _selectedTransformation; }
			set
			{
				_selectedTransformation = value;
				((DelegateCommand<object>)DeleteTransformationCommand).RaiseCanExecuteChanged();
			}
		}

		public IGeometryHelper GeometryHelper
		{
			get { return _geometryHelper; }
			set { _geometryHelper = value; }
		}

		public IUserInputParser UserInputParser
		{
			get { return _userInputParser; }
			set { _userInputParser = value; }
		}

		private ICommand _addTransformationCommand;
		private ICommand _deleteAllTransformationsCommand;
		private ICommand _deleteTransformationCommand;

		private List<Point> _initialPoints;
		private ITransformation2DType _selectedTransformationType;
		private string _parameterNames;
		private IGeometryHelper _geometryHelper;
		private IUserInputParser _userInputParser;
		private readonly ObservableCollection<Path> _transformViewItems;
		private string _parameters;
		private ITransformation2D _selectedTransformation;
		private const int CanvasSideLength = 350;

		public TransformViewUserControlViewModel()
		{
			_geometryHelper = DependencyFactory.Resolve<IGeometryHelper>();
			_userInputParser = DependencyFactory.Resolve<IUserInputParser>();
			_transformViewItems = new ObservableCollection<Path>();
			ServicesFactory.EventService.GetEvent<GenericEvent<List<Point>>>().Subscribe(s =>
			{
				if (s.Topic == "PointsChangedEvent")
				{
					_initialPoints = s.Value;
					DrawTransformation();
				}
			});
			InitializeTransformTypes();
			SelectedTransformationType = TransformationTypes.First();
			Transformations = new ObservableCollection<ITransformation2D>();
			Transformations.CollectionChanged += Transformations_CollectionChanged;
		}

		void Transformations_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			((DelegateCommand<object>)DeleteAllTransformationsCommand).RaiseCanExecuteChanged();
			DrawTransformation();
		}

		private void InitializeTransformTypes()
		{
			TransformationTypes = new List<ITransformation2DType>();
			TransformationTypes.Add(new RotationTransformation2DType());
			TransformationTypes.Add(new ScalingTransformation2DType());
			TransformationTypes.Add(new TranslationTransformation2DType());
			TransformationTypes.Add(new XReflectionTransformation2DType());
			TransformationTypes.Add(new YReflectionTransformation2DType());
		}

		private void DrawTransformation()
		{
			List<Point> convertedPoints = _geometryHelper.ConvertIntoCanvasCoordinates(Transformations.Count == 0 
				? _initialPoints : TransformPoints(_initialPoints), CanvasSideLength);
			TransformViewItems.Clear();
			TransformViewItems.Add(_geometryHelper.MakePath(convertedPoints));
		}

		private List<Point> TransformPoints(List<Point> initialPoints)
		{
			Queue<DenseMatrix> transformationsQueue = new Queue<DenseMatrix>();
			foreach (ITransformation2D transformation in Transformations)
			{
				transformationsQueue.Enqueue(transformation.Matrix);
			}
			DenseMatrix combinedTransformation = Transform2DService.CombineTransformations(transformationsQueue);
			return Transform2DService.TransformPoints(initialPoints, combinedTransformation);
		}

		private void AddTransformation()
		{
			double[] parameters = _userInputParser.StringToTransformParameters(Parameters);
			if (parameters.Count() >= SelectedTransformationType.NumberOfParameters)
			{
				Transformations.Add(_selectedTransformationType.GetTransformation(parameters));
				Parameters = string.Empty;
			}
		}

		private void DeleteAllTransformations()
		{
			Transformations.Clear();
		}

		private void DeleteTransformation()
		{
			Transformations.Remove(SelectedTransformation);
		}
	}
}

using System;
using System.Linq;
using Moq;
using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;
using Transformations2D.WPF.Controls;
using Transformations2D.WPF.Helpers;

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
	public class TransformViewUserControlViewModelTests
	{
		private static TransformViewUserControlViewModel MakeTransformViewUserControlViewModel(
			IGeometryHelper geometryHelper = null, IUserInputParser userInputParser = null)
		{
			TransformViewUserControlViewModel viewModel = new TransformViewUserControlViewModel();
			viewModel.UserInputParser = userInputParser ?? new Mock<IUserInputParser>().Object;
			viewModel.GeometryHelper = geometryHelper ?? new Mock<IGeometryHelper>().Object;
			return viewModel;
		}

		private static ITransformation2D MakeSomeTransformation2D()
		{
			return new XReflectionTransformation2D("name");
		}

		[TestCase(typeof(RotationTransformation2DType))]
		[TestCase(typeof(ScalingTransformation2DType))]
		[TestCase(typeof(TranslationTransformation2DType))]
		[TestCase(typeof(XReflectionTransformation2DType))]
		[TestCase(typeof(YReflectionTransformation2DType))]
		public void TransformationTypes_ViewModelCreated_ContainsAllTypesOfTransformations(Type transformationType)
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();

			Assert.IsTrue(viewModel.TransformationTypes.Any(tt => tt.GetType() == transformationType));
		}

		[Test]
		public void ParameterNames_SelectedTransformationTypeChanges_ChangeParameterNames()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			ITransformation2DType transformationType = new RotationTransformation2DType();

			viewModel.SelectedTransformationType = transformationType;

			Assert.AreEqual(transformationType.ParameterNames, viewModel.ParameterNames);
		}

		[Test]
		public void ParameterNames_ValueChanged_RaisePropertyChanged()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();

			viewModel.ShouldNotifyOn(vm => vm.ParameterNames).When(vm => vm.ParameterNames = "new");
		}

		[Test]
		public void Transformations_ViewModelCreated_IsEmpty()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();

			Assert.IsEmpty(viewModel.Transformations);
		}

		[Test]
		public void Transformations_AddTransformationExecutedWithValidParameters_AddTransformation()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel(null, new UserInputParser());
			viewModel.SelectedTransformationType = new ScalingTransformation2DType();
			viewModel.Parameters = "1, 2";

			viewModel.AddTransformationCommand.Execute(null);

			Assert.IsTrue(viewModel.Transformations.Any(t => t.Description == "Растяжение(1, 2)"));
		}

		[TestCase("abc")]
		[TestCase("1")]
		public void Transformation_AddTransformationExecutedWithInvalidParameters_DoNotAddTransformation(string parameters)
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel(null, new UserInputParser());
			viewModel.SelectedTransformationType = new ScalingTransformation2DType();
			viewModel.Parameters = parameters;

			viewModel.AddTransformationCommand.Execute(null);

			Assert.IsEmpty(viewModel.Transformations);
		}

		[Test]
		public void Parameters_TransformationAdded_ClearText()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel(null, new UserInputParser());
			viewModel.SelectedTransformationType = new ScalingTransformation2DType();
			viewModel.Parameters = "1, 2";

			viewModel.AddTransformationCommand.Execute(null);

			Assert.IsTrue(string.IsNullOrEmpty(viewModel.Parameters));
		}

		[Test]
		public void DeleteAllTransformationsCommand_TwoTransformationsAdded_DeleteAllTransformations()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			viewModel.Transformations.Add(MakeSomeTransformation2D());
			viewModel.Transformations.Add(MakeSomeTransformation2D());

			viewModel.DeleteAllTransformationsCommand.Execute(null);

			Assert.IsEmpty(viewModel.Transformations);
		}

		[Test]
		public void DeleteAllTransformationsCommand_NoTransformationsAdded_CanNotExecuteCommand()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			
			Assert.IsFalse(viewModel.DeleteAllTransformationsCommand.CanExecute(null));
		}

		[Test]
		public void DeleteAllTransformationsCommand_TransformationAdded_CanExecuteCommand()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			viewModel.Transformations.Add(MakeSomeTransformation2D());

			Assert.IsTrue(viewModel.DeleteAllTransformationsCommand.CanExecute(null));
		}

		[Test]
		public void DeleteAllTransformationsCommand_TransformationsCollectionsChanged_CanExecuteChangedRaised()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteAllTransformationsCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;

			viewModel.Transformations.Add(MakeSomeTransformation2D());

			Assert.IsTrue(canExecuteChangedWasRaised);
		}
		
		[Test]
		public void DeleteTransformationCommand_TransformationSelected_DeleteTransformation()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			ITransformation2D transformation = MakeSomeTransformation2D();
			viewModel.Transformations.Add(transformation);
			viewModel.SelectedTransformation = transformation;

			viewModel.DeleteTransformationCommand.Execute(null);

			Assert.IsFalse(viewModel.Transformations.Contains(transformation));
		}
		
		[Test]
		public void DeleteTransformationCommand_NoTransformationSelected_CanNotExecute()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			viewModel.Transformations.Add(MakeSomeTransformation2D());
			viewModel.SelectedTransformation = null;

			Assert.IsFalse(viewModel.DeleteTransformationCommand.CanExecute(null));
		}
		
		[Test]
		public void DeleteTransformationCommand_TransformationSelected_RaiseCanExecuteChanged()
		{
			TransformViewUserControlViewModel viewModel = MakeTransformViewUserControlViewModel();
			bool canExecuteChangedWasRaised = false;
			viewModel.DeleteTransformationCommand.CanExecuteChanged += (o, e) => canExecuteChangedWasRaised = true;

			viewModel.SelectedTransformation = MakeSomeTransformation2D();

			Assert.IsTrue(canExecuteChangedWasRaised);
		}
	}
}

using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;

namespace Transformations2D.UnitTests.TransformationTypesTests
{
	[TestFixture]
	public class ScalingTransformation2DTypeTests
	{
		private static ScalingTransformation2DType MakeScalingTransformationType()
		{
			return new ScalingTransformation2DType();
		}

		[Test]
		public void ScalingTransformation2DType_Initialized_IsInstanceOfITransformationType()
		{
			ScalingTransformation2DType transformation2D = MakeScalingTransformationType();

			Assert.IsInstanceOf<ITransformation2DType>(transformation2D);
		}

		[Test]
		public void Name_ScalingTransformation2DTypeInitialized_ReturnTransformaitonName()
		{
			ITransformation2DType transformation2D = MakeScalingTransformationType();

			Assert.AreEqual("Растяжение", transformation2D.Name);
		}

		[Test]
		public void NumberOfParameters_ScalingTransformation2DTypeInitialized_ReturnOne()
		{
			ITransformation2DType transformation2D = MakeScalingTransformationType();

			Assert.AreEqual(2, transformation2D.NumberOfParameters);
		}

		[Test]
		public void ParametersNames_ScalingTransformation2DTypeInitialized_ReturnParameterName()
		{
			ITransformation2DType transformation2D = MakeScalingTransformationType();

			Assert.AreEqual("Коэффициент X, Коэффициент Y", transformation2D.ParameterNames);
		}

		[Test]
		public void GetTransformation_ArrayWithTwoParameters_ReturnScalingTransformation2D()
		{
			ITransformation2DType transformation2DType = MakeScalingTransformationType();

			Assert.IsInstanceOf<ScalingTransformation2D>(transformation2DType.GetTransformation(new[] { 1.0, 1,0 }));
		}
	}
}

using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;

namespace Transformations2D.UnitTests.TransformationTypesTests
{
	[TestFixture]
	public class RotationTransformation2DTypeTests
	{
		private static RotationTransformation2DType MakeRotationTransformationType()
		{
			return new RotationTransformation2DType();
		}

		[Test]
		public void RotationTransformation2DType_Initialized_IsInstanceOfITransformationType()
		{
			RotationTransformation2DType transformation2D = MakeRotationTransformationType();

			Assert.IsInstanceOf<ITransformation2DType>(transformation2D);
		}

		[Test]
		public void Name_RotationTransformation2DTypeInitialized_ReturnTransformaitonName()
		{
			ITransformation2DType transformation2D = MakeRotationTransformationType();

			Assert.AreEqual("Поворот", transformation2D.Name);
		}

		[Test]
		public void NumberOfParameters_RotationTransformation2DTypeInitialized_ReturnOne()
		{
			ITransformation2DType transformation2D = MakeRotationTransformationType();

			Assert.AreEqual(1, transformation2D.NumberOfParameters);
		}

		[Test]
		public void ParametersNames_RotationTransformation2DTypeInitialized_ReturnParameterName()
		{
			ITransformation2DType transformation2D = MakeRotationTransformationType();

			Assert.AreEqual("Угол", transformation2D.ParameterNames);
		}

		[Test]
		public void GetTransformation_ArrayWithOneParameter_ReturnRotationTransformation2D()
		{
			ITransformation2DType transformation2DType = MakeRotationTransformationType();

			Assert.IsInstanceOf<RotationTransformation2D>(transformation2DType.GetTransformation(new[]{1.0}));
		}
	}
}

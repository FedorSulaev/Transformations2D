using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;

namespace Transformations2D.UnitTests.TransformationTypesTests
{
	[TestFixture]
	public class YReflectionTransformation2DTypeTests
	{
		private static YReflectionTransformation2DType MakeYReflectionTransformationType()
		{
			return new YReflectionTransformation2DType();
		}

		[Test]
		public void YReflectionTransformation2DType_Initialized_IsInstanceOfITransformationType()
		{
			YReflectionTransformation2DType transformation2D = MakeYReflectionTransformationType();

			Assert.IsInstanceOf<ITransformation2DType>(transformation2D);
		}

		[Test]
		public void Name_YReflectionTransformation2DTypeInitialized_ReturnTransformaitonName()
		{
			ITransformation2DType transformation2D = MakeYReflectionTransformationType();

			Assert.AreEqual("Отражение по Y", transformation2D.Name);
		}

		[Test]
		public void NumberOfParameters_YReflectionTransformation2DTypeInitialized_ReturnOne()
		{
			ITransformation2DType transformation2D = MakeYReflectionTransformationType();

			Assert.AreEqual(0, transformation2D.NumberOfParameters);
		}

		[Test]
		public void ParametersNames_YReflectionTransformation2DTypeInitialized_ReturnParameterName()
		{
			ITransformation2DType transformation2D = MakeYReflectionTransformationType();

			Assert.AreEqual(string.Empty, transformation2D.ParameterNames);
		}

		[Test]
		public void GetTransformation_EmptyArray_ReturnYReflectionTransformation2D()
		{
			ITransformation2DType transformation2DType = MakeYReflectionTransformationType();

			Assert.IsInstanceOf<YReflectionTransformation2D>(transformation2DType.GetTransformation(new double[0]));
		}
	}
}

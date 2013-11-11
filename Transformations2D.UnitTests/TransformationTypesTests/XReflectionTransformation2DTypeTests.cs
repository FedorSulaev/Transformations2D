using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;

namespace Transformations2D.UnitTests.TransformationTypesTests
{
	[TestFixture]
	public class XReflectionTransformation2DTypeTests
	{
		private static XReflectionTransformation2DType MakeXReflectionTransformationType()
		{
			return new XReflectionTransformation2DType();
		}

		[Test]
		public void XReflectionTransformation2DType_Initialized_IsInstanceOfITransformationType()
		{
			XReflectionTransformation2DType transformation2D = MakeXReflectionTransformationType();

			Assert.IsInstanceOf<ITransformation2DType>(transformation2D);
		}

		[Test]
		public void Name_XReflectionTransformation2DTypeInitialized_ReturnTransformaitonName()
		{
			ITransformation2DType transformation2D = MakeXReflectionTransformationType();

			Assert.AreEqual("Отражение по X", transformation2D.Name);
		}

		[Test]
		public void NumberOfParameters_XReflectionTransformation2DTypeInitialized_ReturnOne()
		{
			ITransformation2DType transformation2D = MakeXReflectionTransformationType();

			Assert.AreEqual(0, transformation2D.NumberOfParameters);
		}

		[Test]
		public void ParametersNames_XReflectionTransformation2DTypeInitialized_ReturnParameterName()
		{
			ITransformation2DType transformation2D = MakeXReflectionTransformationType();

			Assert.AreEqual(string.Empty, transformation2D.ParameterNames);
		}

		[Test]
		public void GetTransformation_EmptyArray_ReturnXReflectionTransformation2D()
		{
			ITransformation2DType transformation2DType = MakeXReflectionTransformationType();

			Assert.IsInstanceOf<XReflectionTransformation2D>(transformation2DType.GetTransformation(new double[0]));
		}
	}
}

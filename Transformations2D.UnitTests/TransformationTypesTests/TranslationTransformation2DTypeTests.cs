using NUnit.Framework;
using Transformations2D.Transformations;
using Transformations2D.TransformationTypes;

namespace Transformations2D.UnitTests.TransformationTypesTests
{
	[TestFixture]
	public class TranslationTransformation2DTypeTests
	{
		private static TranslationTransformation2DType MakeTranslationTransformation2DType()
		{
			return new TranslationTransformation2DType();
		}

		[Test]
		public void YReflectionTransformation2DType_Initialized_IsInstanceOfITransformationType()
		{
			TranslationTransformation2DType transformation2D = MakeTranslationTransformation2DType();

			Assert.IsInstanceOf<ITransformation2DType>(transformation2D);
		}

		[Test]
		public void Name_TranslationTransformation2DTypeInitialized_ReturnTransformaitonName()
		{
			ITransformation2DType transformation2D = MakeTranslationTransformation2DType();

			Assert.AreEqual("Перенос", transformation2D.Name);
		}

		[Test]
		public void NumberOfParameters_TranslationTransformation2DTypeInitialized_ReturnOne()
		{
			ITransformation2DType transformation2D = MakeTranslationTransformation2DType();

			Assert.AreEqual(2, transformation2D.NumberOfParameters);
		}

		[Test]
		public void ParametersNames_TranslationTransformation2DTypeInitialized_ReturnParameterName()
		{
			ITransformation2DType transformation2D = MakeTranslationTransformation2DType();

			Assert.AreEqual("Смещение по X, Смещение по Y", transformation2D.ParameterNames);
		}

		[Test]
		public void GetTransformation_ArrayWithTwoParameters_ReturnTranslationTransformation2D()
		{
			ITransformation2DType transformation2DType = MakeTranslationTransformation2DType();

			Assert.IsInstanceOf<TranslationTransformation2D>(transformation2DType.GetTransformation(new[] { 1.0, 1, 0 }));
		}
	}
}

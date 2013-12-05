using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Transformations2D.Transformations;

namespace Transformations2D.UnitTests.TransformationsTests
{
	[TestFixture]
	public class TranslationTransformationTests
	{
		[Test]
		public void Description_AfterConstruction_ReturnDescriptionOfTransformation()
		{
			ITransformation2D transformation = new TranslationTransformation2D("Перенос", 1.2d, 3.4d);

			Assert.AreEqual("Перенос(1.2, 3.4)", transformation.Description);
		}

		[Test]
		public void Matrix_AfterConstruction_ReturnMatrix()
		{
			ITransformation2D transformation = new TranslationTransformation2D("name", 1d, 2d);

			Assert.IsAssignableFrom<DenseMatrix>(transformation.Matrix);
		}
	}
}

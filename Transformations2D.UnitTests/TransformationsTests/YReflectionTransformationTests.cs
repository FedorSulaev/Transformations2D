using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Transformations2D.Transformations;

namespace Transformations2D.UnitTests.TransformationsTests
{
	[TestFixture]
	public class YReflectionTransformationTests
	{
		[Test]
		public void Description_AfterConstruction_ReturnDescriptionOfTransformation()
		{
			ITransformation2D transformation = new YReflectionTransformation2D("Отражение по Y");

			Assert.AreEqual("Отражение по Y", transformation.Description);
		}

		[Test]
		public void Matrix_AfterConstruction_ReturnMatrix()
		{
			ITransformation2D transformation = new YReflectionTransformation2D("name");

			Assert.IsAssignableFrom<DenseMatrix>(transformation.Matrix);
		}
	}
}

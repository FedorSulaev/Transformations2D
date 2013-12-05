using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Transformations2D.Transformations;

namespace Transformations2D.UnitTests.TransformationsTests
{
	[TestFixture]
	public class RotationTransformationTests
	{
		[Test]
		public void Description_AfterConstruction_ReturnDescriptionOfTransformation()
		{
			ITransformation2D transformation = new RotationTransformation2D("Поворот", 1.2d);

			Assert.AreEqual("Поворот(1.2)", transformation.Description);
		}

		[Test]
		public void Matrix_AfterConstruction_ReturnMatrix()
		{
			ITransformation2D transformation = new RotationTransformation2D("name", 1d);

			Assert.IsAssignableFrom<DenseMatrix>(transformation.Matrix);
		}
	}
}

using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;
using Transformations2D.Transformations;

namespace Transformations2D.UnitTests.TransformationsTests
{
	[TestFixture]
	public class XReflectionTransformationTests
	{
		[Test]
		public void Description_AfterConstruction_ReturnDescriptionOfTransformation()
		{
			ITransformation2D transformation = new XReflectionTransformation2D("Отражение по X");

			Assert.AreEqual("Отражение по X", transformation.Description);
		}

		[Test]
		public void Matrix_AfterConstruction_ReturnMatrix()
		{
			ITransformation2D transformation = new XReflectionTransformation2D("name");

			Assert.IsAssignableFrom<DenseMatrix>(transformation.Matrix);
		}
	}
}

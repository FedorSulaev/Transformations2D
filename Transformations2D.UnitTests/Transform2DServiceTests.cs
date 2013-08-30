using System.Collections.Generic;
using System.Windows;
using MathNet.Numerics.LinearAlgebra.Double;
using NUnit.Framework;

namespace Transformations2D.UnitTests
{
	[TestFixture]
    public class Transform2DServiceTests
    {
		private const double ErrorDelta = 0.00001;

		private static bool AreEqual(DenseMatrix a, DenseMatrix b)
		{
			Assert.AreEqual(a[0, 0], b[0, 0], ErrorDelta);
			Assert.AreEqual(a[0, 1], b[0, 1], ErrorDelta);
			Assert.AreEqual(a[0, 2], b[0, 2], ErrorDelta);
			Assert.AreEqual(a[1, 0], b[1, 0], ErrorDelta);
			Assert.AreEqual(a[1, 1], b[1, 1], ErrorDelta);
			Assert.AreEqual(a[1, 2], b[1, 2], ErrorDelta);
			Assert.AreEqual(a[2, 0], b[2, 0], ErrorDelta);
			Assert.AreEqual(a[2, 1], b[2, 1], ErrorDelta);
			Assert.AreEqual(a[2, 2], b[2, 2], ErrorDelta);
			return true;
		}

		private bool AreEqual(Point a, Point b)
		{
			Assert.AreEqual(a.X, b.X, ErrorDelta);
			Assert.AreEqual(a.Y, b.Y, ErrorDelta);
			return true;
		}

		private bool AreEqual(List<Point> listA, List<Point> listB)
		{
			for (int i = 0; i < listA.Count; i++)
				AreEqual(listA[i], listB[i]);
			return true;
		}

		[TestCase(0.5d, new[] { 0.87758, -0.47942, 0, 0.47942, 0.87758, 0, 0, 0, 1 })]
		[TestCase(2.5d, new[] { -0.80114, -0.59847, 0, 0.59847, -0.80114, 0, 0, 0, 1})]
		public void MakeRotationMatrix_AngleInRadians_ReturnRotationMatrix(double angle, double[] expectedMatrixData)
		{
			DenseMatrix expected = new DenseMatrix(3, 3, expectedMatrixData);

			DenseMatrix result = Transform2DService.MakeRotationMatrix(angle);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[TestCase(1.1, 1.2, new[] { 1.1, 0, 0, 0, 1.2, 0, 0, 0, 1 })]
		[TestCase(0.9, 0.8, new[] { 0.9, 0, 0, 0, 0.8, 0, 0, 0, 1 })]
		public void MakeScalingMatrix_XYFactors_ReturnScalingMatrix(double xFactor, double yFactor, double[] expectedMatrixData)
		{
			DenseMatrix expected = new DenseMatrix(3, 3, expectedMatrixData);

			DenseMatrix result = Transform2DService.MakeScalingMatrix(xFactor, yFactor);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void MakeXReflectionMatrix_None_ReturnReflectionMatrixAboutX()
		{
			DenseMatrix expected = new DenseMatrix(3, 3, new[] {-1d, 0, 0, 0, 1, 0, 0, 0, 1});

			DenseMatrix result = Transform2DService.MakeXReflectionMatrix();

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void MakeYReflectionMatrix_None_ReturnReflectionMatrixAboutY()
		{
			DenseMatrix expected = new DenseMatrix(3, 3, new[] {1d, 0, 0, 0, -1, 0, 0, 0, 1});

			DenseMatrix result = Transform2DService.MakeYReflectionMatrix();

			Assert.IsTrue(AreEqual(expected, result));
		}

		[TestCase(0.1, 0.2, new[] {1, 0, 0.1, 0, 1, 0.2, 0, 0, 1})]
		[TestCase(-0.1, -0.2, new[] {1, 0, -0.1, 0, 1, -0.2, 0, 0, 1})]
		public void MakeTranslationMatrix_XYDistance_ReturnTranslationMatrix(double xDistance, double yDistance,
		                                                                     double[] expectedMatrixData)
		{
			DenseMatrix expected = new DenseMatrix(3, 3, expectedMatrixData);

			DenseMatrix result = Transform2DService.MakeTranslationMatrix(xDistance, yDistance);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void CombineTransformations_1TransformMatrix_ReturnSameMatrix()
		{
			Queue<DenseMatrix> transformations = new Queue<DenseMatrix>();
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { 1.1, 0, 0, 0, 1.2, 0, 0, 0, 1 }));
			DenseMatrix expected = new DenseMatrix(3, 3, new[] { 1.1, 0, 0, 0, 1.2, 0, 0, 0, 1 });

			DenseMatrix result = Transform2DService.CombineTransformations(transformations);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void CombineTransformations_2TransformMatrices_CombineInOneTransformMatrix()
		{
			Queue<DenseMatrix> transformations = new Queue<DenseMatrix>();
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { 1.1, 0, 0, 0, 1.2, 0, 0, 0, 1 }));
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { 1, 0, 0.1, 0, 1, 0.2, 0, 0, 1 }));
			DenseMatrix expected = new DenseMatrix(3, 3, new[] {1.1, 0, 0.1, 0, 1.2, 0.2, 0, 0, 1});

			DenseMatrix result = Transform2DService.CombineTransformations(transformations);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void CombineTransformations_3TransformMatrices_CombineInOneTransformMatrix()
		{
			Queue<DenseMatrix> transformations = new Queue<DenseMatrix>();
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { 1, 0, 0.2, 0, 1, 0.3, 0, 0, 1 }));
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { 1.2, 0, 0, 0, 1.3, 0, 0, 0, 1 }));
			transformations.Enqueue(new DenseMatrix(3, 3, new[] { -1d, 0, 0, 0, 1, 0, 0, 0, 1 }));
			DenseMatrix expected = new DenseMatrix(3, 3, new[] { -1.2, 0, -0.24, 0, 1.3, 0.39, 0, 0, 1 });

			DenseMatrix result = Transform2DService.CombineTransformations(transformations);

			Assert.IsTrue(AreEqual(expected, result));
		}

		[Test]
		public void TransformPoints_1PointAndTransformMatrix_ReturnTransformedPoint()
		{
			DenseMatrix transformMatrix = new DenseMatrix(3, 3, new[] { 1, 0, 0.1, 0, 1, 0.2, 0, 0, 1 });
			List<Point> points = new List<Point>{new Point(1.0, 1.0)};
			Point expected = new Point(1.1, 1.2);

			List<Point> result = Transform2DService.TransformPoints(points, transformMatrix);

			Assert.IsTrue(AreEqual(expected, result[0]));
		}

		[Test]
		public void TransformPoints_2PointsAndTransformMatrix_ReturnTransformedPoints()
		{
			DenseMatrix transformMatrix = new DenseMatrix(3, 3, new[] { 1.1, 0, 0, 0, 1.2, 0, 0, 0, 1 });
			List<Point> points = new List<Point> {new Point(1.0, 1.0), new Point(2.0, 2.0)};
			List<Point> expected = new List<Point> {new Point(1.1, 1.2), new Point(2.2, 2.4)};

			List<Point> result = Transform2DService.TransformPoints(points, transformMatrix);

			Assert.IsTrue(AreEqual(expected, result));
		}
    }
}

using System;
using System.Collections.Generic;
using System.Windows;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D
{
    public class Transform2DService
    {
	    public static DenseMatrix MakeRotationMatrix(double angle)
	    {
		    return new DenseMatrix(3, 3, new []
			    {
				    Math.Cos(angle), -Math.Sin(angle), 0, 
					Math.Sin(angle), Math.Cos(angle), 0,
					0, 0, 1
			    });
	    }

	    public static DenseMatrix MakeScalingMatrix(double xFactor, double yFactor)
	    {
		    return new DenseMatrix(3, 3, new[]
			    {
					xFactor, 0, 0,
					0, yFactor, 0,
					0, 0, 1
			    });
	    }

	    public static DenseMatrix MakeXReflectionMatrix()
	    {
		    return new DenseMatrix(3, 3, new[]
			    {
				    -1d, 0, 0,
					0, 1, 0,
					0, 0, 1
			    });
	    }

		public static DenseMatrix MakeYReflectionMatrix()
		{
			return new DenseMatrix(3,3,new[]
				{
					1d, 0, 0,
					0, -1, 0,
					0, 0, 1
				});
		}

	    public static DenseMatrix MakeTranslationMatrix(double xDistance, double yDistance)
	    {
		    return new DenseMatrix(3,3,new[]
			    {
				    1, 0, xDistance,
					0, 1, yDistance,
					0, 0, 1
			    });
	    }

	    public static DenseMatrix CombineTransformations(Queue<DenseMatrix> transformations)
	    {
		    DenseMatrix result = transformations.Dequeue();
			int transformNum = transformations.Count;
		    for (int i = 0; i < transformNum; i++)
			    result = (DenseMatrix)result.Multiply(transformations.Dequeue());
		    return result;
	    }

	    public static List<Point> TransformPoints(List<Point> points, DenseMatrix transformMatrix)
	    {
			List<Point> transformedPoints = new List<Point>();
		    foreach (Point point in points)
		    {
				DenseVector pointVector = new DenseVector(new[] { point.X, point.Y, 1 });
				pointVector = (DenseVector)transformMatrix.LeftMultiply(pointVector);
				transformedPoints.Add(new Point(pointVector[0], pointVector[1]));
		    }
		    return transformedPoints;
	    }
    }
}

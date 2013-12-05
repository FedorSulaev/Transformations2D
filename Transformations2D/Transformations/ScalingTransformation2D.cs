using System.Globalization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D.Transformations
{
	public class ScalingTransformation2D : ITransformation2D
	{
		public string Description { get; private set; }
		public DenseMatrix Matrix { get; private set; }

		public ScalingTransformation2D(string name, double xFactor, double yFactor)
		{
			CultureInfo culture = CultureInfo.GetCultureInfo("en");
			Description = name + "(" + xFactor.ToString(culture) + ", " + yFactor.ToString(culture) + ")";
			Matrix = Transform2DService.MakeScalingMatrix(xFactor, yFactor);
		}
	}
}

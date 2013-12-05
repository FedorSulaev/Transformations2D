using System.Globalization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D.Transformations
{
	public class RotationTransformation2D : ITransformation2D
	{
		public string Description { get; private set; }
		public DenseMatrix Matrix { get; private set; }

		public RotationTransformation2D(string name, double angle)
		{
			Description = name + "(" + angle.ToString(CultureInfo.GetCultureInfo("en")) + ")";
			Matrix = Transform2DService.MakeRotationMatrix(angle);
		}
	}
}
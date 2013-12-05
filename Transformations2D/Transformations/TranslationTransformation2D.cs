using System.Globalization;
using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D.Transformations
{
	public class TranslationTransformation2D : ITransformation2D
	{
		public string Description { get; private set; }
		public DenseMatrix Matrix { get; private set; }

		public TranslationTransformation2D(string name, double xDistance, double yDistance)
		{
			CultureInfo culture = CultureInfo.GetCultureInfo("en");
			Description = name + "(" + xDistance.ToString(culture) + ", " + yDistance.ToString(culture) + ")";
			Matrix = Transform2DService.MakeTranslationMatrix(xDistance, yDistance);
		}
	}
}

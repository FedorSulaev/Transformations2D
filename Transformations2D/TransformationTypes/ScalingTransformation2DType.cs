using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public class ScalingTransformation2DType : ITransformation2DType
	{
		public string Name
		{
			get { return "Растяжение"; }
		}

		public int NumberOfParameters
		{
			get { return 2; }
		}

		public string ParameterNames
		{
			get { return "Коэффициент X, Коэффициент Y"; }
		}

		public ITransformation2D GetTransformation(double[] parameters)
		{
			return new ScalingTransformation2D(Name, parameters[0], parameters[1]);
		}
	}
}

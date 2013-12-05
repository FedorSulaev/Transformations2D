using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public class RotationTransformation2DType : ITransformation2DType
	{
		public string Name
		{
			get { return "Поворот"; }
		}

		public int NumberOfParameters
		{
			get { return 1; }
		}

		public string ParameterNames
		{
			get { return "Угол"; }
		}

		public ITransformation2D GetTransformation(double[] parameters)
		{
			return new RotationTransformation2D(Name, parameters[0]);
		}
	}
}
using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public class XReflectionTransformation2DType : ITransformation2DType
	{
		public string Name
		{
			get { return "Отражение по X"; }
		}

		public int NumberOfParameters
		{
			get { return 0; }
		}

		public string ParameterNames
		{
			get { return string.Empty; }
		}

		public ITransformation2D GetTransformation(double[] parameters)
		{
			return new XReflectionTransformation2D(Name);
		}
	}
}

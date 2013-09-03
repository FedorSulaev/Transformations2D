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
	}
}
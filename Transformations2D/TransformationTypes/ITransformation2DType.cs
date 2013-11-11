using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public interface ITransformation2DType
	{
		string Name { get; }
		int NumberOfParameters { get; }
		string ParameterNames { get; }

		ITransformation2D GetTransformation(double[] parameters);
	}
}
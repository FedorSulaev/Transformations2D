using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D.Transformations
{
	public interface ITransformation2D
	{
		string Description { get; }
		DenseMatrix Matrix { get; }
	}
}
using MathNet.Numerics.LinearAlgebra.Double;

namespace Transformations2D.Transformations
{
	public class XReflectionTransformation2D : ITransformation2D
	{
		public string Description { get; private set; }
		public DenseMatrix Matrix { get; private set; }

		public XReflectionTransformation2D(string name)
		{
			Description = name;
			Matrix = Transform2DService.MakeXReflectionMatrix();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}

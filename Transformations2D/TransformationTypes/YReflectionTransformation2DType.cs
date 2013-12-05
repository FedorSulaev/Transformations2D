using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public class YReflectionTransformation2DType : ITransformation2DType
	{
		public string Name
		{
			get { return "Отражение по Y"; }
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
			return new YReflectionTransformation2D(Name);
		}
	}
}

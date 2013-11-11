using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Transformations2D.Transformations;

namespace Transformations2D.TransformationTypes
{
	public class TranslationTransformation2DType : ITransformation2DType
	{
		public string Name
		{
			get { return "Перенос"; }
		}

		public int NumberOfParameters
		{
			get { return 2; }
		}

		public string ParameterNames
		{
			get { return "Смещение по X, Смещение по Y"; }
		}

		public ITransformation2D GetTransformation(double[] parameters)
		{
			return new TranslationTransformation2D(parameters[0], parameters[1]);
		}
	}
}

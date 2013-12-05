using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace Transformations2D.WPF.Helpers
{
	public class UserInputParser : IUserInputParser
	{
		public Point? StringToPoint(string input)
		{
			try
			{
				return Point.Parse(input);
			}
			catch
			{
				return null;
			}
		}

		public double[] StringToTransformParameters(string input)
		{
			char[] splitSymbols = {' '};
			if (input.Contains(','))
			{
				splitSymbols = new []{','};
			}
			string[] parameters = input.Split(splitSymbols);
			double[] parsedParameters;
			try
			{
				parsedParameters = parameters.Select(p => double.Parse(p, CultureInfo.GetCultureInfo("en"))).ToArray();
			}
			catch (Exception)
			{
				parsedParameters = new double[0];
			}
			return parsedParameters;
		}
	}
}

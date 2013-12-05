using System.Globalization;
using System.Linq;
using System.Windows;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Transformations2D.WPF.Helpers;

namespace Transformations2D.WPF.UnitTests
{
	[TestFixture]
	class UserInputParserTests
	{
		private static IUserInputParser GetIUserInputParser()
		{
			return new UserInputParser();
		}

		[TestCase("1,2", "1,2")]
		[TestCase("3,4", "3,4")]
		public void StringToPoint_CoordinatesWithCommaBetween_ReturnPointWithEnteredCoordinates(string coordinates, string expected)
		{
			IUserInputParser parser = GetIUserInputParser();

			Point? result = parser.StringToPoint(coordinates);

			Assert.AreEqual(expected, ((Point)result).ToString(CultureInfo.GetCultureInfo("en")));
		}

		[Test]
		public void StringToPoint_EmptyString_ReturnNull()
		{
			IUserInputParser parser = GetIUserInputParser();

			Point? result = parser.StringToPoint(string.Empty);

			Assert.AreEqual(null, result);
		}

		[Test]
		public void StringToPoint_StringWithoutComma_ReturnNull()
		{
			IUserInputParser parser = GetIUserInputParser();

			Point? result = parser.StringToPoint("12");

			Assert.AreEqual(null, result);
		}

		[TestCase("0", 0d)]
		[TestCase("1", 1d)]
		public void StringToTransformParameters_ValidStringWithOneParameter_ReturnOneParsedParameterInArray(string input, double expected)
		{
			IUserInputParser parser = GetIUserInputParser();

			double[] result = parser.StringToTransformParameters(input);

			Assert.AreEqual(1, result.Count());
			Assert.Contains(expected, result);
		}

		[TestCase("0, 1", 0d, 1d)]
		[TestCase("1 2", 1d, 2d)]
		public void StringToTransformParameters_ValidStringWithTwoParameters_ReturnTwoParsedParametersInArray(string input, double firstExpected,
			double secondExpected)
		{
			IUserInputParser parser = GetIUserInputParser();

			double[] result = parser.StringToTransformParameters(input);

			Assert.AreEqual(2, result.Count());
			Assert.Contains(firstExpected, result);
			Assert.Contains(secondExpected, result);
		}

		[Test]
		public void StringToTransformParameters_InvalidString_ReturnEmptyArray()
		{
			IUserInputParser parser = GetIUserInputParser();

			double[] result = parser.StringToTransformParameters("abc");

			Assert.IsEmpty(result);
		}
	}
}

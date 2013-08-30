using System.Globalization;
using System.Windows;
using NUnit.Framework;
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
	}
}

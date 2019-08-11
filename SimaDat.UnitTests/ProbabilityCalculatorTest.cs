using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Shared;

namespace SimaDat.UnitTests
{
	[TestClass]
	public class ProbabilityCalculatorTest
	{
		[TestMethod]
		public void ProbabilityToKiss()
		{
			// 0.8 * 100 / 200 + 0.09 * (3 - 2) = 0.49
			float p = ProbabilityCalculator.ProbabilityToKiss(100, Models.Enums.FriendshipLevels.Friend);

			p.Should().Be(0.49f);
		}
	}
}

using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models;
using SimaDat.Shared;

namespace SimaDat.UnitTests
{
	[TestClass]
	public class ProbabilityCalculatorTest
	{
		#region Probability to kiss

		[TestMethod]
		public void ProbabilityToKiss_GetMax_WhenKissPointsAreZero()
		{
			float p = ProbabilityCalculator.ProbabilityToKiss(MySettings.MaxCharmForHero, Models.Enums.FriendshipLevels.Lover, 0);

			// Max probability without kiss points (during dating) is extremelly small
			p.Should().Be(0.02f);
		}

		[TestMethod]
		public void ProbabilityToKisst_GetMin_WhenKissPointsAreZero()
		{
			float p = ProbabilityCalculator.ProbabilityToKiss(0, Models.Enums.FriendshipLevels.Familar, 0);

			// Expecting small probability, but not 0
			p.Should().Be(0.0005f);
		}

		#endregion
	}
}

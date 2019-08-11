using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using System;

namespace SimaDat.UnitTests
{
	[TestClass]
    public class ProbabilityBllTest
	{
        private IProbabilityBll _bll = null;

		private Mock<IRandomProvider> _mockRandomProvider = new Mock<IRandomProvider>();

        private Hero _me = null;
        private Girl _girl = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new ProbabilityBll(_mockRandomProvider.Object);

            _me = new Hero();
            _me.CurrentLocationId = 100;
            _me.ModifySkill(HeroSkills.Charm, MySettings.MaxCharmForHero);

            _girl = new Girl("Friend girl", FriendshipLevels.Friend);
            _girl.CurrentLocationId = _me.CurrentLocationId;
        }

        #region Request dating

        [TestMethod]
        public void RequestDating_True()
        {
			SetupDating(20, 0.09);

            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeTrue();
        }

        [TestMethod]
        public void RequestDating_False()
        {
			SetupDating(0, 001);

            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeFalse();
        }

        [TestMethod]
        public void RequestDating_False_WhenMaxCharm()
        {
			// Set to max charm, but random probability to reject dating
			SetupDating(MySettings.MaxCharmForHero, 0.995);

			bool isDating = _bll.RequestDating(_me, _girl);

			// Expecting dating was rejected even max charm
			isDating.Should().BeFalse();
        }

		#endregion

		#region Kiss

		[TestMethod]
		public void Kiss_Exception_WhenDatingIsOver()
		{
			bool isOk = false;
			var dl = SetupForKiss(_girl, 10, 0);
			dl.IsOver = true;

			try
			{
				_bll.Kiss(dl);
			}
			catch (ObjectDoesNotExistException odneex) when (odneex.ObjectId == dl.DatingLocationId)
			{
				isOk = true;
			}

			// Expecting exception when dating is over
			isOk.Should().BeTrue();
		}

		[TestMethod]
		public void Kiss_Success_WhenCharmAndLover()
		{
			var lover = new Girl("Lover girl", FriendshipLevels.Lover);
			var dl = SetupForKiss(lover, MySettings.MaxCharmForHero, 0.97);

			// Expecting success, because high charm gives probablitity ~ 0.98
			_bll.Kiss(dl).Should().BeTrue();
		}

		[TestMethod]
		public void Kiss_Success_WhenLowProbabilityWithoutCharm()
		{
			var datingLocation = SetupForKiss(_girl, 0, 0.08);

			// Expecting low probability (~0.09), but possible
			_bll.Kiss(datingLocation).Should().BeTrue();
		}

		#endregion

		#region Setup

		private void SetupDating(int heroCharm, double randomValue)
		{
			SetHeroCharm(heroCharm);
			_me.ResetTtl();

			_mockRandomProvider.Setup(x => x.NextDouble())
				.Returns(randomValue);
		}

		private DatingLocation SetupForKiss(Girl g, int heroCharm, double randomValue)
		{
			SetHeroCharm(heroCharm);
			_me.ResetTtl();

			_mockRandomProvider.Setup(x => x.NextDouble())
				.Returns(randomValue);

			var datingLocation = new DatingLocation("Test", 0)
			{
				DatingLocationId = 100,
				Girl = g,
				Hero = _me,
			};

			BllFactory.Current.DatingBll.JoinDating(_me, g, datingLocation);

			return datingLocation;
		}

		private void SetHeroCharm(int charm)
		{
			_me.ModifySkill(HeroSkills.Charm, -_me.Charm);
			_me.ModifySkill(HeroSkills.Charm, charm);
		}

		#endregion

	}
}

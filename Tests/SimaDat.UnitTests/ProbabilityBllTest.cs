using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using SimaDat.Core;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Shared;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class ProbabilityBllTest
    {
        private IProbabilityBll _bll;

        // private readonly Mock<IRandomProvider> _mockRandomProvider = new Mock<IRandomProvider>();

        private Hero _me;
        private Girl _girl;
        private readonly IRandomProvider _randomProvider = Substitute.For<IRandomProvider>();

        [TestInitialize]
        public void TestInit()
        {
            _bll = new ProbabilityBll(_randomProvider);

            _me = new Hero { CurrentLocationId = 100 };
            _me.ModifySkill(HeroSkills.Charm, MySettings.MaxCharmForHero);

            _girl = new Girl("Friend girl", FriendshipLevels.Friend) { CurrentLocationId = _me.CurrentLocationId };
        }

        #region Request dating

        [TestMethod]
        public void RequestDating_True()
        {
            SetupDating(20, 0.09);

            var actual = _bll.RequestDating(_me, _girl);

            actual.Should().BeTrue();
        }

        [TestMethod]
        public void RequestDating_False()
        {
            SetupDating(0, 001);

            var actual = _bll.RequestDating(_me, _girl);

            actual.Should().BeFalse();
        }

        [TestMethod]
        public void RequestDating_False_WhenMaxCharm()
        {
            // Set to max charm, but random probability to reject dating
            SetupDating(MySettings.MaxCharmForHero, 0.995);

            var actual = _bll.RequestDating(_me, _girl);

            // Expecting dating was rejected even max charm
            actual.Should().BeFalse();
        }

        #endregion Request dating

        #region Kiss

        [TestMethod]
        public void Kiss_Exception_WhenDatingIsOver()
        {
            var isOk = false;
            var datingLocation = SetupForKiss(_girl, 10, 0);
            datingLocation.IsOver = true;

            try
            {
                _bll.Kiss(datingLocation);
            }
            catch (ObjectDoesNotExistException odneex) when (odneex.ObjectId == datingLocation.DatingLocationId)
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
            var probability = ProbabilityCalculator.ProbabilityToKiss(0, lover.FriendshipLevel);
            var datingLocation = SetupForKiss(lover, MySettings.MaxCharmForHero, probability - 0.0001);

            // Expecting success, because high charm gives probablitity ~ 0.98
            _bll.Kiss(datingLocation).Should().BeTrue();
        }

        [TestMethod]
        public void Kiss_Success_WhenLowProbabilityWithoutCharm()
        {
            // Setup random value to be smaller then smallest probability
            float probability = ProbabilityCalculator.ProbabilityToKiss(0, _girl.FriendshipLevel);
            var datingLocation = SetupForKiss(_girl, 0, probability - 0.0001);

            // Expecting low probability (~0.09), but possible
            _bll.Kiss(datingLocation).Should().BeTrue();
        }

        #endregion Kiss

        #region Setup

        private void SetupDating(int heroCharm, double randomValue)
        {
            SetHeroCharm(heroCharm);
            _me.ResetTtl();

            _randomProvider.NextDouble().Returns(randomValue);
        }

        private DatingLocation SetupForKiss(Girl g, int heroCharm, double randomValue)
        {
            SetHeroCharm(heroCharm);
            _me.ResetTtl();

            _randomProvider.NextDouble().Returns(randomValue);

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

        #endregion Setup
    }
}

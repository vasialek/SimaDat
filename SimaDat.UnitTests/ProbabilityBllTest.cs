using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using SimaDat.Models.Characters;
using SimaDat.Models;
using FluentAssertions;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class ProbabilityBllTest
	{
        private IProbabilityBll _bll = null;
        private Hero _me = null;
        private Girl _girl = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new ProbabilityBll();

            _me = new Hero();
            _me.CurrentLocationId = 100;
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, MySettings.MaxCharmForHero);

            _girl = new Girl("Friend girl", Models.Enums.FriendshipLevels.Friend);
            _girl.CurrentLocationId = _me.CurrentLocationId;
        }

        #region Request dating

        [TestMethod]
        public void RequestDating_True()
        {
            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeTrue();
        }

        [TestMethod]
        public void RequestDating_False()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, -MySettings.MaxCharmForHero);

            bool isDating = _bll.RequestDating(_me, _girl);

            isDating.Should().BeFalse();
        }

        [TestMethod]
        public void RequestDating_50pct()
        {
            int ok = 0;
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, MySettings.MaxCharmForHero / -2);

            for (int i = 0; i < 100; i++)
            {
                if (_bll.RequestDating(_me, _girl))
                {
                    ok++;
                }
            }

            // Expected more less 50% of positive
            ok.Should().BeLessThan(70);
            ok.Should().BeGreaterOrEqualTo(40);
        }

        #endregion
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Characters;
using FluentAssertions;
using SimaDat.Models;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class HeroTest
    {
        private Hero _me = null;

        [TestInitialize]
        public void TestInit()
        {
            _me = new Hero();
            _me.ResetTtl();
        }

        #region Check for min

        [TestMethod]
        public void ModifySkill_IqNotLessThanZerro()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Iq, -1000);

            _me.Iq.Should().BeGreaterOrEqualTo(0);
        }

        [TestMethod]
        public void ModifySkill_CharmNotLessThanZerro()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, -1000);

            _me.Charm.Should().BeGreaterOrEqualTo(0);
        }

        [TestMethod]
        public void ModifySkill_StrengthNotLessThanZerro()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Strength, -1000);

            _me.Strength.Should().BeGreaterOrEqualTo(0);
        }

        #endregion

        #region Check fro max

        [TestMethod]
        public void ModifySkill_IqShouldNotBeHigherThanMax()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Iq, 1000);

            _me.Iq.Should().BeLessOrEqualTo(MySettings.MaxIqForHero);
        }

        [TestMethod]
        public void ModifySkill_CharmShouldNotBeHigherThanMax()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, 1000);

            _me.Charm.Should().BeLessOrEqualTo(MySettings.MaxCharmForHero);
        }

        [TestMethod]
        public void ModifySkill_StrengthShouldNotBeHigherThanMax()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Strength, 1000);

            _me.Strength.Should().BeLessOrEqualTo(MySettings.MaxStrengthForHero);
        }

        #endregion

    }
}

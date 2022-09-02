using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models;
using SimaDat.Models.Characters;

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

        [TestMethod]
        public void ModifySkill_IqNotLessThanZero()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Iq, -1000);

            _me.Iq.Should().BeGreaterOrEqualTo(0);
        }

        [TestMethod]
        public void ModifySkill_CharmNotLessThanZero()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Charm, -1000);

            _me.Charm.Should().BeGreaterOrEqualTo(0);
        }

        [TestMethod]
        public void ModifySkill_StrengthNotLessThanZero()
        {
            _me.ModifySkill(Models.Enums.HeroSkills.Strength, -1000);

            _me.Strength.Should().BeGreaterOrEqualTo(0);
        }

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
    }
}

using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Characters;
using SimaDat.Models.Skills;
using SimaDat.Models.Exceptions;
using FluentAssertions;
using SimaDat.Models;

namespace SimaDat.UnitTests
{
    /// <summary>
    /// Summary description for HeroBllTest
    /// </summary>
    [TestClass]
    public class HeroBllTest
    {
        private Hero _hero = null;
        private IHeroBll _heroBll = Bll.BllFactory.Current.HeroBll;

        private SkillImprovement _improveIq = null;

        [TestInitialize]
        public void TestInit()
        {
            _hero = new Hero
            {
                Name = "Test hero"
            };

            _improveIq = new SkillImprovement { Skill = Models.Enums.HeroSkills.Iq, ImprovementPoints = 1, TtlToUse = 4 };
            //_heroBll = 
        }

        #region Improvement

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Improve_HeroShouldHaveTtl()
        {
            _heroBll.Improve(_hero, _improveIq);
        }

        [TestMethod]
        public void Improve_TtlIsReducedAfterImprovement()
        {
            // Restore TTL
            _heroBll.Sleep(_hero);

            _heroBll.Improve(_hero, _improveIq);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero - _improveIq.TtlToUse);
        }

        #endregion
        
        #region Sleep

        [TestMethod]
        public void Sleep_ShouldRestoreMaxTtl()
        {
            _heroBll.Sleep(_hero);

            _hero.Ttl.Should().Be(MySettings.MaxTtlForHero);
        }

        #endregion
    }
}

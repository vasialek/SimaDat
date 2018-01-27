using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using SimaDat.Models.Characters;
using FluentAssertions;
using System.Linq;
using SimaDat.Models.Exceptions;
using SimaDat.Models;
using SimaDat.Models.Enums;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class CharactersBllTest
    {
        private ICharactersBll _bll = null;
        private Girl _laura = null;
        private Hero _me = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new CharactersBll();

            _me = new Hero
            {
                Name = "Test hero",
                CurrentLocationId = 100,
                HeroId = 1000
            };
            _me.ResetTtl();

            _laura = new Girl
            {
                Name = "Laura",
                Appearance = new Appearance(165, 80, 60, 95) { Hair = Models.Enums.Hairs.Black },
                CurrentLocationId = 100
            };
        }

        #region Create girl

        [TestMethod]
        public void CreateGirl_Ok()
        {
            _bll.CreateGirl(_laura);
        }

        [TestMethod]
        public void FindInLocation_GetOneGirl()
        {
            _bll.CreateGirl(_laura);

            var girls = _bll.FindInLocation(_laura.CurrentLocationId);

            girls.Should().HaveCount(1);
        }

        #endregion

        #region Say hi

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void SayHi_Exception_WhenNoTtl()
        {
            _me.UseTtl(MySettings.MaxTtlForHero);

            // Expecting exception that no TTL
            _bll.SayHi(_me, _laura);
        }

        [TestMethod]
        public void SayHi_ShouldUseTtl()
        {
            int ttl = _me.Ttl;

            _bll.SayHi(_me, _laura);

            _me.Ttl.Should().BeLessThan(ttl);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void SayHi_Exception_WhenDifferentLocations()
        {
            // Ensure that hero is not together with girl
            _bll.CreateGirl(_laura);
            _me.CurrentLocationId = _laura.CurrentLocationId + 1;

            // Expecting exception girl is not here
            _bll.SayHi(_me, _laura);
        }

        [TestMethod]
        public void SayHi_HeroLikesIncreases()
        {
            _bll.CreateGirl(_laura);
            int heroLikes = _laura.HeroLikes;

            _bll.SayHi(_me, _laura);

            _laura.HeroLikes.Should().BeGreaterThan(heroLikes);
        }

        #endregion

        #region Talk with Girl

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void Talk_Exception_WhenDifferentLocations()
        {
            // Ensure that hero is not together with girl
            _bll.CreateGirl(_laura);
            _me.CurrentLocationId = _laura.CurrentLocationId + 1;

            // Expecting exception girl is not here
            _bll.Talk(_me, _laura);
        }

        [TestMethod]
        public void Talk_NoHeroLikes_WhenStranger()
        {
            int likes = _laura.HeroLikes;

            _bll.Talk(_me, _laura);

            _laura.HeroLikes.Should().Be(likes);
        }

        [TestMethod]
        public void Talk_HeroLikesIncrease_WhenSawHimSomewhere()
        {
            var girl = new Girl("Test", FriendshipLevels.SawHimSomewhere);
            girl.CurrentLocationId = _me.CurrentLocationId;
            int likes = girl.HeroLikes;

            _bll.Talk(_me, girl);

            girl.HeroLikes.Should().BeGreaterThan(likes);
        }

        #endregion

        #region Friendship levels

        [TestMethod]
        public void SayHi_StrangerLevelUp()
        {
            _me.CurrentLocationId = _laura.CurrentLocationId;
            var settings = MySettings.Get();

            // Say as many "Hi" to level up Stranger
            for (int i = 0; i < settings.GetLikesForFriendships(Models.Enums.FriendshipLevels.SawHimSomewhere); i++)
            {
                _bll.SayHi(_me, _laura);
            }

            _laura.FriendshipLevel.Should().Be(FriendshipLevels.SawHimSomewhere);
        }

        #endregion
    }
}

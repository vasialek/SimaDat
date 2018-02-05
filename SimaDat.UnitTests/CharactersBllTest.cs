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
using SimaDat.Models.Items;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class CharactersBllTest
    {
        private ICharactersBll _bll = null;
        private Girl _laura = null;
        private Hero _me = null;
        private Gift _gift = null;

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
            _me.Gifts.Clear();

            _laura = new Girl
            {
                Name = "Laura",
                Appearance = new Appearance(165, 80, 60, 95) { Hair = Models.Enums.Hairs.Black },
                CurrentLocationId = 100
            };

            _gift = new Gift { GiftId = 123, GiftTypeId = GiftTypes.Flower, Name = "Test flower", FirendshipPoints = 10, Price = 50 };
        }

        #region Create girl

        [TestMethod]
        public void CreateGirl_Ok()
        {
            _bll.CreateGirl(_laura);
        }

        [TestMethod]
        public void Girl_HeroLikesOnCreation()
        {
            var g = new Girl("Test girl", FriendshipLevels.Familar);

            // Expected hero likes are equal to friendhsip level
            g.HeroLikes.Should().Be(MySettings.Get().GetLikesForFriendships(FriendshipLevels.Familar));
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
        public void Talk_ShouldUseTtl()
        {
            int ttl = _me.Ttl;

            _bll.Talk(_me, _laura);

            _me.Ttl.Should().BeLessThan(ttl);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Talk_Exception_WhenNoTtl()
        {
            _me.UseTtl(MySettings.MaxTtlForHero);

            // Expecting exception that no TTL
            _bll.Talk(_me, _laura);
        }

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

        #region Gifts for girl

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void Present_Exception_WhenGirlIsNotHer()
        {
            // Hero has gift, but girl is not here
            _me.Gifts.Add(_gift);
            _laura.CurrentLocationId = _me.CurrentLocationId + 1;

            // Exception when girl is not here
            _bll.Present(_me, _laura, _gift.GiftTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesNotExistException))]
        public void Present_Exception_WhenNoGift()
        {
            // Exception when no gift
            _bll.Present(_me, _laura, GiftTypes.DiamondRing);
        }

        [TestMethod]
        [ExpectedException(typeof(FriendshipLeveTooLowException))]
        public void Present_Exception_WhenGirlIsNotFamilar()
        {
            _me.Gifts.Add(_gift);

            _bll.Present(_me, _laura, _gift.GiftTypeId);
        }

        [TestMethod]
        public void Present_ReachNewFriendshipLevel()
        {
            var girl = new Girl("Test", FriendshipLevels.Familar);
            girl.CurrentLocationId = _me.CurrentLocationId;
            int likesForFamilar = MySettings.Get().GetLikesForFriendships(FriendshipLevels.Familar);
            int likesForFriend = MySettings.Get().GetLikesForFriendships(FriendshipLevels.Friend);
            // Make gift to reach next friendship level
            _gift.FirendshipPoints = likesForFriend - likesForFamilar + 1;
            _me.Gifts.Add(_gift);

            _bll.Present(_me, girl, _gift.GiftTypeId);

            girl.FriendshipLevel.Should().Be(FriendshipLevels.Friend);
        }

        [TestMethod]
        public void Present_GiftDissapears_AfterPresent()
        {
            var girl = new Girl("Test", FriendshipLevels.Familar);
            girl.CurrentLocationId = _me.CurrentLocationId;
            _me.Gifts.Add(_gift);

            _bll.Present(_me, girl, _gift.GiftTypeId);

            // Gift is moved to girl
            _me.Gifts.Should().HaveCount(0);
        }

        [TestMethod]
        public void Present_ShouldUseTtl()
        {
            var girl = new Girl("Test", FriendshipLevels.Familar);
            girl.CurrentLocationId = _me.CurrentLocationId;
            _me.Gifts.Add(_gift);
            int v = _me.Ttl;

            _bll.Present(_me, girl, _gift.GiftTypeId);

            // TTL should be decreased
            _me.Ttl.Should().BeLessThan(v);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Present_Exception_WhenNoTtl()
        {
            var girl = new Girl("Test", FriendshipLevels.Familar);
            girl.CurrentLocationId = _me.CurrentLocationId;
            _me.Gifts.Add(_gift);
            _me.UseTtl(MySettings.MaxTtlForHero);

            _bll.Present(_me, girl, _gift.GiftTypeId);
        }

        #endregion
    }
}

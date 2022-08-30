using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Items;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class CharactersBllTest
    {
        private ICharactersBll _bll;
        private Girl _laura;
        private Girl _girlFamiliar;
        private Girl _girlFriend;
        private Hero _me;
        private Gift _gift;

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

            // Familar girl in the same location as Hero
            _girlFamiliar = new Girl("Familar girl", FriendshipLevels.Familar);
            _girlFamiliar.CurrentLocationId = _me.CurrentLocationId;

            // Friend girl in the same location as Hero
            _girlFriend = new Girl("Friend girl", FriendshipLevels.Friend);
            _girlFriend.CurrentLocationId = _me.CurrentLocationId;

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

        #endregion Create girl

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

        #endregion Say hi

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

        #endregion Talk with Girl

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

        #endregion Friendship levels

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
            int likesForFamilar = MySettings.Get().GetLikesForFriendships(FriendshipLevels.Familar);
            int likesForFriend = MySettings.Get().GetLikesForFriendships(FriendshipLevels.Friend);
            // Make gift to reach next friendship level
            _gift.FirendshipPoints = likesForFriend - likesForFamilar + 1;
            _me.Gifts.Add(_gift);

            _bll.Present(_me, _girlFamiliar, _gift.GiftTypeId);

            _girlFamiliar.FriendshipLevel.Should().Be(FriendshipLevels.Friend);
        }

        [TestMethod]
        public void Present_GiftDissapears_AfterPresent()
        {
            _me.Gifts.Add(_gift);

            _bll.Present(_me, _girlFamiliar, _gift.GiftTypeId);

            // Gift is moved to girl
            _me.Gifts.Should().HaveCount(0);
        }

        [TestMethod]
        public void Present_ShouldUseTtl()
        {
            _me.Gifts.Add(_gift);
            int v = _me.Ttl;

            _bll.Present(_me, _girlFamiliar, _gift.GiftTypeId);

            // TTL should be decreased
            _me.Ttl.Should().BeLessThan(v);
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void Present_Exception_WhenNoTtl()
        {
            _me.Gifts.Add(_gift);
            _me.UseTtl(MySettings.MaxTtlForHero);

            _bll.Present(_me, _girlFamiliar, _gift.GiftTypeId);
        }

        #endregion Gifts for girl

        #region Ask dating

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void AskDating_Exception_WhenNoTtl()
        {
            _me.UseTtl(MySettings.MaxTtlForHero);

            _bll.AskDating(_me, _girlFriend);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotHereException))]
        public void AskDating_Exception_WhenNoGirl()
        {
            // She is not here
            _girlFriend.CurrentLocationId = _me.CurrentLocationId + 1;

            _bll.AskDating(_me, _girlFriend);
        }

        [TestMethod]
        [ExpectedException(typeof(FriendshipLeveTooLowException))]
        public void AskDating_Exception_WhenGirlIsNotFriend()
        {
            _bll.AskDating(_me, _girlFamiliar);
        }

        [TestMethod]
        public void AskDating_ShouldUseTtl()
        {
            int v = _me.Ttl;

            _bll.AskDating(_me, _girlFriend);

            // Should spent some TTL
            _me.Ttl.Should().BeLessThan(v);
        }

        #endregion Ask dating
    }
}

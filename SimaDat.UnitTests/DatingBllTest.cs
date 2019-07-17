using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models;
using SimaDat.Models.Enums;
using FluentAssertions;
using SimaDat.Models.Items;
using System.Linq;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class DatingBllTest
    {
        private IDatingBll _bll = null;
        private Hero _me = null;
        private Girl _laura = null;
        private DatingLocation _location = null;
        private Gift _giftFlower = null;
        private Gift _giftDiamondRing = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new DatingBll();

            _location = new DatingLocation("Test dating", 100);

            _me = new Hero()
            {
                CurrentLocationId = 100
            };
            _me.SpendMoney(-_location.Price);
            _me.ResetTtl();

            _laura = new Girl("Laura", Models.Enums.FriendshipLevels.Friend);
            _laura.CurrentLocationId = _me.CurrentLocationId;

            _location = new DatingLocation("Test dating", 100);

            _giftFlower = new Gift { GiftId = 123, GiftTypeId = GiftTypes.Flower, Name = "Test flower", FirendshipPoints = 10, Price = 50 };
            _giftDiamondRing = new Gift { GiftId = 124, GiftTypeId = GiftTypes.DiamondRing, Name = "Test diamond ring", FirendshipPoints = 50, Price = 900 };
        }

        #region Join dating

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void JoinDating_Exception_WhenNoTtl()
        {
            _me.UseTtl(MySettings.MaxTtlForHero);

            _bll.JoinDating(_me, _laura, _location);
        }

        [TestMethod]
        [ExpectedException(typeof(NoMoneyException))]
        public void JoinDating_Exception_WhenNoMoney()
        {
            _me.SpendMoney(10000);

            _bll.JoinDating(_me, _laura, _location);
        }

        [TestMethod]
        [ExpectedException(typeof(FriendshipLeveTooLowException))]
        public void JoinDating_Exception_WhenGirlIsLessThanFriend()
        {
            var g = new Girl("NotFriendly", Models.Enums.FriendshipLevels.Familar);

            _bll.JoinDating(_me, g, _location);
        }

        [TestMethod]
        public void JoinDating_MoneyDecrease()
        {
            _me.SpendMoney(-_location.Price);
            int v = _me.Money;

            _bll.JoinDating(_me, _laura, _location);

            // Expecting money are spent
            _me.Money.Should().BeLessThan(v);
        }

        [TestMethod]
        public void JoinDating_TtlShouldDecrease()
        {
            _me.SpendMoney(-_location.Price);
            int v = _me.Ttl;

            _bll.JoinDating(_me, _laura, _location);

            // TTL should be spent
            _me.Ttl.Should().BeLessThan(v);
        }

        #endregion

        #region Dating location state

        [TestMethod]
        public void JoinDating_LocationShouldBeCreated()
        {
            PrepareDatingLocation();

            _bll.Location.Should().NotBeNull();
        }

        [TestMethod]
        public void JoinDating_HeroShouldBe()
        {
            PrepareDatingLocation();

            _bll.Location.Hero.Should().Be(_me);
        }

        [TestMethod]
        public void JoinDating_GirlShouldBe()
        {
            PrepareDatingLocation();

            _bll.Location.Girl.Should().Be(_laura);
        }

        #endregion

        #region Present

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesNotExistException))]
        public void Present_Exception_WhenNoGift()
        {
            // Join dating with Laura
            PrepareDatingLocation();

            _bll.Present(GiftTypes.Flower);
        }

        [TestMethod]
        public void Present_GiftDissapears_AfterPresent()
        {
            // Join dating with Laura
            PrepareDatingLocation(_giftFlower);

            _bll.Present(_giftFlower.GiftTypeId);

            // Gift is moved to girl
            _me.Gifts.Should().HaveCount(0);
        }

        [TestMethod]
        public void Present_KissPointsShouldIncrease()
        {
            // Join dating with Laura
            PrepareDatingLocation(_giftFlower);
            int v = _bll.Location.KissPoints;

            _bll.Present(_giftFlower.GiftTypeId);

            // Girl should be pleased :)
            _bll.Location.KissPoints.Should().BeGreaterThan(v);
        }

        #endregion

        #region Kiss

        [TestMethod]
        [ExpectedException(typeof(BadConditionException))]
        public void Kiss_Exception_WhenGirlNotReady()
        {
            PrepareDatingLocation();

            _bll.Kiss();
        }

        [TestMethod]
        public void Kiss_KissPointDecrease_WhenGirlWasNotReady()
        {
            PrepareDatingLocation(_giftFlower);
            // Need to increase kiss points
            _bll.Present(_giftFlower.GiftTypeId);
            int v = _bll.Location.KissPoints;

            try
            {
                _bll.Kiss();
            }
            catch (BadConditionException bcex)
            {
                // Expecting this exception
            }

            // Girl should be little dissapointed when kiss to early
            _bll.Location.KissPoints.Should().BeLessThan(v);
        }

        [TestMethod]
        public void Kiss_KissPointDecreaseTillZero_WhenGirlWasNotReady()
        {
            PrepareDatingLocation();

            try
            {
                _bll.Kiss();
            }
            catch (BadConditionException bcex)
            {
                // Expecting this exception
            }

            // Do not decrease below 0
            _bll.Location.KissPoints.Should().Be(0);
        }

        [TestMethod]
        public void Kiss_Success()
        {
            PrepareDatingLocation();
            // Should reach kiss level
            for (int i = 0; i < 100; i++)
            {
                _me.Gifts.Add(_giftFlower);
                _bll.Present(_giftFlower.GiftTypeId);
            }

            _bll.Kiss();

            // Expected lover aftef kiss
            _laura.FriendshipLevel.Should().Be(FriendshipLevels.Lover);
        }

        [TestMethod]
        public void Kiss_CheckWasKiss()
        {
            PrepareDatingLocation();
            MakeReadyForKiss();

            _bll.Kiss();

            _bll.Location.WasKiss.Should().BeTrue();
        }

        #endregion

        #region No actions after kiss allowed

        [TestMethod]
        [ExpectedException(typeof(EventIsOverException))]
        public void Present_Fail_AfterKiss()
        {
            PrepareDatingLocation(_giftFlower);
            MakeReadyForKiss();
            _bll.Kiss();

            // This shouls fail, because dating is over after kiss
            _bll.Present(_giftFlower.GiftTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(EventIsOverException))]
        public void Kiss_Fail_AfterKiss()
        {
            PrepareDatingLocation(_giftFlower);
            MakeReadyForKiss();
            _bll.Kiss();

            // This shouls fail, because dating is over after kiss
            _bll.Kiss();
        }

        #endregion

        private void PrepareDatingLocation(Gift giftToAdd = null)
        {
            _me.SpendMoney(-_location.Price);
            _bll.JoinDating(_me, _laura, _location);
            if (giftToAdd != null)
            {
                _me.Gifts.Add(_giftFlower);
            }
        }

        private void MakeReadyForKiss()
        {
            // Should reach kiss level
            for (int i = 0; i < 100; i++)
            {
                _me.Gifts.Add(_giftDiamondRing);
                _bll.Present(_giftDiamondRing.GiftTypeId);
            }
        }

        [TestMethod]
        public void JoinDating_MoneyShouldBeSpent()
        {
            int v = _me.Money;

            _bll.JoinDating(_me, _laura, _location);
            //
            _me.Money.Should().Be(v - _location.Price);
        }

        #region Actions

        [TestMethod]
        public void GetHeroActions_NotNull()
        {
            var actions = _bll.GetHeroActions(_location);

            actions.Count().Should().BeGreaterThan(0);
        }

		#endregion

		#region Present

		[TestMethod]
		public void Present_TtlShouldNotDecrease()
		{
			_bll.JoinDating(_me, _laura, _location);
			int expected = _me.Ttl;

			_bll.Present(_location, Models.Enums.GiftTypes.Flower);

			// No TTL is used during dating
			_me.Ttl.Should().Be(expected);
		}

		[TestMethod]
		public void Present_KissPointsShouldIncrease_AfterFlowerGift()
		{
			_bll.JoinDating(_me, _laura, _location);
			int expected = _location.KissPoints;

			_bll.Present(_location, Models.Enums.GiftTypes.Flower);

			// Expecting Kiss points increases
			_location.KissPoints.Should().BeGreaterThan(expected);
		}

		#endregion
	}
}

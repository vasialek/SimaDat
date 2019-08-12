using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Bll;
using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
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

            _laura = new Girl("Laura", FriendshipLevels.Friend);
            _laura.CurrentLocationId = _me.CurrentLocationId;

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
            var g = new Girl("NotFriendly", FriendshipLevels.Familar);

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

		[TestMethod]
		public void JoinDating_MoneyShouldBeSpent()
		{
			int v = _me.Money;

			_bll.JoinDating(_me, _laura, _location);
			//
			_me.Money.Should().Be(v - _location.Price);
		}

		[TestMethod]
		public void JoinDating_KissPointsIsZero()
		{
			_location.KissPoints = 3;
			_bll.JoinDating(_me, _laura, _location);

			_location.KissPoints.Should().Be(0);
		}

		#endregion

		#region Dating location state

		[TestMethod]
        public void JoinDating_LocationShouldBeCreated()
        {
            var datingLocation = PrepareDatingLocation();

			datingLocation.Should().NotBeNull();
        }

        [TestMethod]
        public void JoinDating_HeroShouldBe()
        {
			var datingLocation = PrepareDatingLocation();

			datingLocation.Hero.Should().Be(_me);
        }

        [TestMethod]
        public void JoinDating_GirlShouldBe()
        {
            var datingLocation = PrepareDatingLocation();

			datingLocation.Girl.Should().Be(_laura);
        }

        #endregion

        #region Present

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesNotExistException))]
        public void Present_Exception_WhenNoGift()
        {
            // Join dating with Laura
            var datingLocation = PrepareDatingLocation();

            _bll.Present(datingLocation, GiftTypes.Flower);
        }

        [TestMethod]
        public void Present_GiftDissapears_AfterPresent()
        {
			// Join dating with Laura
			var datingLocation = PrepareDatingLocation(_giftFlower);

            _bll.Present(datingLocation, _giftFlower.GiftTypeId);

            // Gift is moved to girl
            _me.Gifts.Should().HaveCount(0);
        }

        [TestMethod]
        public void Present_KissPointsShouldIncrease()
        {
			// Join dating with Laura
			var datingLocation = PrepareDatingLocation(_giftFlower);
            int v = datingLocation.KissPoints;

            _bll.Present(datingLocation, _giftFlower.GiftTypeId);

			// Girl should be pleased :)
			datingLocation.KissPoints.Should().BeGreaterThan(v);
        }

		[TestMethod]
		public void Present_TtlShouldNotDecrease()
		{
			var datingLocation = PrepareDatingLocation(_giftFlower);
			int expected = _me.Ttl;

			_bll.Present(datingLocation, _giftFlower.GiftTypeId);

			// No TTL is used during dating
			_me.Ttl.Should().Be(expected);
		}

		[TestMethod]
		public void Present_KissPointsShouldIncrease_AfterFlowerGift()
		{
			var datingLocation = PrepareDatingLocation(_giftFlower);
			int expected = datingLocation.KissPoints;

			_bll.Present(datingLocation, GiftTypes.Flower);

			// Expecting Kiss points increases
			datingLocation.KissPoints.Should().BeGreaterThan(expected);
		}

		#endregion

		#region Kiss

		[TestMethod]
        [ExpectedException(typeof(BadConditionException))]
        public void Kiss_Exception_WhenGirlNotReady()
        {
			var datingLocation = PrepareDatingLocation();

            _bll.Kiss(datingLocation);
        }

        [TestMethod]
        public void Kiss_KissPointDecrease_WhenGirlWasNotReady()
        {
			var datingLocation = PrepareDatingLocation(_giftFlower);
            // Need to increase kiss points
            _bll.Present(datingLocation, _giftFlower.GiftTypeId);
            int v = datingLocation.KissPoints;

            try
            {
                _bll.Kiss(datingLocation);
            }
            catch (BadConditionException bcex)
            {
                // Expecting this exception
            }

			// Girl should be little dissapointed when kiss to early
			datingLocation.KissPoints.Should().BeLessThan(v);
        }

        [TestMethod]
        public void Kiss_KissPointDecreaseTillZero_WhenGirlWasNotReady()
        {
			var datingLocation = PrepareDatingLocation();

            try
            {
                _bll.Kiss(datingLocation);
            }
            catch (BadConditionException bcex)
            {
                // Expecting this exception
            }

			// Do not decrease below 0
			datingLocation.KissPoints.Should().Be(0);
        }

        [TestMethod]
        public void Kiss_Success()
        {
			var datingLocation = PrepareDatingLocation();
            // Should reach kiss level
            for (int i = 0; i < 100; i++)
            {
                _me.Gifts.Add(_giftFlower);
                _bll.Present(datingLocation, _giftFlower.GiftTypeId);
            }

            _bll.Kiss(datingLocation);

			// Expected lover aftef kiss
			datingLocation.Girl.FriendshipLevel.Should().Be(FriendshipLevels.Lover);
        }

        [TestMethod]
        public void Kiss_CheckWasKiss()
        {
			var datingLocation = PrepareDatingLocation();
			MakeReadyForKiss(datingLocation);

			_bll.Kiss(datingLocation);

			datingLocation.WasKiss.Should().BeTrue();
        }

        #endregion

        #region No actions after kiss allowed

        [TestMethod]
        [ExpectedException(typeof(EventIsOverException))]
        public void Present_Fail_AfterKiss()
        {
			var datingLocation = PrepareDatingLocation(_giftFlower);
            MakeReadyForKiss(datingLocation);
            _bll.Kiss(datingLocation);

            // This shouls fail, because dating is over after kiss
            _bll.Present(datingLocation, _giftFlower.GiftTypeId);
        }

        [TestMethod]
        [ExpectedException(typeof(EventIsOverException))]
        public void Kiss_Fail_AfterKiss()
        {
			var datingLocation = PrepareDatingLocation(_giftFlower);
			MakeReadyForKiss(datingLocation);
			_bll.Kiss(datingLocation);

            // This shouls fail, because dating is over after kiss
            _bll.Kiss(datingLocation);
        }

		#endregion

		#region Increase kiss points

		[TestMethod]
		public void IncreaseKissPoints_ShouldBeIncreased_WhenGiftPresented()
		{
			var datingLocation = PrepareDatingLocation(_giftFlower);
			int kp = datingLocation.KissPoints;

			_bll.IncreaseKissPoints(datingLocation, 1);

			// Expecting flower makes girl more happy
			datingLocation.KissPoints.Should().BeGreaterThan(kp);
		}

		[TestMethod]
		public void IncreaseKissPoints_NotIncreased_WhenMaxIsReached()
		{
			var datingLocation = PrepareDatingLocation();

			_bll.IncreaseKissPoints(datingLocation, MySettings.MaxKissPoints + 1);

			// Should not exceed max
			datingLocation.KissPoints.Should().Be(MySettings.MaxKissPoints);
		}

		[TestMethod]
		public void IncreaseKissPoints_NotLessThanZero()
		{
			var datingLocation = PrepareDatingLocation();

			_bll.IncreaseKissPoints(datingLocation, -1);

			// Should be 0, not less
			datingLocation.KissPoints.Should().Be(0);
		}

		#endregion

		private DatingLocation PrepareDatingLocation(Gift giftToAdd = null)
        {
            _me.SpendMoney(-_location.Price);
            _bll.JoinDating(_me, _laura, _location);
            if (giftToAdd != null)
            {
                _me.Gifts.Add(_giftFlower);
            }

			return _location;
        }

        private void MakeReadyForKiss(DatingLocation datingLocation)
        {
            // Should reach kiss level
            for (int i = 0; i < 100; i++)
            {
                _me.Gifts.Add(_giftDiamondRing);
				_bll.Present(datingLocation, _giftDiamondRing.GiftTypeId);
			}
        }

        #region Actions

        [TestMethod]
        public void GetHeroActions_NotNull()
        {
            var actions = _bll.GetHeroActions(_location);

            actions.Count().Should().BeGreaterThan(0);
        }

		#endregion

	}
}

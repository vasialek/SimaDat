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

namespace SimaDat.UnitTests
{
    [TestClass]
    public class DatingBllTest
    {
        private IDatingBll _bll = null;
        private Hero _me = null;
        private Girl _laura = null;
        private DatingLocation _location = null;

        [TestInitialize]
        public void TestInit()
        {
            _bll = new DatingBll();

            _me = new Hero()
            {
                CurrentLocationId = 100
            };
            _me.ResetTtl();

            _laura = new Girl("Laura", Models.Enums.FriendshipLevels.Friend);
            _laura.CurrentLocationId = _me.CurrentLocationId;

            _location = new DatingLocation("Test dating", 100);
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
            _me.SpendMoney(-_location.Price);

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
            PrepareDatingLocation();
            var _gift = new Gift { GiftId = 123, GiftTypeId = GiftTypes.Flower, Name = "Test flower", FirendshipPoints = 10, Price = 50 };
            _me.Gifts.Add(_gift);

            _bll.Present(_gift.GiftTypeId);

            // Gift is moved to girl
            _me.Gifts.Should().HaveCount(0);
        }

        #endregion

        private void PrepareDatingLocation()
        {
            _me.SpendMoney(-_location.Price);
            _bll.JoinDating(_me, _laura, _location);
        }
    }
}

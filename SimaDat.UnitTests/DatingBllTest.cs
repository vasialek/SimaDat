using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models;
using FluentAssertions;
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
        }

        [TestMethod]
        [ExpectedException(typeof(NoTtlException))]
        public void JoinDating_Exception_WhenNoTtl()
        {
            _me.UseTtl(MySettings.MaxTtlForHero);

            _bll.JoinDating(_me, _laura, _location);
        }

        [TestMethod]
        public void JoinDating_TtlShouldDecrease()
        {
            int v = _me.Ttl;

            _bll.JoinDating(_me, _laura, _location);

            _me.Ttl.Should().BeLessThan(v);
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
    }
}

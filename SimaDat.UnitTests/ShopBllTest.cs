using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimaDat.Models.Characters;
using SimaDat.Models.Interfaces;
using SimaDat.Bll;
using SimaDat.Models.Exceptions;
using System.Linq;
using SimaDat.Models.Items;
using System.Collections.Generic;
using FluentAssertions;

namespace SimaDat.UnitTests
{
    [TestClass]
    public class ShopBllTest
    {
        private Hero _me = null;
        private IShopBll _bll = null;

        [TestInitialize]
        public void TestInit()
        {
            _me = new Hero();
            _me.ResetTtl();

            _bll = new ShopBll();
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectDoesNotExistException))]
        public void BuyGift_Exception_WhenBadGiftId()
        {
            _bll.BuyGift(_me, 666);
        }

        [TestMethod]
        [ExpectedException(typeof(NoMoneyException))]
        public void BuyGift_Exception_WhenNoMoney()
        {
            _me.SpendMoney(_me.Money);
            var gift = _bll.GetListOfGifts().First();

            _bll.BuyGift(_me, gift.GiftId);
        }

        [TestMethod]
        public void BuyGift_Ok()
        {
            // Ensure that no gifts and enough money to buy
            var g = _bll.GetListOfGifts().First();
            _me.Gifts = new List<Gift>();
            _me.SpendMoney(-g.Price);

            _bll.BuyGift(_me, g.GiftId);

            // Should buy this gift
            _me.Gifts.Single().GiftId.Should().Be(g.GiftId);
        }

        [TestMethod]
        public void BuyGift_SpendMoney()
        {
            // Ensure that no gifts and enough money to buy
            var g = _bll.GetListOfGifts().First();
            _me.Gifts = new List<Gift>();
            _me.SpendMoney(-g.Price);
            int v = _me.Money;

            _bll.BuyGift(_me, g.GiftId);

            // Money should be spent
            _me.Money.Should().Be(v - g.Price);
        }
    }
}

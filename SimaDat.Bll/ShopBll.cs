using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Items;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Enums;

namespace SimaDat.Bll
{
    public class ShopBll : IShopBll
    {
        public void BuyGift(Hero h, int giftId)
        {
            var g = GetGiftById(giftId);

            if (g.Price > h.Money)
            {
                throw new NoMoneyException($"Hero does not have enough money to buy `{g.Name}` for price of {g.Price}");
            }

            h.SpendMoney(g.Price);
            h.Gifts.Add(g);
        }

        public Gift GetGiftById(int giftId)
        {
            var g = GetListOfGifts().FirstOrDefault(x => x.GiftId == giftId);
            if (g == null)
            {
                throw new ObjectDoesNotExistException("No such gift found", giftId);
            }
            return g;
        }

        public IEnumerable<Gift> GetListOfGifts()
        {
            return new Gift[]
            {
                new Gift { GiftId = 1, GiftTypeId = GiftTypes.Flower, Name = "Flower", Price = 10, FirendshipPoints = 3 },
                new Gift { GiftId = 2, GiftTypeId = GiftTypes.TeddyBear, Name = "Teddy bear", Price = 40, FirendshipPoints = 15 },
                new Gift { GiftId = 3, GiftTypeId = GiftTypes.DiamondRing, Name = "Diamond ring", Price = 1000, FirendshipPoints = 350 }
            };
        }
    }
}

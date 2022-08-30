using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Models.Items;
using System.Collections.Generic;
using System.Linq;

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
            return g ?? throw new ObjectDoesNotExistException("No such gift found", giftId);
        }

        public IEnumerable<Gift> GetListOfGifts()
        {
            return new[]
            {
                new Gift { GiftId = 1, GiftTypeId = GiftTypes.Flower, Name = "Flower", Price = 10, FirendshipPoints = 3 },
                new Gift { GiftId = 2, GiftTypeId = GiftTypes.TeddyBear, Name = "Teddy bear", Price = 40, FirendshipPoints = 15 },
                new Gift { GiftId = 3, GiftTypeId = GiftTypes.DiamondRing, Name = "Diamond ring", Price = 1000, FirendshipPoints = 350 }
            };
        }
    }
}

using SimaDat.Models.Characters;
using SimaDat.Models.Items;
using System.Collections.Generic;

namespace SimaDat.Models.Interfaces
{
    public interface IShopBll
    {
        Gift GetGiftById(int giftId);

        IEnumerable<Gift> GetListOfGifts();

        void BuyGift(Hero h, int giftId);
    }
}

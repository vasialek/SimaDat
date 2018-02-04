using SimaDat.Models.Characters;
using SimaDat.Models.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Interfaces
{
    public interface IShopBll
    {
        Gift GetGiftById(int giftId);

        IEnumerable<Gift> GetListOfGifts();

        void BuyGift(Hero h, int giftId);
    }
}

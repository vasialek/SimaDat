using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Interfaces
{
    public interface ICharactersBll
    {
        void CreateGirl(Girl g);

        // Get
        Girl GetById(int characterId);

        IList<Girl> GetAll();

        // Search
        IList<Girl> FindInLocation(int locationId);

        // Actions
        void SayHi(Hero hero, Girl girl);

        void Talk(Hero hero, Girl girl);

        void Present(Hero h, Girl g, GiftTypes giftTypeId);

        bool AskDating(Hero h, Girl g);
    }
}

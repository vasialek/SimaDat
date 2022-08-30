using SimaDat.Models.Characters;
using SimaDat.Models.Enums;
using System.Collections.Generic;

namespace SimaDat.Models.Interfaces
{
    public interface ICharactersBll
    {
        void CreateGirl(Girl girl);

        Girl GetById(int characterId);

        IList<Girl> GetAll();

        IList<Girl> FindInLocation(int locationId);

        void SayHi(Hero hero, Girl girl);

        void Talk(Hero hero, Girl girl);

        void Present(Hero hero, Girl girl, GiftTypes giftTypeId);

        bool AskDating(Hero hero, Girl girl);
    }
}

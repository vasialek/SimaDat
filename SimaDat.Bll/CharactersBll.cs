using SimaDat.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimaDat.Models.Characters;
using SimaDat.Models.Exceptions;

namespace SimaDat.Bll
{
    public class CharactersBll : ICharactersBll
    {
        private IList<Girl> _girls = new List<Girl>();

        public void CreateGirl(Girl g)
        {
            if (g.CharacterId < 1)
            {
                g.CharacterId = _girls?.Count > 0 ? _girls.Max(x => x.CharacterId) + 1 : 1;
            }
            _girls.Add(g);
        }

        public void SayHi(Hero hero, Girl girl)
        {
            if (hero.CurrentLocationId != girl.CurrentLocationId)
            {
                throw new ObjectNotHereException($"No girl named {girl.Name} in location #{hero.CurrentLocationId}");
            }
            girl.LikeHero();
        }

        public IList<Girl> GetAll()
        {
            return _girls;
        }

        public IList<Girl> FindInLocation(int locationId)
        {
            return _girls.Where(x => x.CurrentLocationId == locationId).ToList();
        }
    }
}

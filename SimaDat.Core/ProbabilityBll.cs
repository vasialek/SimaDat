using SimaDat.Models;
using SimaDat.Models.Characters;
using SimaDat.Models.Datings;
using SimaDat.Models.Exceptions;
using SimaDat.Models.Interfaces;
using SimaDat.Shared;

namespace SimaDat.Core
{
    public class ProbabilityBll : IProbabilityBll
    {
        private readonly IRandomProvider _randomProvider;

        public ProbabilityBll(IRandomProvider randomProvider)
        {
            _randomProvider = randomProvider;
        }

        public bool Kiss(DatingLocation datingLocation)
        {
            if (datingLocation.IsOver)
            {
                throw new ObjectDoesNotExistException($"Dating with {datingLocation.Girl.Name} in {datingLocation.Name} is over", datingLocation.DatingLocationId);
            }

            // Max probability is 0.98
            float probability = ProbabilityCalculator.ProbabilityToKiss(datingLocation.Hero.Charm, datingLocation.Girl.FriendshipLevel);

            return _randomProvider.NextDouble() <= probability;
        }

        public bool RequestDating(Hero h, Girl g)
        {
            // Leave 1% for negative on max charm
            float probability = h.Charm * 0.99f / MySettings.MaxCharmForHero;

            return _randomProvider.NextDouble() <= probability;
        }
    }
}

using SimaDat.Models.Characters;
using SimaDat.Models.Datings;

namespace SimaDat.Models.Interfaces
{
    public interface IProbabilityBll
    {
        bool RequestDating(Hero hero, Girl girl);

        bool Kiss(DatingLocation datingLocation);
    }
}

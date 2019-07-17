using SimaDat.Models.Characters;
using SimaDat.Models.Datings;

namespace SimaDat.Models.Interfaces
{
	public interface IProbabilityBll
    {
        bool RequestDating(Hero h, Girl g);

		bool Kiss(DatingLocation datingLocation);
	}
}

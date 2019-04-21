using SimaDat.Models.Characters;

namespace SimaDat.Models.Interfaces
{
	public interface IProbabilityBll
	{
        bool RequestDating(Hero h, Girl g);
    }
}

using SimaDat.Models.Characters;

namespace SimaDat.UnitTests.FakeClasses
{
    public class HeroProxy : Hero
    {
        public void SetTtl(int ttl)
        {
            Ttl = ttl;
        }
    }
}

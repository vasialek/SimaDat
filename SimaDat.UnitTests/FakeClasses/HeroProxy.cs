using SimaDat.Models.Characters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.UnitTests.FakeClasses
{
    /// <summary>
    /// Lets change protected values of Hero, such as TTL
    /// </summary>
    public class HeroProxy : Hero
    {
        public void SetTtl(int ttl)
        {
            this.Ttl = ttl;
        }
    }
}

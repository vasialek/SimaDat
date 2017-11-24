using SimaDat.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimaDat.Models.Skills
{
    public class SkillImprovement
    {
        public HeroSkills Skill { get; set; }

        public int TtlToUse { get; set; }

        public int ImprovementPoints { get; set; }
    }
}

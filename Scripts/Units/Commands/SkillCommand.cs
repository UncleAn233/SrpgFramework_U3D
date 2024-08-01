using SrpgFramework.Units.Skills;
using SrpgFramework.Units.Units;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units.Commands
{
    public class SkillCommand : Command
    {
        public ISkill skill;
        public virtual IEnumerator Act(Unit unit) { yield return skill.Act(unit); }
    }
}
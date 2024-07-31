using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units.Skills
{
    public class SkillManager : MonoBehaviour
    {
        private Dictionary<string, Skill> abilities { get; set; }

        private void Awake()
        {
            abilities = new();
        }

        public Skill GetSkill(string id)
        {
            if (abilities.TryGetValue(id, out Skill ability))
            {
                return ability;
            }
            else
                return null;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units.Abilities
{
    public class AbilityManager : MonoBehaviour
    {
        private Dictionary<string, Ability> abilities { get; set; }

        private void Awake()
        {
            abilities = new();
        }

        public Ability GetAbility(string id)
        {
            if (abilities.TryGetValue(id, out Ability ability))
            {
                return ability;
            }
            else
                return null;
        }
    }
}
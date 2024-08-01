using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units.Commands
{
    public class CommandManager : MonoBehaviour
    {
        private Dictionary<string, Command> abilities { get; set; }

        private void Awake()
        {
            abilities = new();
        }

        public Command GetCommand(string id)
        {
            if (abilities.TryGetValue(id, out Command ability))
            {
                return ability;
            }
            else
                return null;
        }
    }
}
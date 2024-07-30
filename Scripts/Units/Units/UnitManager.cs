using SrpgFramework.Players;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Units.Units
{
    public class UnitManager : MonoBehaviour
    {
        public HashSet<Unit> Units { get; private set; }

        public void Awake()
        {
            Units = new();
        }

        public HashSet<Unit> GetEnemyUnits(Player player)
        {
            return Units.Where(u => u.Player.IsEnemy(player)).ToHashSet();
        }

        public void NewUnit()
        {

        }
    }
}
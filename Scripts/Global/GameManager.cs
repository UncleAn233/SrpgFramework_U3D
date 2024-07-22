using SrpgFramework.Abilities;
using SrpgFramework.CellGrid;
using SrpgFramework.Players;
using SrpgFramework.Units;
using UnityEngine;

namespace SrpgFramework.Global
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager instance;
        public static GameManager Instance { get => instance; }

        public static LevelManager LevelMgr { get; private set; }
        public static CellGridManager CellGridMgr { get; private set; }
        public static UnitManager UnitMgr { get; private set; }
        public static AbilityManager AbilityMgr { get; private set; }
        public static PlayerManager PlayerMgr { get; private set; }
        
        private void Awake()
        {
            if (instance is not null)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            DontDestroyOnLoad(gameObject);

            LevelMgr = gameObject.AddComponent<LevelManager>();
            CellGridMgr = gameObject.AddComponent<CellGridManager>();
            UnitMgr = gameObject.AddComponent<UnitManager>();
            AbilityMgr = gameObject.AddComponent<AbilityManager>();
            PlayerMgr = gameObject.AddComponent<PlayerManager>();
        }
    }
}
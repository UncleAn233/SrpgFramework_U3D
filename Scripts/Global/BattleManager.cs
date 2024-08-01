using SrpgFramework.Units.Commands;
using SrpgFramework.CellGrid;
using SrpgFramework.Level;
using SrpgFramework.Players;
using SrpgFramework.Units.Units;
using UnityEngine;

namespace SrpgFramework.Global
{
    public class BattleManager : MonoBehaviour
    {
        private static BattleManager instance;
        public static BattleManager Instance { get => instance; }

        public static LevelManager LevelMgr { get; private set; }
        public static CellGridManager CellGridMgr { get; private set; }
        public static UnitManager UnitMgr { get; private set; }
        public static CommandManager CommandMgr { get; private set; }
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
            CommandMgr = gameObject.AddComponent<CommandManager>();
            PlayerMgr = gameObject.AddComponent<PlayerManager>();
        }
    }
}
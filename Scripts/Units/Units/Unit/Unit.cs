using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Players;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace SrpgFramework.Units.Units
{
    public partial class Unit : MonoBehaviour
    {
        /// <summary>
        /// 单位ID 决定初始化时会加载哪个单位数据
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 单位标签 用于附加特殊信息
        /// </summary>
        public HashSet<string> Tags;
        /// <summary>
        /// 单位类型 角色 障碍物等
        /// </summary>
        public UnitType UnitType { get; private set; }
        /// <summary>
        /// 所属Player编号
        /// </summary>
        public int PlayerNumber;
        public Player Player => BattleManager.PlayerMgr.GetPlayer(PlayerNumber);


        [SerializeField]
        [HideInInspector]
        private Cell cell;
        public Cell Cell
        {
            get { return cell; }
            set
            {
                if (cell is not null)
                {
                    cell.Unit = null;
                }
                cell = value;
                cell.Unit = this;
            }
        }

        public Dictionary<string, int> Points { get; private set; }
        
        public MoveUnit Move { get; internal set; }
        public AiUnit Ai { get; internal set; }

        private void Awake()
        {
            Points = new();

            if(UnitType == UnitType.PC || UnitType == UnitType.NPC)
            {
                Move = this.AddComponent<MoveUnit>();
                Ai = this.AddComponent<AiUnit>();
            }
        }

        private void Start()
        {
            Cell.Unit = this;
            Points.Add("Action", 1);
            Points.Add("TotalAction", 1);
        }

        public void SetPos(Cell cell)
        {
            if(cell is not null)
            {
                Cell = cell;
                this.transform.localPosition = cell.transform.localPosition;
            }
        }

        public void Die()
        {
            OnDie?.Invoke();
        }
    }

    public enum UnitType
    {
        PC, NPC, NC
    }
}
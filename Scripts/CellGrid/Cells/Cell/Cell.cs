using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.CellGrid.Cells
{
    /// <summary>
    /// 战棋格子的基本类 默认为正方形
    /// </summary>
    public partial class Cell : MonoBehaviour
    {
        public string Id;
        [SerializeField]
        private Vector2Int _coord;
        /// <summary>
        /// 坐标
        /// </summary>
        public Vector2Int Coord { get { return _coord; } set { _coord = value; } }

        /// <summary>
        /// 地面类型，主要用于单位移动时判定该格子是否可通行
        /// </summary>
        public GroundType GroundType { get; set; } = GroundType.Ground;

        /// <summary>
        /// 该格子上的单位
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// 该格子是否有Unit
        /// </summary>
        public bool IsTaken => Unit is not null;

        /// <summary>
        /// 格子事件，用于处理例如陷阱等情况
        /// </summary>
        private ICellTrigger _cellTrigger;
        public ICellTrigger CellTriger
        {
            get { return _cellTrigger; }
            set
            {
                _cellTrigger?.Unregister(this);
                _cellTrigger = value;
                _cellTrigger?.Register(this);
            }
        }

        /// <summary>
        /// 计算A*寻路时的移动开销 默认为1
        /// </summary>
        public int MoveCost { get; set; } = 1;

        private HashSet<Cell> _neighbors;
        /// <summary>
        /// 周边格子 用于寻路计算
        /// </summary>
        public HashSet<Cell> Neighbors
        {
            get
            {
                if (_neighbors is null)
                {
                    _neighbors = BattleManager.CellGridMgr.Cells.Values.Where(c => GetDistance(c) == 1).ToHashSet();
                }
                return _neighbors;
            }
        }

        private void Start()
        {
            BattleManager.CellGridMgr.RegisterCell(this);
        }

        private void OnDestroy()
        {
            BattleManager.CellGridMgr.UnRegisterCell(this);
        }

        /// <summary>
        /// 计算坐标间的曼哈顿距离
        /// </summary>
        public int GetDistance(Vector2Int other)
        {
            var vec = Coord - other;
            return Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
        }
        /// <summary>
        /// 与其它格子的曼哈顿距离
        /// </summary>
        public int GetDistance(Cell cell)
        {
            return GetDistance(cell.Coord);
        }

        /// <summary>
        /// 以该格子为中心or起点，获取周边格子
        /// </summary>
        /// <param name="range">获取半径</param>
        /// <param name="includingSelf">是否包含自己</param>
        /// <param name="areaShape">形状</param>
        /// <returns></returns>
        public virtual HashSet<Cell> GetNeighborCells(int range, AreaShape areaShape = AreaShape.Circle, bool includingSelf = false)
        {
            HashSet<Cell> result = new();
            Action<Vector2Int, HashSet<Cell>> tryAdd = (vec,result) =>
            {
                if (BattleManager.CellGridMgr.Cells.ContainsKey(vec))
                {
                    result.Add(BattleManager.CellGridMgr.Cells[vec]);
                }
            };

            switch (areaShape)
            {
                case AreaShape.Circle:
                    for (var i = 0; i <= range; i++)
                    {
                        for (var j = 0; j <= range - i; j++)
                        {
                            tryAdd(Coord + new Vector2Int(i, j), result);
                            tryAdd(Coord + new Vector2Int(-i, j), result);
                            tryAdd(Coord + new Vector2Int(i, -j), result);
                            tryAdd(Coord + new Vector2Int(-i, -j), result);
                        }
                    }
                    break;
                case AreaShape.Square:
                    for (var i = 0; i <= range; i++)
                    {
                        for (var j = 0; j <= range; j++)
                        {
                            tryAdd(Coord + new Vector2Int(i, j), result);
                            tryAdd(Coord + new Vector2Int(-i, j), result);
                            tryAdd(Coord + new Vector2Int(i, -j), result);
                            tryAdd(Coord + new Vector2Int(-i, -j), result);
                        }
                    }
                    break;
                case AreaShape.Cross:
                    for(var i = 0; i <= range; i++)
                    {
                        tryAdd(this.Coord + new Vector2Int(i, 0), result);
                        tryAdd(this.Coord + new Vector2Int(0,i), result);
                        tryAdd(this.Coord - new Vector2Int(-i, 0), result);
                        tryAdd(this.Coord - new Vector2Int(0, i), result);
                    }
                    break;
            }
            if (!includingSelf)
            {
                result.Remove(this);
            }
            return result;

        }
    }

    public enum GroundType
    {
        Ground = 0b_0001,
        Water = 0b_0010,
        Unreachable = 0b_0000
    }

    public enum AreaShape
    {
        Circle, 
        Square, 
        Cross
    }
}
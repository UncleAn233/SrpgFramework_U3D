using SrpgFramework.CellGrid.AStar;
using SrpgFramework.Global;
using SrpgFramework.Units;
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
                    _neighbors = GameManager.CellGridMgr.Cells.Values.Where(c => GetDistance(c) == 1).ToHashSet();
                }
                return _neighbors;
            }
        }

        private void Start()
        {
            GameManager.CellGridMgr.RegisterCell(this);
        }

        private void OnDestroy()
        {
            GameManager.CellGridMgr.UnRegisterCell(this);
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
        public virtual HashSet<Cell> GetNeighborCells(int range, bool includingSelf = false, AreaShape areaShape = AreaShape.Circle)
        {
            HashSet<Cell> result;
            switch (areaShape)
            {
                case AreaShape.Circle:
                default:
                    result = GameManager.CellGridMgr.Cells.Values.Where(c => GetDistance(c) <= range).ToHashSet();
                    break;
                case AreaShape.Square:
                    result = GameManager.CellGridMgr.Cells.Values.Where(c =>
                    {
                        var vec = this.Coord - c.Coord;
                        return Mathf.Abs(vec.x) <= range && Mathf.Abs(vec.y) <= range;
                    }).ToHashSet();
                    break;
                case AreaShape.Cross:
                    result = GameManager.CellGridMgr.Cells.Values.Where(c => (this.Coord.x == c.Coord.x || this.Coord.y == c.Coord.y) && GetDistance(c) <= range).ToHashSet();
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
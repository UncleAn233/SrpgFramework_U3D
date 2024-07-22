using SrpgFramework.CellGrid.AStar;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.CellGrid.Cells
{
    /// <summary>
    /// ս����ӵĻ����� Ĭ��Ϊ������
    /// </summary>
    public partial class Cell : MonoBehaviour
    {
        public string Id;
        [SerializeField]
        private Vector2Int _coord;
        /// <summary>
        /// ����
        /// </summary>
        public Vector2Int Coord { get { return _coord; } set { _coord = value; } }

        /// <summary>
        /// �������ͣ���Ҫ���ڵ�λ�ƶ�ʱ�ж��ø����Ƿ��ͨ��
        /// </summary>
        public GroundType GroundType { get; set; } = GroundType.Ground;

        /// <summary>
        /// �ø����ϵĵ�λ
        /// </summary>
        public Unit Unit { get; set; }

        /// <summary>
        /// �ø����Ƿ���Unit
        /// </summary>
        public bool IsTaken => Unit is not null;

        /// <summary>
        /// ����A*Ѱ·ʱ���ƶ����� Ĭ��Ϊ1
        /// </summary>
        public int MoveCost { get; set; } = 1;

        private HashSet<Cell> _neighbors;
        /// <summary>
        /// �ܱ߸��� ����Ѱ·����
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
        /// ���������������پ���
        /// </summary>
        public int GetDistance(Vector2Int other)
        {
            var vec = Coord - other;
            return Mathf.Abs(vec.x) + Mathf.Abs(vec.y);
        }
        /// <summary>
        /// ���������ӵ������پ���
        /// </summary>
        public int GetDistance(Cell cell)
        {
            return GetDistance(cell.Coord);
        }

        /// <summary>
        /// �Ըø���Ϊ����or��㣬��ȡ�ܱ߸���
        /// </summary>
        /// <param name="range">��ȡ�뾶</param>
        /// <param name="includingSelf">�Ƿ�����Լ�</param>
        /// <param name="areaShape">��״</param>
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
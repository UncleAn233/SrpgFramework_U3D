using SrpgFramework.CellGrid.Cells;
using UnityEngine;

namespace SrpgFramework.Units
{
    public class MoveUnit : MonoBehaviour
    {
        private Unit unit;

        public GroundType MovableGroundType { get; set; }

        private void Awake()
        {
            unit = GetComponent<Unit>();
            MovableGroundType = GroundType.Ground;
        }

        private void Start()
        {
            unit.Points.Add("Move", 1);
            unit.Points.Add("TotalMove", 1);
            unit.RegisterEvent(nameof(Unit.TurnEnd), (obj) => { unit.Points["Move"] = unit.Points["TotalMove"]; });
        }

        public bool CanMove()
        {
            return unit.Points["Move"] > 0;
        }

        public virtual bool IsCellMovableTo(Cell cell)
        {
            return cell.IsTaken && (unit.Cell.GroundType & MovableGroundType) > 0;
        }
        public virtual bool IsCellTraversable(Cell cell)
        {
            if((unit.Cell.GroundType & MovableGroundType) == 0)
            {
                return false;
            }
            else
            {
                return !cell.IsTaken || cell.Unit.Player.IsFriend(cell.Unit.Player);
            }
        }
    }
}
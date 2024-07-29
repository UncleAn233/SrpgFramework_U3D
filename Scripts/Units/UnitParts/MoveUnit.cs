using SrpgFramework.CellGrid.Cells;
using System.Collections;
using System.Collections.Generic;
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
            return !cell.IsTaken && (unit.Cell.GroundType & MovableGroundType) > 0;
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

        public virtual IEnumerator Move(Cell destinationCell, IList<Cell> path)
        {
            unit.Points["Move"]--;
            unit.ApplyEvent("MoveStart");

            foreach (var cell in path)
            {
                var destination_pos = cell.transform.localPosition;
                unit.ApplyEvent("Face", destination_pos);
                while (transform.localPosition != destination_pos)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination_pos, Time.deltaTime);
                    yield return null;
                }
            }

            unit.Cell = destinationCell;
            unit.ApplyEvent("MoveEnd");

            yield return null;
        }
    }
}
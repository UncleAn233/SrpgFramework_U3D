using SrpgFramework.CellGrid.Cells;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units.Units
{
    public class MoveUnit : MonoBehaviour
    {
        private Unit unit;

        public int MovePoints { get; set; }
        public int TotalMovePoints { get; set; }

        public GroundType MovableGroundType { get; set; }
        public static float MoveAnimationSpeed = 1.5f;

        public Action OnMoveStart;
        public Action OnMoveEnd;

        private void Awake()
        {
            unit = GetComponent<Unit>();
            MovableGroundType = GroundType.Ground;
        }

        private void Start()
        {
            MovePoints = 1;
            TotalMovePoints = 1;
            unit.OnTurnEnd += (turn) => { MovePoints = TotalMovePoints; };
        }

        public bool CanMove()
        {
            return MovePoints > 0;
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
            MovePoints--;
            OnMoveStart?.Invoke();

            if (MoveAnimationSpeed > 0)
            {
                foreach (var cell in path)
                {
                    var destination_pos = cell.transform.localPosition;
                    unit.FaceTo(cell.Coord);
                    while (transform.localPosition != destination_pos)
                    {
                        transform.localPosition = Vector3.MoveTowards(transform.localPosition, destination_pos, Time.deltaTime * MoveAnimationSpeed);
                        yield return null;
                    }
                }
            }
            else
            {
                unit.transform.localPosition = destinationCell.transform.localPosition;
            }
            unit.Cell = destinationCell;
            OnMoveEnd?.Invoke();

            yield return null;
        }
    }
}
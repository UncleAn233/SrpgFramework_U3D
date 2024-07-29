using SrpgFramework.CellGrid.AStar;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Abilities
{
    public class MoveAbility : Ability
    {
        public Cell Destination { get; set; }   //目的地
        private IList<Cell> path;   //路径
        private HashSet<Cell> moveableArea; //可移动范围
        private Unit unit;  //当前单位

        public override IEnumerator Act(Unit unit)
        {
            path = AStart.GetPath(unit.Cell, Destination, unit.Move);
            yield return unit.Move.Move(Destination, path);
        }

        public override void Enter(Unit unit)
        {
            moveableArea = AStart.GetMoveableArea(unit.Cell, unit.Move, unit.Mov);
            foreach (var cell in moveableArea)
            {
                cell.Highlight(CellHighlighter.Tag_Selectable);
            }
            this.unit = unit;
        }

        public override void Exit()
        {
            foreach(var cell in moveableArea)
            {
                cell.DeHighlight();
            }
            moveableArea = null;
            path = null;
            unit = null;
        }

        public override void OnCellHighlighted(Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                path = AStart.GetPath(unit.Cell, cell, unit.Move);
                foreach (var c in path)
                {
                    c.Highlight(CellHighlighter.Tag_Effect);
                }
            }
            else
                cell.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnCellDehighlighted(Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                if (path == null)
                    return;

                foreach (var c in path)
                {
                    c.Highlight(CellHighlighter.Tag_Selectable);
                }
            }
            else
            {
                cell.DeHighlight();
            }
        }

        public override void OnCellClicked(Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                Destination = cell;
                unit.StartCoroutine(PlayerExecute(unit));
            }
            else
            {
                GameManager.CellGridMgr.ToIdleState();
            }
        }

        public override void OnUnitHighlighted(Unit other)
        {
            other.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnUnitDehighlighted(Unit other)
        {
            if (moveableArea.Contains(other.Cell))
                other.Cell.Highlight(CellHighlighter.Tag_Selectable);
            else
                other.DeHighlight();
        }

        public override bool CanPerform(Unit unit)
        {
            return unit.Move is not null && unit.Move.CanMove();
        }

        public override bool ShouldExecute(Unit unit, Cell cell)
        {
            if(!CanPerform(unit)) 
                return false;

            path = null;
            var top = unit.Ai.CellScoreDict.Where(c => unit.Move.IsCellMovableTo(c.Key)).OrderByDescending(c => c.Value).First();
            
            if(top.Value > unit.Ai.CellScoreDict[unit.Cell])
            {
                Destination = top.Key;
                return true;
            }
            return false;
        }

        public override float Evaluate(Unit unit, Cell cell)
        {
            var totalPath = AStart.GetPath(unit.Cell, cell, unit.Move);
            int cost = 0;
            
            for(var i = 0; i < totalPath.Count; i++)
            {
                cost += totalPath[i].MoveCost;
                if (cost > unit.Mov || !unit.Move.IsCellMovableTo(totalPath[i]))
                {
                    Destination = totalPath[i - 1 < 0 ? 0 : i - 1];
                }
            }

            return unit.Ai.CellScoreDict[Destination];
        }
    }
}
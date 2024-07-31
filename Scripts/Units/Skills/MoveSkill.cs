using SrpgFramework.CellGrid.AStar;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SrpgFramework.Units.Skills
{
    public class MoveSkill : Skill
    {
        public Cell Destination { get; set; }   //目的地
        private IList<Cell> path;   //路径
        private HashSet<Cell> moveableArea; //可移动范围

        public override IEnumerator Act(Unit unit)
        {
            path = AStar.FindPath(unit.Cell, Destination, unit.Move);
            yield return unit.Move.Move(Destination, path);
        }

        public override void Enter(Unit self)
        {
            moveableArea = AStar.GetMoveableArea(self.Cell, self.Move, self.Mov);
            foreach (var cell in moveableArea)
            {
                cell.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public override void Exit(Unit self = null)
        {
            foreach(var cell in moveableArea)
            {
                cell.DeHighlight();
            }
            moveableArea = null;
            path = null;
        }

        public override void OnCellHighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                path = AStar.FindPath(self.Cell, cell, self.Move);
                foreach (var c in path)
                {
                    c.Highlight(CellHighlighter.Tag_Effect);
                }
            }
            else
                cell.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnCellDehighlighted(Unit self, Cell cell)
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

        public override void OnCellClicked(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                Destination = cell;
                self.StartCoroutine(PlayerExecute(self));
            }
            else
            {
                BattleManager.CellGridMgr.ToIdleState();
            }
        }

        public override void OnUnitHighlighted(Unit self, Unit other)
        {
            other.Highlight(CellHighlighter.Tag_Cursor);
        }

        public override void OnUnitDehighlighted(Unit self, Unit other)
        {
            if (moveableArea.Contains(other.Cell))
                other.Cell.Highlight(CellHighlighter.Tag_Selectable);
            else
                other.DeHighlight();
        }

        public override bool CanPerform(Unit self)
        {
            return self.Move is not null && self.Move.CanMove();
        }

        public override bool ShouldExecute(Unit self, Cell cell)
        {
            if(!CanPerform(self)) 
                return false;

            var top = self.Ai.CellScoreDict.Where(c => self.Move.IsCellMovableTo(c.Key)).OrderByDescending(c => c.Value).First();
            if(top.Value > self.Ai.CellScoreDict[self.Cell])
            {
                Destination = top.Key;
                return true;
            }
            return false;
        }

        public override float Evaluate(Unit self)
        {
            var totalPath = AStar.FindPath(self.Cell, Destination, self.Move);
            int cost = 0;
            for (var i = 0; i < totalPath.Count; i++)
            {
                cost += totalPath[i].MoveCost;
                if (cost <= self.Mov && self.Move.IsCellMovableTo(totalPath[i]))
                    Destination = totalPath[i];
                else
                    break;
            }
            
            return self.Ai.CellScoreDict[Destination];
        }
    }
}
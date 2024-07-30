using static SrpgFramework.CellGrid.AStar.AStar;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System.Collections.Generic;

namespace SrpgFramework.CellGrid
{
    public class GridStateSelectUnit : ICellGridState
    {
        private CellGridManager _mgr;
        private Unit _unit;

        private HashSet<Cell> moveableArea;
        private List<Cell> path;
        public GridStateSelectUnit(CellGridManager mgr)
        {
            _mgr = mgr;
        }

        public void Enter(Unit unit = null)
        {
            _unit = unit;
            moveableArea = GetMoveableArea(unit.Cell, unit.Move, unit.Mov);
            foreach (var c in moveableArea)
            {
                c.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public void Exit()
        {
            _unit = null;
            foreach(var c in moveableArea)
            {
                c.DeHighlight();
            }
        }

        public void OnCellClicked(Cell cell)
        {
            BattleManager.CellGridMgr.ToState(_mgr.IdleState);
        }

        public void OnCellHighlighted(Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                path = FindPath(_unit.Cell, cell, _unit.Move);
                foreach (var c in path)
                    c.Highlight(CellHighlighter.Tag_Effect);
            }
            else
            {
                cell.Highlight(CellHighlighter.Tag_Cursor);
            }
        }

        public void OnCellDehighlighted(Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                foreach (var c in path)
                    c.Highlight(CellHighlighter.Tag_Selectable);
            }
            else
            {
                cell.DeHighlight();
            }
        }

        public void OnUnitClicked(Unit unit)
        {
        }

        public void OnUnitDehighlighted(Unit unit)
        {
            unit.DeHighlight();
        }

        public void OnUnitHighlighted(Unit unit)
        {
            unit.Highlight(CellHighlighter.Tag_Cursor);
        }
    }
}
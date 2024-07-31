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

        public void Enter(Unit unit)
        {
            _unit = unit;
            moveableArea = GetMoveableArea(unit.Cell, unit.Move, unit.Mov);
            foreach (var c in moveableArea)
            {
                c.Highlight(CellHighlighter.Tag_Selectable);
            }
        }

        public void Exit(Unit self)
        {
            _unit = null;
            foreach (var c in moveableArea)
            {
                c.DeHighlight();
            }
        }

        public void OnCellClicked(Unit self, Cell cell)
        {
            BattleManager.CellGridMgr.ToState(_mgr.IdleState, null);
        }

        public void OnCellHighlighted(Unit self, Cell cell)
        {
            cell.Highlight(CellHighlighter.Tag_Cursor);
        }

        public void OnCellDehighlighted(Unit self, Cell cell)
        {
            if (moveableArea.Contains(cell))
            {
                cell.Highlight(CellHighlighter.Tag_Selectable);
            }
            else
            {
                cell.DeHighlight();
            }
        }

        public void OnUnitClicked(Unit self, Unit unit)
        {
        }

        public void OnUnitDehighlighted(Unit self, Unit unit)
        {
            unit.DeHighlight();
        }

        public void OnUnitHighlighted(Unit self, Unit unit)
        {
            unit.Highlight(CellHighlighter.Tag_Cursor);
        }
    }
}
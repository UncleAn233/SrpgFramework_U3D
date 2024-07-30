using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units.Units;

namespace SrpgFramework.CellGrid
{
    public class GridStateIdle : ICellGridState
    {
        private CellGridManager _mgr;

        public GridStateIdle(CellGridManager mgr)
        {
            this._mgr = mgr;
        }

        public void Enter(Unit unit = null)
        {
        }

        public void Exit()
        {
        }

        public void OnUnitClicked(Unit unit)
        {
            _mgr.ToState(_mgr.SelectUnitState, unit);
        }

        public void OnUnitHighlighted(Unit unit)
        {
            unit.Highlight("");
        }

        public void OnUnitDehighlighted(Unit unit)
        {
            unit.DeHighlight();
        }

        public void OnCellDehighlighted(Cell cell)
        {
            cell.DeHighlight();
        }

        public void OnCellHighlighted(Cell cell)
        {
            cell.Highlight(CellHighlighter.Tag_Cursor);
        }


        public void OnCellClicked(Cell cell)
        {
        }

    }
}
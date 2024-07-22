using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units;

namespace SrpgFramework.CellGrid
{
    public interface ICellGridState
    {
        public virtual bool CanTranslateTo(ICellGridState nextState)
        {
            return nextState is not null && nextState != this;
        }

        public void OnUnitClicked(Unit unit);
        public void OnUnitHighlighted(Unit unit);
        public void OnUnitDehighlighted(Unit unit);
        public void OnCellDehighlighted(Cell cell);
        public void OnCellHighlighted(Cell cell);
        public void OnCellClicked(Cell cell);
        public void Enter(Unit unit = null);
        public void Exit();
    }
}
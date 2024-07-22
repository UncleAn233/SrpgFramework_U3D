using SrpgFramework.CellGrid;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units;
using System;
using System.Collections;

namespace SrpgFramework.Abilities
{
    public abstract class Ability : ICellGridState
    {
        public virtual IEnumerator Act(Unit unit) { yield return null; }

        public IEnumerator Execute(Unit unit, Action preAction, Action postAction)
        {
            preAction();
            yield return unit.StartCoroutine(Act(unit));
            postAction();
            yield return null;
        }

        public IEnumerator PlayerExecute(Unit unit)
        {
            yield return Execute(unit,
                () =>
                {
                    GameManager.CellGridMgr.ToBlockInputState();
                },

                () =>
                {
                    GameManager.CellGridMgr.ToIdleState();
                });
        }
        public virtual IEnumerator AIExecute(Unit unit)
        {
            yield return Execute(unit, () => { }, () => { unit.Ai.EvaluateUnits(); });
        }

        public void Enter(Unit unit = null) { }
        public void Exit() { }
        public void OnCellClicked(Cell cell) { }
        public void OnCellDehighlighted(Cell cell) { }
        public void OnCellHighlighted(Cell cell) { }
        public void OnUnitClicked(Unit unit) { }
        public void OnUnitDehighlighted(Unit unit) { }
        public void OnUnitHighlighted(Unit unit) { }

        public virtual bool ShouldExecute(Unit unit, Cell cell) { return false; }
        public virtual float Evaluate(Unit unit, Cell cell) { return -1; }
    }
}
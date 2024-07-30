using SrpgFramework.CellGrid;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System;
using System.Collections;

namespace SrpgFramework.Units.Abilities
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
                    BattleManager.CellGridMgr.ToBlockInputState();
                },

                () =>
                {
                    BattleManager.CellGridMgr.ToIdleState();
                });
        }
        public virtual IEnumerator AIExecute(Unit unit)
        {
            yield return Execute(unit, () => { }, () => { unit.Ai.EvaluateUnits(); });
        }

        public virtual void Enter(Unit unit = null) { }
        public virtual void Exit() { }
        public virtual void OnCellClicked(Cell cell) { }
        public virtual void OnCellDehighlighted(Cell cell) { }
        public virtual void OnCellHighlighted(Cell cell) { }
        public virtual void OnUnitClicked(Unit unit) { }
        public virtual void OnUnitDehighlighted(Unit unit) { }
        public virtual void OnUnitHighlighted(Unit unit) { }
        public virtual bool CanPerform(Unit unit) { return true; }

        public bool ShouldExecute(Unit unit) { return ShouldExecute(unit, unit.Cell); }
        public virtual bool ShouldExecute(Unit unit, Cell cell) { return false; }
        public virtual float Evaluate(Unit unit, Cell cell) { return -1; }
    }
}
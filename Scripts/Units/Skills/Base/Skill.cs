using SrpgFramework.CellGrid;
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System;
using System.Collections;

namespace SrpgFramework.Units.Skills
{
    public abstract class Skill : ICellGridState
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
            yield return Execute(unit, () => { }, () =>
            {
                unit.Ai.EvaluateUnits();
                unit.Ai.EvaluateNeighborCells();
            });
        }

        public virtual void Enter(Unit self) { }
        public virtual void Exit(Unit self) { }
        public virtual void OnCellClicked(Unit self, Cell cell) { }
        public virtual void OnCellDehighlighted(Unit self, Cell cell) { }
        public virtual void OnCellHighlighted(Unit self, Cell cell) { }
        public virtual void OnUnitClicked(Unit self, Unit unit) { }
        public virtual void OnUnitDehighlighted(Unit self, Unit unit) { }
        public virtual void OnUnitHighlighted(Unit self, Unit unit) { }
        public virtual bool CanPerform(Unit self) { return true; }

        public bool ShouldExecute(Unit unit) { return ShouldExecute(unit, unit.Cell); }
        /// <summary>
        /// 判断AI是否可以使用该Skill 同时包含一些前置处理
        /// </summary>
        /// <param name="self">使用单位</param>
        /// <param name="cell">假设单位所处的Cell</param>
        /// <returns></returns>
        public virtual bool ShouldExecute(Unit self, Cell cell) { return false; }
        public virtual float Evaluate(Unit self) { return 0; }
    }
}
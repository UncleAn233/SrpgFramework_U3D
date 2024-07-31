using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Units.Units;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Units.Skills
{
    public class AttackSkill : Skill
    {
        public string[] DamageTags;
        public Unit UnitToAttack { get; set; }

        [SerializeField]
        private AreaShape attackShape;
        [SerializeField]
        private int attackRange;
        protected HashSet<Cell> attackableArea;

        public override IEnumerator Act(Unit unit)
        {
            //UnitToAttack.Hp--;
            yield return null;
        }

        public override void Enter(Unit unit)
        {
            attackableArea = unit.Cell.GetNeighborCells(1);
            foreach (var c in attackableArea)
                c.Highlight(CellHighlighter.Tag_Selectable);

            return;
        }
        public override void Exit(Unit self = null)
        {
            foreach (var c in attackableArea)
            {
                c.DeHighlight();
            }
            attackableArea.Clear();
        }

        public override bool CanPerform(Unit self)
        {
            return self.ActionPoints > 0;
        }

        public override void OnCellClicked(Unit self, Cell cell)
        {
            if (!attackableArea.Contains(cell))
                BattleManager.CellGridMgr.ToIdleState();
        }
        public override void OnCellHighlighted(Unit self, Cell cell)
        {
            if (attackableArea.Contains(cell))
                cell.Highlight(CellHighlighter.Tag_Effect);
            else
                cell.Highlight(CellHighlighter.Tag_Cursor);
        }
        public override void OnCellDehighlighted(Unit self, Cell cell)
        {
            if (attackableArea.Contains(cell))
                cell.Highlight(CellHighlighter.Tag_Selectable);
            else
                cell.DeHighlight();
        }

        public override void OnUnitHighlighted(Unit self, Unit target)
        {
            if (attackableArea.Contains(target.Cell) && self.Player.IsEnemy(target.Player))
                target.Cell.Highlight(CellHighlighter.Tag_Effect);
            else
                target.Highlight(CellHighlighter.Tag_Cursor);
        }
        public override void OnUnitDehighlighted(Unit self, Unit target)
        {
            if (attackableArea.Contains(target.Cell) && self.Player.IsEnemy(target.Player))
                target.Cell.Highlight(CellHighlighter.Tag_Selectable);
            else
                target.DeHighlight();
        }
        public override void OnUnitClicked(Unit self, Unit target)
        {
            if (self.Player.IsEnemy(target.Player) && attackableArea.Contains(target.Cell))
            {
                UnitToAttack = target;
                self.StartCoroutine(PlayerExecute(self));
            }
        }

        public override bool ShouldExecute(Unit self, Cell cell)
        {
            if (!CanPerform(self))
                return false;
            attackableArea = cell.GetNeighborCells(1);
            return attackableArea.Any(c => c.Unit is not null && self.Player.IsEnemy(c.Unit.Player));
        }

        public override float Evaluate(Unit self)
        {
            var units = attackableArea.Where(c => c.Unit is not null && self.Player.IsEnemy(c.Unit.Player)).Select(c => c.Unit);
            var top = units.OrderByDescending(u => self.Ai.UnitScoreDict[u] + expectDamage(self, u)).First();
            return self.Ai.UnitScoreDict[top] + expectDamage(self, top);
        }

        private float expectDamage(Unit self, Unit target)
        {
            return 1;
        }
    }
}
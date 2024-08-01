using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units.Units;
using System.Collections;
using System.Collections.Generic;

namespace SrpgFramework.Units.Skills
{
    public interface ISkill 
    {
        public ISet<Cell> GetSelectableArea(Cell cell);
        public ISet<Cell> GetEffectArea(Cell cell);

        public IEnumerator Act(Unit unit);
    }
}
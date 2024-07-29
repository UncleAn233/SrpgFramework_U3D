using SrpgFramework.CellGrid.Cells;
using UnityEngine;

namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public void Highlight(string highlighter)
        {
            this.Cell.Highlight(highlighter);
        }

        public void DeHighlight()
        {
            this.cell.DeHighlight();
        }
    }
}
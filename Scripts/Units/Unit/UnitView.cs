using SrpgFramework.CellGrid.Cells;
using UnityEngine;

namespace SrpgFramework.Units
{
    public partial class Unit
    {
        public void Highlight(CellHighlightTag highlightTag, params object[] objs)
        {
            this.Cell.Highlight(highlightTag, objs);
        }

        public void DeHighlight()
        {
            this.cell.DeHighlight();
        }
    }
}
using UnityEngine;

namespace SrpgFramework.Units.Units
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

        public virtual void FaceTo(Vector2Int vec)
        {
            var face = vec - this.Cell.Coord;
            Vector2Int result = Vector2Int.zero;
            if (face.x == 0)
            {
                result = face.y > 0 ? Vector2Int.up : Vector2Int.down;
            }
            else if (face.y == 0)
            {
                result = face.x > 0 ? Vector2Int.right : Vector2Int.left;
            }
            return;
        }
    }
}
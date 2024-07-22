using UnityEngine;

namespace SrpgFramework.CellGrid.Cells
{
    public class CellHighlighter :MonoBehaviour
    {
        public CellHighlightTag Tag;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            var cell = GetComponentInParent<Cell>();
            cell.OnHighlight += Apply;
        
            spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public void Apply(CellHighlightTag highlightTag, params object[] obj)
        {
            if (highlightTag == Tag)
                spriteRenderer.enabled = true;
            else
                spriteRenderer.enabled = false;
        }
    }

    public enum CellHighlightTag
    {
        Select, Selectable, Effect, Path, DeHighlight
    }
}
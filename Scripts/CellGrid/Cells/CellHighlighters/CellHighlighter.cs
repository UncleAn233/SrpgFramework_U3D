using UnityEngine;

namespace SrpgFramework.CellGrid.Cells
{
    public class CellHighlighter : MonoBehaviour
    {
        public static string Tag_DeHighlight = "";
        public static string Tag_Cursor = "cursor";
        public static string Tag_Selectable = "selectable";
        public static string Tag_Effect = "effect";

        private SpriteRenderer spriteRenderer;
        public string Tag;

        private void Awake()
        {
            var cell = GetComponentInParent<Cell>();
            cell.OnHighlight += Apply;
        
            spriteRenderer = this.GetComponent<SpriteRenderer>();
        }

        public void Apply(string highlightTag)
        {
            if (highlightTag == Tag.ToLower()) 
                spriteRenderer.enabled = true;
            else
                spriteRenderer.enabled = false;
        }
    }
}
using SrpgFramework.Players;
using SrpgFramework.Units;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SrpgFramework.CellGrid.Cells
{
    // ������ΪCell���¼�����
    public partial class Cell
    {
        #region GUI
        public Action<Cell> OnCellClicked;
        public Action<Cell> OnCellHighlighted;
        public Action<Cell> OnCellDehighlighted;
        public Action<Unit> OnUnitClicked;
        public Action<Unit> OnUnitHighlighted;
        public Action<Unit> OnUnitDeHighlighted;
        public Action<Player> OnTurnStart;
        public Action<Player> OnTurnEnd;

        public virtual void OnMouseEnter()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Unit is null)
                {
                    OnCellHighlighted?.Invoke(this);
                }
                else
                {
                    OnUnitHighlighted?.Invoke(Unit);
                }
            }
        }
        public virtual void OnMouseExit()
        {
            if (Unit is null)
                OnCellDehighlighted?.Invoke(this);
            else
                OnUnitDeHighlighted?.Invoke(Unit);
        }
        public virtual void OnMouseDown()
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Unit is null)
                    OnCellClicked?.Invoke(this);
                else
                    OnUnitClicked?.Invoke(Unit);
            }
        }
        #endregion

        #region ����        
        public Action<CellHighlightTag, object[]> OnHighlight;

        public void Highlight(CellHighlightTag highlightTag, params object[] obj)
        {
            OnHighlight?.Invoke(highlightTag, obj);
        }

        public void DeHighlight()
        {
            OnHighlight?.Invoke(CellHighlightTag.DeHighlight, null);
        }
        #endregion
    }
}
using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Units.Units;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.CellGrid {
    public class CellGridManager : MonoBehaviour
    {
        public Dictionary<Vector2Int,Cell> Cells { get; private set; } = new();

        #region CellGridState
        public ICellGridState CurrentGridState { get; private set; }
        
        /// <summary>
        /// Default State As Block Input
        /// </summary>
        public ICellGridState BlockInputState { get; private set; }
        /// <summary>
        /// Default State As Idle
        /// </summary>
        public ICellGridState IdleState { get; private set; }
        /// <summary>
        /// Default State While an Unit is Selected
        /// </summary>
        public ICellGridState SelectUnitState { get; private set; }

        private Unit currentUnit;

        public void Awake()
        {
            BlockInputState = new GridStateBlockInput();
            IdleState = new GridStateIdle(this);
            SelectUnitState = new GridStateSelectUnit(this);
            CurrentGridState = IdleState;
        }

        public bool ToState(ICellGridState nextState, Unit unit)
        {
            if(CurrentGridState.CanTranslateTo(nextState))
            {
                CurrentGridState?.Exit(currentUnit);
                CurrentGridState = nextState;
                currentUnit = unit;
                CurrentGridState.Enter(currentUnit);
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ToBlockInputState()
        {
            return ToState(BlockInputState, null);
        }

        public bool ToIdleState()
        {
            return ToState(IdleState, null);
        }

        public void RegisterCell(Cell cell)
        {
            if(Cells.TryAdd(cell.Coord, cell))
            {
                cell.OnCellClicked += OnCellClicked;
                cell.OnCellHighlighted += OnCellHighlighted;
                cell.OnCellDehighlighted += OnCellDehighlighted;
                cell.OnUnitClicked += OnUnitClicked;
                cell.OnUnitHighlighted += OnUnitHighlighted;
                cell.OnUnitDeHighlighted += OnUnitDehighlighted;
            }
        }

        public void UnRegisterCell(Cell cell)
        {
            if (Cells.Remove(cell.Coord))
            {
                cell.OnCellClicked -= OnCellClicked;
                cell.OnCellHighlighted -= OnCellHighlighted;
                cell.OnCellDehighlighted -= OnCellDehighlighted;
                cell.OnUnitClicked -= OnUnitClicked;
                cell.OnUnitHighlighted -= OnUnitHighlighted;
                cell.OnUnitDeHighlighted -= OnUnitDehighlighted;
            }
        }

        private void OnCellDehighlighted(Cell cell)
        {
            CurrentGridState.OnCellDehighlighted(currentUnit, cell);
        }
        private void OnCellHighlighted(Cell cell)
        {
            CurrentGridState.OnCellHighlighted(currentUnit, cell);
        }
        private void OnCellClicked(Cell cell)
        {
            CurrentGridState.OnCellClicked(currentUnit, cell);
        }
        private void OnUnitClicked(Unit unit)
        {
            CurrentGridState.OnUnitClicked(currentUnit, unit);
        }
        private void OnUnitHighlighted(Unit unit)
        {
            CurrentGridState.OnUnitHighlighted(currentUnit, unit);
        }
        private void OnUnitDehighlighted(Unit unit)
        {
            CurrentGridState.OnUnitDehighlighted(currentUnit, unit);
        }
        #endregion
    }
}
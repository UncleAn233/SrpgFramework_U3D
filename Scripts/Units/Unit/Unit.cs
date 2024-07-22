using SrpgFramework.CellGrid.Cells;
using SrpgFramework.Global;
using SrpgFramework.Players;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Units
{
    public partial class Unit : MonoBehaviour
    {
        private string _id;
        public string ID
        {
            get => _id; 
            set
            {
                if(_id is null)
                {
                    _id = value;
                }
            }
        }
        public HashSet<string> Tags;
        public UnitType UnitType;

        public int PlayerNumber;
        public Player Player => GameManager.PlayerMgr.GetPlayer(PlayerNumber);

        [SerializeField]
        [HideInInspector]
        private Cell cell;
        public Cell Cell
        {
            get { return cell; }
            set
            {
                if (cell is not null)
                {
                    cell.Unit = null;
                }
                cell = value;
                cell.Unit = this;
            }
        }

        public Dictionary<string, int> Points { get; private set; }
        private Dictionary<string, Action<object[]>> events { get; set; }
        
        private void Awake()
        {
            Points = new();
            events = new();
        }

        private void Start()
        {
            Cell.Unit = this;
            Points.Add("Action", 1);
            Points.Add("TotalAction", 1);
            RegisterEvent(nameof(TurnEnd), (obj) => { Points["Action"] = Points["TotalAction"]; });
        }

        public void RegisterEvent(string key, Action<object[]> callback)
        {
            if (events.ContainsKey(key))
            {
                events[key] += callback;
            }
            else
            {
                events.Add(key, (u) => { });
                events[key] += callback;
            }
        }
        public void UnRegisterEvent(string key)
        {
            events.Remove(key);
        }
        public void UnRegisterEvent(string key, Action<object[]> callback)
        {
            if (events.ContainsKey(key))
            {
                events[key] -= callback;
            }
        }
        public void ApplyEvent(string key, params object[] args)
        {
            if (events.ContainsKey(key))
            {
                events[key]?.Invoke(args);
            }
        }

        public void TurnStart(int player)
        {
            if(player == PlayerNumber)
            {
                this.ApplyEvent(nameof(TurnStart));
            }
        }

        public void TurnEnd(int player)
        {
            if(player == PlayerNumber)
            {
                this.ApplyEvent(nameof(TurnEnd));
            }
        }

        public void SetPos(Cell cell)
        {
            if(cell is not null)
            {
                Cell = cell;
                this.transform.localPosition = cell.transform.localPosition;
            }
        }
    }

    public enum UnitType
    {
        PC, NPC, NC
    }
}
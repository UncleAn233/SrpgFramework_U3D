using SrpgFramework.Global;
using SrpgFramework.Level;
using SrpgFramework.Units.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SrpgFramework.Players
{
    public class PlayerManager : MonoBehaviour
    {
        public Action<int> OnPlayerStart;
        public Action<int> OnPlayerEnd;
        public List<Player> Players { get; private set; }
        public Player CurrentPlayer => Players[currentPlayerIndex];
        private int currentPlayerIndex;

        public Action<int> OnTurnStart;
        public Action<int> OnTurnEnd;
        public int CurrentTurn { get; private set; }
        public int MaxTurn { get; private set; }

        private void Awake()
        {
            Players = new();
        }
        private void Start()
        {
/*            var lvManager = BattleManager.LevelMgr;
            lvManager.OnLevelStart += GameStart;
            lvManager.OnLevelEnd += GameEnd;*/
            GeneratePlayers();
            GameStart();
        }

        public Player GetPlayer(int num)
        {
            if (Players.Count < num || num < 0)
            {
                return null;
            }
            return Players[num];
        }

        public Player NextPlayer()
        {
            BattleManager.CellGridMgr.ToBlockInputState();
            OnPlayerEnd(currentPlayerIndex);

            var next = Players.First(p => p.PlayerNumber > currentPlayerIndex && p.HasUnit());
            if(next is null)
            {
                currentPlayerIndex = 0;
                NextTurn();
            }
            else
            {
                currentPlayerIndex = next.PlayerNumber;
            }

            OnPlayerStart(currentPlayerIndex);
            CurrentPlayer.Play();
            return CurrentPlayer;
        }

        public void NextTurn()
        {
            OnTurnEnd?.Invoke(CurrentTurn);
            CurrentTurn++;
            if (CurrentTurn <= MaxTurn)
            {
                OnTurnStart?.Invoke(CurrentTurn);
            }
            else
            {

            }
        }

        public void RegisterUnit(Unit unit)
        {
            OnTurnStart += unit.OnTurnStart;
            OnTurnEnd += unit.OnTurnEnd;
        }

        public void UnRegisterUnit(Unit unit)
        {
            OnTurnStart -= unit.OnTurnStart;
            OnTurnEnd -= unit.OnTurnEnd;
        }

        /// <summary>
        /// 创建Players
        /// </summary>
        public void GeneratePlayers()
        {
            Players.Clear();
            Action<Player, int, PlayerAlignment> regist = (player, num, alignment) =>
            {
                Players.Add(player);
                player.PlayerNumber = num;
                player.Alignment = alignment;
            };
            var playerParent = new GameObject("Players");
            playerParent.transform.parent = this.transform;
            regist(playerParent.AddComponent<HumanPlayer>(), 0, PlayerAlignment.Friend);   //玩家
            regist(playerParent.AddComponent<AiPlayer>(), 1, PlayerAlignment.Friend);   //友军
            regist(playerParent.AddComponent<AiPlayer>(), 2, PlayerAlignment.Enemy);  //敌人
            regist(playerParent.AddComponent<AiPlayer>(), 3, PlayerAlignment.Third); //中立
            Players.Capacity = Players.Count;
        }

        void GameStart()
        {
            currentPlayerIndex = 0;
            OnPlayerStart?.Invoke(currentPlayerIndex);
            CurrentPlayer.Play();
        }

        public void GameEnd()
        {
            CurrentPlayer.StopAllCoroutines();
        }
    }
}
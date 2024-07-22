using SrpgFramework.Global;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SrpgFramework.Players
{
    public class PlayerManager : MonoBehaviour
    {
        public List<Player> Players { get; private set; }
        public Player CurrentPlayer => Players[current];
        private int current;

        public Action<Player> OnTurnStart;
        public Action<Player> OnTurnEnd;

        private void Awake()
        {
            Players = new();
        }
        private void Start()
        {
            var lvManager = GameManager.LevelMgr;
/*            lvManager.OnLevelStart += GameStart;
            lvManager.OnLevelEnd += GameEnd;*/
            GeneratePlayers();
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
            current++;
            if (current >= Players.Count)
            {
                current = 0;
            }
            if (CurrentPlayer.Units.Count == 0)
            {
                return NextPlayer();
            }
            return CurrentPlayer;

        }

        public void EndTurn()
        {
            GameManager.CellGridMgr.ToBlockInputState();

            OnTurnEnd?.Invoke(CurrentPlayer);
            NextPlayer();
            OnTurnStart?.Invoke(CurrentPlayer);
            CurrentPlayer.Play();

            Debug.Log(string.Format("Player {0} turn", CurrentPlayer.PlayerNumber));
        }

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
            regist(playerParent.AddComponent<HumanPlayer>(), 0, PlayerAlignment.Friend);   //���
            regist(playerParent.AddComponent<AiPlayer>(), 1, PlayerAlignment.Friend);   //�Ѿ�
            regist(playerParent.AddComponent<AiPlayer>(), 2, PlayerAlignment.Enemy);  //����
            regist(playerParent.AddComponent<AiPlayer>(), 3, PlayerAlignment.Third); //����
            Players.Capacity = Players.Count;
        }

        void GameStart()
        {
            current = 0;
            OnTurnStart?.Invoke(CurrentPlayer);
            CurrentPlayer.Play();
        }

        public void GameEnd()
        {
            CurrentPlayer.StopAllCoroutines();
        }
    }
}
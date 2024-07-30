using Newtonsoft.Json;
using System;
using UnityEngine;

namespace SrpgFramework.Level
{
    public class LevelManager : MonoBehaviour
    {
        public Action OnLevelStart;
        public Action<LevelData, int> OnLevelLoad;
        public Action OnLevelEnd;

        public bool GameOver { get; private set; }

        /// <summary>
        /// 关卡搭建
        /// </summary>
        public void LoadLevel(string level)
        {
            var json = Resources.Load<TextAsset>($"Json/Level/{level}");
            if (json is null)
            {
                Debug.LogError("Not Find the Level File");
                return;
            }
            var levelData = JsonConvert.DeserializeObject<LevelData>(json.text);

            for(var i = 0; i <= 1; i++)
            {
               OnLevelLoad(levelData, i);
            }
            GameOver = false;
            PlayerReady();
        }

        /// <summary>
        /// 玩家准备
        /// </summary>
        public void PlayerReady()
        {
        }

        public void LevelStart()
        {
            OnLevelStart?.Invoke();
        }

        public void LevelEnd()
        {
            OnLevelEnd?.Invoke();
        }
    }
}
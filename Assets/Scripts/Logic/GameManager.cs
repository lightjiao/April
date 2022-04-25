using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public enum GameState
    {
        GameInit,
        GameStart,
        Game,
        GameOver
    }

    public static class GameManager
    {
        private static GameState _currentState;

        public static GameState CurrentState
        {
            get => _currentState;
            set
            {
                _currentState = value;

                _gameViewManager.gameInitPanel.SetActiveEx(_currentState == GameState.GameInit);
                _gameViewManager.gameStartPanel.SetActiveEx(_currentState == GameState.GameStart);
                _gameViewManager.gamePanel.SetActiveEx(_currentState is GameState.Game or GameState.GameOver);
                _gameViewManager.gameOverBtn.SetActive(_currentState == GameState.GameOver);
            }
        }

        private static GameViewManager _gameViewManager;

        public static PropertyCollection Properties;

        public static Dictionary<int, EventData> EventConfigs = new();

        public static List<int> BornEventPool = new();
        public static List<int> DailyEventPool1 = new();
        public static List<int> DailyEventPool2 = new();
        public static List<int> SpecialEventPool = new();
        public static List<int> EndingEventPool = new();
        public static Dictionary<int, int> HappenedEvent = new();

        public static IEnumerator Init()
        {
            // 初始化游戏
            while (_gameViewManager == null)
            {
                _gameViewManager = GameObject.Find("GameViewManager").GetComponent<GameViewManager>();
                yield return null;
            }

            CurrentState = GameState.GameInit;

            yield return null;

            var request = Resources.LoadAsync<TextAsset>("Event");
            yield return request;
            InitConfig(((TextAsset) request.asset).text);

            CurrentState = GameState.GameStart;
            yield return null;
        }

        /// <summary>
        /// 初始化配置
        /// </summary>
        private static void InitConfig(string dataStr)
        {
            var dataList = CsvParser.ParseData<EventData>(dataStr);
            var dataDict = new Dictionary<int, EventData>();
            foreach (var oneData in dataList)
            {
                dataDict[oneData.Id] = oneData;

                var pool = oneData.Type switch
                {
                    ConstStr.出生事件 => BornEventPool,
                    ConstStr.固定事件 => DailyEventPool1,
                    ConstStr.随机事件 => DailyEventPool2,
                    ConstStr.特殊事件 => SpecialEventPool,
                    ConstStr.必然事件 => EndingEventPool,
                    _ => DailyEventPool2
                };

                // 以权重的方式将事件添加到随即池
                for (var i = 0; i < oneData.Weight; i++)
                {
                    pool.Add(oneData.Id);
                }
            }

            EventConfigs = dataDict;

            Debug.Log(EventConfigs.ToString());
        }
    }
}